using Libery_Frontend.Models;
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
    public partial class AddProduct : ContentPage
    {
        public List<ProductType> productTypes;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            using (var db = new Models.LibraryDBContext())
            {
                productTypes = db.ProductTypes.Select(x => new ProductType { Type = x.Type }).ToList();
                Allproducttype.ItemsSource = productTypes;
            }
        }
        public AddProduct()
        {
            InitializeComponent();
        }

        private async void AddProducttype_Clicked(object sender, EventArgs e)
        {
            using (var db = new Models.LibraryDBContext())
            {
                var newCategory = new ProductType
                {
                    Type = TypeEntry.Text,

                };
                var svar = await DisplayAlert($"Vill du lägga till {TypeEntry.Text} ?", "Är du säker?", "Ja", "Nej");
                if (svar == true)
                {
                    db.Add(newCategory);
                    db.SaveChanges();
                    TypeEntry.Text = "";


                }
                else { }

                productTypes = db.ProductTypes.Select(x => new ProductType { Type = x.Type }).ToList();
                Allproducttype.ItemsSource = productTypes;
            }
        }

        private void Allproducttype_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}