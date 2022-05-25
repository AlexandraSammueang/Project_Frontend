using Dapper;
using Libery_Frontend.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;


namespace Libery_Frontend.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Stats : ContentPage
    {
        public List<OrderDetail> orderDetails;
        public List<Product> produtcs;

        
        public Stats()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
           

        }


        public async Task<List<TopProduct>> GetTopProductAsync(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<TopProduct>> databaseTask = Task<List<TopProduct>>.Factory.StartNew(() =>
            {
                List<TopProduct> result = null;
                {
                    using (var db = new Models.LibraryDBContext())
                    {

                        orderDetails = db.OrderDetails.ToList();
                        produtcs = db.Products.ToList();

                         var newresult = db.OrderDetails.ToList()
                            .GroupBy(l => l.ProductId)
                                  .Select(g => new TopProduct
                                  {
                                      ProductID = g.Key,
                                      orderCount = g.Select(l => l.ProductId).Count()
                                  });
                        var top3 = newresult.OrderByDescending(x => x.orderCount).Take(3).ToList();

                        var top3withname = (from ob in top3
                                            join prod in db.Products on ob.ProductID equals prod.Id
                                            join type in db.ProductTypes on prod.CategoryId equals type.Id
                                            
                                           select new TopProduct { ProductID = ob.ProductID, orderCount = ob.orderCount, ProductName = prod.ProductName, Image = prod.Image, ProductInfo = prod.ProductInfo }).ToList();

                        return top3withname;

                    }



                }
            }
            );

            var taskResult = await databaseTask;

            indicator.IsRunning = false;
            indicator.IsVisible = false;

            return taskResult;
        }


        public async Task<List<TopProduct>> GetLessLendProductAsync(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<TopProduct>> databaseTask = Task<List<TopProduct>>.Factory.StartNew(() =>
            {
                List<TopProduct> result = null;
                {
                    using (var db = new Models.LibraryDBContext())
                    {

                        orderDetails = db.OrderDetails.ToList();
                        produtcs = db.Products.ToList();

                        var newresult = db.OrderDetails.ToList()
                           .GroupBy(l => l.ProductId)
                                 .Select(g => new TopProduct
                                 {
                                     ProductID = g.Key,
                                     orderCount = g.Select(l => l.ProductId).Count()
                                 });
                        var top3 = newresult.OrderBy(x => x.orderCount).Take(3).ToList();

                        var top3withname = (from ob in top3
                                            join prod in db.Products on ob.ProductID equals prod.Id
                                           

                                            select new TopProduct { ProductID = ob.ProductID, orderCount = ob.orderCount, ProductName = prod.ProductName, Image =prod.Image, ProductInfo = prod.ProductInfo }).ToList();

                        return top3withname;

                    }



                }
            }
            );

            var taskResult = await databaseTask;

            indicator.IsRunning = false;
            indicator.IsVisible = false;

            return taskResult;
        }

        public class TopProduct
        {
            public int? ProductID { get; set; }
            public int orderCount { get; set; }
            public string ProductName { get; set; }
            public string ProductInfo { get; set; }
            public string Image { get; set; }

        }

        private async void TopProduct_Clicked(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => { StatsListView.ItemsSource = await GetTopProductAsync(ActivityIndicator); });

        }

        private async void LastProduct_Clicked(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => { StatsListView.ItemsSource = await GetLessLendProductAsync(ActivityIndicator); });

        }
    }
   

}