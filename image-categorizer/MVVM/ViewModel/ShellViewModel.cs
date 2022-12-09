using image_categorizer.Core;
using image_categorizer.MVVM.View;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace image_categorizer.MVVM.ViewModel
{
    class ShellViewModel : ObservableObject
    {
        // Command Properties
        public RelayCommand RunViewCommand { get; set; }
        public RelayCommand SettingViewCommand { get; set; }
        public RelayCommand SummaryViewCommand { get; set; }

        // ViewModel Properties
        /*        public RunViewModel RunVM { get; set; }
                public SummaryViewModel SummaryVM { get; set; }
                public SettingViewModel SettingVM { get; set; }*/

        private static RunViewModel? _runVM;

        public static RunViewModel? RunVM
        {
            get => _runVM ?? (_runVM = new RunViewModel());
            set { _runVM = value; }
        }

        private static SummaryViewModel? _summaryVM;

        public SummaryViewModel? SummaryVM
        {
            get => _summaryVM ?? (_summaryVM = new SummaryViewModel());
            set { _summaryVM = value; }
        }
        private static SettingViewModel? _settingVM;

        public SettingViewModel? SettingVM
        {
            get => _settingVM ?? (_settingVM = new SettingViewModel());
            set { _settingVM = value; }
        }


        public ObservableCollection<string> RulesForComboBox
        {
            get { return _rulesForComboBox; }
        }


        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ShellViewModel()
        {

            CurrentView = RunVM;

            RunViewCommand = new RelayCommand(o =>
            {
                CurrentView = RunVM;
            });
            SummaryViewCommand = new RelayCommand(o =>
            {
                CurrentView = SummaryVM;
            });
            SettingViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingVM;
            });

        }

    }
}
