using image_categorizer.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace image_categorizer.MVVM.Model
{
    class SettingModel : BaseViewModel
    {
        public SettingModel()
        {

        }
        private string[]? _directoryRules = new string[4];

        public string[]? DirectoryRules
        {
            get { return _directoryRules; }
            set { _directoryRules = value; }
        }

        private string[]? _fileNameRules = new string[4];

        public string[]? FileNameRules
        {
            get { return _fileNameRules; }
            set { _fileNameRules = value; }
        }

    }
}
