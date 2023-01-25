using image_categorizer.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace image_categorizer.MVVM.Model
{
    class SettingModel : ObservableObject
    {

        #region Binding Data
        private string? _inputDirectoryPath;

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
        private string? _outputDirectoryPath;

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
            set { _directoryRules = value; OnPropertyChanged(); }
        }

        private List<int> _directoryRulesIndexes = new List<int>(4) { 4, 3, 2, 1 };

        public List<int> DirectoryRulesIndexes
        {
            get { return _directoryRulesIndexes; }
            set { _directoryRulesIndexes = value;
                OnPropertyChanged();
            }
        }

        private List<int> _fileNameRulesIndexes = new List<int>(4) { 4, 3, 2, 1 };

        public List<int> FileNameRulesIndexes
        {
            get { return _fileNameRulesIndexes; }
            set { _fileNameRulesIndexes = value;
                OnPropertyChanged();
            }
        }


        private string[] _fileNameRules = new string[4];

        public string[] FileNameRules
        {
            get { return _fileNameRules; }
            set { _fileNameRules = value; OnPropertyChanged(); }
        }

        private string _oldPathExample = "";

        public string OldPathExample
        {
            get { return _oldPathExample; }
            set { _oldPathExample = value; OnPropertyChanged(); }
        }
        private string _newPathExample = "";

        public string NewPathExample
        {
            get { return _newPathExample; }
            set { _newPathExample = value; OnPropertyChanged(); }
        }

        private string _oldFileNameExample = "Sample.jpg";

        public string OldFileNameExample
        {
            get { return _oldFileNameExample; }
            set { _oldFileNameExample = value; OnPropertyChanged(); }
        }
        private string _newFileNameExample = "";

        public string NewFileNameExample
        {
            get { return _newFileNameExample; }
            set { _newFileNameExample = value; OnPropertyChanged(); }
        }

        #endregion Binding Data

    }
}
