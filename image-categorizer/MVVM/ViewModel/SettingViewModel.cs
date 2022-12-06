using image_categorizer.MVVM.Model;

namespace image_categorizer.MVVM.ViewModel
{
    class SettingViewModel
    {
        private SettingModel _settingModel;

        public SettingModel SettingModel
        {
            get { return _settingModel; }
            set { _settingModel = value; }
        }

    }
}
