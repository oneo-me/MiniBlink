using System.Diagnostics.CodeAnalysis;

namespace MiniBlink.Core
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum wkeConsoleLevel
    {
        wkeLevelDebug = 4,
        wkeLevelLog = 1,
        wkeLevelInfo = 5,
        wkeLevelWarning = 2,
        wkeLevelError = 3,
        wkeLevelRevokedError = 6,
        wkeLevelLast = wkeLevelInfo
    }
}