#!/usr/bin/env python3
# DuckGame-Android: patch FNA3D's SDL3 GPU driver so it can hand the
# final rendered frame to managed code for the redroid readback-blit.
#
# The DGR-FNA fork presents via the SDL3 GPU driver (SDLGPU_SwapBuffers),
# which blits the faux-backbuffer texture to the swapchain and NEVER goes
# through SDL_GL_SwapWindow / the EGL backbuffer. redroid's software
# compositor can't present the SDL SurfaceView layer, so on redroid we
# download the faux-backbuffer texture to a CPU buffer here and let the
# managed BlitView mirror it onto a Canvas (which redroid DOES show).
#
# On real devices g_DuckGameCapture stays 0, so there is zero overhead
# and the native SurfaceView path is used unchanged.
#
# Diagnostics use __android_log_print (reaches logcat on Android), NOT
# SDL_Log (which is not routed to logcat in this SDL3 Android build).

import io, os, sys

ROOT = os.path.dirname(os.path.abspath(__file__))
SRC = os.path.join(ROOT, "..", "FNA", "lib", "FNA3D", "src", "FNA3D_Driver_SDL.c")
SRC = os.path.abspath(SRC)

def patch_file(path, old, new, count=1):
    s = io.open(path, encoding="utf-8").read()
    assert old in s, "anchor not found:\n" + repr(old[:120])
    if count <= 0:
        s = s.replace(old, new)  # replace all
    else:
        s = s.replace(old, new, count)
    io.open(path, "w", encoding="utf-8").write(s)

if not os.path.exists(SRC):
    print("SKIP: " + SRC + " (not present)")
    sys.exit(0)

# Make sure android/log.h is available for diagnostics in this TU.
HAVE_LOG = '#include <android/log.h>\n'
if HAVE_LOG not in io.open(SRC, encoding="utf-8").read():
    patch_file(SRC, "#include <SDL3/SDL.h>\n", "#include <SDL3/SDL.h>\n" + HAVE_LOG)

# 1. Globals + exported capture bridge, inserted just before SDLGPU_SwapBuffers.
GLOBALS = r'''/* DuckGame-Android: readback-blit shared capture buffer + bridge. */
#include <SDL3/SDL_gpu.h>
#include <android/log.h>
static unsigned char *g_DuckGamePixels = NULL;
static int g_DuckGameW = 0;
static int g_DuckGameH = 0;
static int g_DuckGameCapture = 0;

__attribute__((visibility("default"), used))
void DuckGame_SetCapture(int enabled)
{
    g_DuckGameCapture = enabled;
}

__attribute__((visibility("default"), used))
int DuckGame_LockPixels(int *w, int *h, unsigned char **pixels)
{
    if (!g_DuckGameCapture || !g_DuckGamePixels) {
        return 0;
    }
    *w = g_DuckGameW;
    *h = g_DuckGameH;
    *pixels = g_DuckGamePixels;
    return 1;
}

'''

patch_file(
    SRC,
    "static void SDLGPU_SwapBuffers(\n",
    GLOBALS + "static void SDLGPU_SwapBuffers(\n",
)

