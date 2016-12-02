using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PlantomaticVM
{
    public partial class MoreInfoPage : ContentPage
    {
        public MoreInfoPage(string URL)
        {
            InitializeComponent();
            theWeb.Source = new UrlWebViewSource { Url= URL };

        }
    }
}
