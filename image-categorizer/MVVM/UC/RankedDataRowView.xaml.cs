using System.Windows;
using System.Windows.Controls;
using image_categorizer.MVVM.Model;

namespace image_categorizer.MVVM.UC
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
        public RankedDataModel DataModel
        {
            get { return (RankedDataModel)GetValue(DataModelProperty); }
            set { SetValue(DataModelProperty, value); }
        }
        public static readonly DependencyProperty DataModelProperty = DependencyProperty.Register(
            nameof(DataModel), typeof(RankedDataModel), typeof(RankedDataRowView), new PropertyMetadata(null));
    }
}
