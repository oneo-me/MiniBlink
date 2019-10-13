using System.Diagnostics.CodeAnalysis;

namespace MiniBlink.Core
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public enum wkeNavigationType
    {
        WKE_NAVIGATION_TYPE_LINKCLICK,
        WKE_NAVIGATION_TYPE_FORMSUBMITTE,
        WKE_NAVIGATION_TYPE_BACKFORWARD,
        WKE_NAVIGATION_TYPE_RELOAD,
        WKE_NAVIGATION_TYPE_FORMRESUBMITT,
        WKE_NAVIGATION_TYPE_OTHER
    }
}