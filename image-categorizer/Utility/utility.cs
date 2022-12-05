﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

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
                    imageFiles.Add(@file);
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
        public static double[]? GetCoordinate(BitmapMetadata metaData)
        {
            if (metaData != null)
            {
                double[]? ret = new double[2];
                ulong[]? Latitude = metaData.GetQuery("/app1/ifd/gps/subifd:{ulong=2}") as ulong[];
                ulong[]? Longtitude = metaData.GetQuery("/app1/ifd/gps/subifd:{ulong=4}") as ulong[];
                string? latret = metaData.GetQuery("/app1/ifd/gps/subifd:{char=1}") as string;
                string? longret = metaData.GetQuery("/app1/ifd/gps/subifd:{char=3}") as string;
                if (latret == null || longret == null) { return null; }

                ret[0] = ConvertCoordinate(Latitude);
                ret[1] = ConvertCoordinate(Longtitude);
                if (latret == "S")
                { ret[0] -= ret[0] * 2; }
                if (longret == "W")
                { ret[1] -= ret[1] * 2; }
                return ret;
            }
            else return null;

        }
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

