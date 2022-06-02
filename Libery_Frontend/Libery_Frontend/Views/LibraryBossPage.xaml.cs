﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Libery_Frontend.SecondModels;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LibraryBossPage : ContentPage
    {
        public List<User> Users;
        public LibraryBossPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load products asynchronously
            MainThread.BeginInvokeOnMainThread(async () => { UserGroupBossLV.ItemsSource = await GetBossAsync(ActivityIndicator); });
            MainThread.BeginInvokeOnMainThread(async () => { UserGroupLibrarianListview.ItemsSource = await GetLibrarianAsync(ActivityIndicator); });
            MainThread.BeginInvokeOnMainThread(async () => { UserGroupUserLV.ItemsSource = await GetUserAsync(ActivityIndicator); });
        }

        public async Task<List<User>> GetUserAsync(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<User>> databaseTask = Task<List<User>>.Factory.StartNew(() =>
            {
                List<User> result = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {

                        Users = db.Users.ToList();

                        result = Users.Select(x => new User
                        {
                            Username = x.Username,
                            Firstname = x.Firstname,
                            Lastname = x.Lastname,
                            UserGroup = x.UserGroup,
                            Address = x.Address,
                            PostalCode = x.PostalCode,
                            Email = x.Email,
                            PhoneNumber = x.PhoneNumber,
                            City = x.City
                        }).Where(x=>x.UserGroup == "användare").ToList();
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

        public async Task<List<User>> GetLibrarianAsync(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<User>> databaseTask = Task<List<User>>.Factory.StartNew(() =>
            {
                List<User> result = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {

                        Users = db.Users.ToList();

                        result = Users.Select(x => new User
                        {
                            Username = x.Username,
                            Firstname = x.Firstname,
                            Lastname = x.Lastname,
                            UserGroup = x.UserGroup,
                            Address = x.Address,
                            PostalCode = x.PostalCode,
                            Email = x.Email,
                            PhoneNumber = x.PhoneNumber,
                            City = x.City
                        }).Where(x => x.UserGroup == "bibliotekarie").ToList();
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

        public async Task<List<User>> GetBossAsync(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<User>> databaseTask = Task<List<User>>.Factory.StartNew(() =>
            {
                List<User> result = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {

                        Users = db.Users.ToList();

                        result = Users.Select(x => new User
                        {
                            Username = x.Username,
                            Firstname = x.Firstname,
                            Lastname = x.Lastname,
                            UserGroup = x.UserGroup,
                            Address = x.Address,
                            PostalCode = x.PostalCode,
                            Email = x.Email,
                            PhoneNumber = x.PhoneNumber,
                            City = x.City
                        }).Where(x => x.UserGroup == "chef").ToList();
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

        //private async void AddProdButton_Clicked(object sender, EventArgs e)
        //{
        //    RemoveProdFrame.IsVisible = false;
        //    UpdateProdFrame.IsVisible = false;
        //    DefaultFrameText.IsVisible = false;

        //    AddProdFrame.IsVisible = true;

        //    UserModel item = ProductListView.SelectedItem as UserModel;

        //    if (item != null)
        //    {
        //        using (var context = new Models.LibraryDBContext())
        //        {
        //            var personToUpdate = context.Users.Where(x => x.Username == item.Username).FirstOrDefault();
        //            personToUpdate.UserGroup = "bibliotekarie";
        //            context.SaveChanges();

        //            ProductListView.ItemsSource = await GetUserAsync(ActivityIndicator);
        //        }
        //    }
        //    else
        //    {
        //        await DisplayAlert("Ingen användare vald", "Välj en användare för att uppgradera behörighet", "OK");
        //    }
        //}

        //private void RemoveProdButton_Clicked(object sender, EventArgs e)
        //{
        //    AddProdFrame.IsVisible = false;
        //    UpdateProdFrame.IsVisible = false;
        //    DefaultFrameText.IsVisible = false;

        //    RemoveProdFrame.IsVisible = true;

        //}

        //private void UpdateProdButton_Clicked(object sender, EventArgs e)
        //{
        //    AddProdFrame.IsVisible = false;
        //    RemoveProdFrame.IsVisible = false;
        //    DefaultFrameText.IsVisible = false;

        //    UpdateProdFrame.IsVisible = true;
        //}

        //private async void RemoveLiberianButton_Clicked(object sender, EventArgs e)
        //{
        //    RemoveProdFrame.IsVisible = false;
        //    UpdateProdFrame.IsVisible = false;
        //    DefaultFrameText.IsVisible = false;

        //    AddProdFrame.IsVisible = true;

        //    UserModel item = ProductListView.SelectedItem as UserModel;

        //    if (item != null)
        //    {
        //        using (var context = new Models.LibraryDBContext())
        //        {
        //            var personToUpdate = context.Users.Where(x => x.Username == item.Username).FirstOrDefault();
        //            personToUpdate.UserGroup = "användare";

        //            context.SaveChanges();

        //            //REMOVES USER FROM DATABASE
        //            //
        //            //var removePost = context.Users.SingleOrDefault(x => x.UserGroup == item.UserGroup.ToString());
        //            //var removePost = context.Users.Where(x => x.Username == item.Username).FirstOrDefault();
        //            //context.Users.Remove(removePost);
        //            //context.SaveChanges();

        //            ProductListView.ItemsSource = await GetUserAsync(ActivityIndicator);
        //        }
        //    }
        //    else
        //    {
        //        await DisplayAlert("Ingen vald användare", "Välj en användare för att ta bort behörighet", "Ok");
        //    }


        //}
        public async Task<List<User>> GetSingleUser(ActivityIndicator indicator, string userName)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<User>> databaseTask = Task<List<User>>.Factory.StartNew(() =>
            {
                List<User> result = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {

                        Users = db.Users.ToList();

                        result = Users.Select(x => new User
                        {
                            Username = x.Username,
                            Firstname = x.Firstname,
                            Lastname = x.Lastname,
                            UserGroup = x.UserGroup,
                            Address = x.Address,
                            PostalCode = x.PostalCode,
                            Email = x.Email,
                            PhoneNumber = x.PhoneNumber,
                            City = x.City
                        }).Where(x => x.Username == userName).ToList();
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

        private async void UserGroupUserLV_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            UserGroupAdminLVSL.IsVisible = false;
            UserGroupBossLVSL.IsVisible = false;

            User model = UserGroupUserLV.SelectedItem as User;
            SingleUserGroupLV.ItemsSource = await GetSingleUser(ActivityIndicator, model.Username);

            Grid.SetColumnSpan(SingleUserGroup, 2);
            Grid.SetColumn(UserGroupUserLVSL, 3);
            SingleUserGroup.IsVisible = true;
        }

        private async void UserGroupLibrarianListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            UserGroupUserLVSL.IsVisible = false;
            UserGroupBossLVSL.IsVisible = false;

            User model = UserGroupLibrarianListview.SelectedItem as User;
            SingleAdminGroupLV.ItemsSource = await GetSingleUser(ActivityIndicator, model.Username);

            Grid.SetColumnSpan(SingleAdminGroup, 2);
            Grid.SetColumn(UserGroupAdminLVSL, 3);
            SingleAdminGroup.IsVisible = true;
        }

        private async void UserGroupBossLV_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            UserGroupUserLVSL.IsVisible = false;
            UserGroupAdminLVSL.IsVisible = false;

            User model = UserGroupBossLV.SelectedItem as User;
            SingleBossGroupLV.ItemsSource = await GetSingleUser(ActivityIndicator, model.Username);

            Grid.SetColumnSpan(SingleBossGroup, 2);
            Grid.SetColumn(UserGroupBossLVSL, 3);
            SingleBossGroup.IsVisible = true;
        }

        private async void ReduceAuthorityButton_Clicked(object sender, EventArgs e)
        {

            Button btn = sender as Button;
            User item = btn.BindingContext as User;

            if (item != null)
            {
                using (var context = new LibraryDBContext())
                {
                    var personToUpdate = context.Users.Where(x => x.Username == item.Username).FirstOrDefault();
                    personToUpdate.UserGroup = "användare";

                    context.SaveChanges();

                    await DisplayAlert("Uppdatering klar", $"Behörighet för '{item.Username}' har reducerats", "OK");
                }
            }
            else await DisplayAlert("Fel", "Någonting gick fel. Var vänlig testa igen", "OK");

            SingleAdminGroupLV.ItemsSource = await GetSingleUser(ActivityIndicator, item.Username);
        }

        private async void IncreaseAuthorityButton_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            User item = btn.BindingContext as User;

            if (item != null)
            {
                using (var context = new LibraryDBContext())
                {
                    var personToUpdate = context.Users.Where(x => x.Username == item.Username).FirstOrDefault();
                    personToUpdate.UserGroup = "bibliotekarie";

                    context.SaveChanges();
                    await DisplayAlert("Uppdatering klar", $"Behörighet för '{item.Username}' har höjts", "OK");
                }
            }
            else await DisplayAlert("Fel", "Någonting gick fel. Var vänlig testa igen", "OK");

            SingleUserGroupLV.ItemsSource = await GetSingleUser(ActivityIndicator, item.Username);

        }

        private async void BackToListButton_Clicked(object sender, EventArgs e)
        {
            UserGroupAdminLVSL.IsVisible = true;
            UserGroupUserLVSL.IsVisible = true;
            UserGroupBossLVSL.IsVisible = true;
            SingleUserGroup.IsVisible = false;
            SingleAdminGroup.IsVisible = false;
            SingleBossGroup.IsVisible = false;

            Grid.SetColumn(UserGroupUserLVSL, 0);
            Grid.SetColumn(UserGroupAdminLVSL, 1);
            Grid.SetColumn(UserGroupBossLVSL, 2);
            Grid.SetColumn(SingleUserGroup, 0);


            UserGroupUserLV.ItemsSource = await GetUserAsync(ActivityIndicator);
            UserGroupLibrarianListview.ItemsSource = await GetLibrarianAsync(ActivityIndicator);
            UserGroupBossLV.ItemsSource = await GetBossAsync(ActivityIndicator);
        }

        private async void IncreaseAuthorityButton2_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            User item = btn.BindingContext as User;

            if (item != null)
            {
                using (var context = new LibraryDBContext())
                {
                    var personToUpdate = context.Users.Where(x => x.Username == item.Username).FirstOrDefault();
                    personToUpdate.UserGroup = "chef";

                    context.SaveChanges();
                    await DisplayAlert("Uppdatering klar", $"Behörighet för '{item.Username}' har höjts", "OK");
                }
            }
            else await DisplayAlert("Fel", "Någonting gick fel. Var vänlig testa igen", "OK");

            SingleAdminGroupLV.ItemsSource = await GetSingleUser(ActivityIndicator, item.Username);

        }
    }
}