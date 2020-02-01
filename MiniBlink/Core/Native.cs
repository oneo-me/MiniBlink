using System;
using System.Runtime.InteropServices;

namespace MiniBlink.Core
{
    public static class Native
    {
        [DllImport("user32.dll")]
        public static extern bool SetCaretPos(int x, int y);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dll);
    }
}