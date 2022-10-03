﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using image_categorizer.Core;
using Microsoft.WindowsAPICodePack.Dialogs;
using image_categorizer.MVVM.Model;

namespace image_categorizer.MVVM.ViewModel
{
    internal class RunViewModel : ObservableObject
    {
        public RunViewModel()
        {
            SelectInputPathCommand = PathSelectCommand("input");
            SelectOutputPathCommand = PathSelectCommand("output");
            _runModel = new RunModel();
        }
        private RunModel _runModel = new();

        public RunModel RunModel
        {
            get { return _runModel; }
            set { _runModel = value;
                OnPropertyChanged();
            }
        }
        private RelayCommand PathSelectCommand(string mode) {
            RelayCommand ret = new RelayCommand(o =>
            {
                CommonOpenFileDialog openFileDialog = new();
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.IsFolderPicker = true;
                if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    if (mode == "input")
                    {
                        RunModel.InputDirectorytPath = openFileDialog.FileName as string;
                    }
                    else if (mode == "output")
                    {
                        RunModel.OutputDirectorytPath = openFileDialog.FileName as string;
                    }
                }
            });
            return ret;
        }

        public RelayCommand SelectInputPathCommand { get; set; }
        public RelayCommand SelectOutputPathCommand { get; set; }


    }
}
