using Microsoft.Bot.Connector;
using SimpleEchoBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleEchoBot.Extension
{
    public class CardUtil
    {
        public static async void ShowPeopleHeroCard(IMessageActivity message, List<People> input)
        {
            Activity reply = ((Activity)message).CreateReply();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
          
            foreach (var item in input)
            {
                List<CardImage> cardImages = new List<CardImage>();
                cardImages.Add(new CardImage(url: item.Imagen == null? "https://cdn1.iconfinder.com/data/icons/unique-round-blue/93/user-256.png":item.Imagen));
                HeroCard card = new HeroCard()
                {
                    Title = item.Nombres,
                    Subtitle = (item.Puesto == null ? "" : item.Puesto) + "-" + (item.Centro == null ? "" : item.Centro),
                    Text = "Código: " + (item.Codigo == null ? "" : item.Codigo),
                    //Text = "Código: " + item.Codigo + "\n\n\u200CEmail: " + item.EmailAddress + "\n\n\u200CPhone/Anexo: " + item.Phone,
                    Images = cardImages
                };
                reply.Attachments.Add(card.ToAttachment());
            }            
            ConnectorClient connector = new ConnectorClient(new Uri(reply.ServiceUrl));
            await connector.Conversations.SendToConversationAsync(reply);
        }
        public static async void ShowRRHHHolidaysCard(IMessageActivity message, Holiday input)
        {
            Activity reply = ((Activity)message).CreateReply();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            List<CardImage> cardImages = new List<CardImage>(); 
            HeroCard card = new HeroCard()
            {
                //Title = input.people.Nombres,
                //Subtitle = input.people.Puesto,
                Text ="Ud. tiene "+input.Cantidad +" días de vacaciones disponibles",
                Images = cardImages
            };
            reply.Attachments.Add(card.ToAttachment()); 
            ConnectorClient connector = new ConnectorClient(new Uri(reply.ServiceUrl));
            await connector.Conversations.SendToConversationAsync(reply);
        }
        public static async void ShowRRHHvoucherCard(IMessageActivity message, Holiday input)
        {
            Activity reply = ((Activity)message).CreateReply();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "https://cdn0.iconfinder.com/data/icons/office-files-icons/110/Pdf-File-512.png"));
            HeroCard card = new HeroCard()
            {
                Title = "BOLETA DE PAGO",
                Images = cardImages,
                Buttons = new List<CardAction>()
                {
                    new CardAction(ActionTypes.DownloadFile, "Ver", value:"https://botteo.herokuapp.com/files/BoletaPago.pdf"), 
                }
            }; 

            reply.Attachments.Add(card.ToAttachment());
            ConnectorClient connector = new ConnectorClient(new Uri(reply.ServiceUrl));
            await connector.Conversations.SendToConversationAsync(reply);
        }
     
    }
}