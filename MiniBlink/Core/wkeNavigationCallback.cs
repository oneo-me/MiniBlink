using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace MiniBlink.Core
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public delegate bool wkeNavigationCallback(IntPtr webView, IntPtr param, wkeNavigationType navigationType, IntPtr url);
}