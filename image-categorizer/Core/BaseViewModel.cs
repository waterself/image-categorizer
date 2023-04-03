using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_categorizer.Core
{
    public class BaseViewModel : ObservableObject, ICloneable
    {
        protected string ProgramDir { get; set; }
        private DependencyContainer container;
        public BaseViewModel()
        {
            container = new DependencyContainer();
            ProgramDir = container.programDir;    
        }

        public object Clone()
        {
            return this.MemberwiseClone();

        }
    }
}
