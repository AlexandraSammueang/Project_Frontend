
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
           
            //Photo2.Source = "https://images.axiellmedia.com/cover/1000178/1000178_202010071345.jpg?height=292";
            //Book2.Text = "Jan Guillous debutroman Ondskan kom 1981 och räknas redan som en klassiker inom den moderna svenska litteraturen. " +
            //    "Ondskan är en berättelse om våldet i den svenska vardagen, " +
            //    "där vi får följa huvudpersonen Eriks fostran i ondska från hemmet med den sadistiske fadern till skolgården med dess gängbildningar.";
            
            
            //Photo3.Source = "https://s3.eu-north-1.amazonaws.com/bookis-se.web.production/books/47360/full.jpg";
            //Book3.Text = "Saruman har besegrats, men kampen är inte över.Mordors styrkor har nämligen siktats in sig på konungarnas gamla stad Minas Tirith i Gondor," +
            //    " och ett slag verkar oundvikligt på Pelennors fält." +
            //    "Samtidigt fortsätter Frodo, Sam och Gollum sin tröstlösa vandring i Mordor, över Gorgorothplattan för att förgöra ringen i Domberget." +
            //    "Hela tiden måste de undvika Saurons öga som vakar i Barad - dûr.";

            //Photo4.Source = "https://s1.adlibris.com/images/54131167/harry-potter-och-hemligheternas-kammare.jpg";
            //Book4.Text = "Sommarlovet är äntligen över! Harry Potter har längtat tillbaka till sitt andra år på Hogwarts skola för häxkonster och trolldom. " +
            //    "Men hur ska han stå ut med den omåttligt stroppige professor Lockman? Vad döljer Hagrids förflutna? Och vem är egentligen Missnöjda Myrtle?" +
            //    "De verkliga problemen börjar när någon, eller något, förstenar den ena Hogwartseleven efter den andra.Är det Harrys fiende, Draco Malfoy, som ligger bakom?" +
            //    " Eller är det den som alla på Hogwarts misstänker -Harry Potter själv?";

            //Photo5.Source = "https://s1.adlibris.com/images/59846707/tim-biografin-om-avicii.jpg";
            //Book5.Text = "Som ett fyrverkeri mot natthimlen exploderade DJ:n och producenten Avicii. En musikalisk visionär som genom sin känsla för melodier definierade epoken då svensk och europeisk housemusik tog över världen." +
            //    "Men Tim Bergling var också en introvert och skör ung man som blev vuxen i ett omänskligt uppskruvat tempo." +
            //    "Efter ett antal akuta sjukhusvistelser slutade han turnera sommaren 2016.Knappt två år senare tog han sitt liv.";

        }

        private void ReserveBook(object sender, System.EventArgs e)
        {
            (sender as Button).Text = "Du har nu bokat en bok ";
        }
    }
    
}