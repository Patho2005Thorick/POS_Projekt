using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Chatapp_Desktop_Version
{
    internal class Chat
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("participants")]
        public List<string> Participants { get; set; }

        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
