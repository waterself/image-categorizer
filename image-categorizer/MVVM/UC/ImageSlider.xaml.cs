using image_categorizer.Core;
using image_categorizer.MVVM.UC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace image_categorizer.MVVM.UC
{
    /// <summary>
    /// ImageSlider.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageSlider : UserControl
    {
        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value >= 0 && value < ImageSources.Count)
                {
                    _selectedIndex = value;
                }
                else if (value >= ImageSources.Count)
                {
                    _selectedIndex = ImageSources.Count - 1;
                }
                else
                {
                    _selectedIndex = 0;
                }
            }
        }
        /* public bool IsLeftButtonEnable { get; set; }
         public bool IsRightButtonEnable { get; set; }*/


        public bool IsLeftButtonEnable
        {
            get { return (bool)GetValue(IsLeftButtonEnableProperty); }
            set { SetValue(IsLeftButtonEnableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsLeftButtonEnable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLeftButtonEnableProperty =
            DependencyProperty.Register("IsLeftButtonEnable", typeof(bool), typeof(ImageSlider), new PropertyMetadata(null));


        public bool IsRightButtonEnable
        {
            get { return (bool)GetValue(IsRightButtonEnableProperty); }
            set { SetValue(IsRightButtonEnableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRightButtonEnable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRightButtonEnableProperty =
            DependencyProperty.Register("IsRightButtonEnable", typeof(bool), typeof(ImageSlider), new PropertyMetadata(null));




        public ImageSlider()
        {
            _selectedIndex = 0;
            NextButtonCommand = NextCommand();
            PreviousButtonCommand = PreviousCommand();
            IsLeftButtonEnable = false;
            IsRightButtonEnable = true;
            InitializeComponent();
        }



        public List<string> ImageSources
        {
            get { return (List<string>)GetValue(ImageSourcesProperty); }
            set { SetValue(ImageSourcesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSources.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourcesProperty =
            DependencyProperty.Register("ImageSources", typeof(List<string>), typeof(ImageSlider), new PropertyMetadata(new List<string>()));




        public string? CurrentImageSource
        {
            get { return (string?)GetValue(CurrentImageSourceProperty); }
            set { SetValue(CurrentImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentImageSourceProperty =
            DependencyProperty.Register("CurrentImageSource", typeof(string), typeof(ImageSlider), new PropertyMetadata(null));



        public string? PreviousImageSource
        {
            get { return (string?)GetValue(PreviousImageSourceProperty); }
            set { SetValue(PreviousImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviousImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviousImageSourceProperty =
            DependencyProperty.Register("PreviousImageSource", typeof(string), typeof(ImageSlider), new PropertyMetadata(null));



        public string? NextImageSource
        {
            get { return (string?)GetValue(NextImageSourceProperty); }
            set { SetValue(NextImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NextImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NextImageSourceProperty =
            DependencyProperty.Register("NextImageSource", typeof(string), typeof(ImageSlider), new PropertyMetadata(null));



        public RelayCommand NextButtonCommand { get; set; }
        public RelayCommand PreviousButtonCommand { get; set; }
        private RelayCommand NextCommand()
        {
            RelayCommand ret = new RelayCommand(o =>
            {
                NextImageSource = FindValueAtIndex(ImageSources, SelectedIndex + 2);
                CurrentImageSource = FindValueAtIndex(ImageSources, SelectedIndex + 1);
                PreviousImageSource = FindValueAtIndex(ImageSources, SelectedIndex);
                SelectedIndex += 1;
                CheckButtonEnable();
            });
            return ret;
        }
        private RelayCommand PreviousCommand()
        {
            RelayCommand ret = new RelayCommand(o =>
            {
                NextImageSource = FindValueAtIndex(ImageSources, SelectedIndex - 2);
                CurrentImageSource = FindValueAtIndex(ImageSources, SelectedIndex - 1);
                PreviousImageSource = FindValueAtIndex(ImageSources, SelectedIndex);
                SelectedIndex -= 1;
                CheckButtonEnable(); // need OnPropertyChange
            });
            return ret;
        }
        private void CheckButtonEnable() {
            IsLeftButtonEnable = (SelectedIndex > 0) ? true : false;
            IsRightButtonEnable = (SelectedIndex < ImageSources.Count - 1) ? true : false;
        }

        private string? FindValueAtIndex(List<string> list, int index)
        {
            if (index < 0)
            {
                return null;
            }
            else if (index > list.Count - 1)
            {
                return null;
            }
            else return list[index];
        }
    }
}
