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
    public partial class AddAndDelete : ContentPage
    {
        public List<Models.Author> Authors;

        public AddAndDelete()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            using (var db = new Models.LibraryDBContext())
            {
                Authors = db.Authors.ToList();
            }
        }

        private async void AddButton_Clicked(object sender, System.EventArgs e)
        {
            using (var db = new Models.LibraryDBContext())
            {
                var newProduct = new Product
                {
                    ProductName = ProductNameEntry.Text,
                    ProductInfo = ProductInfoEntry.Text,
                    Isbn = ISBNEntry.Text,
                    //AuthorId = Convert.ToInt32(ProductInfoEntry.Text),
                    AuthorId = Authors.Last().Id,
                };

                try
                {
                    db.Add(newProduct);
                    db.SaveChanges();
                    
                    var svar = await DisplayAlert("Lägga till en produkt", "Är du helt säker?", "Ja", "Nej");
                   

                    
                }
                catch (Exception)
                {
                    Console.WriteLine("Ingen produkt tillagd");
                }
            }
        }
    }
}
