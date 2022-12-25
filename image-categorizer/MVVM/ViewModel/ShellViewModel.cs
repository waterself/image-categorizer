﻿using image_categorizer.Core;
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


        private static RunViewModel? _runVM;

        public static RunViewModel? RunVM
        {
            get { 
                if(_runVM == null)
                    _runVM = new RunViewModel();
                return _runVM;
            }
            set { _runVM = value; }
        }

        private static SummaryViewModel? _summaryVM;

        public static SummaryViewModel? SummaryVM
        {
            get => _summaryVM ?? (_summaryVM = new SummaryViewModel());
            set { _summaryVM = value; }
        }
        private static SettingViewModel? _settingVM;

        public static SettingViewModel? SettingVM
        {
            get =>  (_settingVM ?? (_settingVM = new SettingViewModel()));
            set { _settingVM = value; }
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
            GeoCoding.GeoCodingInit();
            SQLite.SQLiteinit();
            CurrentView = RunVM;

            RunViewCommand = new RelayCommand(o =>
            {
                //call RunVM Constructor
                CurrentView = RunVM.Clone();
            });
            SummaryViewCommand = new RelayCommand(o =>
            {
                CurrentView = SummaryVM.Clone();
            });
            SettingViewCommand = new RelayCommand(o =>
            {
                CurrentView = SettingVM.Clone();
            });

        }

    }
}
