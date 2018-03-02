using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot.Model
{
    [Serializable]
    public class UserLogin
    {
        public string UserOrEmailAdrees { get; set; }
        public string Password { get; set; }
    }
}