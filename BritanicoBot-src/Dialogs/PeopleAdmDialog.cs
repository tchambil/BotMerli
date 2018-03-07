using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleEchoBot.Extension;
using SimpleEchoBot.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SimpleEchoBot.Dialogs
{
    [Serializable]
    public class PeopleAdmDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            message.Attachments.Add(SettingsCardDialog.CardPeopleAdm().ToAttachment());     
            message.Text = $"¡Correcto! Tenemos estas opciones.";
            await context.PostAsync(message);
            context.Wait(MessageRecievedAsync);
        }
        public virtual async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            string CategoryName = message.Text;
            if (CategoryName != null)
            {
                switch (CategoryName)
                {
                    case SettingsCardDialog.PeoSearch:
                        context.Call(new SearchPeopleAdmDialog(), ResumeAfterOptionDialog);
                        break;
                    case SettingsCardDialog.PeoRRHH:
                        context.Call(new RRHHPeopleDialog(new UserLogin(), new People()), ResumeAfterOptionDialog);
                        break;
                    case SettingsCardDialog.ResetPassword:
                        context.Call(new ResetPasswordDialog(), ResumeAfterOptionDialog);
                        break; 
                    default:
                        await context.PostAsync(string.Format(CultureInfo.CurrentCulture, "La opción {0} no es válida. Por favor intente de nuevo", CategoryName));
                        await StartAsync(context);
                        break;

                } 
            }
            else
            {

                await context.PostAsync(string.Format(CultureInfo.CurrentCulture, "La opción {0} no es válida. Por favor intente de nuevo", CategoryName));
                context.Wait(this.MessageRecievedAsync);
            }
        }
        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
            //await StartAsync(context);
        }
    }
}