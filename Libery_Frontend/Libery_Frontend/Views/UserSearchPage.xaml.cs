﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using System.Globalization;
using Xamarin.Essentials;
using Libery_Frontend.SecondModels;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserSearchPage : ContentPage
    {
        private CancellationTokenSource _tokenSource;
        public List<Product> Products;
        public List<ProductType> ProdType;
        public List<ShoppingCart> ShoppingCarts;
        public UserSearchPage()
        {
            InitializeComponent();
        }

        public int i = 0;

        public async Task<IEnumerable<IGrouping<string, Product>>> SearchProductsAsync(string input)
        {
            Task<IEnumerable<IGrouping<string, Product>>> databaseTask = Task<IEnumerable<IGrouping<string, Product>>>.Factory.StartNew(() =>
            {
                IEnumerable<IGrouping<string, Product>> groupedResult = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {
                        var query = from product in db.Products
                                    where product.ProductName.ToLower().Contains(input.ToLower())
                                    join prodType in db.ProductTypes on product.ProductType.Id equals prodType.Id

                                    select new
                                    {
                                        ProductType = prodType.Type,
                                        Product = product
                                    };

                        var grouped = from item in query.ToList()
                                      group item.Product by item.ProductType into g
                                      select g;
                        //select new GroupedProducts { ProductType = g.Key, Products = g.ToList() };

                        groupedResult = grouped;
                    }
                }

                catch (Exception ex)
                {
                    // Display modal for error
                }
                return groupedResult;
            }
            );

            var taskResult = await databaseTask;

            return taskResult;
        }

        public async Task Search(String input)
        {

            await Task.Delay(600);

            if (!input.Equals(SearchBarInput.Text))
            {
                return;
            }

            if (!string.IsNullOrEmpty(input))
            {
                SearchListView.BeginRefresh();
                ActivityIndicator.IsRunning = true;
                ActivityIndicator.IsVisible = true;

                var result = await SearchProductsAsync(input);
                SearchListView.ItemsSource = result ?? null;

                ActivityIndicator.IsVisible = false;
                ActivityIndicator.IsRunning = false;
                SearchListView.EndRefresh();
            }
            else
            {
                SearchListView.ItemsSource = null;
            }
        }

        private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = e.NewTextValue;
            await Search(input);

        }

        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            string input = SearchBarInput.Text;
            await Search(input);
        }

        private async void BookProductButton_Clicked_1(object sender, EventArgs e)
        {
            ShoppingCart cart = new ShoppingCart();
            Product item = SearchListView.SelectedItem as Product;
            DateTime returnDate = DateTime.Now.AddDays(30);
            CultureInfo dateTimeLanguage = CultureInfo.GetCultureInfo("sv-SE");

            if (item != null)
            {

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    using (var context = new LibraryDBContext())
                    {
                        ProdType = context.ProductTypes.ToList();
                        Products = context.Products.ToList();

                        cart.ProductId = item.Id;
                        cart.UserId = LoginPage.Username;
                        cart.DateBooked = DateTime.Now;
                        cart.ReturnDate = DateTime.Now.AddDays(30);

                        ShoppingCarts = context.ShoppingCarts.Where(x => x.ProductId == item.Id && x.UserId == LoginPage.Username).ToList();

                        if (ShoppingCarts.Any())
                        {
                            await DisplayAlert("Redan bokad", "Du har redan bokat denna produkt", "OK");

                        }
                        else
                        {
                            context.Add(cart);
                            context.SaveChanges();

                            var typeOfProduct = context.ProductTypes.Where(x => x.Id == item.ProductTypeId).FirstOrDefault();
                            var ProdTypeName = new ProductType
                            {
                                Type = typeOfProduct.Type
                            };

                            await DisplayAlert($"{ProdTypeName.Type} bokad",
                                $"{item.ProductName} är bokad.\nLämnas tillbaks senast {returnDate.ToString("dddd, MMMM dd, yyyy", dateTimeLanguage)}", "OK");
                        }
                    }
                    SearchListView.SelectedItem = null;
                });
            }
            else
                await DisplayAlert("Produkt ej vald", "Välj en produkt för att boka", "OK");

        }
    }
}