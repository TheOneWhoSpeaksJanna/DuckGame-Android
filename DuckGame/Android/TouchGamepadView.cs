using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using SDL3;

namespace DuckGame.Android
{
    /// <summary>
    /// Transparent on-screen gamepad drawn over the SDL surface. Touching a button
    /// injects a real SDL key via SdlKeyboardInjector, so Duck Game responds exactly
    /// as if a keyboard were pressed. Layout is computed from the screen size; no
    /// game behavior is hardcoded — only which physical key each button represents.
    /// </summary>
    public class TouchGamepadView : View
    {
        private class Pad
        {
            public RectF Bounds;
            public SDL.SDL_Keycode Key;
            public string Label;
            public bool Down;
        }

        private readonly List<Pad> _pads = new List<Pad>();
        private readonly Dictionary<int, Pad> _active = new Dictionary<int, Pad>();
        private Paint _paintFill, _paintStroke, _paintText;
        private int _w, _h;

        public TouchGamepadView(Context context) : base(context)
        {
            _paintFill = new Paint { Alpha = 90, Color = Color.Argb(90, 255, 255, 255) };
            _paintStroke = new Paint { Color = Color.White, StrokeWidth = 3, AntiAlias = true };
            _paintStroke.SetStyle(Paint.Style.Stroke);
            _paintText = new Paint { Color = Color.White, TextSize = 38, AntiAlias = true, TextAlign = Paint.Align.Center };
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            _w = w; _h = h;
            BuildLayout();
        }

        private void BuildLayout()
        {
            _pads.Clear();
            int d = (int)(Math.Min(_w, _h) * 0.16);
            int pad = d / 3;
            int baseY = _h - d * 2 - pad;
            int leftX = pad;
            // D-pad (left side)
            _pads.Add(new Pad { Bounds = new RectF(leftX, baseY - d, leftX + d, baseY), Key = SdlKeyboardInjector.Map.Up, Label = "▲" });
            _pads.Add(new Pad { Bounds = new RectF(leftX, baseY + d, leftX + d, baseY + 2 * d), Key = SdlKeyboardInjector.Map.Down, Label = "▼" });
            _pads.Add(new Pad { Bounds = new RectF(leftX - d, baseY, leftX, baseY + d), Key = SdlKeyboardInjector.Map.Left, Label = "◀" });
            _pads.Add(new Pad { Bounds = new RectF(leftX + d, baseY, leftX + 2 * d, baseY + d), Key = SdlKeyboardInjector.Map.Right, Label = "▶" });
            // Action buttons (right side)
            int rx = _w - d - pad;
            int by = baseY;
            _pads.Add(new Pad { Bounds = new RectF(rx, by - d, rx + d, by), Key = SdlKeyboardInjector.Map.Jump, Label = "X" });
            _pads.Add(new Pad { Bounds = new RectF(rx + d, by, rx + 2 * d, by + d), Key = SdlKeyboardInjector.Map.Shoot, Label = "Z" });
            _pads.Add(new Pad { Bounds = new RectF(rx - d, by, rx, by + d), Key = SdlKeyboardInjector.Map.Grab, Label = "C" });
            // Top-right: start / back
            int tx = _w - d - pad;
            int ty = pad;
            _pads.Add(new Pad { Bounds = new RectF(tx, ty, tx + d, ty + d), Key = SdlKeyboardInjector.Map.Start, Label = "⏎" });
            _pads.Add(new Pad { Bounds = new RectF(tx - d - pad, ty, tx - pad, ty + d), Key = SdlKeyboardInjector.Map.Back, Label = "ESC" });
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            foreach (var p in _pads)
            {
                _paintFill.Color = p.Down ? Color.Argb(160, 120, 200, 255) : Color.Argb(70, 255, 255, 255);
                canvas.DrawRoundRect(p.Bounds, 18, 18, _paintFill);
                canvas.DrawRoundRect(p.Bounds, 18, 18, _paintStroke);
                canvas.DrawText(p.Label, p.Bounds.CenterX(), p.Bounds.CenterY() + 12, _paintText);
            }
            PostInvalidateDelayed(16);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            int action = e.ActionMasked;
            int idx = e.ActionIndex;
            float x = e.GetX(idx), y = e.GetY(idx);
            int pid = e.GetPointerId(idx);

            switch (action)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    Press(x, y, pid);
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.PointerUp:
                    Release(pid);
                    break;
                case MotionEventActions.Move:
                    // update which pad each pointer is over
                    if (_active.TryGetValue(pid, out var cur))
                    {
                        if (!cur.Bounds.Contains(x, y))
                        {
                            SdlKeyboardInjector.KeyUp(cur.Key); cur.Down = false;
                            _active.Remove(pid);
                        }
                    }
                    foreach (var p in _pads)
                    {
                        if (p.Bounds.Contains(x, y) && !_active.ContainsKey(pid))
                        {
                            SdlKeyboardInjector.KeyDown(p.Key); p.Down = true;
                            _active[pid] = p; break;
                        }
                    }
                    break;
            }
            return true;
        }

        private void Press(float x, float y, int pid)
        {
            foreach (var p in _pads)
            {
                if (p.Bounds.Contains(x, y))
                {
                    SdlKeyboardInjector.KeyDown(p.Key); p.Down = true;
                    _active[pid] = p; return;
                }
            }
        }

        private void Release(int pid)
        {
            if (_active.TryGetValue(pid, out var p))
            {
                SdlKeyboardInjector.KeyUp(p.Key); p.Down = false;
                _active.Remove(pid);
            }
        }
    }
}
