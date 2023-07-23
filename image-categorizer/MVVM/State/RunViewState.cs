using image_categorizer.Core;
using image_categorizer.MVVM.Model;
using System.Collections.Generic;
using System.Threading;

namespace image_categorizer.MVVM.State
{
    class RunViewState : ObservableObject
    {
        #region Binding Data
        private int _fileCount;

        public int FileCount
        {
            get { return _fileCount; }
            set
            {
                _fileCount = value;
                OnPropertyChanged();
            }
        }

        private int _maxProgress = 100;

        public int MaxProgress
        {
            get { return _maxProgress; }
            set { _maxProgress = value; OnPropertyChanged(); }
        }


        //private string? _inputDirectoryPath = null;

        private string? _inputDirectoryPath = "D:\\Test";

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
        private string? _outputDirectoryPath = "D:\\output";

        public string? OutputDirectorytPath
        {
            get { return _outputDirectoryPath; }
            set
            {
                _outputDirectoryPath = value;
                OnPropertyChanged();
            }
        }

        private bool _isIdle = true;

        public bool IsIdle
        {
            get { return _isIdle; }
            set
            {
                _isIdle = value;
                OnPropertyChanged();
            }
        }

        private string _runButtonIcon;

        public string RunButtonIcon
        {
            get { return _runButtonIcon; }
            set { _runButtonIcon = value; }
        }

        #endregion Binding Data

        #region Logical Data
        private Dictionary<string, ImageDetailsModel>? _fileWithdetails = new();

        public Dictionary<string, ImageDetailsModel>? FileWithDetails
        {
            get { return _fileWithdetails; }
            set { _fileWithdetails = value; }
        }
        #endregion Logical Data

        private long _categorizeProgress = 0;

        public long CategorizeProgress
        {
            get
            {

                return _categorizeProgress;
            }
            set
            {
                _categorizeProgress = value;
                OnPropertyChanged();
            }
        }
        public long ProgressIncrement()
        {
            Interlocked.Increment(ref _categorizeProgress);
            OnPropertyChanged(nameof(_categorizeProgress));
            return _categorizeProgress;
        }




    }
}
