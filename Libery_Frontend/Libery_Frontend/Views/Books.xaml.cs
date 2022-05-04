using Libery_Frontend.Models;
using Microsoft.EntityFrameworkCore;
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
        public List<Models.Product> productList;
        public List<ProductType> prodTypesList;
        public Books()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (var db = new Models.LibraryDBContext())
            {
                
               productList = db.Products.ToList();
                prodTypesList = db.ProductTypes.ToList();

                //var prodList = prodTypesList.Join(prodTypesList,
                //    product => product.Id,
                //    type => type.Id,
                //    (product, type) => new ProductType { Id = product.Id, Type = type.Type }).ToList();

                //Products = db.Products.Join(db.ProductTypes(p => p.ID = p.ProductTypeID));

                var pList = productList.Join(prodTypesList,
                                           product => product.CategoryId,
                                           type => type.Id,
                                           (product, type) => new
                                           {
                                               Id = product.Id,
                                               CategoryId = product.CategoryId,
                                               Type = type.Type,

                                           });
                ProductListView.ItemsSource = pList;

            }


           


        }
    }

    class ProductModel
    {
        public string Name { get; set; } = default;
    }
}