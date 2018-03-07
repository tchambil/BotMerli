using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;
using SimpleEchoBot.Services;
using Microsoft.Bot.Connector;
using SimpleEchoBot.Model;
using System.Diagnostics;
using SimpleEchoBot.Extension;
using System.Globalization;
using System.Collections.Generic;
using System.Threading;

namespace SimpleEchoBot.Dialogs
{
    [Serializable]
    public class SearchPeopleAdmDialog : IDialog<object>
    {
       
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Indícame el nombre y apellidos de la persona:");
            context.Wait(MessageRecievedAsync);
        }
        public virtual async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            try
            {
                PeopeAppService searchService = new PeopeAppService();
                List<People> searchResult = await searchService.SearchByNamePeople(message.Text);
                if (searchResult.Count > 0)
                {
                    CardUtil.ShowPeopleHeroCard(message, searchResult);
                    Thread.Sleep(4000);
                    await SelectedConfirm(context);
                }
                else
                {

                    await context.PostAsync(string.Format(CultureInfo.CurrentCulture, "¡LO SIENTO...! No encontré la información. Por favor,  intente nuevamente"));
                    await StartAsync(context);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error when searching for people: {e.Message}");
            }
           // context.Done<object>(null);
        }
        private async Task SelectedConfirm(IDialogContext context)
        {
            PromptDialog.Confirm(context, Confirmed, "¿Desea buscar a otro colaborador?");

        }

        public async Task Confirmed(IDialogContext context, IAwaitable<bool> argument)
        {
            var reply = context.MakeMessage();
            bool isCorrect = await argument;
            if (isCorrect)
            {
                await StartAsync(context);
            }
            else
            {

                context.Done<object>(null);
            }
        }
       
    }
}