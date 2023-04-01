using image_categorizer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_categorizer.MVVM.Model
{
    public class OnBoardingModel : ObservableObject
    {
        

        private string? _currenImageSource;

        public string? CurrentImageSource
        {
            get { return _currenImageSource; }
            set
            {
                _currenImageSource = value;
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


        private List<string>? _onboardingImages;

        public List<string>? OnboardingImages
        {
            get { return _onboardingImages; }
            set { _onboardingImages = value; }
        }
        private bool _isLeftButtonEnable;

        public bool IsLeftButtonEnable
        {
            get { return _isLeftButtonEnable; }
            set
            {
                _isLeftButtonEnable = value;
                OnPropertyChanged();
            }
        }

        private bool _isRightButtonEnable;

        public OnBoardingModel(List<string> onboardingImages, bool isLeftButtonEnable, bool isRightButtonEnable)
        {
            _onboardingImages = onboardingImages;
            _isLeftButtonEnable = isLeftButtonEnable;
            _isRightButtonEnable = isRightButtonEnable;
            _currenImageSource = onboardingImages.Count > 0 ? onboardingImages[0] : null;
            _previousImageSource = null;
            _nextImageSource = onboardingImages.Count > 1 ? onboardingImages[1] : null;
        }

        public bool IsRightButtonEnable
        {
            get { return _isRightButtonEnable; }
            set { _isRightButtonEnable = value; }
        }
    }
}
