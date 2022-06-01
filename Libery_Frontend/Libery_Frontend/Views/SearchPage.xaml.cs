using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Libery_Frontend.SecondModels;
using System.Threading;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        private CancellationTokenSource _tokenSource;

        public SearchPage()
        {
            InitializeComponent();
        }

        public int i = 0;

        public async Task<IEnumerable<IGrouping<string, Product>>> SearchProductsAsync(string input)
        {
            Task<IEnumerable<IGrouping<string, Product>>> databaseTask = Task<
                IEnumerable<IGrouping<string, Product>>
            >.Factory.StartNew(
                () =>
                {
                    IEnumerable<IGrouping<string, Product>> groupedResult = null;
                    try
                    {
                        using (var db = new LibraryDBContext())
                        {
                            var query =
                                from product in db.Products
                                where product.ProductName.ToLower().Contains(input.ToLower())
                                join prodType in db.ProductTypes
                                    on product.ProductType.Id equals prodType.Id
                                select new { ProductType = prodType.Type, Product = product };

                            var grouped =
                                from item in query.ToList()
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

        private async void BookProductButton_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert(
                "Inloggning krävs",
                "Du måste logga in för att kunna låna en produkt.\n Vill du logga in?",
                "Logga in",
                "Avbryt"
            );
            if (answer)
            {
                var tab = new MainPage();
                tab.CurrentPage = tab.Children[4];

                await Application.Current.MainPage.Navigation.PushModalAsync(
                    new NavigationPage(tab)
                );
            }
            else
                return;
        }
    }
}
