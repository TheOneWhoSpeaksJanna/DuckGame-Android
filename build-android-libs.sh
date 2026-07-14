#!/usr/bin/env bash
# Build FNA's native libraries for Android arm64-v8a using the NDK.
# SDL2 is cloned from the official repo; FAudio/FNA3D/Theorafile/MojoShader
# come from the FNA submodules. Fully self-contained (no external lib host).
set -euo pipefail

API=21
ABI=arm64-v8a
NDK="$ANDROID_NDK_HOME"
TOOLCHAIN="$NDK/build/cmake/android.toolchain.cmake"
OUT="$1"
mkdir -p "$OUT"
FNA="$PWD/FNA/lib"

build_cmake() {
  local name="$1"; local src="$2"; local extra="$3"
  echo "=== building $name ==="
  local b="$src/build-android"
  rm -rf "$b"; mkdir -p "$b"
  ( cd "$b" && cmake -GNinja \
      -DCMAKE_TOOLCHAIN_FILE="$TOOLCHAIN" \
      -DANDROID_ABI="$ABI" -DANDROID_PLATFORM=android-$API \
      -DCMAKE_INCLUDE_PATH="$SDL3_INCLUDE_DIR" \
      -DCMAKE_BUILD_TYPE=Release $extra "$src" \
      && cmake --build . -j"$(nproc 2>/dev/null || echo 4)" )
  find "$b" -name "*.so" -exec cp {} "$OUT/" \;
}

# ---- SDL3 (the DGR-FNA fork targets SDL3) ----
SDL_TAG="release-3.2.4"
echo "=== cloning SDL3 $SDL_TAG ==="
rm -rf /tmp/SDL3
git clone --depth 1 --branch "$SDL_TAG" https://github.com/libsdl-org/SDL.git /tmp/SDL3
# DuckGame-Android: patch SDL3's Android driver so it works without SDL's Java
# SDLActivity glue (the .NET Android host supplies the native window directly).
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
if [ -f "$SCRIPT_DIR/patches/patch_sdl3_android.py" ]; then
  echo "=== patching SDL3 Android driver ==="
  ( cd /tmp/SDL3 && python3 "$SCRIPT_DIR/patches/patch_sdl3_android.py" )
fi
echo "=== version-script bridge symbols present? ==="
grep -c "SDL_AndroidSetNativeWindowFromSurface" /tmp/SDL3/src/dynapi/SDL_dynapi.sym
# Set SDL3 paths BEFORE first build_cmake call (it references SDL3_INCLUDE_DIR).
export SDL3_INCLUDE_DIR="/tmp/SDL3/include"
export SDL3_DIR="/tmp/SDL3/build-android/SDL3Config.cmake"
# Make SDL3 headers globally visible to ALL sub-builds (incl. MojoShader) by
# installing them into the NDK sysroot include dir.
SYSROOT_INC="$NDK/toolchains/llvm/prebuilt/linux-x86_64/sysroot/usr/include"
mkdir -p "$SYSROOT_INC/SDL3"
cp -rf /tmp/SDL3/include/SDL3/. "$SYSROOT_INC/SDL3/"
build_cmake sdl3 /tmp/SDL3 "-DSDL_STATIC=OFF -DSDL_SHARED=ON -DANDROID=ON"
# Use the SDL3 build dir's own libSDL3.so directly (it's copied to OUT by build_cmake too).
SDL3_REAL="$(find /tmp/SDL3/build-android -name 'libSDL3.so' | head -1)"
export SDL3_LIB="$SDL3_REAL"

# Verify the DuckGame-Android SDL bridge symbols are actually exported.
echo "=== verifying SDL Android bridge exports (lib=$SDL3_REAL) ==="
NM="$NDK/toolchains/llvm/prebuilt/linux-x86_64/bin/llvm-nm"
echo "--- dynamic (exported) symbols matching SDL_Android: ---"
"$NM" -D "$SDL3_REAL" 2>/dev/null | grep "SDL_Android" || echo "(none in dynamic table)"
echo "--- ALL (including hidden) symbols matching SDL_Android: ---"
"$NM" "$SDL3_REAL" 2>/dev/null | grep "SDL_Android" || echo "(none anywhere)"
COUNT="$("$NM" -D "$SDL3_REAL" 2>/dev/null | grep -c "SDL_Android")"
echo "exported SDL_Android symbol count = $COUNT"
if [ "$COUNT" -ge 4 ]; then
  echo "OK: all 4 bridge symbols exported"
