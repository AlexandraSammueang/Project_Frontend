﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libery_Frontend.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotACustomerProductPage : ContentPage
    {

        public List<Models.Product> Products;
        public List<Models.ProductType> ProdType;
        public NotACustomerProductPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
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
    }
}