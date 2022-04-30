
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LibraryPage : ContentPage
    {
        public LibraryPage()
        {
            InitializeComponent();

            Photo1.Source = "https://s1.adlibris.com/images/61836845/vegansk-hemkunskap.jpg";
            Book1.Text = "Det här är boken för dig som är nyfiken på vegansk matlagning, men kanske inte riktigt vet var du ska börja." +
                "Gustav Johansson, som driver Sveriges största veganska blogg ”Jävligt gott”, berättar här om tekniker och råvaror och visar hur du kan använda dem på olika sätt." +
                "Recepten är enkla – de innehåller sällan fler än sju ingredienser och är klara på omkring 30 minuter – och precis som i Gustavs tidigare böcker är de förstås också jävligt goda." +
                "Oavsett om du är ny i köket, redan lagar veganskt men vill hitta nya genvägar eller har velat prova men tyckt att det verkar krångligt så finns det något för dig i ”Vegansk hemkunskap”.";
           
            //Photo2.Source = "http://img.tradera.net/images/026/473480026_aae509a8-d9fc-403a-8bdd-157019a66fb6.jpg";
            //Photo3.Source = "https://static-se.bookis.com/books/497469/full.jpg";
            //Photo4.Source = "https://s1.adlibris.com/images/54131167/harry-potter-och-hemligheternas-kammare.jpg";
            //Photo5.Source = "https://s1.adlibris.com/images/59846707/tim-biografin-om-avicii.jpg";

        }

     
    }
    
}