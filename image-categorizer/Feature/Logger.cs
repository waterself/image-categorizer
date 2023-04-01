using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace image_categorizer
{
    public class Logger
    {
        private int errorCount;
        private string taskName;
        private string logFileName;
        private string logFolder;
        private StreamWriter streamWriter;
        private Queue<string> logQueue;

        public Logger(string programDir, string taskName)
        {
            errorCount = 0;
            logQueue = new();
            logFolder = $"{programDir}Log";
            this.taskName = taskName;
            logFileName = $"{logFolder}\\{taskName}.{DateTime.Now.ToString("yyyy-MMddHHmmssff")}{taskName}.txt";

        }
        public void WriteLog(string message, bool isError, bool writeNow = false)
        {
            if (isError) { errorCount++; }
            logQueue.Enqueue($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff")}] : {message}");
            if (writeNow == true && errorCount > 0)
            {
                DirectoryInfo di = new(logFolder);
                if (!di.Exists)
                {
                    di.Create();
                }
                if (!File.Exists(logFileName))
                {
                    using (File.Create(logFileName));
                }
                using (streamWriter = new StreamWriter(logFileName))
                {
                    lock (streamWriter)
                    {
                        while (logQueue.Count > 0)
                        {
                            string log = logQueue.Dequeue();
                            streamWriter.WriteLine(log);
                        }

                    }
                }
            }
        }
        public bool ShowLogFile() {
            if (errorCount > 0)
            {
                MessageBoxResult result = MessageBox.Show($"{errorCount} errors are found, would you like to check log file?", "Log Dialog", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                { 
                    case MessageBoxResult.Yes:
                        System.Diagnostics.Process.Start("Notepad.exe", logFileName);
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
