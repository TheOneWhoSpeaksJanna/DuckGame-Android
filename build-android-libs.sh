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
      -DCMAKE_BUILD_TYPE=Release $extra "$src" \
      && cmake --build . -j"$(nproc 2>/dev/null || echo 4)" )
  find "$b" -name "*.so" -exec cp {} "$OUT/" \;
}

# ---- SDL2 (official source) ----
SDL_TAG="release-2.28.5"
echo "=== cloning SDL2 $SDL_TAG ==="
git clone --depth 1 --branch "$SDL_TAG" https://github.com/libsdl-org/SDL.git /tmp/SDL2
build_cmake sdl2 /tmp/SDL2 "-DSDL_STATIC=OFF -DSDL_SHARED=ON -DANDROID=ON"

# ---- FAudio ----
build_cmake faudio "$FNA/FAudio" "-DBUILD_TESTS=OFF -DBUILD_UTILS=OFF -DBUILD_EXAMPLES=OFF"

# ---- FNA3D (includes MojoShader) ----
build_cmake fna3d "$FNA/FNA3D" "-DBUILD_TESTS=OFF"

# ---- Theorafile ----
build_cmake theorafile "$FNA/Theorafile" "-DBUILD_TESTS=OFF"

echo "=== produced libs ==="
ls -la "$OUT"
# sanity: ensure the core libs exist
for lib in libSDL2.so libFAudio.so libFNA3D.so libtheorafile.so; do
  [ -f "$OUT/$lib" ] || { echo "MISSING $lib"; exit 1; }
done
echo "ALL NATIVE LIBS PRESENT"
