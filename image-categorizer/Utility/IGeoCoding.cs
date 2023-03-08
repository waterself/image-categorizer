namespace image_categorizer
{
    public interface IGeoCoding
    {
        void GeoCodingInit();
        string? GetLocation(double? latitude, double? longitude);
    }
}