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
using image_categorizer.Core;

namespace image_categorizer.MVVM.View
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
        public string? NameLabel
        {
            get { return GetValue(NamelabelProperty) as string; }
            set { SetValue(NamelabelProperty, value); }
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

        public static readonly DependencyProperty NamelabelProperty = DependencyProperty.Register(
           nameof(NameLabel), typeof(string), typeof(PathSelector), new PropertyMetadata(null));

        public static readonly DependencyProperty DirectoryPathProperty = DependencyProperty.Register(
            nameof(DirectoryPath), typeof(string), typeof(PathSelector), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectButtonCommandProperty = DependencyProperty.Register(
            nameof(SelectButtonCommand), typeof(RelayCommand), typeof(PathSelector), new PropertyMetadata(null));



    }
}
