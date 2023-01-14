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
    /// CategorizePreviewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CategorizePreviewer : UserControl
    {
        public CategorizePreviewer()
        {
            InitializeComponent();
        }
        public string? CategorizePreViewerNameLabel
        {
            get { return (string)GetValue(CategorizePreViewerNamelabelProperty); }
            set { SetValue(CategorizePreViewerNamelabelProperty, value); }
        }

        public string  OldValue {
            get { return(string)GetValue(OldValueProperty); }
            set { SetValue(OldValueProperty, value); }
        }
        public string NewValue {
            get { return((string)GetValue(NewValueProperty)); }
            set { SetValue(NewValueProperty, value); }
        }

        public static readonly DependencyProperty CategorizePreViewerNamelabelProperty = DependencyProperty.Register(
           nameof(CategorizePreViewerNameLabel), typeof(string), typeof(PathSelector), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty OldValueProperty = DependencyProperty.Register(
            nameof(OldValue), typeof(string), typeof(CategorizePreviewer), new PropertyMetadata(null));

        public static readonly DependencyProperty NewValueProperty = DependencyProperty.Register(
            nameof(NewValue), typeof(string), typeof(CategorizePreviewer), new PropertyMetadata(null));

    }
}
