using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_categorizer.MVVM.Model
{
    public class InsertQueryModel
    {
        public string fileOutputPath;
        public string dateTime;
        public string format;
        public string cameraModel;
        public string location;
        public string currentTime;

        public InsertQueryModel(string fileOutputPath, string dateTime, string format, string cameraModel, string location, string currentTime)
        {
            this.fileOutputPath = fileOutputPath;
            this.dateTime = dateTime;
            this.format = format;
            this.cameraModel = cameraModel = (cameraModel == null || cameraModel == "") ? "No Camera Data" : this.cameraModel = cameraModel;
            this.location = (location == null || location == "/") ? "No Location Data" : this.location = location;
            this.currentTime = currentTime;
        }
    }
}
