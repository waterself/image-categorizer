using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_categorizer.Core
{
    public class DependencyContainer
    {
        public string programDir;

        public DependencyContainer()
        {
            programDir = AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
