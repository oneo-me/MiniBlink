using System.Diagnostics.CodeAnalysis;

namespace MiniBlink.Core
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum wkeCursorInfoType
    {
        WkeCursorInfoPointer = 0,
        WkeCursorInfoCross = 1,
        WkeCursorInfoHand = 2,
        WkeCursorInfoIBeam = 3,
        WkeCursorInfoWait = 4,
        WkeCursorInfoHelp = 5,
        WkeCursorInfoEastResize = 6,
        WkeCursorInfoNorthResize = 7,
        WkeCursorInfoNorthEastResize = 8,
        WkeCursorInfoNorthWestResize = 9,
        WkeCursorInfoSouthResize = 10,
        WkeCursorInfoSouthEastResize = 11,
        WkeCursorInfoSouthWestResize = 12,
        WkeCursorInfoWestResize = 13,
        WkeCursorInfoNorthSouthResize = 14,
        WkeCursorInfoEastWestResize = 15,
        WkeCursorInfoNorthEastSouthWestResize = 16,
        WkeCursorInfoNorthWestSouthEastResize = 17,
        WkeCursorInfoColumnResize = 18,
        WkeCursorInfoRowResize = 19
    }
}