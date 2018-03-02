﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace SimpleEchoBot.Extension
{
    public static class Utility
    {
        public static string Image2Base64(this string url)
        {
            try
            {
                var ruta = System.Web.Hosting.HostingEnvironment.MapPath(url);

                using (Image image = Image.FromFile(ruta))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();                         
                        string base64String = Convert.ToBase64String(imageBytes);
                        return $"data:image/jpg;base64,{base64String}";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }
    }
}