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
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("contacts")]
        public List<User> Contacts { get; set; }

        public string username()
        {
            return $"{UserName}";
        }
    }
}
