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

namespace image_categorizer.MVVM.View
{
    /// <summary>
    /// RuleSelector.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RuleSelector : UserControl
    {
        public RuleSelector()
        {
            InitializeComponent();
        }
        public string LabelName { get; set; }

        private string[] _selectedRule = new string[4];

        public string[] SelectedRule
        {
            get { return _selectedRule; }
            set { _selectedRule = value; }
        }

        public static readonly DependencyProperty SelectedRuleProperty = DependencyProperty.Register(
               "SelectedRule", typeof(string[]), typeof(RuleSelector), new PropertyMetadata(null));
            
    }
}
