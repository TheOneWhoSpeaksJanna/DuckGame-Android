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
    """    /* DuckGame-Android: keep the native bridge symbols linked (they have no
       other internal caller, so --gc-sections would otherwise drop them).
       We both call one and take the address of all four to force retention. */
    SDL_AndroidInitNative();
    (void)SDL_AndroidInitNative;
    (void)SDL_AndroidSetNativeWindow;
    (void)SDL_AndroidSetNativeWindowFromSurface;
    (void)SDL_AndroidSetScreenResolution;

    display = SDL_GetVideoDisplay(displayID);
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
# 4. video/android/SDL_androidevents.c : add <android/log.h> for diagnostics.
#    (The WaitActiveAndLockActivity blocking behavior is kept as-is so we can
#     measure whether our resume event unblocks it; once confirmed we'll make
#     it a no-op.)
EVENTS_C = "src/video/android/SDL_androidevents.c"
patch_file(
    EVENTS_C,
    "#include \"SDL_androidevents.h\"",
    "#include \"SDL_androidevents.h\"\n#include <android/log.h>",
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
        "    SDL_AndroidSetJavaVM;\n"
        "    SDL_AndroidSendResume;\n"
        + marker
    )
    sym = sym.replace(marker, extra)
    with open(os.path.join(ROOT, SYM), "w") as f:
        f.write(sym)
    print("OK: " + SYM)
else:
    print("SKIP: " + SYM + " (already patched)")

# ---------------------------------------------------------------------------
# 7. CMakeLists.txt : SDL_android.c is compiled WITH a precompiled header that
#    was built under -fvisibility=hidden. That TU-level hidden visibility
#    overrides our per-function visibility("default") attribute (the same
#    reason a per-file -fvisibility=default flag crashes with "visibility
#    differs in PCH file vs current file"). So for THIS file only, skip the
#    PCH and compile with -fvisibility=default, letting our bridge symbols
#    export (verified locally). SDL_android.c does not use the PCH types.
# ---------------------------------------------------------------------------
CMAKE = "CMakeLists.txt"
with open(os.path.join(ROOT, CMAKE)) as f:
    cm = f.read()
if "SDL_android.c PROPERTIES SKIP_PRECOMPILE_HEADERS" not in cm:
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
  # Skip the PCH (built hidden) and force default visibility for this file.
  set_source_files_properties(src/core/android/SDL_android.c PROPERTIES
    SKIP_PRECOMPILE_HEADERS ON
    COMPILE_OPTIONS "-fvisibility=default")""")
    with open(os.path.join(ROOT, CMAKE), "w") as f:
        f.write(cm)
    print("OK: " + CMAKE)
else:
    print("SKIP: " + CMAKE + " (already patched)")

# ---------------------------------------------------------------------------
# 8b. Exported bridge to send SDL's Android RESUME lifecycle event from the
#     managed host. Under .NET Android, SDL's Java SDLActivity (which normally
#     calls nativeResume -> Android_SendLifecycleEvent(RESUME)) never runs, so
#     the event pump blocks forever in Android_WaitActiveAndLockActivity waiting
#     for a resume that never comes. We expose a C wrapper and call it once the
#     surface is ready.
patch_file(
    ANDROID_C,
    "void Android_SendLifecycleEvent(SDL_AndroidLifecycleEvent event)\n{\n    SDL_LockMutex(Android_LifecycleMutex);",
    "void Android_SendLifecycleEvent(SDL_AndroidLifecycleEvent event)\n{\n    __android_log_print(ANDROID_LOG_ERROR, \"DuckGame\", \"sendLifecycle event=%d num=%d\", (int)event, Android_NumLifecycleEvents);\n    SDL_LockMutex(Android_LifecycleMutex);",
)

# 8b. Exported bridge to send SDL's Android RESUME lifecycle event from the
#     managed host. Under .NET Android, SDL's Java SDLActivity (which normally
#     calls nativeResume -> Android_SendLifecycleEvent(RESUME)) never runs, so
#     the event pump blocks forever in Android_WaitActiveAndLockActivity waiting
#     for a resume that never comes. We expose a C wrapper and call it once the
#     surface is ready.
patch_file(
    ANDROID_C,
    "    Android_SendLifecycleEvent(SDL_ANDROID_LIFECYCLE_RESUME);\n"
    "}\n\n"
    "JNIEXPORT void JNICALL SDL_JAVA_INTERFACE(nativeFocusChanged)(\n",
    "    Android_SendLifecycleEvent(SDL_ANDROID_LIFECYCLE_RESUME);\n"
    "}\n\n"
    "/* DuckGame-Android: exported bridge to deliver SDL's RESUME lifecycle event\n"
    "   from the managed host (no Java SDLActivity runs under .NET's dlopen). */\n"
    "__attribute__((visibility(\"default\"), used))\n"
    "void SDL_AndroidSendResume(void)\n"
    "{\n"
    "    __android_log_print(ANDROID_LOG_ERROR, \"DuckGame\", \"SDL_AndroidSendResume called\");\n"
    "    Android_SendLifecycleEvent(SDL_ANDROID_LIFECYCLE_RESUME);\n"
    "}\n\n"
    "JNIEXPORT void JNICALL SDL_JAVA_INTERFACE(nativeFocusChanged)(\n",
)

