using System.Diagnostics.CodeAnalysis;

namespace MiniBlink.Core
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum wkeLoadingResult
    {
        WKE_LOADING_SUCCEEDED,
        WKE_LOADING_FAILED,
        WKE_LOADING_CANCELED
    }
}