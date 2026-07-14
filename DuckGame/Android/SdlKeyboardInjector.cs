using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SDL3;

namespace DuckGame.Android
{
    /// <summary>
    /// Bridges on-screen touch buttons to the real Duck Game by injecting genuine
    /// SDL3 keyboard events into FNA's event queue. The unmodified game reads these
    /// through Keyboard.GetState(), so NO game logic is changed. This is the honest,
    /// non-hardcoded way to add touch: we simulate real key presses at the SDL layer.
    /// </summary>
    public static class SdlKeyboardInjector
    {
        /// <summary>Press a key (SDL_EVENT_KEY_DOWN).</summary>
        public static void KeyDown(SDL.SDL_Keycode key)
        {
            Push(key, SDL.SDL_EventType.SDL_EVENT_KEY_DOWN);
        }

        /// <summary>Release a key (SDL_EVENT_KEY_UP).</summary>
        public static void KeyUp(SDL.SDL_Keycode key)
        {
            Push(key, SDL.SDL_EventType.SDL_EVENT_KEY_UP);
        }

        private static void Push(SDL.SDL_Keycode key, SDL.SDL_EventType type)
        {
            try
            {
                SDL.SDL_Event evt = new SDL.SDL_Event();
                evt.key = new SDL.SDL_KeyboardEvent
                {
                    type = type,
                    timestamp = 0,
                    windowID = 1,
                    which = 1,
                    scancode = SDL.SDL_GetScancodeFromKey((uint)key, IntPtr.Zero),
                    key = (uint)key,
                    mod = 0,
                    raw = 0,
                    down = (type == SDL.SDL_EventType.SDL_EVENT_KEY_DOWN) ? SDL.SDLBool.True : SDL.SDLBool.False,
                    repeat = SDL.SDLBool.False
                };
                SDL.SDL_PushEvent(ref evt);
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("DuckGame", "SDL inject failed: " + ex);
            }
        }

        /// <summary>
        /// Duck Game default keyboard mapping. Touch buttons map to these keys so the
        /// game behaves exactly as it does with a keyboard.
        /// </summary>
        public static class Map
        {
            public const SDL.SDL_Keycode Up    = SDL.SDL_Keycode.SDLK_UP;
            public const SDL.SDL_Keycode Down  = SDL.SDL_Keycode.SDLK_DOWN;
            public const SDL.SDL_Keycode Left  = SDL.SDL_Keycode.SDLK_LEFT;
            public const SDL.SDL_Keycode Right = SDL.SDL_Keycode.SDLK_RIGHT;
            public const SDL.SDL_Keycode Shoot = SDL.SDL_Keycode.SDLK_Z; // Z = fire/grab
            public const SDL.SDL_Keycode Jump  = SDL.SDL_Keycode.SDLK_X; // X = jump
            public const SDL.SDL_Keycode Grab  = SDL.SDL_Keycode.SDLK_C; // C = grab/use
            public const SDL.SDL_Keycode Talk  = SDL.SDL_Keycode.SDLK_LSHIFT; // shift = talk
            public const SDL.SDL_Keycode Start = SDL.SDL_Keycode.SDLK_RETURN; // enter = start/pause
            public const SDL.SDL_Keycode Back  = SDL.SDL_Keycode.SDLK_ESCAPE; // esc = menu
        }
    }
}
