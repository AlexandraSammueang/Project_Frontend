using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libery_Frontend.SecondModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateProductPage : ContentPage
    {
        public List<Product> Products;
        public List<ProductType> ProdType;
        public List<AuthorName> aut;
        public ProductModel pModel;
        public UpdateProductPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load products asynchronously
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                using (var db = new LibraryDBContext())
                {
                    AuthorIDPicker.ItemsSource = db.Authors.Select(x => new AuthorName { Firstname = x.Firstname, Lastname = x.Lastname, AuthorId = x.Id }).ToList();
                    ProductTypePicker.ItemsSource = db.ProductTypes.Select(x => new ProductType { Id = x.Id, Type = x.Type }).ToList();
                    CategoryIDPicker.ItemsSource = db.ProductCategories.Select(x => new ProductCategory { Id = x.Id, Category = x.Category }).ToList();
                }
                ProductListView.ItemsSource = await GetProductsAsync(ActivityIndicator); 
            });
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

                        Products = db.Products.ToList();
                        ProdType = db.ProductTypes.ToList();

                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) => 
                        new ProductModel
                        { 
                            Image = p.Image,
                            Name = p.ProductName, 
                            Info = p.ProductInfo, 
                            Type = pi.Type
                        }).ToList();
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
        
        public async Task<List<ProductModel>> GetProductsFullListAsync(ActivityIndicator indicator, string prodName, string prodCategory)
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

                        Products = db.Products.ToList();
                        ProdType = db.ProductTypes.ToList();

                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) => 
                        new ProductModel
                        { 
                            Image = p.Image,
                            Name = p.ProductName, 
                            Info = p.ProductInfo, 
                            Type = pi.Type,
                            UnitPrice = p.Price,
                            AuthorID = p.AuthorId,
                            DirectorID = p.DirectorId,
                            ReleaseDate = p.ReleaseDate,
                            IsBookable = p.IsBookable,
                            CategoryID = p.CategoryId,
                            ISBN = p.Isbn
                        }).Where(x => x.Name == prodName && x.Type == prodCategory).ToList();
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

        private async void ProductListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ProductModel model = ProductListView.SelectedItem as ProductModel;
            SecondProductListView.ItemsSource = await GetProductsFullListAsync(ActivityIndicator, model.Name, model.Type);
            AllProdsFrame.IsVisible = false;
            SingleProdFrame.IsVisible = true;

        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            EntryView.IsVisible = true;
        }

        private void BackToListButton_Clicked(object sender, EventArgs e)
        {
            AllProdsFrame.IsVisible = true;
            DefaultFrameText.IsVisible = true;
            SingleProdFrame.IsVisible= false;
            EntryView.IsVisible = false;
        }

        private void UpdateButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}