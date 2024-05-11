using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Chatapp_Desktop_Version
{
    public class User
    { 

        [JsonPropertyName("username")]
        public string UserName { get; set; }


        [JsonPropertyName("email")]
        private string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("chat_IDs")]
        private List<string> Chat_IDs { get; set; }

        [JsonPropertyName("contacts")]
        public List<string> Contacts { get; set; }

        public string username()
        {
            return $"{UserName}";
        }
    }
}
