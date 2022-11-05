﻿using image_categorizer.Core;

namespace image_categorizer.MVVM.Model
{
    internal class RunModel : ObservableObject
    {
        private int _fileCount;

        public int FileCount
        {
            get { return _fileCount; }
            set { _fileCount = value; }
        }


        private string? _inputDirectoryPath = "Please Select Input Directory";

        public string InputDirectorytPath
        {
            get { return _inputDirectoryPath; }
            set
            {
                _inputDirectoryPath = value;
                OnPropertyChanged();
            }
        }

        private string? _outputDirectoryPath = "Please Select Output Directory";

        public string OutputDirectorytPath
        {
            get { return _outputDirectoryPath; }
            set
            {
                _outputDirectoryPath = value;
                OnPropertyChanged();
            }
        }

    }
}
