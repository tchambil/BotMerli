using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleEchoBot.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SimpleEchoBot.Dialogs
{
    [Serializable]
    public class ScoreDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var message = context.MakeMessage();            
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "https://botteo.herokuapp.com/img/evaluate.png"));
            HeroCard plCard = new HeroCard()
            {
                Images = cardImages,
                Buttons =  new List<CardAction>()
                {
                    
                    
                    new CardAction(ActionTypes.ImBack, "1. Satisfecho", value:"1"),
                    new CardAction(ActionTypes.ImBack, "2. Neutral", value:"2"),
                    new CardAction(ActionTypes.ImBack, "3. Insatisfecho", value:"3"),
                                    }
            };             
           
            Attachment plAttachment = plCard.ToAttachment();
            message.Attachments.Add(plAttachment);
           // message.Text = "¿Que tan satisfecho estás con el servicio?";
            message.AttachmentLayout = "list";

            await context.PostAsync(message);
            context.Wait(MessageRecievedAsync);
        }
        public virtual async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = context.MakeMessage();
            var answer = await result;
            string CategoryName = answer.Text;
            if (CategoryName != null)
            {
                message.Text = "Muchas gracias, fue un placer ayudarlo.";
                await context.PostAsync(message);
            }
            else
            {

                await context.PostAsync(string.Format(CultureInfo.CurrentCulture, "La opción {0} no es válida. Por favor intente de nuevo", CategoryName));
               
            }
            Session.Greet = false;
            context.Done<object>(null);
        }
      
    }
}