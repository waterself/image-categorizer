using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.IsolatedStorage;

namespace image_categorizer
{
    public static class IsolateStorage
    {
        private static IsolatedStorageFile _isoStore;
        private static string _isoFIleName = "Settings.txt";
        public static string? ReadStorageValue(string key)
        {
            List<string> Lines = new List<string>();
            using (IsolatedStorageFileStream isostream = new(_isoFIleName, FileMode.Open, _isoStore))
            {
                using (StreamReader reader = new StreamReader(isostream))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Lines.Add(line);
                    }
                }
            }
            foreach (string line in Lines)
            {
                if (line.Contains(key))
                {
                    return line;
                }
            }
            return null;
        }
        public static void WriteStorageValue(string key, string value)
        {
            using (IsolatedStorageFileStream isoStream = new(_isoFIleName, FileMode.Append, _isoStore))
            {
                using (StreamWriter writer = new StreamWriter(isoStream))
                {
                    writer.WriteLine(String.Format($"{key}:{value}"));
                }
            }
        }
        public static void IsolateStorageInit()
        {
            _isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            if (!_isoStore.FileExists(_isoFIleName))
            {
                using (IsolatedStorageFileStream isoStream = new("Settings.txt", System.IO.FileMode.CreateNew, _isoStore))
                {
                    using (StreamWriter writer = new StreamWriter(isoStream))
                    {
                        writer.WriteLine("SettingFile Initialized");
                    }
                }
            }
        }
    }
}
