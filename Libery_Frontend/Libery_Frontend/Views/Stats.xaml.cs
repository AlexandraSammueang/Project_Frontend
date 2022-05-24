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

        static string connstring = "Server=tcp:newtonlibrary.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=teammars;Password=!ilY7e&L$X6Sbr6;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        public Stats()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () => { StatsListView.ItemsSource = await GetProductsAsync(ActivityIndicator); });


        }
        //public static List<OrderDetail> orders()
        //{
        //    var orders = new List<OrderDetail>();
        //    string sql = "SELECT TOP(3) COUNT(OrderDetails.ProductID) AS antal" +
        //                "FROM OrderDetails " +
        //                "Where ProductID =10"
        //                ;
        //    List<OrderDetail> orderDetails = new List<OrderDetail>();
        //    using (var connection = new SqlConnection(connstring))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            orderDetails = connection.Query<OrderDetail>(sql).ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //        return orders;
        //    }
        //}

        public async Task<List<TopProduct>> GetProductsAsync(ActivityIndicator indicator)
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

                        //result = db.OrderDetails.Join(produtcs, p=> p.ProductId, pi => pi.ProductName,(p,pi) => p.ProductId).ToList();
                        //result = db.OrderDetails.Select(x => new OrderDetail { ProductId = x.ProductId }).ToList();
                        //var resulttmp = from order in db.OrderDetails
                        //         select order;
                        //var resulttmp2 = resulttmp.GroupBy(x => x.ProductId).OrderByDescending(x=> x.Count());
                        Debug.WriteLine(".............................");

                        var newresult = db.OrderDetails.ToList()
                            .GroupBy(l => l.ProductId)
                                  .Select(g => new TopProduct
                                  {
                                      ProductID = g.Key,
                                      orderCount = g.Select(l => l.ProductId).Count()
                                  });
                        var top3 = newresult.OrderByDescending(x => x.orderCount).Take(3).ToList();

                        var top3withname = (from ob in top3
                                            
                                           select new TopProduct { ProductID = ob.ProductID, orderCount = ob.orderCount, ProductName = db.Products.Where(x=> x.Id==ob.ProductID).FirstOrDefault().ProductName }).ToList();

                        //Debug.WriteLine(top3.FirstOrDefault().orderCount.ToString() ?? "Hej");



                        //var top3 = resulttmp2.Take(3).ToList();



                        //foreach (var item in top3)
                        //{

                        //    Debug.WriteLine(item.ToString());
                        //}
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
        }


    }
   

}