using image_categorizer.Core;
using System.Collections.Generic;


namespace image_categorizer.MVVM.Model
{
    internal class RunModel : ObservableObject
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



        private string? _inputDirectoryPath = null; //"Please Select Input Directory";

        public string? InputDirectorytPath
        {
            get { return _inputDirectoryPath; }
            set
            {
                _inputDirectoryPath = value;
                OnPropertyChanged();
            }
        }

        private string? _outputDirectoryPath = null; //"Please Select Output Directory";

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
