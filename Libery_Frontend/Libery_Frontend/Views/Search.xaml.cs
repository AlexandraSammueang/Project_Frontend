using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Libery_Frontend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;


namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Search : ContentPage
    {
       
        public List<Models.Product> Products;

        public Search()
        {
            InitializeComponent();

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

         // MainThread.BeginInvokeOnMainThread(async () => { ProductListView.ItemsSource = await GetProductsAsync(ActivityIndicator); });
        }
        private async void SearchButtom_Clicked(object sender, System.EventArgs e)
        {
            // var userPassword = new List<Models.Product>();
            string searchprod = SearchEntry.Text;

            using (var db = new Models.LibraryDBContext())
            {


                try
                {
                    var products = db.Products.ToList();
                  
                    var searchproducts = products.Where(x => x.ProductName.Contains(searchprod)).ToList();

                    db.Add(products);
                    db.SaveChanges();

                }
                catch (Exception)
                {
                    await DisplayAlert("Inga resultat", "Försök söka igen", "OK");
                }
            }
        }


    }
}

//from prod in products where prod.ProductName.Contains(searchProd) orderby prod.ProductName //descending  select prod.Id + " " + prod.ProductName.ToUpper();
