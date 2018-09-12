using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class DialogModel
    {
        public User Friend { get; set; }

        public User Me { get; set; }

        public List<Message> Messages { get; set; }
    }
}