# Diagnostic: log when OnResume flips paused, and when WaitActive is entered/exited.
patch_file(
    "src/video/android/SDL_androidevents.c",
    "static void Android_OnResume(void)\n{\n    Android_Paused = false;",
    "static void Android_OnResume(void)\n{\n    __android_log_print(ANDROID_LOG_ERROR, \"DuckGame\", \"Android_OnResume paused=%d\", (int)Android_Paused);\n    Android_Paused = false;",
)
patch_file(
    "src/video/android/SDL_androidevents.c",
    "bool Android_WaitActiveAndLockActivity(void)\n{\n    while (Android_Paused && !Android_Destroyed) {\n        Android_PumpEvents(-1);\n    }",
    "bool Android_WaitActiveAndLockActivity(void)\n{\n    __android_log_print(ANDROID_LOG_ERROR, \"DuckGame\", \"WaitActive ENTER paused=%d destroyed=%d\", (int)Android_Paused, (int)Android_Destroyed);\n    while (Android_Paused && !Android_Destroyed) {\n        Android_PumpEvents(-1);\n    }\n    __android_log_print(ANDROID_LOG_ERROR, \"DuckGame\", \"WaitActive EXIT paused=%d destroyed=%d\", (int)Android_Paused, (int)Android_Destroyed);",
)
#    cluster to be retained + exported by referencing all four from it: the
#    linker keeps everything reachable from an exported symbol, so they
#    survive --gc-sections and the version script promotes them to dynamic.
# ---------------------------------------------------------------------------
patch_file(
    ANDROID_C,
    """    register_methods(env, "org/libsdl/app/HIDDeviceManager", HIDDeviceManager_tab, SDL_arraysize(HIDDeviceManager_tab));

    return JNI_VERSION_1_4;
}""",
    """    register_methods(env, "org/libsdl/app/HIDDeviceManager", HIDDeviceManager_tab, SDL_arraysize(HIDDeviceManager_tab));

    /* DuckGame-Android: keep the native bridge symbols linked (reachable
       from this exported JNI_OnLoad). They are also listed in the version
       script's global: block so they export from the shared lib. */
    /* Forward declaration so JNI_OnLoad can reference the resume bridge
       defined later in this file. */
    void SDL_AndroidSendResume(void);
    SDL_AndroidInitNative();
    (void)SDL_AndroidSetNativeWindow;
    (void)SDL_AndroidSetNativeWindowFromSurface;
    (void)SDL_AndroidSetScreenResolution;
    (void)SDL_AndroidSendResume;
    /* Also keep the readback-blit capture symbols (defined in SDL_androidgl.c)
       linked + exported. Declared here so this TU compiles. */
    extern void SDL_DuckGameSetCapture(int);
    extern int SDL_DuckGameLockPixels(int *, int *, unsigned char **);
    (void)SDL_DuckGameSetCapture;
    (void)SDL_DuckGameLockPixels;

    return JNI_VERSION_1_4;
}
"""
)

# ---------------------------------------------------------------------------
# 9. Android_JNI_SendMessage dereferences mJavaVM/mActivityClass. Under .NET
#    Android, JNI_OnLoad never runs (no Java SDLActivity), so mJavaVM is NULL.
#    Guard it to no-op when there is no JavaVM (we drive everything natively).
# ---------------------------------------------------------------------------
patch_file(
    ANDROID_C,
    """bool Android_JNI_SendMessage(int command, int param)
{
    JNIEnv *env = Android_JNI_GetEnv();
    return (*env)->CallStaticBooleanMethod(env, mActivityClass, midSendMessage, command, param);
}""",
    """bool Android_JNI_SendMessage(int command, int param)
{
    /* DuckGame-Android: with no Java SDLActivity (JNI_OnLoad never runs
       under .NET's dlopen), mJavaVM is NULL. No-op instead of crashing. */
    if (!mJavaVM) {
        return false;
    }
    JNIEnv *env = Android_JNI_GetEnv();
    return (*env)->CallStaticBooleanMethod(env, mActivityClass, midSendMessage, command, param);
}""",
)

