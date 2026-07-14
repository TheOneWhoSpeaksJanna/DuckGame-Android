using System;
using System.Runtime.InteropServices;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;

namespace DuckGame.Android
{
    /// <summary>
    /// A normal Android View that displays the game frame by drawing a Bitmap on
    /// a Canvas. redroid's software compositor cannot present the SDL
    /// SurfaceView hardware layer (it shows black / "SOLID_COLOR"), but it CAN
    /// composite a Canvas-drawn View. So on redroid we render the game normally
    /// into the SurfaceView (FNA needs it as the EGL target) and, each frame,
    /// copy the backbuffer (captured natively in the patched SDL3) onto this
    /// View. On real devices this View is never shown and the SurfaceView is the
    /// only output, so behaviour is unchanged there.
    /// </summary>
    public class BlitView : View
    {
        private Bitmap _bmp;
        private readonly object _lock = new object();
        private int _w, _h;
        private byte[] _rgba; // top-down RGBA managed copy

        public BlitView(Context context) : base(context) { }

        /// <summary>
        /// Called from the blit thread with the captured, bottom-up RGBA buffer.
        /// We flip rows to top-down and store a Bitmap for OnDraw.
        /// </summary>
        public void PushFrame(int w, int h, IntPtr nativePixels)
        {
            if (w <= 0 || h <= 0 || nativePixels == IntPtr.Zero) return;
            byte[] src;
            lock (_lock)
            {
                if (_rgba == null || _rgba.Length != w * h * 4)
                    _rgba = new byte[w * h * 4];
                src = _rgba;
                // copy native -> managed
                Marshal.Copy(nativePixels, src, 0, w * h * 4);
            }
            // Flip vertically (GL is bottom-up) and build an ARGB Bitmap.
            int stride = w * 4;
            int[] argb = new int[w * h];
            for (int y = 0; y < h; y++)
            {
                int srcRow = (h - 1 - y) * stride;
                int dstRow = y * w;
                for (int x = 0; x < w; x++)
                {
                    int i = srcRow + x * 4;
                    byte r = src[i], g = src[i + 1], b = src[i + 2], a = src[i + 3];
                    argb[dstRow + x] = (a << 24) | (r << 16) | (g << 8) | b;
                }
            }
            Bitmap bmp;
            lock (_lock)
            {
                if (_bmp == null || _bmp.Width != w || _bmp.Height != h)
                {
                    _bmp?.Recycle();
                    _bmp = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
                }
                bmp = _bmp;
            }
            bmp.SetPixels(argb, 0, w, 0, 0, w, h);
            PostInvalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            Bitmap bmp;
            lock (_lock) bmp = _bmp;
            if (bmp != null && !bmp.IsRecycled)
            {
                // Scale the frame to the view size (handles letterbox/orientation).
                var rect = new Rect(0, 0, canvas.Width, canvas.Height);
                canvas.DrawBitmap(bmp, null, rect, null);
            }
            else
            {
                canvas.DrawColor(global::Android.Graphics.Color.Black);
            }
        }
    }
}
