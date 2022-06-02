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
        public List<ProductCategory> categories;
        public List<ShoppingCart> shoppings;
        

        public Stats()
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
                                        select new TopProduct { OrderId = ob.OrderId , ProductName = prod.ProductName,  }).ToList();



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


        #region This method shows which products whos has been most lend 
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
                                            join type in db.ProductCategories on prod.CategoryId equals type.Id
                                            join t in db.ProductTypes on prod.ProductTypeId equals t.Id


                                            select new TopProduct { ProductID = ob.ProductID, orderCount = ob.orderCount, Type = t.Type, CategoryName = type.Category, ProductName = prod.ProductName, Image = prod.Image, ProductInfo = prod.ProductInfo }).ToList();

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
        #endregion

        #region This method shows top category 
        public async Task<List<TopProduct>> GetTopCategoryAsync(ActivityIndicator indicator)
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
                        categories = db.ProductCategories.ToList();

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
                                            join category in db.ProductCategories on prod.CategoryId equals category.Id
                                            join t in db.ProductTypes on prod.ProductTypeId equals t.Id
                                            where prod.CategoryId != 0


                                            select new TopProduct { Type = t.Type, orderCount = ob.orderCount, CategoryName = category.Category }).ToList();

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
        #endregion

     
        #region This method shows which products a user should be returned

        public async Task<List<TopProduct>> BooksToReturnCategoryAsync(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<TopProduct>> databaseTask = Task<List<TopProduct>>.Factory.StartNew(() =>
            {
            List<TopProduct> result = null;
            {
                using (var db = new Models.LibraryDBContext())
                {

                    var shopping = db.ShoppingCarts.Select(x => new TopProduct { OrderId = x.UserId, DateBooked = x.DateBooked, ReturnDate = x.ReturnDate });

                    var cart = shopping.OrderBy(x => x.ReturnDate).ToList();

                    var c = (from ob in cart
                            select new TopProduct { OrderId = ob.OrderId, ReturnDate = ob.ReturnDate, DateBooked = ob.DateBooked }).ToList();
                       
                        
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

        #region This method shows which products has been least lend
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
                                            join type in db.ProductCategories on prod.CategoryId equals type.Id
                                            join t in db.ProductTypes on prod.ProductTypeId equals t.Id

                                            select new TopProduct { ProductID = ob.ProductID, orderCount = ob.orderCount, Type = t.Type, CategoryName = type.Category, ProductName = prod.ProductName, Image = prod.Image, ProductInfo = prod.ProductInfo }).ToList();

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
        #endregion

        #region This method shows which user who has lend the most products
        public async Task<List<TopProduct>> GetTopUserProductAsync(ActivityIndicator indicator)
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

                        var result1 = db.OrderDetails.ToList().GroupBy(x => x.OrderId).Select(l => new TopProduct { OrderId = l.Key, orderCount = l.Select(x => x.OrderId).Count() });

                        var newresult = result1.OrderByDescending(x => x.orderCount).Take(4).ToList();

                        var rest = (from ob in newresult
                                    select new TopProduct { OrderId = ob.OrderId }).ToList();



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

        private async void TopProduct_Clicked(object sender, EventArgs e)
        {
            ProductsListView.IsVisible = true;
            UserListView.IsVisible = false;
            CategoryListView.IsVisible = false;
            BooksToReturnListView.IsVisible = false;
            StatsforUser.IsVisible = false;
            MainThread.BeginInvokeOnMainThread(async () => { ProductsListView.ItemsSource = await GetTopProductAsync(ActivityIndicator); });

        }

        private async void LastProduct_Clicked(object sender, EventArgs e)
        {
            ProductsListView.IsVisible = true;
            UserListView.IsVisible = false;
            CategoryListView.IsVisible = false;
            BooksToReturnListView.IsVisible = false;
            StatsforUser.IsVisible = false;

            MainThread.BeginInvokeOnMainThread(async () => { ProductsListView.ItemsSource = await GetLessLendProductAsync(ActivityIndicator); });

        }

        private async void TopCategoryProduct_Clicked(object sender, EventArgs e)
        {
            CategoryListView.IsVisible = true;
            UserListView.IsVisible = false;
            ProductsListView.IsVisible = false;
            BooksToReturnListView.IsVisible = false;
            StatsforUser.IsVisible = false;

            MainThread.BeginInvokeOnMainThread(async () => { CategoryListView.ItemsSource = await GetTopCategoryAsync(ActivityIndicator); });

        }

        private async void TopUser_Clicked(object sender, EventArgs e)
        {
            ProductsListView.IsVisible = false;
            UserListView.IsVisible = true;
            CategoryListView.IsVisible = false;
            BooksToReturnListView.IsVisible = false;
            StatsforUser.IsVisible = false;
            MainThread.BeginInvokeOnMainThread(async () => { UserListView.ItemsSource = await GetTopUserProductAsync(ActivityIndicator); });

        }

        private async void BooksToReturnButton_Clicked(object sender, EventArgs e)
        {

            BooksToReturnListView.IsVisible = true;
            ProductsListView.IsVisible = false;
            CategoryListView.IsVisible = false;
            UserListView.IsVisible = false;
            StatsforUser.IsVisible = false;

            MainThread.BeginInvokeOnMainThread(async () => { BooksToReturnListView.ItemsSource = await BooksToReturnCategoryAsync(ActivityIndicator); });


        }

        private async void UserStats_Clicked(object sender, EventArgs e)
        {
            
            BooksToReturnListView.IsVisible = false;
            ProductsListView.IsVisible = false;
            CategoryListView.IsVisible = false;
            UserListView.IsVisible = false;
            StatsforUser.IsVisible = true;
            MainThread.BeginInvokeOnMainThread(async () => { StatsforUser.ItemsSource = await GetStatsforUser(ActivityIndicator); });
        }
    }


}