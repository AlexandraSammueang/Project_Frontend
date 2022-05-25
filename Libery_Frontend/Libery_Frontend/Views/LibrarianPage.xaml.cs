using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Libery_Frontend.Models;


namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LibrarianPage : ContentPage
    {
        public List<Models.Product> Products;
        public List<Models.ProductType> ProdType;
        public LibrarianPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();


            MainThread.BeginInvokeOnMainThread(async () => { ProductListView.ItemsSource = await GetProductAsync(ActivityIndicator); });
        }

        public async Task<List<ProductModel>> GetProductAsync(ActivityIndicator indicator)
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

                        Products = db.Products.ToList();
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

        private async void AddProdButton_Clicked(object sender, EventArgs e)
        {
            var Add = new AddAndDelete();

            RemoveProdFrame.IsVisible = false;
            UpdateProdFrame.IsVisible = false;
            //DefaultFrameText.IsVisible = false;

            AddProdFrame.IsVisible = true;

            //ProductModel item = ProductListView.SelectedItem as ProductModel;

            //if (item != null)
            //{
            //    using (var context = new Models.LibraryDBContext())
            //    {
            //        var personToUpdate = context.Products.Where(x => x.ProductName == item.Name).FirstOrDefault();
            //        personToUpdate.ProductName = "";
            //        context.SaveChanges();

            //        ProductListView.ItemsSource = await GetProductAsync(ActivityIndicator);
            //    }
            //}
            //else
            //{
            //    await DisplayAlert("Ingen product vald", "Välj en product att uppdatera", "OK");
            //}
        }

        private void RemoveProdButton_Clicked(object sender, EventArgs e)
        {
            AddProdFrame.IsVisible = false;
            UpdateProdFrame.IsVisible = false;
            //DefaultFrameText.IsVisible = false;

            RemoveProdFrame.IsVisible = true;

        }

        private void UpdateProdButton_Clicked(object sender, EventArgs e)
        {
            AddProdFrame.IsVisible = false;
            RemoveProdFrame.IsVisible = false;
            //DefaultFrameText.IsVisible = false;

            UpdateProdFrame.IsVisible = true;
        }

        private async void RemoveProductButton_Clicked(object sender, EventArgs e)
        {

            RemoveProdFrame.IsVisible = false;
            UpdateProdFrame.IsVisible = false;
            // DefaultFrameText.IsVisible = false;

            AddProdFrame.IsVisible = true;

            ProductModel item = ProductListView.SelectedItem as ProductModel;

            if (item != null)
            {
                using (var context = new Models.LibraryDBContext())
                {
                    var removePost = context.Products.Where(x => x.ProductName == item.Name).FirstOrDefault();
                    context.Products.Remove(removePost);
                    var svar = await DisplayAlert("Ta bort vald produkt", "Är du helt säker?", "Ja", "Nej");
                    if (svar == true)
                    {
                        context.SaveChanges();
                        ProductListView.ItemsSource = await GetProductAsync(ActivityIndicator);
                    }
                    else
                    { }



                }
            }
            else
            {
                await DisplayAlert("Ingen vald product", "Välj en product att ta bort", "Ok");
            }


        }
        public class ProductModel
        {
            public string Image { get; set; } = default;
            public string Name { get; set; } = default;
            public string Info { get; set; } = default;
            public string Type { get; set; } = default;
        }

        private void LogoutButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
