﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot.Model
{
    [Serializable]
    public class ResultAutenticate
    {
        public bool Result { get; set; }
        public string Codigo { get; set; }
        public string Nombres { get; set; }
    }
}