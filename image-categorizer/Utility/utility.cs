using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_categorizer
{
    public class Utility
    {
        public static List<string> GetImageFiles(string filePath)
        {
            List<string> imageFiles = new();
            foreach (string file in Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories))
            {
                if (Regex.IsMatch(file, @".jpg|.png|.bmp|.JPG|.PNG|.BMP|.JPEG|.jpeg$"))
                {
                    imageFiles.Add(file);
                }
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
                    return dateTime.ToString("yyyy-MM-dd");
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
                    return dateTime.ToString("HH:mm:ss");
                }
                else return null;
            }
            else return null;
        }

        public static string? GetCameraModelWithCameraManufacturer(string CameraManufacturer,
            string CameraModel)
        {
            if (CameraManufacturer != null && CameraModel != null)
            {
                return String.Format($"{CameraManufacturer} {CameraModel}");
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
        public static List<string>? ListDistinct(List<string>? list)
        {
            List<string>? targetList = new();
            if (list != null)
            {
                foreach (string item in list)
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
        //public static string rename
    }
}
