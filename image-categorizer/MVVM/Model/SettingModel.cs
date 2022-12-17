using image_categorizer.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace image_categorizer.MVVM.Model
{
    class SettingModel : BaseViewModel
    {
        #region Binding Data
        private string? _inputDirectoryPath = "C:\\Test";

        public string? InputDirectorytPath
        {
            get { return _inputDirectoryPath; }
            set
            {
                _inputDirectoryPath = value;
                OnPropertyChanged();
            }
        }

        //"Please Select Output Directory";
        //private string? _outputDirectoryPath = null;
        private string? _outputDirectoryPath = "C:\\output";

        public string? OutputDirectorytPath
        {
            get { return _outputDirectoryPath; }
            set
            {
                _outputDirectoryPath = value;
                OnPropertyChanged();
            }
        }
        //using index
        private string[] _directoryRules = new string[4];

        public string[] DirectoryRules
        {
            get { return _directoryRules; }
            set { _directoryRules = value; }
        }

        private string[] _fileNameRules = new string[4];

        public string[] FileNameRules
        {
            get { return _fileNameRules; }
            set { _fileNameRules = value; }
        }
        #endregion Binding Data

    }
}
