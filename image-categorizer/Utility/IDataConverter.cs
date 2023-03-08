using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace image_categorizer
{
    public interface IDataConverter
    {
        int[] ArrayLengthCheck(int[]? array, int size);
        string[] ArrayLengthCheck(string[]? array, int size);
        double ConvertCoordinate(ulong[] coordinate);
        double ConvertToUnsignedRational(ulong value);
        string deleteRegex(string input, string regex);
        string? FormatDateTaken(string? date);
        string FormatIsoDateTime(string dateTaken);
        string? FormatTimeTaken(string? date);
        string? FormatYear(string dateTime);
        string? FormatYearMonth(string dateTime);
        string? GetCameraModelWithCameraManufacturer(string CameraManufacturer, string CameraModel);
        double[]? GetCoordinate(BitmapMetadata metaData);
        Dictionary<T, List<T?>>? GetSameValueList<T>(List<T>? list);
        List<T>? ListDistinct<T>(List<T>? list);
    }
}