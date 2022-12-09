using image_categorizer.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace image_categorizer.MVVM.Model
{
    class SettingModel : ObservableObject
    {
        private string[] _directoryRules = new string[4];

        public string[] DirectoryRules
        {
            get { return _directoryRules; }
            set { _directoryRules = value; }
        }

        private List<string>? _fileNameRules = new(4);

        public List<string>? FileNameRules
        {
            get { return _fileNameRules; }
            set { _fileNameRules = value; }
        }

    }
}
