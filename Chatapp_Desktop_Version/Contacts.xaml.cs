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
using System.Windows.Shapes;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Chatapp_Desktop_Version
{
    /// <summary>
    /// Interaktionslogik für Contacts.xaml
    /// </summary>
    public partial class Contacts : Window
    {
        private User user = new User();
        private HttpClient httpClient;
        private const string BaseUrl = "http://localhost:8080/ThorChat/users/contacts";
        private const string BaseUrl2 = "http://localhost:8080/ThorChat/users";
        private User newContact = new User();

        public Contacts(User currentuser)
        {
            InitializeComponent();
            user = currentuser;
            Contactslist.ItemsSource = user.Contacts;
            httpClient = new HttpClient();

        }

        

        private async void Add_Contact_Click(object sender, RoutedEventArgs e)
        {


           

            try
            {
                string newContactString = AddContactInput.Text;



                string requestUrl =  $"{ BaseUrl2}/{newContactString}";

                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {


                    string responseBody = await response.Content.ReadAsStringAsync();
                    newContact = JsonSerializer.Deserialize<User>(responseBody);

                    if (!user.Contacts.Contains(newContact.username()))
                    {
                        user.Contacts.Add(newContact.username());
                        newContact.Contacts.Add(user.username());
                        Contactslist.ItemsSource = user.Contacts;

                        // Serialize the data to JSON
                        string jsonData = JsonSerializer.Serialize(user);
                        string jsonData1 = JsonSerializer.Serialize(newContact);

                        var response1 = httpClient.PutAsJsonAsync(BaseUrl, jsonData).Result;
                        var response2 = httpClient.PutAsJsonAsync(BaseUrl, jsonData1).Result;



                        if (response1.IsSuccessStatusCode && response1.IsSuccessStatusCode)
                        {
                            // Handle success
                            MessageBox.Show("Contact added successfully.");
                        }
                        else
                        {
                            // Handle failure
                            MessageBox.Show("Failed to add contact.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Contact already exists.");
                    }


                }
                else
                {
                    MessageBox.Show("Invalid username. Please try again.");
                }

           

            

            }
            catch (Exception ex)
            {
                string s = ex.Message;
                MessageBox.Show("" + ex);

            }

            
        }
    }
}
