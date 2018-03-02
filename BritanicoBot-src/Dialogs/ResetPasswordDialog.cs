using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;
using SimpleEchoBot.Services;
using Microsoft.Bot.Connector;
using SimpleEchoBot.Model;
using System.Diagnostics;
using SimpleEchoBot.Extension;
using System.Globalization;

namespace SimpleEchoBot.Dialogs
{
    [Serializable]
    public class ResetPasswordDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Indícanos su correo electrónico:");
            context.Wait(MessageRecievedAsync);
        }
        public virtual async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            await context.PostAsync(string.Format(CultureInfo.CurrentCulture, "Esta sección se encuentra en contrucción pronto tendremos novedades.", message.Text));
            context.Done<object>(null);
        }
    }
}