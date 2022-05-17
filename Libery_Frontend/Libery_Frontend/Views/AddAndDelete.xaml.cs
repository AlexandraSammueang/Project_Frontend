﻿using Libery_Frontend.Models;
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


            }

          
        }

        private async void AddButton_Clicked(object sender, System.EventArgs e)
        {

            AuthorName item = pickerarray.SelectedItem as AuthorName;
            Author aut;

            using (var db = new Models.LibraryDBContext())
            {
                 aut = db.Authors.Where(x => x.Id == item.AuthorId).FirstOrDefault();


                var newProduct = new Product
                {
                    ProductName = ProductNameEntry.Text,
                    ProductInfo = ProductInfoEntry.Text,
                    Isbn = ISBNEntry.Text,
                    AuthorId = aut.Id,
                    //AuthorId = Authors.Last().Id,
                    ProductTypeId = Convert.ToInt32(ProductTypeIdEntry.Text),
                    Image = ImageEntry.Text,
                    CategoryId = Convert.ToInt32(CategoryIdEntry.Text),
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
       



    }

}
