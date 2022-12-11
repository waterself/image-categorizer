using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public string? LabelName {
            get { return GetValue(LabelNameProperty) as string; }
            set { SetValue(LabelNameProperty, value); }
        }

        /*        public ObservableCollection<string>? RulesForComboBox
                {
                    get { return GetValue(RulesForComboBoxProperty) as ObservableCollection<string>; }
                    set { SetValue(RulesForComboBoxProperty, value); }
                }*/
        private ObservableCollection<string> _rulesForComboBox = new() {
            "None",
            "Date",
            "CameraModel",
            "Format",
            //"Location"
        };
        public ObservableCollection<string> RulesForComboBox
        {
            get { return _rulesForComboBox; }
            set { _rulesForComboBox = value; }
        }

        public string[]? RulesArray
        {
            get { return GetValue(RulesArrayProperty) as string[]; }
            set { SetValue(RulesArrayProperty, value); }
        }
        public static readonly DependencyProperty LabelNameProperty = DependencyProperty.Register(
            "LabelName", typeof(string), typeof(RuleSelector), new PropertyMetadata(string.Empty));

/*        public static readonly DependencyProperty RulesForComboBoxProperty = DependencyProperty.Register(
            "RulesForComboBox", typeof(ObservableCollection<string>), typeof(RuleSelector), new PropertyMetadata(null));*/

        public static readonly DependencyProperty RulesArrayProperty = DependencyProperty.Register(
               "RulesArray", typeof(string[]), typeof(RuleSelector), new PropertyMetadata(null));
            
    }
}
