using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using image_categorizer.Core;

namespace image_categorizer.MVVM.Model
{
    class ShellModel : ObservableObject
    {
        private string _programDirectory;

        public string ProgramDirectory
        {
            get { return _programDirectory; }
            set { _programDirectory = value; }
        }


    }
}
