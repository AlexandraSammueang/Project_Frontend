using Libery_Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProduct : ContentPage
    {
        public List<ProductType> productTypes;
        public List<Models.ProductCategory> categories;
        public List<AuthorName> authorNames;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            using (var db = new Models.LibraryDBContext())
            {
                productTypes = db.ProductTypes.Select(x => new ProductType { Type = x.Type }).ToList();
                Allproducttype.ItemsSource = productTypes;
            }
            using (var db = new Models.LibraryDBContext())
            {

                var category = db.ProductCategories.Select(x => new ProductCategory { Id = x.Id, Category = x.Category }).ToList();

                PickerCategory.ItemsSource = category;
            }
            using (var db = new Models.LibraryDBContext())
            {
                authorNames = db.Authors.Select(x => new AuthorName { Firstname = x.Firstname, Lastname = x.Lastname }).ToList();
                AllAuthor.ItemsSource = authorNames;
            }
        }
        public AddProduct()
        {
            InitializeComponent();
        }

        private async void AddProducttype_Clicked(object sender, EventArgs e)
        {
            using (var db = new Models.LibraryDBContext())
            {
                var newCategory = new ProductType
                {
                    Type = TypeEntry.Text,

                };
                var svar = await DisplayAlert($"Vill du lägga till {TypeEntry.Text} ?", "Är du säker?", "Ja", "Nej");
                if (svar == true)
                {
                    db.Add(newCategory);
                    db.SaveChanges();
                    TypeEntry.Text = "";


                }
                else { }

                productTypes = db.ProductTypes.Select(x => new ProductType { Type = x.Type }).ToList();
                Allproducttype.ItemsSource = productTypes;
            }
        }

        private void Allproducttype_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            var category = new ProductCategory();

            if (CategoryEntry.Text != null)
            {
                using (var db = new Models.LibraryDBContext())
                {
                    category.Category = CategoryEntry.Text;

                    db.Add(category);
                    db.SaveChanges();

                    await DisplayAlert("Kategori tillagd", $"Du har lagt till kategorin {category.Category}", "Gå vidare");
                    var categoryList = db.ProductCategories.Select(x => new ProductCategory { Id = x.Id, Category = x.Category }).ToList();

                    PickerCategory.ItemsSource = categoryList;
                }

            }
            else await DisplayAlert("Ingen vara tillagd", "Skriv in namnet på kategorin du vill lägga till", "OK");
        }
        private async void AddAuthor_Clicked(object sender, EventArgs e)
        {
            using (var db = new Models.LibraryDBContext())
            {
                var newAuthor = new Author
                {
                    Firstname = FirstNameEntry.Text,
                    Lastname = LastNameEntry.Text,
                };
                var svar = await DisplayAlert($"Vill du lägga till {FirstNameEntry.Text} {LastNameEntry.Text}?", "Är du säker?", "Ja", "Nej");
                if (svar == true)
                {
                    db.Add(newAuthor);
                    db.SaveChanges();
                    FirstNameEntry.Text = "";
                    LastNameEntry.Text = "";
                }
                else { }

                authorNames = db.Authors.Select(x => new AuthorName { Firstname = x.Firstname, Lastname = x.Lastname }).ToList();
                AllAuthor.ItemsSource = authorNames;
            }

        }

        private void AllAuthor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = (Picker)sender;
            if (s.SelectedIndex == -1) return;

            AuthorName author = (AuthorName)s.SelectedItem;

            string personsAsString = s.Items[s.SelectedIndex];
            AuthorName author2 = (AuthorName)s.ItemsSource[s.SelectedIndex];

            FirstNameEntry.Text = personsAsString;

            if (personsAsString.Contains(' '))
            {
                var split = personsAsString.Split(' ');
                FirstNameEntry.Text = split[0];
                LastNameEntry.Text = split[1];


            }
        }
    }
}