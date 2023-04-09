using image_categorizer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_categorizer.MVVM.ViewModel
{
    public class LicenseViewModel : BaseViewModel
    {
        public Uri GeoNameUri { get; set; } = new("https://www.geonames.org/");

        public void HyperLinkNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
        }

    }
}