# 2. After the swapchain blit is flushed, download the faux-backbuffer
#    texture (the final rendered image) into the shared CPU buffer.
CAPTURE = r'''    /* DuckGame-Android: readback-blit capture of the final frame. */
    if (g_DuckGameCapture) {
        __android_log_print(ANDROID_LOG_INFO, "DuckGame", "SDLGPU_SwapBuffers capture reached; fb=%p", (void*)renderer->fauxBackbufferColorTexture);
        if (renderer->fauxBackbufferColorTexture != NULL) {
            int cw = (int) renderer->fauxBackbufferColorTexture->createInfo.width;
            int ch = (int) renderer->fauxBackbufferColorTexture->createInfo.height;
            if (cw > 0 && ch > 0) {
                size_t need = (size_t) cw * (size_t) ch * 4;
                if (!g_DuckGamePixels || g_DuckGameW != cw || g_DuckGameH != ch) {
                    if (g_DuckGamePixels) SDL_free(g_DuckGamePixels);
                    g_DuckGamePixels = (unsigned char *) SDL_malloc(need);
                    g_DuckGameW = cw;
                    g_DuckGameH = ch;
                    __android_log_print(ANDROID_LOG_INFO, "DuckGame", "capture alloc %dx%d (%zu bytes) ptr=%p", cw, ch, need, (void*)g_DuckGamePixels);
                }
                if (g_DuckGamePixels) {
                    SDL_GPUTransferBuffer *tb = SDL_CreateGPUTransferBuffer(
                        renderer->device,
                        &(SDL_GPUTransferBufferCreateInfo){
                            SDL_GPU_TRANSFERBUFFERUSAGE_DOWNLOAD,
                            (Uint32) need
                        }
                    );
                    __android_log_print(ANDROID_LOG_INFO, "DuckGame", "capture tb=%p", (void*)tb);
                    if (tb != NULL) {
                        SDL_GPUCommandBuffer *dl = SDL_AcquireGPUCommandBuffer(renderer->device);
                        if (dl != NULL) {
                            SDL_GPUCopyPass *cp = SDL_BeginGPUCopyPass(dl);
                            SDL_GPUTextureRegion reg = {
                                renderer->fauxBackbufferColorTexture->texture,
                                0, 0, 0, 0, 0, (Uint32) cw, (Uint32) ch, 1
                            };
                            SDL_GPUTextureTransferInfo ti = { tb, 0, (Uint32) cw, (Uint32) ch };
                            SDL_DownloadFromGPUTexture(cp, &reg, &ti);
                            SDL_EndGPUCopyPass(cp);
                            SDL_SubmitGPUCommandBuffer(dl);
                            __android_log_print(ANDROID_LOG_INFO, "DuckGame", "capture download submitted");
                        }
                        SDL_WaitForGPUIdle(renderer->device);
                        void *mapped = SDL_MapGPUTransferBuffer(renderer->device, tb, false);
                        __android_log_print(ANDROID_LOG_INFO, "DuckGame", "capture mapped=%p", mapped);
                        if (mapped != NULL) {
                            SDL_memcpy(g_DuckGamePixels, mapped, need);
                            SDL_UnmapGPUTransferBuffer(renderer->device, tb);
                        }
                        SDL_ReleaseGPUTransferBuffer(renderer->device, tb);
                    }
                }
            }
        }
    }

'''

patch_file(
    SRC,
    "\tSDLGPU_INTERNAL_FlushCommands(renderer);\n",
    "\tSDLGPU_INTERNAL_FlushCommands(renderer);\n" + CAPTURE,
)

# 3. Probe the FNA3D dispatchers (FNA3D.c) to confirm the game actually
#    initializes FNA3D and requests presents. Use __android_log_print.
DISP = os.path.join(ROOT, "..", "FNA", "lib", "FNA3D", "src", "FNA3D.c")
DISP = os.path.abspath(DISP)
if os.path.exists(DISP):
    DLOG = '#include <android/log.h>\n'
    if DLOG not in io.open(DISP, encoding="utf-8").read():
        patch_file(DISP, "#include <SDL3/SDL.h>\n", "#include <SDL3/SDL.h>\n" + DLOG)
    # FNA3D_PrepareWindowAttributes probe (called by GraphicsDevice ctor).
    patch_file(
        DISP,
        "uint32_t FNA3D_PrepareWindowAttributes(void)\n{\n",
        "uint32_t FNA3D_PrepareWindowAttributes(void)\n{\n"
        "\t__android_log_print(ANDROID_LOG_INFO, \"DuckGame\", \"FNA3D_PrepareWindowAttributes called\");\n",
    )
    # FNA3D_SwapBuffers dispatcher probe.
    patch_file(
        DISP,
        "\tTRACE_SWAPBUFFERS\n",
        "\tTRACE_SWAPBUFFERS\n"
        "\t__android_log_print(ANDROID_LOG_INFO, \"DuckGame\", \"FNA3D_SwapBuffers called\");\n",
    )
    # FNA3D_CreateDevice dispatcher probe.
    patch_file(
        DISP,
        "\tTRACE_CREATEDEVICE\n",
        "\tTRACE_CREATEDEVICE\n"
        "\t__android_log_print(ANDROID_LOG_INFO, \"DuckGame\", \"FNA3D_CreateDevice called; selectedDriver=%d\", selectedDriver);\n",
    )
    # GPU device creation success probe in the SDL driver.
    patch_file(
        SRC,
        "\trenderer->device = device;\n",
        "\trenderer->device = device;\n"
        "\t__android_log_print(ANDROID_LOG_INFO, \"DuckGame\", \"SDLGPU device created OK\");\n",
    )
    print("OK: " + DISP + " + device probe")
else:
    print("SKIP: " + DISP + " (not present)")

print("OK: " + SRC)

