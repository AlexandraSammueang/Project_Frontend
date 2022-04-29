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
            }

            ProductListView.ItemsSource = Products;
        }
    }

    class ProductModel
    {
        public string Name { get; set; } = default;
    }
}