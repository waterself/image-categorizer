using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_categorizer.MVVM.Model
{
    internal class SettingModel
    {
        private bool _cameraModelEnable;
        public bool CameraModelEnable
        {
            get { return _cameraModelEnable; }
            set { _cameraModelEnable = value; }
        }

        private bool _LocationEnable;
        public bool LocationEnable
        {
            get { return _LocationEnable; }
            set { _LocationEnable = value; }
        }

        private bool _dateTakenEnable;

        public bool DateTakenEnable
        {
            get { return _dateTakenEnable; }
            set { _dateTakenEnable = value; }
        }

        private bool _formatEnable;

        public bool FormatEnable
        {
            get { return _formatEnable; }
            set { _formatEnable = value; }
        }


    }
}
