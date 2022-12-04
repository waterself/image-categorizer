using image_categorizer.Core;
using System.Collections.Generic;


namespace image_categorizer.MVVM.Model
{
    class RunModel : ObservableObject
    {
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

        private Dictionary<string, ImageDetails>? _fileWithdetails = new();

        public Dictionary<string, ImageDetails>? FileWithDetails
        {
            get { return _fileWithdetails; }
            set { _fileWithdetails = value; }
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



    }
}
