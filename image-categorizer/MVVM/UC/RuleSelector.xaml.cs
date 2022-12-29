﻿using System;
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
        //boolean array to control combobox's isEnable

        public string? NameLabel {
            get { return GetValue(NameLabelProperty) as string; }
            set { SetValue(NameLabelProperty, value); }
        }

        public ObservableCollection<string>? RulesForComboBox
        {
            get { return GetValue(RulesForComboBoxProperty) as ObservableCollection<string>; }
            set { SetValue(RulesForComboBoxProperty, value); }
        }
        public int[] ComboBoxSelectedindex
        {
            get { return GetValue(ComboBoxSelectedIndexProperty) as int[]; }
            set { SetValue(ComboBoxSelectedIndexProperty, value); }
        }
        public string[]? RulesArray
        {
            get { return (string[]?)GetValue(RulesArrayProperty); }
            set { SetValue(RulesArrayProperty, value); }
        }
        public static readonly DependencyProperty NameLabelProperty = DependencyProperty.Register(
            nameof(NameLabel), typeof(string), typeof(RuleSelector), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty RulesArrayProperty = DependencyProperty.Register(
       nameof(RulesArray), typeof(string[]), typeof(RuleSelector), new PropertyMetadata(new string[4] {"None", "None", "None", "None" }));

        public static readonly DependencyProperty RulesForComboBoxProperty = DependencyProperty.Register(
            nameof(RulesForComboBox), typeof(ObservableCollection<string>), typeof(RuleSelector), new PropertyMetadata(null));

        public static readonly DependencyProperty ComboBoxSelectedIndexProperty = DependencyProperty.Register(
            nameof(ComboBoxSelectedindex), typeof(int[]), typeof(RuleSelector), new PropertyMetadata(new int[4] { 0, 0, 0, 0 }));
            
    }
}