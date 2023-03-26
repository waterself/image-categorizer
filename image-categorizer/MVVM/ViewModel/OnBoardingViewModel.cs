using image_categorizer.Core;
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
        public OnBoardingViewModel() {
            _onboardingImages = new() {
                /*"pack://application:,,,/image-categorizer/Assets/RunView.png",
                "pack://application:,,,/image-categorizer/Assets/SettingView.png",
                "pack://application:,,,/image-categorizer/Assets/SummaryView.png"*/
                @"\Assets\OnBoardingAssets\RunView.png",
                @"\Assets\OnBoardingAssets\SettingView.png",
                @"\Assets\OnBoardingAssets\SummaryView.png"

        };
            CurrentImageSource = OnboardingImages.First();
        }

        private string? _currenImageSource;
            
        public string? CurrentImageSource
        {
            get { return _currenImageSource; }
            set { _currenImageSource = value;
                OnPropertyChanged();
            }
        }

        private string? _previousImageSource;

        public string? PreviousImageSource
        {
            get { return _previousImageSource; }
            set { _previousImageSource = value; }
        }
        private string? _nextImageSource;

        public string? NextImageSource
        {
            get { return _nextImageSource; }
            set { _nextImageSource = value; }
        }


        private List<string> _onboardingImages;

        public List<string> OnboardingImages
        {
            get { return _onboardingImages; }
            set { _onboardingImages = value; }
        }

    }
}
