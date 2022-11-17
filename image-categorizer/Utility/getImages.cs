using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_categorizer.Utility
{
    static class Utility
    {
        public static List<string> GetImageFiles(string filePath)
        { 
            List<string> imageFiles = new List<string>();
            foreach (string file in Directory.GetFiles(filePath))
            {
                if (Regex.IsMatch(file, @".jpg|.png|.bmp|.JPG|.PNG|.BMP|.JPEG|.jpeg$"))
                { 
                    imageFiles.Add(file);
                }
            }
            return imageFiles;
        }
    }
}
