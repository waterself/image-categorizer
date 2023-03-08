﻿using image_categorizer.Core;

using System.Data.SQLite;
using image_categorizer.MVVM.View;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System;
using image_categorizer;

namespace image_categorizer.MVVM.ViewModel
{
    class ShellViewModel : ObservableObject
    {
        // Command Properties
        public RelayCommand RunViewCommand { get; set; }
        public RelayCommand SettingViewCommand { get; set; }
        public RelayCommand SummaryViewCommand { get; set; }

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

        }

    }
}
