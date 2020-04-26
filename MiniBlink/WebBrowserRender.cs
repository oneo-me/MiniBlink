using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace MiniBlink
{
    public static class WebBrowserRender
    {
        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        static extern IntPtr GetCurrentObject(IntPtr dc, int uObjectType);

        public static void Render(Image image, IntPtr handle, IntPtr hdc, int x, int y, int w, int h)
        {
            try
            {
                var hBitmap = GetCurrentObject(hdc, 7);
                if (hBitmap == IntPtr.Zero)
                    return;

                WriteableBitmap wb = null;
                if (image.Source is WriteableBitmap current)
                    wb = current;

                if (wb == null || wb.PixelWidth < w || wb.PixelHeight < h)
                    image.Source = wb = new WriteableBitmap(w, h, 96d, 96d, PixelFormats.Pbgra32, null);

                wb.Lock();

                using (var backBitmap = new Bitmap(wb.PixelWidth, wb.PixelHeight, wb.BackBufferStride, PixelFormat.Format32bppPArgb, wb.BackBuffer))
                using (var graphics = Graphics.FromImage(backBitmap))
                using (var fromHBitmap = System.Drawing.Image.FromHbitmap(hBitmap))
                    graphics.DrawImage(fromHBitmap, new Rectangle(x, y, w, h), new Rectangle(x, y, w, h), GraphicsUnit.Pixel);

                wb.AddDirtyRect(new Int32Rect(x, y, w, h));
                wb.Unlock();

                DeleteObject(hBitmap);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }
    }
}