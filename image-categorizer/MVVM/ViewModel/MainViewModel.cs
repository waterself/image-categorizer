using image_categorizer.Core;

namespace image_categorizer.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand RunViewCommand { get; set; }
        public RelayCommand SettingViewCommand { get; set; }
        public RelayCommand SummeryViewCommand { get; set; }

        public RunViewModel RunVM { get; set; }
        public SummeryViewModel SummerVM { get; set; }
        public SettingViewModel SettingVM { get; set; }

        private object _currentView;

        public MainViewModel()
        {
            RunVM = new();
            SummerVM = new();
            SettingVM = new();

            CurrentView = RunVM;

            RunViewCommand = new RelayCommand(o =>
            {
                CurrentView = RunVM;
            });

            SummeryViewCommand = new RelayCommand(o =>
            {
                CurrentView = SummerVM;
            });
            SettingViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingVM;
            });

        }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
    }
}
