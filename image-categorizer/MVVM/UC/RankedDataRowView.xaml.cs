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
using image_categorizer.MVVM.Model;

namespace image_categorizer.MVVM.View
{
    /// <summary>
    /// RankedDataRowView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RankedDataRowView : UserControl
    {
        public RankedDataRowView()
        {
            InitializeComponent();
        }
        public RankedDataModel DataModel {
            get { return (RankedDataModel)GetValue(DataModelProperty); }
            set { SetValue(DataModelProperty, value); }
        }
        public static readonly DependencyProperty DataModelProperty = DependencyProperty.Register(
            nameof(DataModel), typeof(RankedDataModel), typeof(RankedDataRowView), new PropertyMetadata(null));
    }
}
