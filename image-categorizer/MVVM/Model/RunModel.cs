using image_categorizer.Core;
using System.Collections.Generic;


namespace image_categorizer.MVVM.Model
{
    class RunModel : ObservableObject
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

        //private string? _inputDirectoryPath = null;

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
        #endregion Binding Data

        #region Logical Data
        private Dictionary<string, ImageDetails>? _fileWithdetails = new();

        public Dictionary<string, ImageDetails>? FileWithDetails
        {
            get { return _fileWithdetails; }
            set { _fileWithdetails = value; }
        }
        #endregion Logical Data
    }
}
