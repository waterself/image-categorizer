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
using image_categorizer.MVVM.Model;

namespace image_categorizer.MVVM.View
{
    /// <summary>
    /// RankedDataView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RankedDataView : UserControl
    {
        public RankedDataView()
        {
            InitializeComponent();
        }
        public ObservableCollection<RankedDataModel> RankedData {
            get { return (ObservableCollection<RankedDataModel>)GetValue(RankedDataProperty); }
            set { SetValue(RankedDataProperty, value); }
        }

        public static readonly DependencyProperty RankedDataProperty = DependencyProperty.Register(
            nameof(RankedData), typeof(ObservableCollection<RankedDataModel>), typeof(RankedDataView), new PropertyMetadata(null));
    }
}
