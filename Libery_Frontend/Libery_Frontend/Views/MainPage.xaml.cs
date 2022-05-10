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

        public MainPage(int index)
        {
            if (index == 1)
            {
                //NavigationPage pag = new NavigationPage(new LibrarianPage());
                //var pagadd = this;
                //pag.Title = "testsida";
                //pagadd.Children.Add(pag);
                //Children.Add(pag);
                //Children.Add(new LibrarianPage());


                Page pag = new LibrarianPage();
                pag.Title = "test";
                Children.Add(pag);

            }
            
            InitializeComponent();
        }
        public MainPage()
        {
            //NavigationPage navpag = new NavigationPage(new LibrarianPage());
            //navpag.Title = "testsida";
            //Children.Add(navpag);

            //Page pag = new LibrarianPage();
            //pag.Title = "aaaaaaaaaa";
            //Children.Add(pag);

            InitializeComponent();
        }
    }
}