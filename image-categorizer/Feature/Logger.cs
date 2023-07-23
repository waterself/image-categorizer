using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace image_categorizer
{
    public class Logger : ILogger
    {
        private int ErrorCount { get; set; }
        private string LogFileName { get; set; }
        private string LogFolder { get; set; }
        private string TaskName { get; set; }
        private StreamWriter StreamWriter { get; set; }
        private Queue<string> LogQueue { get; set; }

        public Logger(string programDir, string taskName)
        {
            ErrorCount = 0;
            LogQueue = new();
            TaskName = taskName;
            LogFolder = $"{programDir}Log";
            LogFileName = $"{LogFolder}\\{DateTime.Now.ToString("yyyy-MMddHHmmssff")}_{taskName}.txt";

        }
        public void WriteLog(string message, bool isError, bool writeNow = false)
        {
            if (isError) { ErrorCount += 1; }
            LogQueue.Enqueue($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff")}] : {message}");
            if (writeNow == true && ErrorCount > 0)
            {
                DirectoryInfo di = new(LogFolder);
                if (!di.Exists)
                {
                    di.Create();
                }
                if (!File.Exists(LogFileName))
                {
                    using (File.Create(LogFileName));
                }
                using (StreamWriter = new StreamWriter(LogFileName))
                {
                    lock (StreamWriter)
                    {
                        while (LogQueue.Count > 0)
                        {
                            string log = LogQueue.Dequeue();
                            StreamWriter.WriteLine(log);
                        }

                    }
                }
            }
        }
        public bool ShowLogFile()
        {
            if (ErrorCount > 0)
            {
                MessageBoxResult result = MessageBox.Show($"{ErrorCount} errors are found, would you like to check log file?", "Log Dialog", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        System.Diagnostics.Process.Start("Notepad.exe", LogFileName);
                        System.Diagnostics.Process.Start("Explorer.exe", LogFolder);
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        break;
                }
                return true;
            }
            else return false;
        }
    }
}
