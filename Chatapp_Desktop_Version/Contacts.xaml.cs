﻿using System;
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
using System.Collections.ObjectModel;

namespace Chatapp_Desktop_Version
{
   
    public partial class Contacts : Window
    {
        private User user = new User();
        private HttpClient httpClient;
        private const string BaseUrl = "http://localhost:8080/ThorChat/users/contacts";
        private const string BaseUrl2 = "http://localhost:8080/ThorChat/users";
        private User newContact = new User();

        /*private ObservableCollection<string> contacts;

        public ObservableCollection<string> ContactsList
        {
            get { return contacts; }
            private set { contacts = value; }
        }*/

        public Contacts(User currentuser)
        {
            InitializeComponent();
            user = currentuser;

            // Füge vorhandene Kontakte der Listbox hinzu
            foreach (string contact in user.Contacts)
            {
                Contactslist.Items.Add(contact);
            }

            // contacts = new ObservableCollection<string>(user.Contacts);
            DataContext = this;
            httpClient = new HttpClient();
        }

        // Methode zum Hinzufügen eines Kontakts
        private async void Add_Contact_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string newContactString = AddContactInput.Text;
                AddContactInput.Text = null;
                string data = user.UserName + "," + newContactString;

                string requestUrl = $"{BaseUrl2}/{newContactString}";

                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    newContact = JsonSerializer.Deserialize<User>(responseBody);

                    if (!user.Contacts.Contains(newContact.UserName))
                    {
                        var response1 = httpClient.PutAsJsonAsync(BaseUrl, data).Result;

                        user.Contacts.Add(newContact.UserName);
                        Contactslist.Items.Add(newContact.UserName);
                        // ContactsList.Add(newContact.UserName);

                        if (response1.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Kontakt erfolgreich hinzugefügt.");
                        }
                        else
                        {
                            MessageBox.Show("Fehler beim Hinzufügen des Kontakts.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Kontakt existiert bereits.");
                    }
                }
                else
                {
                    MessageBox.Show("Kontakt existiert bereits.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Hinzufügen des Kontakts: " + ex.Message);
            }
        }

        // Methode zum Behandeln der Auswahl eines Kontakts aus der Liste
        private void Contactslist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Contactslist.SelectedItem != null)
            {
                string selectedContact = Contactslist.SelectedItem.ToString();
                Chat_Fenster chat_fenster = new Chat_Fenster(user.UserName, selectedContact);
                Close();
                chat_fenster.Show();
            }
        }
    }
}
