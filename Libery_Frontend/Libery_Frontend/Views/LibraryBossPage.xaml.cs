using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LibraryBossPage : ContentPage
    {
        public List<Models.User> User;
       
        public LibraryBossPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load products asynchronously
            MainThread.BeginInvokeOnMainThread(async () => { ProductListView.ItemsSource = await GetUserAsync(ActivityIndicator); });
        }

        public async Task<List<UserModel>> GetUserAsync(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<UserModel>> databaseTask = Task<List<UserModel>>.Factory.StartNew(() =>
            {
                List<UserModel> result = null;
                try
                {
                    using (var db = new Models.LibraryDBContext())
                    {

                        User = db.Users.ToList();
                        result = User.Select(x => new UserModel
                        {Firstname = x.Firstname, Lastname = x.Lastname, Username = x.Username, UserGroup = x.UserGroup }).ToList();
                        //ProdType = db.ProductTypes.ToList();

                        //result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) => new ProductModel { Image = p.Image, Name = p.ProductName, Info = p.ProductInfo, Type = pi.Type }).ToList();
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

        private void AddProdButton_Clicked(object sender, EventArgs e)
        {
            RemoveProdFrame.IsVisible = false;
            UpdateProdFrame.IsVisible = false;
            DefaultFrameText.IsVisible = false;

            AddProdFrame.IsVisible = true;
        }

        private void RemoveProdButton_Clicked(object sender, EventArgs e)
        {
            AddProdFrame.IsVisible = false;
            UpdateProdFrame.IsVisible = false;
            DefaultFrameText.IsVisible = false;

            RemoveProdFrame.IsVisible = true;
        }

        private void UpdateProdButton_Clicked(object sender, EventArgs e)
        {
            AddProdFrame.IsVisible = false;
            RemoveProdFrame.IsVisible = false;
            DefaultFrameText.IsVisible = false;

            UpdateProdFrame.IsVisible = true;
        }

        public class UserModel
        {
            public string Username { get; set; } = default;
            public string Firstname { get; set; } = default;
            public string Lastname { get; set; } = default;
            public string UserGroup { get; set; } = default;
        }
    }
}