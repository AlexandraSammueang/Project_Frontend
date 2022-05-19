using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Libery_Frontend.Models;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookPage : ContentPage
    {


        public List<Models.Product> Products;
        public List<Models.ProductType> ProdType;

        public BookPage()
        {
            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load products asynchronously
            MainThread.BeginInvokeOnMainThread(async () => { BookListView.ItemsSource = await GetProductsAsync(ActivityIndicator); });
        }

        public async Task<List<ProductModel>> GetProductsAsync(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<ProductModel>> databaseTask = Task<List<ProductModel>>.Factory.StartNew(() =>
            {
                List<ProductModel> result = null;
                try
                {
                    using (var db = new Models.LibraryDBContext())
                    {
                        Products = db.Products.Where(x => x.EVersion == false).ToList();
                        ProdType = db.ProductTypes.ToList();

                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) => new ProductModel { Image = p.Image, Name = p.ProductName, Info = p.ProductInfo, Type = pi.Type }).ToList();
                    }


                }


                catch (Exception ex)
                {
                    // Display modal for error
                }
                return result;
            }
            );

            var taskResult = await databaseTask;

            indicator.IsRunning = false;
            indicator.IsVisible = false;

            return taskResult;
        }
        private async void BookProductButton_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Inloggning krävs", "Du måste logga in för att kunna boka en produkt.\n Vill du logga in?", "Logga in", "Avbryt");
            if (answer)
            {
                var tab = new MainPage();
                tab.CurrentPage = tab.Children[5];

                await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(tab));
            }
            else return;
        }

        private async void BookProductButton_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Inloggning krävs", "Du måste logga in för att kunna boka en produkt.\n Vill du logga in?", "Logga in", "Avbryt");
            if (answer)
            {
                var tab = new MainPage();
                tab.CurrentPage = tab.Children[5];

                await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(tab));
            }
            else return;
        }

    }
}
    
