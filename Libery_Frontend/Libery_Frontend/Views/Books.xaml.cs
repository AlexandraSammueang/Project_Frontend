using Libery_Frontend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
    public partial class Books : ContentPage
    {
        public List<Models.Product> Products;
        public Books()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (var db = new Models.LibraryDBContext())
            {
                
               Products = db.Products.ToList();
                //Products = db.Products.Join(db.ProductTypes(p => p.ID = p.ProductTypeID));
                // ProductBooks = db.Products.Where(x => x.ProductType == 1);

                // var productList = Products;
                // var BookProducts = db.Products.Join(ProductType == ProductTypeId);
                

            }

            ProductListView.ItemsSource = Products;

           


        }
    }

    class ProductModel
    {
        public string Name { get; set; } = default;
    }
}