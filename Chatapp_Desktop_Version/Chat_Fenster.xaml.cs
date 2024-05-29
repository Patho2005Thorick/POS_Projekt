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
        private const string GetChatUrl = "http://localhost:8080/ThorChat/chats";
        private const string UpdateChatUrl = "http://localhost:8080/ThorChat/chats/update";

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

        }

        

        private async void function()
        {
            try
            {
                // Construct request URLs
                string currentUserUrl = $"{BaseUrl}/{userName}";
                string contactUrl = $"{BaseUrl}/{contactName}";

                // Create tasks for both requests
                var currentUserTask = httpClient.GetAsync(currentUserUrl);
                var contactTask = httpClient.GetAsync(contactUrl);

                // Await both tasks
                await Task.WhenAll(currentUserTask, contactTask);

                var currentUserResponse = await currentUserTask;
                var contactResponse = await contactTask;

                if (currentUserResponse.IsSuccessStatusCode && contactResponse.IsSuccessStatusCode)
                {
                    string currentUserBody = await currentUserResponse.Content.ReadAsStringAsync();
                    string contactBody = await contactResponse.Content.ReadAsStringAsync();

                    currentuser = JsonSerializer.Deserialize<User>(currentUserBody);
                    contact = JsonSerializer.Deserialize<User>(contactBody);

                    bool chatFound = false;

                    // Find a common Chat_ID
                    foreach (string id in currentuser.Chat_IDs)
                    {
                        foreach (string id2 in contact.Chat_IDs)
                        {
                            if (chatFound)
                            {
                                break;
                            }
                            else if (id.Equals(id2))
                            {
                                // Common Chat_ID found
                                string chatUrl = $"{GetChatUrl}/{id}";
                                HttpResponseMessage chatResponse = await httpClient.GetAsync(chatUrl);

                                if (chatResponse.IsSuccessStatusCode)
                                {
                                    string chatBody = await chatResponse.Content.ReadAsStringAsync();
                                    currentchat = JsonSerializer.Deserialize<Chat>(chatBody);

                                    foreach (Message message in currentchat.Messages)
                                    {
                                        Messages.Add(message.Content);
                                    }
                                   
                                    chatFound = true;
                                }
                                break;
                            }
                        }
                        if (chatFound)
                        {
                            break;
                        }
                    }

                    // If no common chat was found, create a new chat
                    if (!chatFound)
                    {
                        List<string> participants = new List<string>
                        {
                            currentuser.UserName,
                            contact.UserName
                        };

                        var json = JsonSerializer.Serialize(participants);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        HttpResponseMessage createChatResponse = await httpClient.PostAsync(BaseUrl2, content);

                        if (createChatResponse.IsSuccessStatusCode)
                        {
                            string chatBody = await createChatResponse.Content.ReadAsStringAsync();
                            currentchat = JsonSerializer.Deserialize<Chat>(chatBody);
                            MessageBox.Show("Chat added successfully.");
                        }
                        else
                        {
                            // Handle failure
                            MessageBox.Show("Failed to add chat.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Failed to get user or contact details.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }



        private  async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MessageTextBox.Text))
            {
                string timestamp = $"{DateTime.Now:dd HH:mm}";
                Messages.Add($"Me: {MessageTextBox.Text}");

                Random rm = new Random();
               int randomId =  rm.Next(100000, 500000);

                Message message = new Message
                {
                    Id = $"{randomId}",
                    Sender = currentuser.UserName,
                    Content = MessageTextBox.Text,
                };

               
                try
                {
                    
                    currentchat.Messages.Add(message);
                    var json = JsonSerializer.Serialize(currentchat);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    MessageBox.Show(json);
                    

                    var response = httpClient.PutAsync($"{UpdateChatUrl}/{currentchat.Id}", content).Result;
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
                MessageTextBox.Clear();
            }
        }
    }

   
}
