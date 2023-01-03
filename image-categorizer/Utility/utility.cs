using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace image_categorizer
{
    public static class Utility
    {
        public static string deleteRegex(string input, string regex)
        {
            return Regex.Replace(input, regex, "");
        }
        public static List<string> GetImageFiles(string filePath)
        {
            List<string> imageFiles = new();
            //need exception handling
            try
            {
                foreach (string file in Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories))
                {
                    string fileExtension = Path.GetExtension(file);
                    if (Regex.IsMatch(fileExtension, @".jpg|.png|.bmp|.JPG|.PNG|.BMP|.JPEG|.jpeg$"))
                    {
                        imageFiles.Add(@file);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return imageFiles;
        }
        public static bool FileExistsCheck(string file)
        {
            if (File.Exists(file))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string? FormatDateTaken(string? date)
        {
            if (date != null)
            {
                DateTime dateTime = new();
                if (DateTime.TryParse(date, out dateTime))
                {
                    return dateTime.ToString("yyyyMMdd");
                }
                else return null;
            }
            else return null;
        }
        public static string? FormatTimeTaken(string? date)
        {
            if (date != null)
            {
                DateTime dateTime = new();
                if (DateTime.TryParse(date, out dateTime))
                {
                    return dateTime.ToString("HHmmss");
                }
                else return null;
            }
            else return null;
        }

        public static string? FormatYearMonth(string dateTime)
        {
            DateTime datetime = new();
            if (DateTime.TryParse(dateTime, out datetime))
            {
                return datetime.ToString("yyyy-MM");
            }
            else return null;
        }
        public static string? FormatYear(string dateTime)
        {
            DateTime datetime = new();
            if (DateTime.TryParse(dateTime, out datetime))
            {
                return datetime.ToString("yyyy");
            }
            else return null;
        }

        public static string FormatIsoDateTime(string dateTaken)
        {
            DateTime dateTime = new();
            if (dateTaken != null)
            {
                if (DateTime.TryParse(dateTaken, out dateTime))
                {
                    return dateTime.ToString("yyyy-MM-dd HH:MM:ss");
                }
                else return "";
            }
            else return "";
        }

        public static string? GetCameraModelWithCameraManufacturer(string CameraManufacturer,
            string CameraModel)
        {
            if (CameraManufacturer != null && CameraModel != null)
            {
                string formatedCameraModel = Regex.Replace(CameraModel, CameraManufacturer, "");
                string formated = deleteRegex(String.Format($"{CameraManufacturer} {formatedCameraModel}"), "[\\/:*?\"<>|]");
                string[] splited = formated.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                return String.Join(" ", splited);

            }
            else if (CameraManufacturer == null && CameraModel != null)
            {
                return String.Format($"{CameraModel}");
            }
            else if (CameraManufacturer != null && CameraModel == null)
            {
                return String.Format($"{CameraManufacturer}");
            }
            else
            {
                return null;
            }

        }
        public static List<T>? ListDistinct<T>(List<T>? list)
        {
            List<T>? targetList = new();
            if (list != null)
            {
                foreach (T item in list)
                {
                    if (!targetList.Contains(item))
                    {
                        targetList.Add(item);
                    }
                }
                return targetList;
            }
            else
            {
                return null;
            }
        }
        public static Dictionary<T, List<T?>>? GetSameValueList<T>(List<T>? list)
        {
            Dictionary<T, List<T>>? result = new Dictionary<T, List<T>>();
            if (list != null)
            {
                foreach (T item in list)
                {
                    if (!result.ContainsKey(item))
                    {
                        result.Add(item, new List<T>() { item });
                    }
                    else {
                        result[item].Add(item);
                    }
                }
            }
            return result;
        }
        public static int[] ArrayLengthCheck(int[]? array, int size)
        {
            int[] result = new int[size];
            if (array == null)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = 0;
                }
            }
            else if (array.Length < size)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = array[i];
                }
            }
            else if (array.Length > size)
            {
                for (int i = 0; i < size; i++)
                {
                    result[i] = array[i];
                }
            }
            else
            {
                result = array;
            }
            return result;
        }
        public static string[] ArrayLengthCheck(string[]? array, int size)
        {
            string[] result = new string[size];
            if (array == null)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = "None";
                }
            }
            else if (array.Length < size)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = array[i];
                }
            }
            else if (array.Length > size)
            {
                for (int i = 0; i < size; i++)
                {
                    result[i] = array[i];
                }
            }
            else
            {
                result = array;
            }
            return result;
        }
             
        public static double[]? GetCoordinate(BitmapMetadata metaData)
        {
            if (metaData != null)
            {
                double[]? ret = new double[2];
                ulong[]? Latitude = metaData.GetQuery("/app1/ifd/gps/subifd:{ulong=2}") as ulong[];
                ulong[]? longitude = metaData.GetQuery("/app1/ifd/gps/subifd:{ulong=4}") as ulong[];
                string? latret = metaData.GetQuery("/app1/ifd/gps/subifd:{char=1}") as string;
                string? longret = metaData.GetQuery("/app1/ifd/gps/subifd:{char=3}") as string;
                if (latret == null || longret == null || Latitude == null || longitude == null) { return null; }

                ret[0] = ConvertCoordinate(Latitude);
                ret[1] = ConvertCoordinate(longitude);
                if (latret == "S")
                { ret[0] -= ret[0] * 2; }
                if (longret == "W")
                { ret[1] -= ret[1] * 2; }
                return ret;
            }
            else return null;

        }
        //need null check
        public static double ConvertCoordinate(ulong[] coordinate)
        {
            double degrees = ConvertToUnsignedRational(coordinate[0]);
            double minutes = ConvertToUnsignedRational(coordinate[1]);
            double seconds = ConvertToUnsignedRational(coordinate[2]);
            return degrees + (minutes / 60.0) + (seconds / 3600);
        }
        public static double ConvertToUnsignedRational(ulong value)
        {
            //0xFFFFFFFFL Unsignedintmax
            return (value & 0xFFFFFFFFL) / (double)((value & 0xFFFFFFFF00000000L) >> 32);
        }
    }
}


