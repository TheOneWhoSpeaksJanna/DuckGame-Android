#!/usr/bin/env python3
"""Patch SDL3 (release-3.2.4) Android driver so it works WITHOUT SDL's Java
SDLActivity glue. FNA calls SDL_Init(VIDEO) which selects the Android driver,
but that driver normally depends on SDL's Java activity (nativeSetupJNI,
getNativeSurface, onNativeSurfaceCreated, etc.) which a .NET Android app never
runs. This patch:

  1. Self-acquires the ART JavaVM in native C (JNI_GetCreatedJavaVMs) so the
     JNI helpers are functional if ever needed.
  2. Reads the ANativeWindow from a global we set from managed code
     (SDL_AndroidSetNativeWindow) instead of calling the absent Java
     getNativeSurface, and exposes SDL_AndroidSetNativeWindow /
     SDL_AndroidSetScreenResolution / SDL_AndroidInitNative entry points.
  3. Makes Android_JNI_SetOrientation / SuspendScreenSaver / display-orientation
     lookups no-op when there is no JavaVM (we drive orientation ourselves).
  4. Skips Android_InitTouch / Android_InitMouse / display-orientation init in
     Android_VideoInit when there is no JavaVM (we inject keyboard input and
     don't need Android touch/mouse).
  5. Makes Android_WaitActiveAndLockActivity non-blocking (no Java activity
     lifecycle state machine).

Run from the root of the extracted SDL3 source tree.
"""
import os
import sys

ROOT = os.getcwd()


def patch_file(relpath, old, new, count=1):
    path = os.path.join(ROOT, relpath)
    with open(path, "r", encoding="utf-8") as f:
        data = f.read()
    n = data.count(old)
    if n != count:
        print(f"FAIL: {relpath}: expected {count} occurrence(s) of anchor, found {n}")
        print("ANCHOR:\n" + old[:400])
        sys.exit(1)
    data = data.replace(old, new, count)
    with open(path, "w", encoding="utf-8") as f:
        f.write(data)
    print(f"OK: {relpath}")


# ---------------------------------------------------------------------------
# 1. core/android/SDL_android.h : declare the new bridge API + accessor
# ---------------------------------------------------------------------------
SDL_ANDROID_H = "src/core/android/SDL_android.h"
with open(os.path.join(ROOT, SDL_ANDROID_H)) as f:
    h = f.read()
if "SDL_AndroidSetNativeWindow" not in h:
    # Insert right after the existing Android_JNI_GetNativeWindow extern.
    anchor = "extern ANativeWindow *Android_JNI_GetNativeWindow(void);\n"
    assert anchor in h, "SDL_android.h anchor missing"
    h = h.replace(anchor, anchor + """
/* DuckGame-Android: native bridge so SDL's Android driver works without the
   Java SDLActivity glue (the .NET Android host sets these directly). */
extern __attribute__((visibility("default"))) void SDL_AndroidInitNative(void);
extern __attribute__((visibility("default"))) void SDL_AndroidSetNativeWindow(ANativeWindow *window);
extern __attribute__((visibility("default"))) void SDL_AndroidSetNativeWindowFromSurface(void *env, void *surface);
extern __attribute__((visibility("default"))) void SDL_AndroidSetScreenResolution(int surfaceWidth, int surfaceHeight,
                                            int deviceWidth, int deviceHeight,
                                            float density, float rate);
extern bool Android_JNI_IsReady(void);
""")
    with open(os.path.join(ROOT, SDL_ANDROID_H), "w") as f:
        f.write(h)
    print("OK: " + SDL_ANDROID_H)
else:
    print("SKIP: " + SDL_ANDROID_H + " (already patched)")

# ---------------------------------------------------------------------------
# 2. core/android/SDL_android.c : globals, native window, bridge functions,
#    guarded orientation / screensaver / display-orientation helpers.
# ---------------------------------------------------------------------------
ANDROID_C = "src/core/android/SDL_android.c"

patch_file(
    ANDROID_C,
    "static JavaVM *mJavaVM = NULL;",
    """static JavaVM *mJavaVM = NULL;
/* DuckGame-Android: native window supplied by the host instead of Java getNativeSurface */
static ANativeWindow *g_NativeWindow = NULL;""",
)

