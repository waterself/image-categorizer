using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_categorizer.MVVM.Model
{
    class ImageDetails
    {
        public string? FileName { get; set; }
        public string? DateTaken { get; set; }
        public string? TimeTaken { get; set; }
        public string? Location { get; set; }
        public string? CameraModel { get; set; }
        public string? Format { get; set; }

    }
}
