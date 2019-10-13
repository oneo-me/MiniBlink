using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MiniBlink.Core
{
    public static class Blink
    {
        const string blink = "node.dll";

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        static extern void wkeInit();

        static bool isInit;

        public static void Init()
        {
            if (isInit)
                return;

            isInit = true;
            wkeInit();
        }

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeCreateWebView();

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeCreateWebWindow(wkeWindowType type, IntPtr parent, int x, int y, int width, int height);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeDestroyWebWindow(IntPtr webWindow);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeDestroyWebView(IntPtr webWindow);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetHandle(IntPtr webView, IntPtr wnd);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetHandleOffset(IntPtr webView, int x, int y);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeResize(IntPtr webView, int w, int h);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void wkeLoadURLW(IntPtr webView, [In] [MarshalAs(UnmanagedType.LPWStr)] string url);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeLoadHTMLW(IntPtr webView, [In] [MarshalAs(UnmanagedType.LPWStr)] string html);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetNavigationToNewWindowEnable(IntPtr webView, bool b);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetNpapiPluginsEnabled(IntPtr webView, bool b);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetCookieEnabled(IntPtr webView, bool enable);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetCookieJarPath(IntPtr webView, [MarshalAs(UnmanagedType.LPWStr)] [In] string path);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetUserAgentW(IntPtr handle, [In] [MarshalAs(UnmanagedType.LPWStr)] string str);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern wkeCursorInfoType wkeGetCursorInfoType(IntPtr webView);

        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeFireMouseWheelEvent(IntPtr webView, int x, int y, int delta, uint flags);

        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeFireMouseEvent(IntPtr webView, uint message, int x, int y, uint flags);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeSetFocus(IntPtr webView);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeKillFocus(IntPtr webView);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnPaintUpdated(IntPtr webView, wkePaintUpdatedCallback callback, IntPtr callbackParam);

        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool wkeFireKeyDownEvent(IntPtr webView, uint virtualKeyCode, uint flags, [MarshalAs(UnmanagedType.I1)] bool systemKey);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool wkeFireKeyUpEvent(IntPtr webView, uint virtualKeyCode, uint flags, [MarshalAs(UnmanagedType.I1)] bool systemKey);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool wkeFireKeyPressEvent(IntPtr webView, uint charCode, uint flags, [MarshalAs(UnmanagedType.I1)] bool systemKey);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnLoadingFinish(IntPtr webView, wkeLoadingFinishCallback callback, IntPtr param);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public static extern IntPtr wkeGetStringW(IntPtr @string);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnNavigation(IntPtr webView, wkeNavigationCallback callback, IntPtr param);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern void wkeOnConsole(IntPtr webView, wkeConsoleMessageCallback callback, IntPtr param);

        [DllImport(blink, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr wkeGetString(IntPtr @string);

        public static string Utf8IntptrToString(IntPtr ptr)
        {
            var data = new List<byte>();
            var off = 0;
            while (true)
            {
                var ch = Marshal.ReadByte(ptr, off++);
                if (ch == 0)
                    break;
                data.Add(ch);
            }

            return Encoding.UTF8.GetString(data.ToArray());
        }
    }
}