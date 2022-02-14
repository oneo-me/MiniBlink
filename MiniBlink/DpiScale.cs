namespace MiniBlink
{
    public struct DpiScale
    {
        public double DpiScaleX { get; }

        public double DpiScaleY { get; }

        public double PixelsPerDip
        {
            get => DpiScaleY;
        }

        public double PixelsPerInchX
        {
            get => 96.0 * DpiScaleX;
        }

        public double PixelsPerInchY
        {
            get => 96.0 * DpiScaleY;
        }

        public DpiScale(double dpiScaleX, double dpiScaleY)
        {
            DpiScaleX = dpiScaleX;
            DpiScaleY = dpiScaleY;
        }
    }
}
