using Libery_Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            var userPassword = new List<User>();
            string userName = UsernameEntry.Text;

            using (var context = new LibraryDBContext())
            {
                userPassword = context.Users
                    .Where(x => x.Username == userName)
                    .Select(x => new User() { Password = x.Password })
                    .ToList();
                var usernameToCheck = context.Users.Where(x => x.Username == userName);
                if (usernameToCheck.Any())
                {
                    string password = PasswordEntry.Text;
                    bool correctPassword = BCrypt.Net.BCrypt.Verify(
                        password,
                        userPassword[0].Password
                    );
                    if (correctPassword == true)
                    {
                        await Navigation.PushAsync(new MainPage());
                    }
                    else
                        await DisplayAlert(
                            "Felaktigt Login",
                            "Användarnamn eller lösenord finns inte. Var vänlig försök igen",
                            "OK"
                        );
                }
                else
                {
                    await DisplayAlert(
                        "Felaktig Login", 
                        "Användarnamn eller lösenord finns inte. Var vänlig försöker igen",
                        "OK");
                }
            }
        }
    }
}
