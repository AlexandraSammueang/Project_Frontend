using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Libery_Frontend.Models;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        public async Task<IEnumerable<IGrouping<string, Product>>> SearchProductsAsync(string input)
        {
            Task<IEnumerable<IGrouping<string, Product>>> databaseTask = Task<IEnumerable<IGrouping<string, Product>>>.Factory.StartNew(() =>
            {
                IEnumerable<IGrouping<string, Product>> groupedResult = null;
                try
                {
                    using (var db = new Models.LibraryDBContext())
                    {
                        var query = from product in db.Products where product.ProductName.ToLower().Contains(input.ToLower())
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


                        Debug.WriteLine($"Found {grouped.Count()} Products");

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

        private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = e.NewTextValue;

            if (!string.IsNullOrEmpty(input))
            {
                ActivityIndicator.IsRunning = true;
                ActivityIndicator.IsVisible = true;

                var result  = await SearchProductsAsync(input);
                SearchListView.ItemsSource = result ?? null;

                ActivityIndicator.IsVisible = false;
                ActivityIndicator.IsRunning = false;
            }
            else
            {
                SearchListView.ItemsSource = null;
            }
        }
    }
}