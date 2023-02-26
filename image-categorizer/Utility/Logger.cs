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
        private string logFileName;
        private string logFolder;
        private StreamWriter streamWriter;
        private Queue<string> logQueue;

        public Logger(string programDir) {
            logQueue = new();
            logFolder = $"{programDir}Log";
            logFileName = $"{logFolder}\\{DateTime.Now.ToString("yyyy-MMddHHmmssff")}task.txt";
            DirectoryInfo di = new(logFolder);
            if (!di.Exists)
            { 
                di.Create();
            }
            if (!File.Exists(logFileName))
            {
                using (File.Create(logFileName)) ;
            }
                

            
        }
        public void WriteLog(string message, bool writeNow = false)
        {
            logQueue.Enqueue($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff")}] : {message}");
            if (writeNow == true)
            {
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
    }
}
