using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            libraryPhoto.Source = $"https://static1biblioteket.stockholm.se/sites/default/files/Stadsbiblioteket-3.jpg";

            timeLabel.Text =
                           "MÅN  10:00-16:00 \n" +
                           "TIS  9:00-18:00 \n" +
                           "ONS  9:00-19:00 \n" +
                           "TOR  8:00-16:00 \n" +
                           "FRE  10:00-17:00 \n" +
                           "LÖR  11:00-15:00 \n" +
                           "SÖN  12:00-15:00";

        }

        
    }
}