# 4. OpenGL driver readback capture (used on redroid, where the Vulkan HAL
#    segfaults during device init and we force FNA3D_FORCE_DRIVER=OpenGL).
#    The SDLGPU capture (section 1-2) is inert in that case, so mirror the
#    same bridge here and grab the final frame via glReadPixels from the
#    real backbuffer FBO before the GL swap.
OPENGL_SRC = os.path.join(ROOT, "..", "FNA", "lib", "FNA3D", "src", "FNA3D_Driver_OpenGL.c")
OPENGL_SRC = os.path.abspath(OPENGL_SRC)
if os.path.exists(OPENGL_SRC):
    ohead = io.open(OPENGL_SRC, encoding="utf-8").read()
    if "#include <android/log.h>" not in ohead:
        patch_file(OPENGL_SRC, "#include <SDL3/SDL.h>\n", "#include <SDL3/SDL.h>\n#include <android/log.h>\n")

    OPENGL_GLOBALS = (
        "/* DuckGame-Android: readback-blit shared capture buffer + bridge "
        "(OpenGL driver). */\n"
        "static unsigned char *g_DuckGamePixels = NULL;\n"
        "static int g_DuckGameW = 0;\n"
        "static int g_DuckGameH = 0;\n"
        "static int g_DuckGameCapture = 0;\n\n"
        "__attribute__((visibility(\"default\"), used))\n"
        "void DuckGame_SetCapture(int enabled)\n"
        "{\n"
        "    g_DuckGameCapture = enabled;\n"
        "}\n\n"
        "__attribute__((visibility(\"default\"), used))\n"
        "int DuckGame_LockPixels(int *w, int *h, unsigned char **pixels)\n"
        "{\n"
        "    if (!g_DuckGameCapture || !g_DuckGamePixels) {\n"
        "        return 0;\n"
        "    }\n"
        "    *w = g_DuckGameW;\n"
        "    *h = g_DuckGameH;\n"
        "    *pixels = g_DuckGamePixels;\n"
        "    return 1;\n"
        "}\n\n"
    )
    # Insert the globals just before the first OPENGL_ function (OPENGL_SwapBuffers).
    patch_file(
        OPENGL_SRC,
        "static void OPENGL_SwapBuffers(\n",
        OPENGL_GLOBALS + "static void OPENGL_SwapBuffers(\n",
    )

    # Capture the final frame from the real backbuffer FBO right before each
    # GL swap inside OPENGL_SwapBuffers (the image lives there after the
    # blit-to-backbuffer step). We anchor on the unique trailing context of
    # the two SDL_GL_SwapWindow calls inside OPENGL_SwapBuffers so we don't
    # touch the other SDL_GL_SwapWindow callers elsewhere in the file.
    OPENGL_CAPTURE = (
        "    /* DuckGame-Android: readback-blit capture of the final frame (OpenGL). */\n"
        "    if (g_DuckGameCapture) {\n"
        "        int cw = renderer->backbuffer->width;\n"
        "        int ch = renderer->backbuffer->height;\n"
        "        if (cw > 0 && ch > 0) {\n"
        "            size_t need = (size_t) cw * (size_t) ch * 4;\n"
        "            if (!g_DuckGamePixels || g_DuckGameW != cw || g_DuckGameH != ch) {\n"
        "                if (g_DuckGamePixels) SDL_free(g_DuckGamePixels);\n"
        "                g_DuckGamePixels = (unsigned char*) SDL_malloc(need);\n"
        "                g_DuckGameW = cw; g_DuckGameH = ch;\n"
        "            }\n"
        "            if (g_DuckGamePixels) {\n"
        "                renderer->glBindFramebuffer(GL_READ_FRAMEBUFFER, renderer->realBackbufferFBO);\n"
        "                renderer->glReadPixels(0, 0, cw, ch, GL_RGBA, GL_UNSIGNED_BYTE, g_DuckGamePixels);\n"
        "                renderer->currentReadFramebuffer = renderer->realBackbufferFBO;\n"
        "            }\n"
        "        }\n"
        "    }\n"
    )
    patch_file(
        OPENGL_SRC,
        "\tSDL_GL_SwapWindow((SDL_Window*) overrideWindowHandle);\n\n"
        "\t\tBindFramebuffer(renderer, renderer->backbuffer->opengl.handle);\n",
        OPENGL_CAPTURE
        + "\tSDL_GL_SwapWindow((SDL_Window*) overrideWindowHandle);\n\n"
        "\t\tBindFramebuffer(renderer, renderer->backbuffer->opengl.handle);\n",
    )
    patch_file(
        OPENGL_SRC,
        "\tSDL_GL_SwapWindow((SDL_Window*) overrideWindowHandle);\n\t}\n\n"
        "\t/* Run any threaded commands */\n",
        OPENGL_CAPTURE
        + "\tSDL_GL_SwapWindow((SDL_Window*) overrideWindowHandle);\n\t}\n\n"
        "\t/* Run any threaded commands */\n",
    )
    print("OK: " + OPENGL_SRC)
else:
    print("SKIP: " + OPENGL_SRC + " (not present)")

