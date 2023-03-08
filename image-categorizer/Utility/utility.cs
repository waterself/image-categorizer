using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace image_categorizer
{
    public class Utility : DataConverter, IUtility
    {
        public Utility(ref Logger logger)
        {
            UtilityLogger = logger;
        }
        public Logger UtilityLogger { get; set; }

        public bool FileExistsCheck(string file)
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
        public List<string> GetImageFiles(string filePath)
        {
            List<string> imageFiles = new();
            //need exception handling
            try
            {
                foreach (string file in Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories))
                {
                    string fileExtension = Path.GetExtension(file);
                    if (Regex.IsMatch(fileExtension, @".jpg|.jpeg|.png|.gif|.bmp|.tiff|.psd|.raw|.cr2|.nef|.orf|.sr2$"))
                    {
                        imageFiles.Add(@file);
                    }
                }
            }
            catch (Exception e)
            {
                UtilityLogger.WriteLog(e.Message, true);
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return imageFiles;
        }

        public List<string> GetVideoFiles(string filePath)
        {
            List<string> videoFiles = new();
            try
            {
                foreach (string file in Directory.GetFiles(filePath, "*.*", SearchOption.AllDirectories))
                {
                    string fileExtension = Path.GetExtension(file);
                    if (Regex.IsMatch(fileExtension, @".mp4|.avi|.wmv|.mov|.flv|.mkv|.webm|.vob|.ogv|.m4v|.3gp|.3g2|.mpeg|.mpg|.m2v|.m4v|.svi|.3gpp|.3gpp2|.mxf|.roq|.nsv|.flv|.f4v|.f4p|.f4a|.f4b$"))
                    {
                        videoFiles.Add(@file);
                    }
                }
            }
            catch (Exception e)
            {
                UtilityLogger.WriteLog(e.Message, true);
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return videoFiles;

        }
    }
}


