using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_categorizer.Core
{
    class BaseViewModel : INotifyPropertyChanged, ICloneable
    {
        protected IUtility _utility;
        public BaseViewModel() {
            _utility = new Utility();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));  
        }

        public object Clone()
        {
            return this.MemberwiseClone();

        }
    }
}
