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


        public MainWindow()
        {
            InitializeComponent();
            httpClient = new HttpClient();
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            string username = username_input.Text;
            string password = password_input.Password;

            string requestUrl = $"{BaseUrl}/{username}/{password}";

            HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                
                string responseBody = await response.Content.ReadAsStringAsync();
                User user = JsonSerializer.Deserialize<User>(responseBody);

                if(username.Equals(user.UserName) && password.Equals(user.Password))
                {
                    MessageBox.Show("Login Successfull.");
                    Contacts contacts = new Contacts();
                    Close();
                    contacts.Show();
                    
                }

                
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private void Open_Registration(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
        }
    }
}
