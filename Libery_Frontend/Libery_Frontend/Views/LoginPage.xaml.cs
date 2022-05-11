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
        public string username = null;

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
                    .Select(x => new User() { Password = x.Password, UserGroup = x.UserGroup })
                    .ToList();
                var usernameToCheck = context.Users.Where(x => x.Username == userName);
                if (usernameToCheck.Any())
                {
                    string password = PasswordEntry.Text;
                    bool correctPassword = BCrypt.Net.BCrypt.Verify(
                        password,
                        userPassword[0].Password
                    );

                    if (correctPassword == true && userPassword[0].UserGroup == "chef")
                    {

                        Page pageToAdd = new LibraryBossPage();
                        var homePage = new MainPage();
                        homePage.Title = $"{userName} - Inloggad";
                        pageToAdd.Title = "Bibliotekschef";
                        homePage.Children.Add(pageToAdd);
                        await Navigation.PushAsync(homePage);

                        UsernameEntry.Text = "";
                        PasswordEntry.Text = "";

                    }

                    else if (correctPassword == true && userPassword[0].UserGroup == "bibliotekarie")
                    {

                        Page pageToAdd = new LibrarianPage();
                        var homePage = new MainPage();
                        homePage.Title = $"{userName} - Inloggad";
                        pageToAdd.Title = "Bibliotekarie";
                        homePage.Children.Add(pageToAdd);
                        await Navigation.PushAsync(homePage);

                        UsernameEntry.Text = "";
                        PasswordEntry.Text = "";
                    }

                    else if (correctPassword == true && userPassword[0].UserGroup == "användare")
                    {
                        Page pageToAdd = new UserAccountPage(userName);
                        var homePage = new MainPage();
                        homePage.Title = $"{userName} - Inloggad";
                        pageToAdd.Title = "Kundkorg";
                        homePage.Children.Add(pageToAdd);
                        await Navigation.PushAsync(homePage);

                        UsernameEntry.Text = "";
                        PasswordEntry.Text = "";
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
        
        public void HideTab(int index)
        {
            TabbedPage theTabbedPage = App.Current.MainPage as TabbedPage;

             if (index < theTabbedPage.Children.Count())
            {
                theTabbedPage.Children.RemoveAt(index);
            }
        }

        public async void AddTab(int index)
        {
            TabbedPage theTabbedPage = App.Current.MainPage as TabbedPage;

            var page = new LibrarianPage();

                if (theTabbedPage.Children.Contains(page))
                {
                    theTabbedPage.Children.Insert(index, page);
                }
            }
        }
    }

