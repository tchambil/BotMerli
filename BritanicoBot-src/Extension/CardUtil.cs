using Microsoft.Bot.Builder.Dialogs;
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
                    Subtitle = (item.Puesto == null ? "" : item.Puesto) + "\n\n\u200C" + (item.Centro == null ? "" : item.Centro),
                    // Text = "Código: " + (item.Codigo == null ? "\n\n\u200C" : item.Codigo),
                    Text = "Email: " + (item.Email == null ? "" : item.Email),
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
        public static async void ShowSearchDocumentCard(IDialogContext context)
        {
            List<CardImage> cardImages = new List<CardImage>();
            List<CardImage> cardImages2 = new List<CardImage>();
            CardAction plButton = new CardAction(ActionTypes.OpenUrl, "", value: "https://botteo.herokuapp.com/img/searchdocs.png");
            CardAction plButton2 = new CardAction(ActionTypes.OpenUrl, "", value: "https://botteo.herokuapp.com/img/searchdocs2.png");

            CardImage cardImage = new CardImage(url: "https://botteo.herokuapp.com/img/searchdocs.png", tap: plButton);
            CardImage cardImage2 = new CardImage(url: "https://botteo.herokuapp.com/img/searchdocs2.png", tap: plButton2);
            cardImages.Add(cardImage);
            cardImages2.Add(cardImage2);
            HeroCard heroCard = new HeroCard()
            {
                Images = cardImages,
                Tap = plButton
            };
            HeroCard heroCard2 = new HeroCard()
            {
                Images = cardImages2,
                Tap = plButton2
            };

            var reply = context.MakeMessage();
            reply.Text = $"¡MUY BIEN...! Dirígete al ícono de la llave en la parte superior derecha de la intranet y colocar el nombre del documento en la barra de búsqueda.";
            Attachment attachment = heroCard.ToAttachment();
            reply.Attachments.Add(attachment);
            Attachment attachment2 = heroCard2.ToAttachment();
            reply.Attachments.Add(attachment2);
            await context.PostAsync(reply);
        }
    }
}