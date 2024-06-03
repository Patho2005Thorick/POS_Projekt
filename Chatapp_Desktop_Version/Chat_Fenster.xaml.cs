using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
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
        private Chat currentchat { get; set; } = new Chat();
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

            // Bindet Messages zu der ListView
            MessagesListView.ItemsSource = Messages;

            Timer timer = new Timer(10000);
            timer.Elapsed += async (sender, e) => await CheckForUpdates();
            timer.AutoReset = true;
            timer.Start();
        }

        // Methode zum Überprüfen von Aktualisierungen im Chat
        private async Task CheckForUpdates()
        {
            string chatUrl = $"{GetChatUrl}/{currentchat.Id}";
            HttpResponseMessage chatResponse = await httpClient.GetAsync(chatUrl);

            if (chatResponse.IsSuccessStatusCode)
            {
                string chatBody = await chatResponse.Content.ReadAsStringAsync();
                Chat UpdatedChat = JsonSerializer.Deserialize<Chat>(chatBody);

                if (currentchat.Messages.Count != UpdatedChat.Messages.Count)
                {
                    Messages = new ObservableCollection<string>();
                    foreach (Message message in UpdatedChat.Messages)
                    {
                        Messages.Add(message.Content);
                    }
                }
            }
        }

        // Methode zur Initialisierung und Synchronisierung von Benutzer- und Chatdaten
        private async void function()
        {
            try
            {
                // Konstruiere die Anfrage-URLs
                string currentUserUrl = $"{BaseUrl}/{userName}";
                string contactUrl = $"{BaseUrl}/{contactName}";

                // Erstelle Tasks für beide Anfragen
                var currentUserTask = httpClient.GetAsync(currentUserUrl);
                var contactTask = httpClient.GetAsync(contactUrl);

                // Warte auf beide Tasks
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

                    // Finde eine gemeinsame Chat-ID
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
                                // Gemeinsame Chat-ID gefunden
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

                    // Wenn kein gemeinsamer Chat gefunden wurde, erstelle einen neuen Chat
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
                            MessageBox.Show("Chat erfolgreich hinzugefügt.");
                        }
                        else
                        {
                            // Behandle den Fehler
                            MessageBox.Show("Fehler beim Hinzufügen des Chats.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Benutzer- oder Kontaktdetails konnten nicht abgerufen werden.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ein Fehler ist aufgetreten: {ex.Message}");
            }
        }

        // Methode zum Senden einer Nachricht
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MessageTextBox.Text))
            {
                string timestamp = $"{DateTime.Now:dd HH:mm}";
                Messages.Add($"Me: {MessageTextBox.Text}");

                Random rm = new Random();
                int randomId = rm.Next(100000, 500000);

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
                        MessageBox.Show("Chat erfolgreich auf dem Server aktualisiert.");
                    }
                    else if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Fehler beim Aktualisieren des Chats auf dem Server.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Aktualisieren des Chats auf dem Server: " + ex.Message);
                }
                MessageTextBox.Clear();
            }
        }

        // Methode zum Löschen eines Chats
        private async void Delete_Chat(object sender, RoutedEventArgs e)
        {
            /*Wenn der Benutzer einen Chat löscht, kann er nicht mehr auf den Chat zugreifen,
             * da er nicht mehr die Chat-ID hat. Die Datenbank ist jedoch weiterhin in der Datenbank gespeichert.
             */
            Messages = null;
            string DeleteUrl = "http://localhost:8080/ThorChat/chats/delete";
            string data = $"{currentchat.Id},{currentuser.UserName}";
            var content = new StringContent(data, Encoding.UTF8,
