using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace MiniBlink.Core
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public delegate void wkeLoadUrlEndCallback(IntPtr webView, IntPtr param, [MarshalAs(UnmanagedType.LPStr)] string url, IntPtr job, IntPtr buf, int len);
}