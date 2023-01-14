using System.Windows;
using System.Windows.Controls;
using image_categorizer.Core;

namespace image_categorizer.MVVM.UC
{
    /// <summary>
    /// PathSelector.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PathSelector : UserControl
    {
        public PathSelector()
        {
            InitializeComponent();
        }
        public string? PathSelectorNameLabel
        {
            get { return GetValue(PathSelectorNameLabelProperty) as string; }
            set { SetValue(PathSelectorNameLabelProperty, value); }
        }
        public string? DirectoryPath
        {
            get { return GetValue(DirectoryPathProperty) as string; } 
            set { SetValue(DirectoryPathProperty, value); } 
        }
        public RelayCommand SelectButtonCommand
        {
            get { return (RelayCommand)GetValue(SelectButtonCommandProperty); }
            set { SetValue(SelectButtonCommandProperty, value); }
        }

        public static readonly DependencyProperty PathSelectorNameLabelProperty = DependencyProperty.Register(
           nameof(PathSelectorNameLabel), typeof(string), typeof(PathSelector), new PropertyMetadata(null));

        public static readonly DependencyProperty DirectoryPathProperty = DependencyProperty.Register(
            nameof(DirectoryPath), typeof(string), typeof(PathSelector), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectButtonCommandProperty = DependencyProperty.Register(
            nameof(SelectButtonCommand), typeof(RelayCommand), typeof(PathSelector), new PropertyMetadata(null));



    }
}
