using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_categorizer
{
    public class Logger
    {
        private string logFileName;
        private string logFolder;
        private object logLock = new object();
        private Queue<string> logQueue;

        public Logger(string programDir) {
            logQueue = new();
            logFolder = $"{programDir}\\Log";
            logFileName = $"{logFolder}\\{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff")}task.txt";
            DirectoryInfo di = new(logFolder);
            if (!di.Exists)
            { 
                di.Create();
            }
            File.CreateText(logFileName);
        }
        public void WriteLog(string message, bool writeNow = false)
        {
            logQueue.Enqueue($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff")}] : {message}");
            if (writeNow == true)
            {
                lock (logLock)
                {
                    using (StreamWriter sw = new StreamWriter(logFileName))
                    {
                        while (logQueue.Count > 0)
                        {
                            string log = logQueue.Dequeue();
                            sw.WriteLine(log);
                        }
                    }
                }
            }
        }
    }
}
