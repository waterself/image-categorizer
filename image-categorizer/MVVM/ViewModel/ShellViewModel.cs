using image_categorizer.Core;

namespace image_categorizer.MVVM.ViewModel
{
    class ShellViewModel : ObservableObject
    {
        // Command Properties
        public RelayCommand RunViewCommand { get; set; }
        public RelayCommand SettingViewCommand { get; set; }
        public RelayCommand SummaryViewCommand { get; set; }

        // ViewModel Properties
        public RunViewModel RunVM { get; set; }
        public SummaryViewModel SummaryVM { get; set; }
        public SettingViewModel SettingVM { get; set; }


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
            RunVM = new();
            SummaryVM = new();
            SettingVM = new();

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
