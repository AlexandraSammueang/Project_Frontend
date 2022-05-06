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

            labelTest.Text =
                           "MÅN  9:00-18:00 \n" +
                           "TIS  9:00-18:00 \n" +
                           "ONS  9:00-18:00 \n" +
                           "TOR  9:00-18:00 \n" +
                           "FRE  9:00-18:00 \n" +
                           "LÖR  9:00-18:00 \n" +
                           "SÖN  9:00-18:00";

        }

        
    }
}