using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using SimpleEchoBot.Extension;
using SimpleEchoBot.Model;
using SimpleEchoBot.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SimpleEchoBot.Dialogs
{
    [Serializable]
    public class SearchDocenteDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Indícanos el nombre y apellidos del docente:");
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

                    await context.PostAsync(string.Format(CultureInfo.CurrentCulture, "No hemos encontrado información. Por favor intente de nuevo"));
                    await StartAsync(context);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error when searching for people: {e.Message}");
            }
           // context.Done<object>(null);
        }

        private Task SelectedConfirm(IDialogContext context)
        {
            PromptDialog.Confirm(context, Confirmed, "¿Desea buscar a otro docente?");
            return Task.CompletedTask;
        }

        public async Task Confirmed(IDialogContext context, IAwaitable<bool> argument)
        {
       
        
            bool isCorrect = await argument;
            if (isCorrect)
            {
                await StartAsync(context);
            }
            else
            {
                context.Call(new ScoreDialog(), ResumeAfterOptionDialog);               
            }
        }
        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
        }

    }
}