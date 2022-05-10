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
    public partial class MainPage : TabbedPage
    {

        public MainPage(string authority)
        {
            if (authority.ToLower() == "bib")
            {
                Page pag = new LibrarianPage();
                pag.Title = "Bibliotekarie";
                Children.Add(pag);
            }

            if (authority.ToLower() == "chef")
            {
                Page pag = new LibraryBossPage();
                pag.Title = "Chef";
                Children.Add(pag);
                var a = 0;
            }
            
            InitializeComponent();
        }
        public MainPage()
        {
           InitializeComponent();
        }
    }
}