#!/usr/bin/env python3
"""Patch SDL3 (release-3.2.4) Android driver so it works WITHOUT SDL's Java
SDLActivity glue. FNA calls SDL_Init(VIDEO) which selects the Android driver,
but that driver normally depends on SDL's Java activity (nativeSetupJNI,
getNativeSurface, onNativeSurfaceCreated, etc.) which a .NET Android app never
runs. This patch:

  1. Self-acquires the ART JavaVM in native C (JNI_GetCreatedJavaVMs) so the
     JNI helpers are functional if ever needed.
  2. Reads the ANativeWindow from a global we set from managed code
     (SDL_AndroidSetNativeWindow / ...FromSurface) instead of calling the
     absent Java getNativeSurface, and exposes SDL_AndroidInitNative /
     SDL_AndroidSetNativeWindow(FromSurface) / SDL_AndroidSetScreenResolution
     entry points with default visibility + 'used' so they survive
     -fvisibility=hidden and --gc-sections and are exported from libSDL3.so.
  3. Makes Android_JNI_SetOrientation / display-orientation lookups no-op when
     there is no JavaVM (we drive orientation ourselves).
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
#    guarded orientation helpers.
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

# Insert the new bridge functions + Android_JNI_IsReady before
# Android_JNI_SetActivityTitle.
BRIDGE = """bool Android_JNI_IsReady(void)
{
    return mJavaVM != NULL;
}

/* DuckGame-Android: 'used' keeps these symbols from being dropped by
   --gc-sections (no internal callers); 'visibility default' exports them
   from libSDL3.so despite the target's -fvisibility=hidden. */
__attribute__((visibility("default"), used))
void SDL_AndroidInitNative(void)
{
    /* mJavaVM is already captured by SDL3's JNI_OnLoad when .NET Android
       dlopen()s libSDL3.so, so there is nothing to do here. Kept as an
       explicit entry point the host can call before SDL_Init(VIDEO). */
}

__attribute__((visibility("default"), used))
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
__attribute__((visibility("default"), used))
void SDL_AndroidSetNativeWindowFromSurface(void *env, void *surface)
{
    if (!surface) {
        SDL_AndroidSetNativeWindow(NULL);
        return;
    }
    ANativeWindow *window = ANativeWindow_fromSurface((JNIEnv *)env, (jobject)surface);
    SDL_AndroidSetNativeWindow(window);
}

__attribute__((visibility("default"), used))
void SDL_AndroidSetScreenResolution(int surfaceWidth, int surfaceHeight,
                                    int deviceWidth, int deviceHeight,
                                    float density, float rate)
{
    Android_SetScreenResolution(surfaceWidth, surfaceHeight, deviceWidth,
                                deviceHeight, density, rate);
}

void Android_JNI_SetActivityTitle(const char *title)
{"""

patch_file(
    ANDROID_C,
    "void Android_JNI_SetActivityTitle(const char *title)\n{",
    BRIDGE,
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
# 6. src/dynapi/SDL_dynapi.sym : the version script restricts exported
#    symbols. Add our DuckGame-Android bridge symbols before the
#    "extra symbols" marker (otherwise they're hidden by `local: *`).
# ---------------------------------------------------------------------------
SYM = "src/dynapi/SDL_dynapi.sym"
with open(os.path.join(ROOT, SYM)) as f:
    sym = f.read()
if "SDL_AndroidSetNativeWindowFromSurface" not in sym:
    marker = "    # extra symbols go here (don't modify this line)\n"
    assert marker in sym, "SDL_dynapi.sym marker missing"
    extra = (
        "    SDL_AndroidInitNative;\n"
        "    SDL_AndroidSetNativeWindow;\n"
        "    SDL_AndroidSetNativeWindowFromSurface;\n"
        "    SDL_AndroidSetScreenResolution;\n"
        + marker
    )
    sym = sym.replace(marker, extra)
    with open(os.path.join(ROOT, SYM), "w") as f:
        f.write(sym)
    print("OK: " + SYM)
else:
    print("SKIP: " + SYM + " (already patched)")

print("All SDL3 Android patches applied.")
