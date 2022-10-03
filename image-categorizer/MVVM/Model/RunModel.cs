using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using image_categorizer.Core;

namespace image_categorizer.MVVM.Model
{
    internal class RunModel : ObservableObject
    {
        private string? _inputDirectoryPath = "Please Select Input Directory";

        public string InputDirectorytPath
        {
            get { return _inputDirectoryPath; }
            set { _inputDirectoryPath = value;
                OnPropertyChanged();
            }
        }

        private string? _outputDirectoryPath = "Please Select Output Directory";

        public string OutputDirectorytPath
        {
            get { return _outputDirectoryPath; }
            set { _outputDirectoryPath = value;
                OnPropertyChanged();
            }
        }

    }
}
