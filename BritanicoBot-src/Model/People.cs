using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot.Model
{
    [Serializable]
    public class People
    { 
        public string Codigo{ get; set; }
        public string Imagen{ get; set; }
        public string Puesto{ get; set; }
        public string Centro{ get; set; }
        public string Nombres{ get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
    }
}