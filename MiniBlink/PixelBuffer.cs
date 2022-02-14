using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MiniBlink
{
    public class PixelBuffer : IDisposable
    {
        public byte[] DIB;

        readonly List<CefRect> _dirtyRects = new();

        public readonly int Width;

        public readonly int Height;

        public WriteableBitmap Surface;

        public PixelBuffer(int width, int height)
        {
            Width = width;
            Height = height;
            DIB = ArrayPool<byte>.Shared.Rent(width * height * 4);
        }

        ~PixelBuffer()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            var buffer = Interlocked.Exchange(ref DIB, null);
            if (buffer != null)
                ArrayPool<byte>.Shared.Return(buffer);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int Stride
        {
            get => Width * 4;
        }

        public int Size
        {
            get => Width * Height * 4;
        }

        public void AddDirtyRects(CefRect[] dirtyRects)
        {
            _dirtyRects.AddRange(dirtyRects);
        }

        public Int32Rect GetDirtyRectangle()
        {
            if (_dirtyRects.Count == 0)
                return new Int32Rect();
            CefRect r = _dirtyRects[0];
            var dirtyRect = new Int32Rect(r.X, r.Y, r.Width, r.Height);
            for (var i = 1; i < _dirtyRects.Count; i++)
                Union(ref dirtyRect, _dirtyRects[i]);
            return dirtyRect;
        }

        public void ClearDirtyRectangle()
        {
            _dirtyRects.Clear();
        }

        static void Union(ref Int32Rect self, CefRect rect)
        {
            int x = Math.Min(self.X, rect.X);
            int right = Math.Max(self.X + self.Width, rect.X + rect.Width);
            int y = Math.Min(self.Y, rect.Y);
            int bottom = Math.Max(self.Y + self.Height, rect.Y + rect.Height);
            self = new Int32Rect(x, y, right - x, bottom - y);
        }
    }
}
