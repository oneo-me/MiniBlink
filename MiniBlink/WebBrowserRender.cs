using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace MiniBlink
{
    public static class WebBrowserRender
    {
        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        static extern IntPtr GetCurrentObject(IntPtr dc, int uObjectType);

        public static void Render(FrameworkElement element, IntPtr handle, IntPtr hdc, int x, int y, int w, int h, Image image)
        {
            var width = (int)element.ActualWidth;
            var height = (int)element.ActualHeight;

            var hBitmap = IntPtr.Zero;

            try
            {
                hBitmap = GetCurrentObject(hdc, 7);
                if (hBitmap != IntPtr.Zero)
                    image.Source = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, new Int32Rect(0, 0, width, height), null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (hBitmap != IntPtr.Zero)
                    DeleteObject(hBitmap);
            }
        }
    }
}