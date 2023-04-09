using image_categorizer.Core;
using image_categorizer.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace image_categorizer.MVVM.ViewModel
{
    class OnBoardingViewModel : BaseViewModel
    {
        private OnBoardingModel? _onBoardingModel;

        public OnBoardingModel? OnBoardingModel
        {
            get { return _onBoardingModel; }
            set { _onBoardingModel = value; }
        }


        public OnBoardingViewModel()
        {
            List<string> files = new List<string>();
            int fileCount = 13;
            for (int i = 1; i <= fileCount; i++) {
                files.Add($"\\Assets\\OnBoardingAssets\\{i}.png");
            }
            OnBoardingModel = new OnBoardingModel(
                    onboardingImages: new(files),
                    isLeftButtonEnable: false,
                    isRightButtonEnable: true
                );
        }
    }
}
