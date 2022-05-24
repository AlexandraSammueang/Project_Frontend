using Libery_Frontend.SecondModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Libery_Frontend.Views;


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
                            Type = pi.Type,
                            ProId = p.Id,
                            Pages = p.BookPages,
                            AuthorID = p.AuthorId,
                            CategoryID = p.CategoryId,
                            ReleaseDate = p.ReleaseDate
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
                            ProId = p.Id,
                            Image = p.Image,
                            Name = p.ProductName,
                            Info = p.ProductInfo,
                            Type = pi.Type,
                            UnitPrice = p.Price,
                            AuthorID = p.AuthorId,
                            DirectorID = p.DirectorId,
                            ReleaseDate = p.ReleaseDate,
                            IsBookable = p.IsBookable,
                            Category = prodCategory,
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


            Product prod;
            ProductCategory cat;
            ProductType prodType;

            ProductModel model = ProductListView.SelectedItem as ProductModel;
            SecondProductListView.ItemsSource = await GetProductsFullListAsync(ActivityIndicator, model.Name, model.Type);
            AllProdsFrame.IsVisible = false;
            SingleProdFrame.IsVisible = true;

            ISBNEntry.Text = model.ISBN;
            PriceEntry.Text = model.UnitPrice.ToString();
            AmountOfPagesEntry.Text = model.Pages.ToString();
            ImageURLEntry.Text = model.Image;
            DescriptionEntry.Text = model.Info;

            using (var db = new LibraryDBContext())
            {
                prod = db.Products.Where(x => x.AuthorId == model.AuthorID).FirstOrDefault();
                cat = db.ProductCategories.Where(x => x.Id == model.CategoryID).FirstOrDefault();
                prodType = db.ProductTypes.Where(x => x.Type == model.Type).FirstOrDefault();
            }
            var authorIDPicker = AuthorIDPicker;

            for (int i = 0; i < AuthorIDPicker.Items.Count; i++)
            {
                string authorID = authorIDPicker.Items[i].ToString();
                string[] autID = authorID.Split('(', ')', ' ');
                if (Convert.ToInt32(autID[3]) == prod.AuthorId)
                {
                    AuthorIDPicker.SelectedIndex = i;
                }
            }

            var categoryIDPicker = CategoryIDPicker;

            for (int i = 0; i < CategoryIDPicker.Items.Count; i++)
            {
                string category = categoryIDPicker.Items[i].ToString();
                if (category.ToLower() == cat.Category.ToLower())
                {
                    CategoryIDPicker.SelectedIndex = i;
                }
            }

            var prodTypePicker = ProductTypePicker;

            for (int i = 0; i < ProductTypePicker.Items.Count; i++)
            {
                string pType = prodTypePicker.Items[i].ToString();
                if (pType.ToLower() == prodType.Type.ToLower())
                {
                    ProductTypePicker.SelectedIndex = i;
                }
            }

            if (prod.ReleaseDate != null)
            {
                RealeseDatePicker.Date = (DateTime)prod.ReleaseDate;
            }
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            EntryView.IsVisible = true;


        }

        private async void BackToListButton_Clicked(object sender, EventArgs e)
        {
            ProductListView.ItemsSource = await GetProductsAsync(ActivityIndicator);
            AllProdsFrame.IsVisible = true;
            DefaultFrameText.IsVisible = true;
            SingleProdFrame.IsVisible = false;
            EntryView.IsVisible = false;

        }

        private async void UpdateButton_Clicked(object sender, EventArgs e)
        {
            var prodToRecieve = ProductListView.SelectedItem as ProductModel;

            Product prodToUpdate;
            using (var db = new LibraryDBContext())
            {
                prodToUpdate = db.Products.Single(x => x.Id == prodToRecieve.ProId);
                prodToUpdate.ProductInfo = DescriptionEntry.Text;
                db.SaveChanges();
            }
            ProductListView.ItemsSource = await GetProductsAsync(ActivityIndicator);
            SecondProductListView.ItemsSource = await GetProductsFullListAsync(ActivityIndicator, prodToUpdate.ProductName, prodToRecieve.Type);

        }
    }
}