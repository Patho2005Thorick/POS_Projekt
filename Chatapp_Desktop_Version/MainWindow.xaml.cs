using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chatapp_Desktop_Version
{

    public partial class MainWindow : Window
    {

        private readonly HttpClient httpClient;
        private const string BaseUrl = "http://localhost:8080/ThorChat/users";
        private User user;


        public MainWindow()
        {
            InitializeComponent();
            // Initialisiere HttpClient für die Kommunikation mit dem Server
            httpClient = new HttpClient();
        }

        // Methode zur Behandlung des Anmeldevorgangs
        private async void Login(object sender, RoutedEventArgs e)
        {
            // Erfasse den Benutzernamen und das Passwort aus den Eingabefeldern
            string username = username_input.Text;
            string password = password_input.Password;

            // Zusammensetzen der Anfrage-URL mit Benutzername und Passwort
            string requestUrl = $"{BaseUrl}/{username}/{password}";

            // Sende eine HTTP-GET-Anfrage an den Server
            HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

            // Behandlung der Serverantwort
            if (response.IsSuccessStatusCode)
            {
                // Lese den Inhalt der Antwort
                string responseBody = await response.Content.ReadAsStringAsync();
                // Deserialisiere die Benutzerdaten aus der Antwort
                User user = JsonSerializer.Deserialize<User>(responseBody);
                this.user = user;

                // Überprüfe, ob der Benutzername und das Passwort korrekt sind
                if (username.Equals(user.UserName) && password.Equals(user.Password))
                {
                    MessageBox.Show("Anmeldung erfolgreich.");
                    // Öffne das Kontaktfenster
                    Contacts contacts = new Contacts(user);
                    Close();

                    try
                    {
                        contacts.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                // Zeige eine Fehlermeldung bei ungültigen Anmeldeinformationen an
                MessageBox.Show("Ungültiger Benutzername oder Passwort. Bitte versuchen Sie es erneut.");
            }
        }

        // Methode zum Öffnen des Registrierungsfensters
        private void Open_Registration(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            Close();
            registration.Show();
        }
    }
}
