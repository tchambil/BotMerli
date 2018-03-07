using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot.Model
{
    [Serializable]
    public static class Session
    {
        public static bool Result { get; set; }
        public static string Codigo { get; set; }
        public static string Nombre { get; set; }
        public static DateTime Expiration { get; set; }
        public static TimeSpan Start { get; set; }
    }
}