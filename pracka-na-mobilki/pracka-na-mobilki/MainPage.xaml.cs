using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace pracka_na_mobilki
{
    public partial class MainPage : ContentPage
    {
        private string ostatniplik { get; set; }
        private bool zalogowany { get; set; }
        private readonly string loginuzytkownika = "login";
        private readonly string haslouzytkownika = "haslo";

        private CancellationTokenSource token;
        public MainPage()
        {
            InitializeComponent();
        }

        private void zaloguj_Clicked(object sender, EventArgs e)
        {
            string wpisanylogin = login.Text;
            string wpisanehaslo = haslo.Text;

            if (loginuzytkownika == wpisanylogin && haslouzytkownika == wpisanehaslo)
            {
                tekst.IsReadOnly = false;
                zapisz.IsEnabled = true;
                zobacz.IsEnabled = true;
                zalogowany = true;
            }
            else
            {
                tekst.IsReadOnly = true;
                zapisz.IsEnabled = false;
                zobacz.IsEnabled = false;
                zalogowany = false;
            }
        }

        private async void zapisz_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(Path.Combine(FileSystem.AppDataDirectory, "dane")))
                {
                    Directory.CreateDirectory(Path.Combine(FileSystem.AppDataDirectory, "dane"));
                }

                string nazwa = Path.Combine(FileSystem.AppDataDirectory, "dane", $"{losowytekst(8)}.txt");

                DateTime czas = DateTime.Now;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                token = new CancellationTokenSource();
                Location lokalizacja = await Geolocation.GetLocationAsync(request, token.Token);

                string t = 
                    czas.ToString() + "\n" + 
                    (lokalizacja != null ? lokalizacja.Latitude.ToString() + " " + lokalizacja.Longitude.ToString() + "\n" : "") +
                    tekst.Text;
 
                File.WriteAllText(nazwa, t);

                informacja.Text = $"Nazwa zapisanego pliku: {nazwa}";

                ostatniplik = nazwa;
            }
            catch (Exception)
            {
                informacja.Text = "Wystąpił błąd";
            }
        }

        private void zobacz_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ostatniplik))
            {
                if (File.Exists(ostatniplik))
                {
                    dane.Text = Path.GetFileName(ostatniplik) + "\n" + File.ReadAllText(ostatniplik);
                }
            }
           
        }
        private string losowytekst(int n)
        {
            Random rand = new Random();
            string litery = "abcdefghijklmnoprstuwxyz0123456789";
            string wynik = "";

            for (int i = 0; i < n; i++)
            {
                wynik += litery[rand.Next(0, litery.Length - 1)];
            }

            return wynik;
        }
    }
}
