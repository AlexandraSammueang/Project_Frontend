using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libery_Frontend.Models;
using BCrypt.Net;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public List<Models.User> UserRegisterInfo;
        User user = new User();
        public RegisterPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

           
        }

        //Registration
        private async void RegisterButton_Clicked(object sender, System.EventArgs e)
        {
            if (EmailEntry.Text != "" && PostcodeEntry.Text != "" && CityEntry.Text != ""
                && PhonenumberEntry.Text != "" && LastnameEntry.Text != "" && FirstnameEntry.Text != ""
                 && ConfirmPasswordEntry.Text != "" && PasswordEntry.Text != "")
            {
                using (var context = new LibraryDBContext())
                {
                    var checkUsernameAvailability = context.Users.Where(x => x.Username == UsernameEntry.Text);
                    if (checkUsernameAvailability.Any())
                    {
                        await DisplayAlert("Upptaget användarnamn", "Användarnamnet upptaget. Var vänlig välj ett annat.", "OK");
                        return;
                    }

                    else if (ConfirmPasswordEntry.Text != PasswordEntry.Text)
                    {
                        await DisplayAlert("Felaktigt lösenord", "Lösenord matchar inte. Försök igen", "OK");
                        return;
                    }

                    else
                    {
                        user.Username = UsernameEntry.Text;
                        user.Password = PasswordEntry.Text;
                        user.Password = BCrypt.Net.BCrypt.HashPassword(PasswordEntry.Text, 10);
                        user.Firstname = FirstnameEntry.Text;
                        user.Lastname = LastnameEntry.Text;
                        user.Lastname = LastnameEntry.Text;
                        user.PhoneNumber = PhonenumberEntry.Text;
                        user.City = CityEntry.Text;
                        user.PostalCode = PostcodeEntry.Text;
                        user.Email = EmailEntry.Text;

                        context.Users.Add(user);
                        context.SaveChanges();

                        await DisplayAlert("Registrerad", "Registrering klar", "OK");
                    }
                }
            }
            else await DisplayAlert("Information saknas", "Ett eller flera fält fattas", "OK");
        }
    }
}