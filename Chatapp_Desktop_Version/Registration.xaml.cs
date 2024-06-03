using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.Json;

namespace Chatapp_Desktop_Version
{
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private async void Registration_(object sender, RoutedEventArgs e)
        {
            // Holen des Benutzernamens und Passworts aus den Eingabefeldern
            string username = Username.Text;
            string password = Password.Password;

            // Löschen der Eingabefelder
            Username.Text = "";
            Password.Password = "";

            // Überprüfen, ob Benutzername und Passwort nicht leer sind
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                try
                {
                    var url = "http://localhost:8080/ThorChat/users";
                    User user = new User();
                    user.UserName = username;
                    user.Password = password;

                    // Senden der Benutzerdaten an die API
                    using (var httpClient = new HttpClient())
                    {
                        var json = JsonSerializer.Serialize(user);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync(url, content);

                        // Behandlung der Antwort der API
                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();
                            MessageBox.Show("Registrierung erfolgreich: " + responseData);
                            MainWindow main = new MainWindow();
                            Close();
                            main.Show();
                        }
                        else
                        {
                            MessageBox.Show("Registrierung fehlgeschlagen. Status Code: " + response.StatusCode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Registrierung fehlgeschlagen: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Ungültige Eingabe");
            }
        }
    }
}
