using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Chatapp_Desktop_Version
{
    /// <summary>
    /// Interaktionslogik für Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {
        public Chat()
        {
            InitializeComponent();
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageInput.Text;
            // Add code here to send the message to your backend
            // Then, add the message to the chat window
            AddMessage(message, true); // Assuming outgoing message
        }

        private void AddMessage(string message, bool outgoing)
        {
            var messageElement = new TextBlock
            {
                Text = message,
                Background = Brushes.LightGray, // Adjust background color as needed
                Padding = new Thickness(5, 10,5,10),
                MaxWidth = 400, // Adjust as needed
                TextWrapping = TextWrapping.Wrap
            };

            if (outgoing)
            {
                messageElement.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else
            {
                messageElement.HorizontalAlignment = HorizontalAlignment.Left;
            }

            ChatMessages.Children.Add(messageElement);
        }
    }
}
