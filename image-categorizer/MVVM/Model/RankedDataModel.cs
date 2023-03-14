using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_categorizer.MVVM.Model
{
     public class RankedDataModel
    {
        public RankedDataModel(int index = 0, string name = "", double rate = 0, int count=0)
        { 
            this.Count = count;
            this.Rate = rate;
            this.Name = name;
            this.Index = index;
        }
        public int Index { get; set; }
        public string? Name { get; set; }
        public double Rate { get; set; }
        public int Count { get; set; }
    }
}
