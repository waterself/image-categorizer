using image_categorizer.Core;
using image_categorizer.MVVM.Model;
using System;
using System.Collections.Generic;
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
            OnBoardingModel = new OnBoardingModel(
                    onboardingImages: new() {
                    //need register to Properties/Resource.resx
                    @"\Assets\OnBoardingAssets\RunView.png",
                    @"\Assets\OnBoardingAssets\SettingView.png",
                    @"\Assets\OnBoardingAssets\SummaryView.png" },
                    isLeftButtonEnable: false,
                    isRightButtonEnable: true
                );
        }
    }
}
