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
        public AddAndDelete()
        {
            InitializeComponent();

            using (var db = new Models.LibraryDBContext())
            {
                var newProduct = new Product
                {
                    ProductName = ProductNameEntry.Text,
                    ProductCategory = ProductCategory.Text;
                    Name = FirstnameEntry.Text;




                try
                {
                    db.Add(newProduct);
                    db.SaveChanges();
                    Console.WriteLine("You have added 1 new product");
                }
                catch (Exception)
                {
                    Console.WriteLine("You failed to add a product");
                }
            }
    }
}