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
        public string ProgramDir { get; set; }
        private DependencyContainer container;
        public BaseViewModel()
        {
            container = new DependencyContainer();
            ProgramDir = container.programDir;    
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