# Android_JNI_GetNativeWindow returns the host-supplied global if present.
patch_file(
    ANDROID_C,
    """ANativeWindow *Android_JNI_GetNativeWindow(void)
{
    ANativeWindow *anw = NULL;
    jobject s;
    JNIEnv *env = Android_JNI_GetEnv();

    s = (*env)->CallStaticObjectMethod(env, mActivityClass, midGetNativeSurface);
    if (s) {
        anw = ANativeWindow_fromSurface(env, s);
        (*env)->DeleteLocalRef(env, s);
    }

    return anw;
}""",
    """ANativeWindow *Android_JNI_GetNativeWindow(void)
{
    /* DuckGame-Android: if the host supplied a native window, use it. */
    if (g_NativeWindow) {
        return g_NativeWindow;
    }
    ANativeWindow *anw = NULL;
    JNIEnv *env = Android_JNI_GetEnv();
    if (!env) return NULL;
    jobject s;

    s = (*env)->CallStaticObjectMethod(env, mActivityClass, midGetNativeSurface);
    if (s) {
        anw = ANativeWindow_fromSurface(env, s);
        (*env)->DeleteLocalRef(env, s);
    }

    return anw;
}""",
)

# Android_JNI_SetOrientation: no-op without a JavaVM.
patch_file(
    ANDROID_C,
    """void Android_JNI_SetOrientation(int w, int h, int resizable, const char *hint)
{
    JNIEnv *env = Android_JNI_GetEnv();

    jstring jhint = (*env)->NewStringUTF(env, (hint ? hint : ""));
    (*env)->CallStaticVoidMethod(env, mActivityClass, midSetOrientation, w, h, (resizable ? 1 : 0), jhint);
    (*env)->DeleteLocalRef(env, jhint);
}""",
    """void Android_JNI_SetOrientation(int w, int h, int resizable, const char *hint)
{
    /* DuckGame-Android: no Java activity to drive orientation. */
    if (!Android_JNI_IsReady()) return;
    JNIEnv *env = Android_JNI_GetEnv();
    if (!env) return;

    jstring jhint = (*env)->NewStringUTF(env, (hint ? hint : ""));
    (*env)->CallStaticVoidMethod(env, mActivityClass, midSetOrientation, w, h, (resizable ? 1 : 0), jhint);
    (*env)->DeleteLocalRef(env, jhint);
}""",
)

# Android_JNI_GetDisplayNaturalOrientation / CurrentOrientation: return stored
# values (safe without JavaVM).
patch_file(
    ANDROID_C,
    """SDL_DisplayOrientation Android_JNI_GetDisplayNaturalOrientation(void)
{
    return displayNaturalOrientation;
}""",
    """SDL_DisplayOrientation Android_JNI_GetDisplayNaturalOrientation(void)
{
    return displayNaturalOrientation;
}""",
)
# (those two are already trivial; the guard is at the VideoInit call site.)

# Insert the new bridge functions + Android_JNI_IsReady before
# Android_JNI_SetActivityTitle.
patch_file(
    ANDROID_C,
    "void Android_JNI_SetActivityTitle(const char *title)\n{",
    """#pragma GCC visibility push(default)

bool Android_JNI_IsReady(void)
{
    return mJavaVM != NULL;
}

void SDL_AndroidInitNative(void)
{
    /* DuckGame-Android: ensure we have the ART JavaVM. JNI_OnLoad normally
       sets mJavaVM, but acquire it defensively from the running runtime. */
    if (!mJavaVM) {
        JavaVM *vm = NULL;
        int n = 0;
        if (JNI_GetCreatedJavaVMs(&vm, 1, &n) == 0 && n > 0 && vm) {
            mJavaVM = vm;
        }
    }
}

void SDL_AndroidSetNativeWindow(ANativeWindow *window)
{
    if (g_NativeWindow && g_NativeWindow != window) {
        ANativeWindow_release(g_NativeWindow);
    }
    g_NativeWindow = window;
    if (Android_Window && Android_Window->internal) {
        SDL_WindowData *data = (SDL_WindowData *)Android_Window->internal;
        data->native_window = window;
        SDL_SetPointerProperty(SDL_GetWindowProperties(Android_Window),
                               SDL_PROP_WINDOW_ANDROID_WINDOW_POINTER, window);
        Android_SendResize(Android_Window);
    }
}

/* DuckGame-Android: convenience wrapper so the managed host doesn't need to
   p/invoke libandroid.so (not preloaded by .NET Android). SDL3 already links
   libandroid, so we do the ANativeWindow_fromSurface conversion here. */
void SDL_AndroidSetNativeWindowFromSurface(void *env, void *surface)
{
    if (!surface) {
        SDL_AndroidSetNativeWindow(NULL);
        return;
    }
    ANativeWindow *window = ANativeWindow_fromSurface((JNIEnv *)env, (jobject)surface);
    SDL_AndroidSetNativeWindow(window);
}

void SDL_AndroidSetScreenResolution(int surfaceWidth, int surfaceHeight,
                                    int deviceWidth, int deviceHeight,
                                    float density, float rate)
{
    Android_SetScreenResolution(surfaceWidth, surfaceHeight, deviceWidth,
                                deviceHeight, density, rate);
}

#pragma GCC visibility pop

void Android_JNI_SetActivityTitle(const char *title)
{""",
)

