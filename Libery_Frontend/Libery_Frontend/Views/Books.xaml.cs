using Libery_Frontend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Books : ContentPage
    {
        public List<Models.Product> Products;
        public List<Models.ProductType> ProdType;
        public Books()
        {
            InitializeComponent();

        }

        protected  override void OnAppearing()
        {
            base.OnAppearing();

            // Load products asynchronously
            MainThread.BeginInvokeOnMainThread(async () => { ProductListView.ItemsSource = await GetProductsAsync(ActivityIndicator); });
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

                        Products = db.Products.ToList();
                        ProdType = db.ProductTypes.ToList();

                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) => new ProductModel { Image = p.Image, Name = p.ProductName, Info = p.ProductInfo, Type = pi.Type, Stock = p.StockValue }).ToList();
                    }
                }

                catch (Exception ex)
                {
                    // Display modal for error
                }
                return result;
            }
            );

            var taskResult =  await databaseTask;

            indicator.IsRunning = false;
            indicator.IsVisible = false;

            return taskResult;
        }

        private async void LendFunction(object sender, EventArgs e)
        {
            bool input = await DisplayAlert("Bekräftelse", "vill du låna boken?", "Ja", "Nej"); 

            if (input) // om man trycker ja, ska boken lånas. och stock value ska sänkas med 1, när stock = 0 ska knappen inte kunna tryckas
            {
                
            }

            Debug.WriteLine(input);
        }
    }

    public class ProductModel
    {
        public string Image { get; set; } = default;
        public string Name { get; set; } = default;
        public string Info { get; set; } = default;
        public string Type { get; set; } = default;
        public int? Stock { get; set; } = default;

    }
}