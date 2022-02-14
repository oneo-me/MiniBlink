using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MiniBlink
{
    public sealed class OffscreenGraphics
    {
        readonly object _syncRoot = new();

        public void Render(DrawingContext drawingContext)
        {
            lock (_syncRoot)
                if (ViewPixels != null)
                {
                    var surface = GetSurface(ViewPixels);
                    drawingContext.DrawImage(surface, new Rect(0, 0, surface.Width, surface.Height));

                    var pixelBuffer = PopupPixels;
                    if (pixelBuffer == null)
                        return;

                    surface = GetSurface(pixelBuffer);
                    drawingContext.DrawImage(surface, new Rect(_popupBounds.X, _popupBounds.Y, surface.Width, surface.Height));
                }
        }

        public PixelBuffer PopupPixels { get; set; } = new PixelBuffer();
        public PixelBuffer ViewPixels { get; set; }

        WriteableBitmap GetSurface(PixelBuffer pixelBuffer)
        {
            if (!Monitor.IsEntered(_syncRoot))
                throw new InvalidOperationException();

            var surface = pixelBuffer.Surface;
            if (surface == null
                || surface.PixelWidth != pixelBuffer.Width
                || surface.PixelHeight != pixelBuffer.Height)
            {
                var dpi = DpiScale;
                surface = new WriteableBitmap(pixelBuffer.Width, pixelBuffer.Height, dpi.PixelsPerInchX, dpi.PixelsPerInchY, PixelFormats.Bgra32, null);
                pixelBuffer.Surface = surface;
            }

            surface.Lock();
            try
            {
                Marshal.Copy(pixelBuffer.DIB, 0, surface.BackBuffer, pixelBuffer.Size);
                surface.AddDirtyRect(pixelBuffer.GetDirtyRectangle());
                pixelBuffer.ClearDirtyRectangle();
            }
            finally
            {
                surface.Unlock();
            }

            return surface;
        }

        public static DpiScale DpiScale { get; set; } = new()
    }
}
