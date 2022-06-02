﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatsForUser : ContentPage
    {
        public StatsForUser()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();


        }

        #region Shows history of products for a user
        public async Task<List<TopProduct>> GetStatsforUser(ActivityIndicator indicator)
        {

            indicator.IsVisible = true;
            indicator.IsRunning = true;

            Task<List<TopProduct>> databaseTask = Task<List<TopProduct>>.Factory.StartNew(() =>
            {
                List<TopProduct> tops = null;
                {

                    using (var db = new Models.LibraryDBContext())
                    {
                        var username = LoginPage.Username;
                        var result = db.OrderDetails.Where(x => x.OrderId == username).ToList();

                        var rest = (from ob in result
                                    join prod in db.Products on ob.ProductId equals prod.Id
                                    select new TopProduct { ProductName = prod.ProductName, ReturnDate = ob.CustomerReturnBooked }).ToList();



                        return rest;
                    }

                }
            }
            );

            var taskResult = await databaseTask;

            indicator.IsRunning = false;
            indicator.IsVisible = false;

            return taskResult;
        }
        #endregion

        #region This method shows which products a user should be returned

        public async Task<List<TopProduct>> BooksToReturnForAUser(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<TopProduct>> databaseTask = Task<List<TopProduct>>.Factory.StartNew(() =>
            {
                List<TopProduct> result = null;
                {
                    using (var db = new Models.LibraryDBContext())
                    {

                        //var shopping = db.ShoppingCarts.Select(x => new TopProduct { OrderId = x.UserId, ReturnDate = x.ReturnDate });

                        var username = LoginPage.Username;
                        var userinfo = db.Users.Where(x => x.Username == LoginPage.Username).ToList();

                        var c = (from ob in userinfo
                                 join s in db.ShoppingCarts on ob.Username equals s.UserId
                                 join p in db.Products on s.ProductId equals p.Id
                                 select new TopProduct { ProductName = p.ProductName, ReturnDate = s.ReturnDate }).ToList();


                        return c;

                    }



                }
            }
            );

            var taskResult = await databaseTask;

            indicator.IsRunning = false;
            indicator.IsVisible = false;

            return taskResult;
        }
        #endregion
        private async void UserStats_Clicked(object sender, EventArgs e)
        {

            StatsforUser.IsVisible = true;
            MainThread.BeginInvokeOnMainThread(async () => { StatsforUser.ItemsSource = await GetStatsforUser(ActivityIndicator); });
        }
        public class TopProduct
        {
            public int? ProductID { get; set; }
            public int orderCount { get; set; }
            public string ProductName { get; set; }
            public string ProductInfo { get; set; }
            public string Image { get; set; }


            public string Type { get; set; }
            public int? Category { get; set; }
            public string CategoryName { get; set; }
            public string OrderId { get; set; }

            public DateTime? DateBooked { get; set; }
            public DateTime? ReturnDate { get; set; }

        }

        private void BooksStats_Clicked(object sender, EventArgs e)
        {

            MainThread.BeginInvokeOnMainThread(async () => { BooksToReturnUser.ItemsSource = await BooksToReturnForAUser(ActivityIndicator); });

        }
    }
}