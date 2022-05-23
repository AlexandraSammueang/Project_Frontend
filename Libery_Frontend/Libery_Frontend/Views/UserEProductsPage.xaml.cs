﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Libery_Frontend.SecondModels;
using System.Globalization;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserEProductsPage : ContentPage
    {

        public UserEProductsPage()
        {
            InitializeComponent();
        }
        public List<Product> Products;
        public List<ProductType> ProdType;
        public List<ShoppingCart> ShoppingCarts;



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
                    using (var db = new LibraryDBContext())
                    {
                        Products = db.Products.Where(x => x.EVersion == true).ToList();
                        ProdType = db.ProductTypes.ToList();

                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) => new ProductModel { Image = p.Image, Name = p.ProductName, Info = p.ProductInfo, Type = pi.Type, ProId = p.Id }).ToList();
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
        private void Books_Clicked(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => { BookListView.ItemsSource = await GetBooksAsync(ActivityIndicator); });

        }
        private async void BackToList_Clicked(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => { BookListView.ItemsSource = await GetProductsAsync(ActivityIndicator); });
        }



        private async void Movie_Clicked(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => { BookListView.ItemsSource = await GetMovieAsync(ActivityIndicator); });

        }

        public async Task<List<ProductModel>> GetBooksAsync(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<ProductModel>> databaseTask = Task<List<ProductModel>>.Factory.StartNew(() =>
            {
                List<ProductModel> result = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {
                        Products = db.Products.Where(x => x.ProductTypeId == 3).ToList();
                        ProdType = db.ProductTypes.ToList();

                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) => new ProductModel { Image = p.Image, Name = p.ProductName, Info = p.ProductInfo, Type = pi.Type, ProId = p.Id }).ToList();
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
        public async Task<List<ProductModel>> GetMovieAsync(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<ProductModel>> databaseTask = Task<List<ProductModel>>.Factory.StartNew(() =>
            {
                List<ProductModel> result = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {
                        Products = db.Products.Where(x => x.ProductTypeId == 2).ToList();
                        ProdType = db.ProductTypes.ToList();

                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) => new ProductModel { Image = p.Image, Name = p.ProductName, Info = p.ProductInfo, Type = pi.Type, ProId = p.Id }).ToList();
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
            ShoppingCart cart = new ShoppingCart();
            ProductModel item = BookListView.SelectedItem as ProductModel;
            DateTime returnDate = DateTime.Now.AddDays(30);
            CultureInfo dateTimeLanguage = CultureInfo.GetCultureInfo("sv-SE");

            if (item != null)
            {

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    using (var context = new LibraryDBContext())
                    {

                        cart.ProductId = item.ProId;
                        cart.UserId = LoginPage.Username;
                        cart.DateBooked = DateTime.Now;
                        cart.ReturnDate = DateTime.Now.AddDays(30);

                        ShoppingCarts = context.ShoppingCarts.Where(x => x.ProductId == item.ProId && x.UserId == LoginPage.Username).ToList();

                        if (ShoppingCarts.Any())
                        {
                            await DisplayAlert("Redan bokad", "Du har redan bokat denna produkt", "OK");

                        }
                        else
                        {
                            context.Add(cart);
                            context.SaveChanges();

                            var typeOfProduct = item.Type;
                            await DisplayAlert($"{typeOfProduct} bokad",
                                $"{item.Name} är bokad.\nLämnas tillbaks senast {returnDate.ToString("dddd, MMMM dd, yyyy", dateTimeLanguage)}", "OK");
                        }
                    }
                    BookListView.SelectedItem = null;
                });
            }
            else
                await DisplayAlert("Produkt ej vald", "Välj en produkt för att boka", "OK");


        }
    }




}

