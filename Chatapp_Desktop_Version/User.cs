using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Chatapp_Desktop_Version
{
    internal class User
    {

        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        public string userdata()
        {
            return $"{UserName}, {Password}";
        }
    }
}
