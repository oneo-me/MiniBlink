using System.Diagnostics.CodeAnalysis;

namespace MiniBlink.Core
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public enum WindowsMessage
    {
        WM_MOUSEMOVE = 0x0200,
        WM_NCHITTEST = 0x0084
    }
}