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
        public List<Models.ProductType> ProdType;
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
                ProdType = db.ProductTypes.ToList();

                var prodType = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) => new { p, pi }).ToList();

                ProductListView.ItemsSource = prodType;

            }

            




        }
    }

    class ProductModel
    {
        public string Name { get; set; } = default;
    }
}