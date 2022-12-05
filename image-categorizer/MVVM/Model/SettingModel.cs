using image_categorizer.Core;
using System.Collections.Generic;

namespace image_categorizer.MVVM.Model
{
    class SettingModel : ObservableObject
    {
        private List<string>? _directoryRules;

        public List<string>? DirectoryRules
        {
            get { return _directoryRules; }
            set { _directoryRules = value; }
        }

        private List<string>? _fileNameRules;

        public List<string>? FileNameRules
        {
            get { return _fileNameRules; }
            set { _fileNameRules = value; }
        }


        public static List<string> RulesForComboBox = new()
        {
            "None",
            "Date",
            "CameraModel",
            "Format",
            "Location"
        };

    }
}
