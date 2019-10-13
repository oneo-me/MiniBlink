using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace MiniBlink.Core
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public delegate void wkeConsoleMessageCallback(IntPtr webView, IntPtr param, wkeConsoleLevel level, IntPtr message, IntPtr sourceName, int sourceLine, IntPtr stackTrace);
}