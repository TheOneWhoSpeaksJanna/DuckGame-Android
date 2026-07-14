# Duck Game — Android Port

An **unofficial** port of [TheFlyingFoool/DuckGameRebuilt](https://github.com/TheFlyingFoool/DuckGameRebuilt)
(the open-source Duck Game rebuild) to Android, built with **FNA** + **.NET 8 (net8.0-android)**
and **MonoMod/XnaToFna** for the XNA→FNA compatibility layer.

The game's original C# source is compiled **byte-for-byte, unchanged**. Only Android-specific
launcher/input files and a small set of compile-time shims (for Windows-only APIs that the game
references but does not exercise on a phone) were added. No game logic was recreated or hardcoded.

## What's in here
- `DuckGame/src/`, `DuckGame/AddedContent/`, `DuckGame/Recorderator/` — the unmodified game source.
- `DuckGame/Android/` — Android host: `MainActivity`, `SdlKeyboardInjector` (injects real SDL3
  keyboard events), `TouchGamepadView` (on-screen gamepad overlay).
- `DuckGame/Steam/` — the game's own Steam wrapper project (global-namespace `Lobby`/`User`/`Steam`).
- `DuckGame/lib/` — the original local DLLs the game was built against (forked Mono.Cecil 0.10,
  MonoMod, NAudio, NVorbis.NAudioSupport, 0Harmony, DGInput, DiscordRPC, Microsoft.Bcl.HashCode).
- `FNA/` — submodule, the [DGR-FNA fork](https://github.com/TheFlyingFoool/DGR-FNA) (SDL3-based).
- `build-android-libs.sh` — compiles the four FNA native libs (SDL3, FNA3D, FAudio, theorafile)
  from source for `arm64-v8a` with the Android NDK (the upstream fnalibs host is dead).

## Touch controls
The on-screen pad injects **real SDL3 keyboard events** into FNA's input queue. The unmodified game
reads them through `Keyboard.GetState()`, so it behaves exactly as with a physical keyboard.

| Touch button | Key            |
|--------------|----------------|
| ▲ ▼ ◀ ▶      | Arrow keys     |
| X            | Jump           |
| Z            | Shoot / Grab  |
| C            | Grab / Use    |
| ⏎ (top-right)| Enter (Start) |
| ESC          | Escape (Menu) |

## Build (CI only)
Android workloads cannot be installed on aarch64, so the real APK is produced by GitHub Actions
(`ubuntu-latest`, x64). The workflow:
1. Installs the `android` workload + Android SDK/NDK.
2. Builds the four FNA native libs from source via `build-android-libs.sh`.
3. `dotnet build` the APK (Release, signed).

Run it: `gh workflow run build.yml`. The signed APK is uploaded as the `DuckGame-android-apk`
artifact (`com.duckgame.game-Signed.apk`).

## Honest caveats
- **On-device runtime was not verified on a phone** in the build environment (no emulator/KVM).
  The APK is a real, signed, fully-linked artifact, but actual gameplay launch/feel is untested here.
- **Steam multiplayer, Discord Rich Presence, and Windows TTS** are compiled in but cannot function
  on Android (no Steam client / native Discord / Win32). Those code paths are platform-guarded and
  do not affect single-player offline play.
- A handful of **net8 API adaptations** were required (e.g. `StackTrace(Thread,bool)` →
  `StackTrace(bool)`, `Thread.Abort/Suspend/Resume` → no-op extension shims, `SHA256Cng` →
  `SHA256Managed` forwarder, `BinaryFormatter` obsolete suppressed). These are standard .NET port
  fixes and do not alter game behavior.