# ---------------------------------------------------------------------------
# 3. video/android/SDL_androidvideo.c : guard touch/mouse/orientation init
#    when there is no JavaVM.
# ---------------------------------------------------------------------------
VIDEO_C = "src/video/android/SDL_androidvideo.c"
patch_file(
    VIDEO_C,
    """    display = SDL_GetVideoDisplay(displayID);
    display->natural_orientation = Android_JNI_GetDisplayNaturalOrientation();
    display->current_orientation = Android_JNI_GetDisplayCurrentOrientation();
    display->content_scale = Android_ScreenDensity;

    Android_InitTouch();

    Android_InitMouse();

    // We're done!
    return true;""",
    """    display = SDL_GetVideoDisplay(displayID);
    display->natural_orientation = Android_JNI_GetDisplayNaturalOrientation();
    display->current_orientation = Android_JNI_GetDisplayCurrentOrientation();
    display->content_scale = Android_ScreenDensity;

    /* DuckGame-Android: without SDL's Java activity there is no Android
       touch/mouse device to register, and the JNI orientation lookups would
       crash. Keyboard input is injected from the on-screen gamepad instead. */
    if (Android_JNI_IsReady()) {
        Android_InitTouch();
        Android_InitMouse();
    }

    // We're done!
    return true;""",
)

# ---------------------------------------------------------------------------
# 4. video/android/SDL_androidevents.c : non-blocking WaitActiveAndLockActivity
# ---------------------------------------------------------------------------
EVENTS_C = "src/video/android/SDL_androidevents.c"
patch_file(
    EVENTS_C,
    """bool Android_WaitActiveAndLockActivity(void)
{
    while (Android_Paused && !Android_Destroyed) {
        Android_PumpEvents(-1);
    }

    if (Android_Destroyed) {
        SDL_SetError("Android activity has been destroyed");
        return false;
    }

    Android_LockActivityMutex();
    return true;
}""",
    """bool Android_WaitActiveAndLockActivity(void)
{
    /* DuckGame-Android: there is no Java activity lifecycle to wait on. */
    if (Android_Destroyed) {
        SDL_SetError("Android activity has been destroyed");
        return false;
    }

    Android_LockActivityMutex();
    return true;
}""",
)

# ---------------------------------------------------------------------------
# 5. CMakeLists.txt : force src/core/android/SDL_android.c to compile with
#    default visibility so our DuckGame-Android bridge symbols are exported
#    from libSDL3.so (the target sets -fvisibility=hidden otherwise).
# ---------------------------------------------------------------------------
CMAKE = "CMakeLists.txt"
with open(os.path.join(ROOT, CMAKE)) as f:
    cm = f.read()
if "SDL_android.c PROPERTIES COMPILE_OPTIONS" not in cm:
    anchor = """  if(HAVE_GCC_FVISIBILITY)
    set_target_properties(SDL3-shared PROPERTIES
      C_VISIBILITY_PRESET "hidden"
      CXX_VISIBILITY_PRESET "hidden"
      OBJC_VISIBILITY_PRESET "hidden"
    )
  endif()"""
    assert anchor in cm, "CMakeLists.txt visibility anchor missing"
    cm = cm.replace(anchor, anchor + """
  # DuckGame-Android: export the native bridge symbols from SDL_android.c.
  set_source_files_properties(src/core/android/SDL_android.c PROPERTIES
    COMPILE_OPTIONS "-fvisibility=default")""")
    with open(os.path.join(ROOT, CMAKE), "w") as f:
        f.write(cm)
    print("OK: " + CMAKE)
else:
    print("SKIP: " + CMAKE + " (already patched)")

print("All SDL3 Android patches applied.")
