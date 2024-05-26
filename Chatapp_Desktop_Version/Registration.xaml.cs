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
    /// <summary>
    /// Interaktionslogik für Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private async void Registration_(object sender, RoutedEventArgs e)
        {

            string username = Username.Text;
            string password = Password.Password;
            
            Username.Text = "";
            Password.Password = "";
         
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                try
                {
                    var url = "http://localhost:8080/ThorChat/users";
                    User user = new User();
                    user.UserName = username;
                    user.Password = password;
                 

                    using (var httpClient = new HttpClient())
                    {
                        var json = JsonSerializer.Serialize(user);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseData = await response.Content.ReadAsStringAsync();
                            MessageBox.Show("Registration successful: " + responseData);
                            MainWindow main = new MainWindow();
                            Close();
                            main.Show();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Status Code: " + response.StatusCode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Registration failed: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Invalid Input");
            }


        }
    }
}
