using Libery_Frontend.SecondModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserAccountPage : ContentPage
    {
        public List<Product> Products;
        public List<ProductType> ProdType;
        public List<User> Users;
        public List<ShoppingCart> ShoppingCarts;
        public UserAccountPage(string username)
        {
            InitializeComponent();
            BindingContext = username;
        }
        public UserAccountPage()
        {

            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load products asynchronously
            MainThread.BeginInvokeOnMainThread(async () => { ProductListView.ItemsSource = await GetProductsAsync(ActivityIndicator); });
        }

        public async Task<List<shoppingCartTestModel>> GetProductsAsync(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<shoppingCartTestModel>> databaseTask = Task<List<shoppingCartTestModel>>.Factory.StartNew(() =>
            {
                List<shoppingCartTestModel> cartResultone = null;
                using (var db = new LibraryDBContext())
                {
                    Products = db.Products.ToList();
                    ProdType = db.ProductTypes.ToList();
                    Users = db.Users.ToList();
                    ShoppingCarts = db.ShoppingCarts.ToList();


                    cartResultone = ShoppingCarts.Join(Products, p => p.ProductId, pi => pi.Id, (p, pi) => new shoppingCartTestModel
                    {
                        AccountName = p.UserId,
                        ID = p.Id,
                        AuthorID = (int)pi?.AuthorId,
                        ISBN = pi.Isbn,
                        ProductTypeID = (int)pi?.ProductTypeId,
                        ProductName = pi.ProductName,
                        CategoryID = (int)pi?.CategoryId,
                        ProductInfo = pi.ProductInfo,
                        ReleaseDate = pi.ReleaseDate.Value,
                        DateBooked = p.DateBooked.Value, //can use this syntax to get date
                        ReturnDate = (DateTime)p.ReturnDate //and this syntax
                    }).Where(x => x.AccountName == LoginPage.Username).ToList();

                    cartResultone = cartResultone.Join(ProdType, p => p.ProductTypeID, pi => pi.Id, (p, pi) => new shoppingCartTestModel
                    {
                        AccountName = p.AccountName,
                        ID = p.ID,
                        AuthorID = (int)p.AuthorID,
                        ISBN = p.ISBN,
                        ProductTypeID = (int)p.ProductTypeID,
                        ProductName = p.ProductName,
                        CategoryID = (int)p.CategoryID,
                        ProductInfo = p.ProductInfo,
                        ReleaseDate = p.ReleaseDate,
                        DateBooked = p.DateBooked,
                        ReturnDate = (DateTime)p.ReturnDate,
                        prodType = pi.Type
                    }).ToList();


                }
                return cartResultone;
            }
            );

            var taskResult = await databaseTask;

            indicator.IsRunning = false;
            indicator.IsVisible = false;

            return taskResult;
        }
    }

    public class shoppingCartTestModel
    {
        public int ID { get; set; } = default;
        public int AuthorID { get; set; } = default;
        public string ISBN { get; set; } = default;
        public int ProductTypeID { get; set; } = default;
        public int CategoryID { get; set; } = default;
        public string ProductName { get; set; } = default;
        public string ProductInfo { get; set; } = default;
        public DateTime ReleaseDate { get; set; } = default;
        public DateTime DateBooked { get; set; } = default;
        public DateTime ReturnDate { get; set; } = default;
        public string UserID { get; set; } = default;
        public int ProductID { get; set; } = default;
        public string AccountName { get; set; }
        public string prodType { get; set; }

    }
}