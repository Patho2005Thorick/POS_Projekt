using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chatapp_Desktop_Version
{
    public partial class Chat_Fenster : Window
    {
        private string contactName { get; set; }
        private string userName { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        private Chat currentchat { get; set; }
        private HttpClient httpClient;
        private User currentuser;
        private User contact;
        private const string BaseUrl = "http://localhost:8080/ThorChat/users";
        private const string BaseUrl2 = "http://localhost:8080/ThorChat/chats/create";
        private const string UpdateChatUrl = "http://localhost:8080/ThorChat/chats/update"; // Assuming you have this endpoint

        public Chat_Fenster(string userName, string contactName)
        {
            InitializeComponent();
            this.userName = userName;
            this.contactName = contactName;
            Messages = new ObservableCollection<string>();
            DataContext = this;
            httpClient = new HttpClient();
            currentchat = new Chat();
            function();

            // Bind Messages to the ListView
            MessagesListView.ItemsSource = Messages;

            // Register the closing event
            this.Closing += Chat_Fenster_Closing;
        }

        private async void function()
        {
            try
            {
                string requestUrl = $"{BaseUrl}/{userName}";

                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    currentuser = JsonSerializer.Deserialize<User>(responseBody);

                    requestUrl = $"{BaseUrl}/{contactName}";

                    response = await httpClient.GetAsync(requestUrl);
                    // Serialize the data to JSON

                    if (response.IsSuccessStatusCode)
                    {
                        responseBody = await response.Content.ReadAsStringAsync();
                        contact = JsonSerializer.Deserialize<User>(responseBody);

                        foreach (string id in currentuser.Chat_IDs)
                        {
                            foreach (string id2 in contact.Chat_IDs)
                            {
                                if (id.Equals(id2))
                                {
                                    currentchat.Id = id2;
                                    break;
                                }
                            }
                            if (!string.IsNullOrEmpty(currentchat.Id))
                            {
                                break;
                            }
                        }

                        if (string.IsNullOrEmpty(currentchat.Id))
                        {
                            List<string> participants = new List<string>
                            {
                                currentuser.UserName,
                                contact.UserName
                            };

                            var json = JsonSerializer.Serialize(participants);
                            var content = new StringContent(json, Encoding.UTF8, "application/json");

                            response = await httpClient.PostAsync(BaseUrl2, content);

                            if (response.IsSuccessStatusCode)
                            {
                                responseBody = await response.Content.ReadAsStringAsync();
                                currentchat = JsonSerializer.Deserialize<Chat>(responseBody);

                                currentuser.Chat_IDs.Add(currentchat.Id);
                                contact.Chat_IDs.Add(currentchat.Id);

                                var response1 = httpClient.PutAsJsonAsync(BaseUrl, data).Result;

                                user.Contacts.Add(newContact.UserName);
                                Contactslist.Items.Add(newContact.UserName);
                                // ContactsList.Add(newContact.UserName);


                                if (response1.IsSuccessStatusCode)
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
                        }
                    }
                }
                else
                {
                    MessageBox.Show("A Problem has occurred.");
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                MessageBox.Show("" + ex);
            }
        }

        private async void Chat_Fenster_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                var json = JsonSerializer.Serialize(currentchat);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PutAsync($"{UpdateChatUrl}/{currentchat.Id}", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Succeded to update chat on server.");
                }

                else if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Failed to update chat on server.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating chat on server: " + ex.Message);
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MessageTextBox.Text))
            {
                string timestamp = $"{DateTime.Now:dd HH:mm}";
                Messages.Add($"Me: {MessageTextBox.Text}\n\t\t\t\t {timestamp}");

                Message message = new Message
                {
                    Sender = currentuser.UserName,
                    Content = MessageTextBox.Text,
                    Timestamp = DateTime.Now
                };

                currentchat.Messages.Add(message);
                MessageTextBox.Clear();
            }
        }
    }

   
}
