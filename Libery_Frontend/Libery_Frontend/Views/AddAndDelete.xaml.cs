using Libery_Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAndDelete : ContentPage
    {
        public List<Models.Author> Authors;
        public List<AuthorName> aut;
        public AddAndDelete()
        {

            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();


            using (var db = new Models.LibraryDBContext())
            {
                aut = db.Authors.Select(x => new AuthorName { Firstname = x.Firstname, Lastname = x.Lastname, AuthorId = x.Id }).ToList();

                pickerarray.ItemsSource = aut;

                var type = db.ProductTypes.Select(x => new ProductType { Id = x.Id, Type = x.Type }).ToList();

                PickerProductType.ItemsSource = type;

                var category = db.ProductCategories.Select(x => new ProductCategory { Id = x.Id, Category = x.Category }).ToList();

                PickerCategoryID.ItemsSource = category;

            }

        }

        private async void AddButton_Clicked(object sender, System.EventArgs e)
        {

            AuthorName item = pickerarray.SelectedItem as AuthorName;
            Author aut;
            ProductType type = PickerProductType.SelectedItem as ProductType;
            ProductCategory category = PickerCategoryID.SelectedItem as ProductCategory;

            if (item != null)
            {
                using (var db = new Models.LibraryDBContext())
                {
                    
                    aut = db.Authors.Where(x => x.Id == item.AuthorId).FirstOrDefault();


                    var newProduct = new Product
                    {
                        ProductName = ProductNameEntry.Text,
                        ProductInfo = ProductInfoEntry.Text,
                        Isbn = ISBNEntry.Text,
                        AuthorId = aut.Id,
                        ProductTypeId = type.Id,
                        Image = ImageEntry.Text,
                        CategoryId = category.Id,
                        Price = Convert.ToDouble(PriceEntry.Text),
                        BookPages = Convert.ToInt32(BookPagesEntry.Text)

                    };
                    var svar = await DisplayAlert("Vill du lägga till produkten", "Är du helt säker?", "Ja", "Nej");


                    if (svar == true)
                    {
                        db.Add(newProduct);
                        db.SaveChanges();
                    }
                    else { }


                }

            }
            else
            {
                using (var db = new Models.LibraryDBContext())
                {
                    var newProduct = new Product
                    {
                        ProductName = ProductNameEntry.Text,
                        ProductInfo = ProductInfoEntry.Text,
                        Isbn = ISBNEntry.Text,
                        AuthorId = InsertAuthor(),
                        ProductTypeId = type.Id,
                        Image = ImageEntry.Text,
                        CategoryId = category.Id,
                        Price = Convert.ToDouble(PriceEntry.Text),
                        BookPages = Convert.ToInt32(BookPagesEntry.Text)

                    };
                    var svar = await DisplayAlert("Vill du lägga till produkten", "Är du helt säker?", "Ja", "Nej");


                    if (svar == true)
                    {
                        db.Add(newProduct);
                        db.SaveChanges();
                    }
                    else { }
                }
            }
        }
        private void picker_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            var s = (Picker)sender;
            if (s.SelectedIndex == -1) return;

            AuthorName author = (AuthorName)s.SelectedItem;

            string personsAsString = s.Items[s.SelectedIndex];
            AuthorName author2 = (AuthorName)s.ItemsSource[s.SelectedIndex];

            AFirstnameEntry.Text = personsAsString;

            if (personsAsString.Contains(' '))
            {
                var split = personsAsString.Split(' ');
                AFirstnameEntry.Text = split[0];
                ALastnameEntry.Text = split[1];


            }
        }

        private void PickerProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = (Picker)sender;
            if (s.SelectedIndex == -1) return;

            ProductType producttype = (ProductType)s.SelectedItem;

            string personsAsString = s.Items[s.SelectedIndex];
            ProductType author2 = (ProductType)s.ItemsSource[s.SelectedIndex];

            ProductTypeIdEntry.Text = personsAsString;
        }

        private void PickerCategoryID_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = (Picker)sender;
            if (s.SelectedIndex == -1) return;

            ProductCategory categorytype = (ProductCategory)s.SelectedItem;

            string personsAsString = s.Items[s.SelectedIndex];
            ProductCategory author2 = (ProductCategory)s.ItemsSource[s.SelectedIndex];

            CategoryIdEntry.Text = personsAsString;
        }

        private int? InsertAuthor()
        {
            using (var db = new Models.LibraryDBContext())
            {

                var newAuthor = new Author
                {
                    Firstname = AFirstnameEntry.Text,
                    Lastname = ALastnameEntry.Text
                };
                db.Add(newAuthor);
                db.SaveChanges();
                return newAuthor.Id;
            }
        }

        private void GetISBN_Clicked(object sender, System.EventArgs e)
        {
            WebSite();
        }
        public void WebSite()
        {

            webSite.Source = $"http://libris.kb.se/hitlist?q=linkisxn:{ISBN_kod.Text}";
        }

    }

}
