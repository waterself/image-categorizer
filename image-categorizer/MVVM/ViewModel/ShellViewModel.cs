using image_categorizer.Core;
using System.Data.SQLite;
using image_categorizer.MVVM.View;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System;
using image_categorizer;

namespace image_categorizer.MVVM.ViewModel
{
    class ShellViewModel : BaseViewModel
    {
        // Command Properties
        public RelayCommand RunViewCommand { get; set; }
        public RelayCommand SettingViewCommand { get; set; }
        public RelayCommand SummaryViewCommand { get; set; }


        private RunViewModel? _runVM;

        public RunViewModel? RunVM
        {
            get => _runVM ?? (_runVM = new RunViewModel());
            set { _runVM = value; }
        }
        public SettingViewModel? SettingVM
        {
            get { return _settingVM ?? (_settingVM = new SettingViewModel()); }
            set { _settingVM = value; }
        }

        private SummaryViewModel? _summaryVM;

        public SummaryViewModel? SummaryVM
        {
            get => _summaryVM ?? (_summaryVM = new SummaryViewModel());
            set { _summaryVM = value; }
        }
        private SettingViewModel? _settingVM;



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
            GeoCoding.GeoCodingInit();
            SQLite.SQLiteinit();
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
