﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// LicenseView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LicenseView : UserControl
    {
        public LicenseView()
        {
            InitializeComponent();
        }
        public void HyperLinkNavigate(object sender, RequestNavigateEventArgs e)
        {
            var uri = e.Uri.AbsoluteUri;
            var sInfo = new System.Diagnostics.ProcessStartInfo(uri)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
            e.Handled = true;
        }
    }
}
