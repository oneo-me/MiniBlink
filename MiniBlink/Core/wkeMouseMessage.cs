using System.Diagnostics.CodeAnalysis;

namespace MiniBlink.Core
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public enum wkeMouseMessage : uint
    {
        WKE_MSG_MOUSEMOVE = 0x0200,
        WKE_MSG_LBUTTONDOWN = 0x0201,
        WKE_MSG_LBUTTONUP = 0x0202,
        WKE_MSG_LBUTTONDBLCLK = 0x0203,
        WKE_MSG_RBUTTONDOWN = 0x0204,
        WKE_MSG_RBUTTONUP = 0x0205,
        WKE_MSG_RBUTTONDBLCLK = 0x0206,
        WKE_MSG_MBUTTONDOWN = 0x0207,
        WKE_MSG_MBUTTONUP = 0x0208,
        WKE_MSG_MBUTTONDBLCLK = 0x0209,
        WKE_MSG_MOUSEWHEEL = 0x020A
    }
}