using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatapp_Desktop_Version
{
    internal class Chat
    {
        public string Id { get; set; }
        public List<string> Participants { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