else
  echo "FAIL: fewer than 4 bridge symbols exported"; exit 1
fi

echo "=== verifying FNA3D capture symbol exports ==="
FNA3D_SO="$(find "$OUT" -name 'libFNA3D.so' 2>/dev/null | head -1)"
echo "libFNA3D.so = $FNA3D_SO"
if [ -z "$FNA3D_SO" ]; then
  echo "FAIL: libFNA3D.so not found under $OUT"; exit 1
fi
"$NM" -D "$FNA3D_SO" 2>/dev/null | grep "DuckGame" || echo "(none in dynamic table)"
CAPCOUNT=$( "$NM" -D "$FNA3D_SO" 2>/dev/null | grep -c "DuckGame" )
echo "exported DuckGame symbol count = $CAPCOUNT"
if [ "$CAPCOUNT" -ge 2 ]; then
  echo "OK: capture symbols exported"
else
  echo "FAIL: capture symbols not exported"; exit 1
fi

# ---- FAudio ----
build_cmake faudio "$FNA/FAudio" "-DBUILD_TESTS=OFF -DBUILD_UTILS=OFF -DBUILD_EXAMPLES=OFF -DSDL3_DIR=$SDL3_DIR -DSDL3_INCLUDE_DIRS=$SDL3_INCLUDE_DIR -DSDL3_LIBRARIES=$SDL3_LIB"
# ---- FNA3D (includes MojoShader) ----
# FNA3D is built with BUILD_SDL3=ON so it links libSDL3.so and
# the SDL3 GPU driver (the DGR-FNA fork's only working present
# path) is available. We then PATCH the SDL3 GPU driver so it
# hands the final rendered frame to managed code (readback-blit),
# which mirrors it onto a Canvas on redroid (whose software
# compositor can't present the SDL SurfaceView layer). On real
# devices g_DuckGameCapture stays 0 and the native SurfaceView
# path is used unchanged. OpenGL is also available and works.
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
if [ -f "$SCRIPT_DIR/patches/patch_fna3d_capture.py" ]; then
  echo "=== patching FNA3D GPU driver (readback-blit capture) ==="
  python3 "$SCRIPT_DIR/patches/patch_fna3d_capture.py"
fi
build_cmake fna3d "$FNA/FNA3D" "-DBUILD_TESTS=OFF -DBUILD_SDL3=ON -DSDL3_DIR=$SDL3_DIR -DSDL3_INCLUDE_DIRS=$SDL3_INCLUDE_DIR -DSDL3_LIBRARIES=$SDL3_LIB"
# ---- Theorafile (Makefile-based; bundle ogg/theora/vorbis) ----
echo "=== building theorafile (Makefile) ==="
TF="$FNA/Theorafile"
NDK_CLANG="$NDK/toolchains/llvm/prebuilt/linux-x86_64/bin/clang"
( cd "$TF" && make clean && make CC="$NDK_CLANG --target=aarch64-none-linux-android$API --sysroot=$NDK/toolchains/llvm/prebuilt/linux-x86_64/sysroot -fPIC -O3" \
    CFLAGS="-fPIC -O3" LDFLAGS="-shared" lib )
cp -f "$TF/libtheorafile.so" "$OUT/"

echo "=== produced libs ==="
ls -la "$OUT"
# sanity: ensure the core libs exist
for lib in libSDL3.so libFAudio.so libFNA3D.so libtheorafile.so; do
  [ -f "$OUT/$lib" ] || { echo "MISSING $lib"; exit 1; }
done
echo "ALL NATIVE LIBS PRESENT"
