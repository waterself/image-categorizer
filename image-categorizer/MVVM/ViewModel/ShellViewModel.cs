using image_categorizer.Core;

using System.Data.SQLite;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System;
using System.Windows;
using System.Windows.Input;
using image_categorizer.MVVM.View;

namespace image_categorizer.MVVM.ViewModel
{
    class ShellViewModel : ObservableObject
    {
        // Command Properties
        public RelayCommand RunViewCommand { get; set; }
        public RelayCommand SettingViewCommand { get; set; }
        public RelayCommand SummaryViewCommand { get; set; }
        public RelayCommand LicenseViewCommand { get; set; }
        public ICommand OnBoardingViewCommand { get; set; }

        /* private RunViewModel? _runVM;

         public RunViewModel? RunVM
         {
             get { return new RunViewModel(); }
             set { _runVM = value; }
         }
         private SettingViewModel? _settingVM;
         public SettingViewModel? SettingVM
         {
             get { return new SettingViewModel(); }
             set { _settingVM = value; }
         }

         private SummaryViewModel? _summaryVM;

         public SummaryViewModel? SummaryVM
         {
             get { return new SummaryViewModel(); }
             set { _summaryVM = value; }
         }*/


        private BaseViewModel? _currentView;
        public BaseViewModel? CurrentView
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

            CurrentView = new RunViewModel();

            RunViewCommand = new RelayCommand(o =>
            {
                CurrentView = new RunViewModel();
            });
            SummaryViewCommand = new RelayCommand(o =>
            {
                CurrentView = new SummaryViewModel();
            });
            SettingViewCommand = new RelayCommand(o =>
            {
                CurrentView = new SettingViewModel();
            });
            LicenseViewCommand = new RelayCommand(o => {
                CurrentView = new LicenseViewModel();
            });
            OnBoardingViewCommand = new RelayCommand(o=> { ShowOnboarding(); });

            FirstRunCheck(Properties.Settings.Default.IsFirstRun);

        }
        public void FirstRunCheck(bool isFirstRun) {
            if (isFirstRun) {
                Properties.Settings.Default.IsFirstRun = false;
                Properties.Settings.Default.Save();
                ShowOnboarding();
            }
        }

        public void ShowOnboarding()
        {
            OnBoardingView onBoardingView = new();
            OnBoardingViewModel onBoardingViewModel = new();
            onBoardingView.DataContext = onBoardingViewModel;
            onBoardingView.Title = "Tutorial";
            onBoardingView.Show();
        }

    }
}
