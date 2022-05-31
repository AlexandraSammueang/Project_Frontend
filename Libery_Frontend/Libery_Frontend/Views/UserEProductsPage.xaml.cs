﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Libery_Frontend.SecondModels;
using System.Globalization;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserEProductsPage : ContentPage
    {

        public UserEProductsPage()
        {
            InitializeComponent();
        }
        public List<Product> Products;
        public List<ProductType> ProdType;
        public List<ShoppingCart> ShoppingCarts;
        public List<ProductCategory> Category;
        public List<Author> autName;
        public List<Director> dirName;


        public async Task<List<ProductModel>> GetBooksAsync()
        {

            Task<List<ProductModel>> databaseTask = Task<List<ProductModel>>.Factory.StartNew(() =>
            {
                List<ProductModel> result = null;
                List<ProductModel> catRes = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {
                        Category = db.ProductCategories.ToList();
                        Products = db.Products.ToList();
                        ProdType = db.ProductTypes.ToList();
                        autName = db.Authors.ToList();

                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) =>
                        new ProductModel
                        {
                            Image = p.Image,
                            Name = p.ProductName,
                            Info = p.ProductInfo,
                            Type = pi.Type,
                            ProId = p.Id,
                            InfoConcat = p.ProductInfo,
                            Pages = p.BookPages,
                            AuthorID = p.AuthorId,
                            DirectorID = p.DirectorId,
                            CategoryID = p.CategoryId,
                            ReleaseDate = p.ReleaseDate,
                            UnitPrice = p.Price,
                            ISBN = p.Isbn,
                            IsBookable = p.IsBookable
                        }).ToList();

                        result = result.Join(Category, pi => pi.CategoryID, p => p.Id, (p, pi) =>
                        new ProductModel
                        {
                            Image = p.Image,
                            Name = p.Name,
                            Info = p.Info,
                            InfoConcat = p.Info,
                            Type = p.Type,
                            ProId = p.ProId,
                            Pages = p.Pages,
                            AuthorID = p.AuthorID,
                            CategoryID = p.CategoryID,
                            ReleaseDate = p.ReleaseDate,
                            UnitPrice = p.UnitPrice,
                            ISBN = p.ISBN,
                            IsBookable = p.IsBookable,
                            Category = pi.Category,
                            DirectorID = p.DirectorID
                        }).ToList();


                        result = result.Join(autName, pi => pi.AuthorID, p => p.Id, (p, pi) =>
                        new ProductModel
                        {
                            Image = p.Image,
                            Name = p.Name,
                            Info = p.Info,
                            InfoConcat = p.Info,
                            Type = p.Type,
                            ProId = p.ProId,
                            Pages = p.Pages,
                            AuthorID = p.AuthorID,
                            CategoryID = p.CategoryID,
                            ReleaseDate = p.ReleaseDate,
                            UnitPrice = p.UnitPrice,
                            ISBN = p.ISBN,
                            IsBookable = p.IsBookable,
                            Category = p.Category,
                            DirectorID = p.DirectorID,
                            AuthorName = pi.Firstname + " " + pi.Lastname
                        }).Where(x => x.Type == "E-Bok").ToList();


                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].InfoConcat = String.Concat(result[i].Info.Substring(0, 60), "...");
                        }
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
            return taskResult;
        }


        public async Task<List<ProductModel>> GetEMoviesAsync()
        {

            Task<List<ProductModel>> databaseTask = Task<List<ProductModel>>.Factory.StartNew(() =>
            {
                List<ProductModel> result = null;
                List<ProductModel> catRes = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {
                        Category = db.ProductCategories.ToList();
                        Products = db.Products.ToList();
                        ProdType = db.ProductTypes.ToList();
                        dirName = db.Directors.ToList();

                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) =>
                        new ProductModel
                        {
                            Image = p.Image,
                            Name = p.ProductName,
                            Info = p.ProductInfo,
                            Type = pi.Type,
                            ProId = p.Id,
                            InfoConcat = p.ProductInfo,
                            Pages = p.BookPages,
                            AuthorID = p.AuthorId,
                            DirectorID = p.DirectorId,
                            CategoryID = p.CategoryId,
                            ReleaseDate = p.ReleaseDate,
                            UnitPrice = p.Price,
                            ISBN = p.Isbn,
                            IsBookable = p.IsBookable
                        }).ToList();

                        result = result.Join(Category, pi => pi.CategoryID, p => p.Id, (p, pi) =>
                        new ProductModel
                        {
                            Image = p.Image,
                            Name = p.Name,
                            Info = p.Info,
                            InfoConcat = p.Info,
                            Type = p.Type,
                            ProId = p.ProId,
                            Pages = p.Pages,
                            CategoryID = p.CategoryID,
                            ReleaseDate = p.ReleaseDate,
                            UnitPrice = p.UnitPrice,
                            ISBN = p.ISBN,
                            IsBookable = p.IsBookable,
                            Category = pi.Category,
                            DirectorID = p.DirectorID
                        }).Where(x => x.Type == "E-Film").ToList();


                        result = result.Join(dirName, pi => pi.DirectorID, p => p.Id, (p, pi) =>
                        new ProductModel
                        {
                            Image = p.Image,
                            Name = p.Name,
                            Info = p.Info,
                            InfoConcat = p.Info,
                            Type = p.Type,
                            ProId = p.ProId,
                            Pages = p.Pages,
                            CategoryID = p.CategoryID,
                            ReleaseDate = p.ReleaseDate,
                            UnitPrice = p.UnitPrice,
                            ISBN = p.ISBN,
                            IsBookable = p.IsBookable,
                            Category = p.Category,
                            DirectorID = p.DirectorID,
                            AuthorName = pi.Firstname + " " + pi.Lastname
                        }).ToList();


                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].InfoConcat = String.Concat(result[i].Info.Substring(0, 60), "...");
                        }
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
            return taskResult;
        }


        public async Task<List<ProductModel>> GetBooksFullListAsync(string prodName, string prodType, string prodCat, string authorName)
        {
            Task<List<ProductModel>> databaseTask = Task<List<ProductModel>>.Factory.StartNew(() =>
            {
                List<ProductModel> result = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {

                        Products = db.Products.ToList();
                        ProdType = db.ProductTypes.ToList();
                        Category = db.ProductCategories.ToList();

                        var pCat = Category.Where(x => x.Id == Products.FirstOrDefault().CategoryId).ToList();

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
                            Category = prodCat,
                            ISBN = p.Isbn,
                            AuthorName = authorName
                        }).Where(x => x.Name == prodName && x.Type == prodType).ToList();
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
            return taskResult;
        }

        private async void EBooksButton_Clicked(object sender, EventArgs e)
        {
            EBooksListview.Opacity = 0;
            EBooksListview.IsVisible = true;
            MainThread.BeginInvokeOnMainThread(async () => { EBooksListview.ItemsSource = await GetBooksAsync(); });

            await Task.WhenAll(EBooksListview.FadeTo(1, 1000), EMoviesListview.FadeTo(0, 500));

            EMoviesListview.IsVisible = false;
            EMoviesListview.IsVisible = false;
        }

        private async void EMoviesButton_Clicked(object sender, EventArgs e)
        {
            EMoviesListview.Opacity = 0;
            EMoviesListview.IsVisible = true;
            MainThread.BeginInvokeOnMainThread(async () => { EMoviesListview.ItemsSource = await GetEMoviesAsync(); });

            await Task.WhenAll(EMoviesListview.FadeTo(1, 1000), EBooksListview.FadeTo(0, 500));

            EBooksListview.IsVisible = false;
            EBooksListview.IsVisible = false;
        }

        private async void EBooksListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (InspectProductListView.Opacity == 0)
            {
                await Task.WhenAll(InspectProductListView.FadeTo(1, 1000));
            }


            ProductModel model = EBooksListview.SelectedItem as ProductModel;
            InspectProductListView.ItemsSource = await GetBooksFullListAsync(model.Name, model.Type, model.Category, "Författare: " + model.AuthorName);

        }

        private async void EMoviesListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (InspectProductListView.Opacity == 0)
            {
                await Task.WhenAll(InspectProductListView.FadeTo(1, 1000));
            }


            ProductModel model = EMoviesListview.SelectedItem as ProductModel;
            InspectProductListView.ItemsSource = await GetBooksFullListAsync(model.Name, model.Type, model.Category, "Regissör: " + model.AuthorName);


        }

        private async void BookProductButton_Clicked(object sender, EventArgs e)
        {
            ShoppingCart cart = new ShoppingCart();
            DateTime returnDate = DateTime.Now.AddDays(30);
            CultureInfo dateTimeLanguage = CultureInfo.GetCultureInfo("sv-SE");

            Button btn = sender as Button;
            ProductModel item = btn.BindingContext as ProductModel;



            if (item.IsBookable != true)
            {
                await DisplayAlert("Går ej att boka", "Denna produkt går för tillfället inte att boka", "OK");
                return;
            }

            else if (item != null)
            {

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    using (var context = new LibraryDBContext())
                    {

                        cart.ProductId = item.ProId;
                        cart.UserId = LoginPage.Username;
                        cart.DateBooked = DateTime.Now;
                        cart.ReturnDate = DateTime.Now.AddDays(30);

                        ShoppingCarts = context.ShoppingCarts.Where(x => x.ProductId == item.ProId && x.UserId == LoginPage.Username).ToList();

                        if (ShoppingCarts.Any())
                        {
                            await DisplayAlert("Redan bokad", "Du har redan bokat denna produkt", "OK");

                        }
                        else
                        {
                            context.Add(cart);
                            context.SaveChanges();

                            var typeOfProduct = item.Type;
                            await DisplayAlert($"{typeOfProduct} bokad",
                                $"{item.Name} är bokad.\nLämnas tillbaks senast {returnDate.ToString("dddd, MMMM dd, yyyy", dateTimeLanguage)}", "OK");
                        }
                    }
                });
            }
            else
                await DisplayAlert("Produkt ej vald", "Välj en produkt för att boka", "OK");


        }
    }




}


