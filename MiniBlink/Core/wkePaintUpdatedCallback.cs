using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace MiniBlink.Core
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public delegate void wkePaintUpdatedCallback(IntPtr webView, IntPtr param, IntPtr hdc, int x, int y, int cx, int cy);
}