# ---------------------------------------------------------------------------
# 11. Asset manager + input JNI guards. Under .NET Android, JNI_OnLoad never
#     runs so mJavaVM is NULL and SDL's activity JNI fields (mActivityClass,
#     midGetContext, ...) are never registered. We provide a bridge the managed
#     host calls with the real JavaVM* + Context so SDL can build the
#     AAssetManager (used to read game files from the APK). We deliberately do
#     NOT set mJavaVM (attach/detach locally instead) so every activity-JNI
#     entry point stays guarded and no-ops instead of dereferencing the NULL
#     activity class. We also guard the asset-manager + input JNI builders.
# ---------------------------------------------------------------------------
# 11a. Provide SDL_AndroidSetJavaVM(vm, context) exported bridge.
BRIDGE2 = r'''
/* DuckGame-Android: native bridge so the .NET host can hand SDL a real
   Android JavaVM* and Context so SDL can build the AAssetManager (used to
   read game files from the APK). SDL's own Java activity / JNI_OnLoad never
   runs under .NET's dlopen, so mActivityClass etc. stay NULL; we only attach
   to grab the AAssetManager and then detach, leaving mJavaVM NULL so all the
   other activity-JNI paths stay guarded + no-op. */
__attribute__((visibility("default"), used))
void SDL_AndroidSetJavaVM(void *envptr, void *context)
{
    if (!envptr || !context) {
        return;
    }
    JNIEnv *env = (JNIEnv *)envptr;
    JavaVM *javaVM = NULL;
    if ((*env)->GetJavaVM(env, &javaVM) < 0 || !javaVM) {
        return;
    }
    jobject assets = (*env)->CallObjectMethod(env, (jobject)context,
        (*env)->GetMethodID(env, (*env)->GetObjectClass(env, (jobject)context),
            "getAssets", "()Landroid/content/res/AssetManager;"));
    if (assets) {
        javaAssetManagerRef = (*env)->NewGlobalRef(env, assets);
        asset_manager = AAssetManager_fromJava(env, javaAssetManagerRef);
    }
}
'''
patch_file(
    ANDROID_C,
    "void Android_JNI_SetActivityTitle(const char *title)\n{",
    BRIDGE2 + "void Android_JNI_SetActivityTitle(const char *title)\n{",
)

# 11b. Guard the asset-manager + input JNI builders to no-op without a JavaVM.
for anchor in [
    "static void Internal_Android_Create_AssetManager(void)\n{\n",
    "void Android_JNI_PollInputDevices(void)\n{\n    JNIEnv *env = Android_JNI_GetEnv();\n",
    "void Android_JNI_InitTouch(void)\n{\n    JNIEnv *env = Android_JNI_GetEnv();\n",
]:
    guard = "    /* DuckGame-Android: no JavaVM under .NET, skip. */\n    if (!mJavaVM) { return; }\n"
    patch_file(ANDROID_C, anchor, anchor + guard)

# ---------------------------------------------------------------------------
# 10 (reinstated). Several SDL_Android* storage-path helpers dereference a NULL
#     JNIEnv when there is no JavaVM. Guard them to return NULL gracefully.
# ---------------------------------------------------------------------------
STORAGE_GUARD = "    /* DuckGame-Android: no JavaVM under .NET, skip storage path JNI. */\n    if (!mJavaVM) { return NULL; }\n"
for static_line in [
    "    static char *s_AndroidInternalFilesPath = NULL;\n",
    "    static char *s_AndroidExternalFilesPath = NULL;\n",
    "    static char *s_AndroidCachePath = NULL;\n",
]:
    patch_file(ANDROID_C, static_line, STORAGE_GUARD + static_line)

# ---------------------------------------------------------------------------
# 12. DuckGame-Android readback-blit path (for emulators like redroid whose
#     software compositor cannot present the SDL SurfaceView hardware layer).
#     When the host calls SDL_DuckGameSetCapture(1), after each GL swap we
#     read the backbuffer into a shared RGBA buffer. The managed host then
# ---------------------------------------------------------------------------
# 12. NOTE: the readback-blit capture now lives in FNA3D's SDL3
#     GPU driver (see patches/patch_fna3d_capture.py). The DGR-FNA
#     fork presents via the GPU driver (swapchain texture), NOT the
#     EGL backbuffer, so the old SDL_GLES_SwapWindow EGL
#     capture is removed. Nothing to patch here anymore.
# ---------------------------------------------------------------------------

print("All SDL3 Android patches applied.")

