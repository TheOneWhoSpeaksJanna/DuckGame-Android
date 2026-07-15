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

import io, os, sys

ROOT = os.path.dirname(os.path.abspath(__file__))
SRC = os.path.join(ROOT, "..", "FNA", "lib", "FNA3D", "src", "FNA3D_Driver_SDL.c")
SRC = os.path.abspath(SRC)

def patch_file(path, old, new):
    s = io.open(path, encoding="utf-8").read()
    assert old in s, "anchor not found:\n" + repr(old[:120])
    s = s.replace(old, new, 1)
    io.open(path, "w", encoding="utf-8").write(s)

if not os.path.exists(SRC):
    print("SKIP: " + SRC + " (not present)")
    sys.exit(0)

# 1. Globals + exported capture bridge, inserted just before SDLGPU_SwapBuffers.
GLOBALS = r'''/* DuckGame-Android: readback-blit shared capture buffer + bridge. */
#include <SDL3/SDL_gpu.h>
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
        SDL_Log(SDL_LOG_CATEGORY_APPLICATION, "DuckGame: SDLGPU_SwapBuffers capture reached; fb=%p", (void*)renderer->fauxBackbufferColorTexture);
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
                    SDL_Log(SDL_LOG_CATEGORY_APPLICATION, "DuckGame: capture alloc %dx%d (%zu bytes) ptr=%p", cw, ch, need, (void*)g_DuckGamePixels);
                }
                if (g_DuckGamePixels) {
                    SDL_GPUTransferBuffer *tb = SDL_CreateGPUTransferBuffer(
                        renderer->device,
                        &(SDL_GPUTransferBufferCreateInfo){
                            SDL_GPU_TRANSFERBUFFERUSAGE_DOWNLOAD,
                            (Uint32) need
                        }
                    );
                    SDL_Log(SDL_LOG_CATEGORY_APPLICATION, "DuckGame: capture tb=%p", (void*)tb);
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
                            SDL_Log(SDL_LOG_CATEGORY_APPLICATION, "DuckGame: capture download submitted");
                        }
                        SDL_WaitForGPUIdle(renderer->device);
                        void *mapped = SDL_MapGPUTransferBuffer(renderer->device, tb, false);
                        SDL_Log(SDL_LOG_CATEGORY_APPLICATION, "DuckGame: capture mapped=%p", mapped);
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

print("OK: " + SRC)
