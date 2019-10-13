using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace MiniBlink.Core
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public delegate bool wkeLoadUrlBeginCallback(IntPtr webView, IntPtr param, [MarshalAs(UnmanagedType.LPStr)] string url, IntPtr job);
}