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
        private static string _username;
        public static string Username
        {
            get { return _username; }
            set { _username = value; }
        }
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
                        Page pageToAddFourth = new NotACustomerSearchPage();
                        Page pageToAddFifth = new NotACustomerProductPage();

                        var homePage = new MainPage();

                        homePage.Title = $"{userName} - Inloggad";
                        pageToAdd.Title = "Bibliotekschef";
                        pageToAddFourth.Title = "Sök";
                        pageToAddFifth.Title = "Alla produkter";

                        homePage.Children.RemoveAt(6);
                        homePage.Children.RemoveAt(5);
                        homePage.Children.RemoveAt(2);
                        homePage.Children.RemoveAt(1);

                        homePage.Children.Insert(1, pageToAddFourth);
                        homePage.Children.Insert(2, pageToAddFifth);

                        homePage.Children.Add(pageToAdd);
                        await Navigation.PushAsync(homePage);
                        
                        UsernameEntry.Text = "";
                        PasswordEntry.Text = "";

                    }

                    else if (correctPassword == true && userPassword[0].UserGroup == "bibliotekarie")
                    {

                        Page pageToAdd = new LibrarianPage();
                        Page pageToAddSecond = new ProductDelete2();
                        Page pageToAddThird = new AddAndDelete();
                        Page pageToAddFourth = new NotACustomerSearchPage();
                        Page pageToAddFifth = new NotACustomerProductPage();

                        var homePage = new MainPage();
                        homePage.Title = $"{userName} - Inloggad";
                        pageToAdd.Title = "Bibliotekarie";
                        pageToAddSecond.Title = "Ta bort/Lägg till";
                        pageToAddThird.Title = "Test";
                        pageToAddFourth.Title = "Sök";
                        pageToAddFifth.Title = "Alla produkter";

                        homePage.Children.RemoveAt(6);
                        homePage.Children.RemoveAt(5);
                        homePage.Children.RemoveAt(2);
                        homePage.Children.RemoveAt(1);

                        homePage.Children.Insert(1, pageToAddFourth);
                        homePage.Children.Insert(2, pageToAddFifth);

                        homePage.Children.Add(pageToAdd);
                        homePage.Children.Add(pageToAddSecond);
                        homePage.Children.Add(pageToAddThird);
                        await Navigation.PushAsync(homePage);

                        UsernameEntry.Text = "";
                        PasswordEntry.Text = "";
                    }

                    else if (correctPassword == true && userPassword[0].UserGroup == "användare")
                    {
                        Page pageToAdd = new UserAccountPage(userName);
                        Page pageToAddSecond = new UserAccountProductsPage();
                        Page pageToAddThird = new UserSearchPage();

                        var homePage = new MainPage();
                        homePage.Title = $"{userName} - Inloggad";
                        pageToAdd.Title = "Kundkorg";
                        pageToAddSecond.Title = "Alla produkter";
                        pageToAddThird.Title = "Sök";

                        homePage.Children.RemoveAt(6);
                        homePage.Children.RemoveAt(5);
                        homePage.Children.RemoveAt(2);
                        homePage.Children.RemoveAt(1);
                        homePage.Children.Insert(1, pageToAddThird);
                        homePage.Children.Insert(2, pageToAddSecond);

                        homePage.Children.Add(pageToAdd);
                        await Navigation.PushAsync(homePage);

                        Username = userName;

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

        public void AddTab(int index)
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

