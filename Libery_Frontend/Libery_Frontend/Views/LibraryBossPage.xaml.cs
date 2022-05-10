using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LibraryBossPage : ContentPage
    {
        public List<Models.User> Users;
        public LibraryBossPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load products asynchronously
            MainThread.BeginInvokeOnMainThread(async () => { ProductListView.ItemsSource = await GetProductsAsync(ActivityIndicator); });
        }

        public async Task<List<UserModel>> GetProductsAsync(ActivityIndicator indicator)
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

                        Users = db.Users.ToList();

                        result = Users.Select(x => new UserModel
                        {
                            Username = x.Username,
                            Firstname = x.Firstname,
                            Lastname = x.Lastname,
                            UserGroup = x.UserGroup
                        }).ToList();
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

            UserModel item = ProductListView.SelectedItem as UserModel;

            if (item != null)
            {
                using (var context = new Models.LibraryDBContext())
                {
                    var personToUpdate = context.Users.Where(x => x.Username == item.Username).FirstOrDefault();
                    personToUpdate.UserGroup = "bibliotekarie";
                    context.SaveChanges();
                }
            }
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