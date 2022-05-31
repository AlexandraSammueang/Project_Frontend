using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Libery_Frontend.SecondModels;


namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class E_Media : ContentPage
    {
        
        public List<Product> Products;
        public List<ProductType> ProdType;
        public E_Media()
        {
            InitializeComponent();
        }


        public async Task<List<ProductModel>> GetProductsAsync()
        {
            Task<List<ProductModel>> databaseTask = Task<List<ProductModel>>.Factory.StartNew(() =>
            {
                List<ProductModel> result = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {
                        Products = db.Products.ToList();
                        ProdType = db.ProductTypes.ToList();

                       
                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) => new ProductModel { Image = p.Image, Name = p.ProductName, Info = p.ProductInfo, Type = pi.Type, InfoConcat = p.ProductInfo }).ToList();
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].InfoConcat = String.Concat(result[i].Info.Substring(0, 100), "...");
                        }
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


            return taskResult;
        }

        public async Task<List<ProductModel>> GetBooksAsync()
        {

            Task<List<ProductModel>> databaseTask = Task<List<ProductModel>>.Factory.StartNew(() =>
            {
                List<ProductModel> result = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {
                        Products = db.Products.Where(x => x.ProductTypeId == 3).ToList();
                        ProdType = db.ProductTypes.ToList();

                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) => new ProductModel { Image = p.Image, Name = p.ProductName, Info = p.ProductInfo, Type = pi.Type, InfoConcat = p.ProductInfo }).ToList();
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].InfoConcat = String.Concat(result[i].Info.Substring(0, 100), "...");
                        }
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

            return taskResult;
        }
        public async Task<List<ProductModel>> GetMovieAsync()
        {

            Task<List<ProductModel>> databaseTask = Task<List<ProductModel>>.Factory.StartNew(() =>
            {
                List<ProductModel> result = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {
                        Products = db.Products.Where(x => x.ProductTypeId == 2).ToList();
                        ProdType = db.ProductTypes.ToList();

                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) => new ProductModel { Image = p.Image, Name = p.ProductName, Info = p.ProductInfo, Type = pi.Type, InfoConcat = p.ProductInfo }).ToList();
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].InfoConcat = String.Concat(result[i].Info.Substring(0, 100), "...");
                        }
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

            return taskResult;
        }

        private async void BookProductButton_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Inloggning krävs", "Du måste logga in för att kunna boka en produkt.\n Vill du logga in?", "Logga in", "Avbryt");
            if (answer)
            {
                var tab = new MainPage();
                tab.CurrentPage = tab.Children[4];

                await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(tab));
            }
            else return;
        }

        private async void AllProdButton_Clicked(object sender, EventArgs e)
        {
            FirstListView.Opacity = 0;
            FirstListView.IsVisible = true;

            FirstListView.ItemsSource = await GetProductsAsync();
            await Task.WhenAll(FirstListView.FadeTo(1, 1000), SecondListView.FadeTo(0, 500), ThirdListView.FadeTo(0, 500));

            SecondListView.IsVisible = false;
            ThirdListView.IsVisible = false;

        }
        private async void EBooksButton_Clicked(object sender, EventArgs e)
        {
            SecondListView.Opacity = 0;
            SecondListView.IsVisible = true;
            MainThread.BeginInvokeOnMainThread(async () => { SecondListView.ItemsSource = await GetBooksAsync(); });

            await Task.WhenAll(SecondListView.FadeTo(1, 1000), FirstListView.FadeTo(0, 500), ThirdListView.FadeTo(0, 500));

            FirstListView.IsVisible = false;
            ThirdListView.IsVisible = false;
        }

        private async void EMoviesButton_Clicked(object sender, EventArgs e)
        {
            ThirdListView.Opacity = 0;
            ThirdListView.IsVisible = true;
            MainThread.BeginInvokeOnMainThread(async () => { ThirdListView.ItemsSource = await GetMovieAsync(); });

            await Task.WhenAll(ThirdListView.FadeTo(1, 1000), SecondListView.FadeTo(0, 500), FirstListView.FadeTo(0, 500));

            SecondListView.IsVisible = false;
            FirstListView.IsVisible = false;
        }
    }


}