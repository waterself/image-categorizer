using image_categorizer.MVVM.Model;
using image_categorizer.Core;
using System.Collections.ObjectModel;

namespace image_categorizer.MVVM.ViewModel
{
    class SettingViewModel : ObservableObject
    {
        private static SettingModel _settingModel;

        public SettingModel SettingModel
        {
            get => _settingModel ?? (_settingModel = new SettingModel());
            set
            {
                _settingModel = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand SaveSetting()
        {
            RelayCommand ret = new(o =>
            {
                RuleTest();
            });
            return ret;
        }

        public RelayCommand SaveButtonCommand { get; set; }


        public void RuleTest()
        {
            for (int i = 0; i < 4; i++)
            {
                System.Diagnostics.Debug.WriteLine(SettingModel.DirectoryRules[i]);
            }
        }

        private readonly ObservableCollection<string> _rulesForComboBox = new() {
            "None",
            "Date",
            "CameraModel",
            "Format",
            "Location"
        };

        public ObservableCollection<string> RulesForComboBox
        {
            get { return _rulesForComboBox; }
        }

        public SettingViewModel()
        {
            SaveButtonCommand = SaveSetting();
            _settingModel = new();
        }
    }
}
