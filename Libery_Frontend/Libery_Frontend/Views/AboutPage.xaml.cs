using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            timeLabel.Text =
                           "Öppettider \n\n\n" +
                           "MÅN 10:00-16:00 \n" +
                           "TIS 9:00-18:00 \n" +
                           "ONS 9:00-19:00 \n" +
                           "TOR 8:00-16:00 \n" +
                           "FRE 10:00-17:00 \n" +
                           "LÖR 11:00-15:00 \n" +
                           "SÖN 12:00-15:00";


        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            var tab = new MainPage();
            tab.CurrentPage = tab.Children[0];
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(tab));

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await DisplayAlert("Bra klickat", "Du har klickat på en bild", "OK");
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            var tab = new MainPage();
            tab.CurrentPage = tab.Children[2];

            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(tab));
        }

        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            await DisplayAlert("Bra klickat", "Du har klickat på en bild", "OK");
        }

        private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            await DisplayAlert("Bra klickat", "Du har klickat på en bild", "OK");
        }
    }
}