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
    public class RRHHPeopleDialog : IDialog<object>
    {
        private UserLogin user;
        private People people;
        private ResultAutenticate login;
        public RRHHPeopleDialog(UserLogin _user, People _people)
        {
            user = _user;
            people = _people;
        }
      
        public async Task StartAsync(IDialogContext context)
        {
            //await context.PostAsync("Para obtener información ingresa tu código de empleado.");
            if (!Session.Result)
            {
                PromptDialog.Text(context, ResumePeopleUserName, "Para obtener información, ingresa tu código de empleado:");
            }
            else
            {
                 
                await GetInformationRRHHSign(context);
            }               
             
        }
        private async Task ResumePeopleUserName(IDialogContext context, IAwaitable<string> result)
        {
            var answer = await result;
            user.UserOrEmailAdrees = answer;
            PromptDialog.Text(context, GetInformationRRHH, "Ahora, ingresa tu DNI:");
        }
        private async Task GetInformationRRHHSign(IDialogContext context)
        { 
            try
            {
                PeopeAppService searchService = new PeopeAppService();
                if (!Session.Result)
                {
                    await StartAsync(context);
                }
                if (Session.Result)
                {
                    var message = context.MakeMessage();
                    message.Attachments = new List<Attachment>();
                    message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    message.Attachments.Add(SettingsCardDialog.CardRRHH().ToAttachment());
                    message.Text = $"Hola, " + Session.Nombre;
                    await context.PostAsync(message);
                    context.Wait(MessageRecievedAsync);
                }
                else
                {

                    await context.PostAsync(string.Format(CultureInfo.CurrentCulture, "Tus datos no son correctos. Por favor intenta nuevamente", user.UserOrEmailAdrees));
                    PromptDialog.Text(context, ResumePeopleUserName, "Ingresa nuevamente tu código de empleado:");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error when searching for people: {e.Message}");
            }

        }
        private async Task GetInformationRRHH(IDialogContext context, IAwaitable<string> result)
        {
            var answer = await result;
           
            try
            {
                PeopeAppService searchService = new PeopeAppService();
               
                user.Password = answer;
                login = await searchService.Autenticate(user);
                Session.Result = login.Result;
                Session.Codigo = login.Codigo;
                Session.Nombre = login.Nombres;
                Session.Expiration = DateTime.UtcNow;
                if (Session.Result)
                {
                    var message = context.MakeMessage();
                    message.Attachments = new List<Attachment>();
                    message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    message.Attachments.Add(SettingsCardDialog.CardRRHH().ToAttachment());
                    message.Text = $"Hola, " + Session.Nombre;
                    await context.PostAsync(message);
                    context.Wait(MessageRecievedAsync);
                }
                else
                {

                      await context.PostAsync(string.Format(CultureInfo.CurrentCulture, "Tus datos no son correctos. Por favor intenta nuevamente", user.UserOrEmailAdrees));
                    PromptDialog.Text(context, ResumePeopleUserName, "Ingresa nuevamente tu código de empleado:");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error when searching for people: {e.Message}");
                await context.PostAsync("¡LO SIENTO...! Por el momento no esta disponible este servicio. Por favor, intente más tarde.");
                context.Done<object>(null);
            }
         
        }

        public virtual async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var CategoryName = await result;
            try
            {
                PeopeAppService searchService = new PeopeAppService();
                Holiday holiday = await searchService.GetRRHHHolidays(login);
                holiday.Nombres = Session.Nombre;
                if (CategoryName != null)
                {
                    switch (CategoryName.Text)
                    {
                        case SettingsCardDialog.RRHHHolidays:
                            CardUtil.ShowRRHHHolidaysCard(CategoryName, holiday);                       
                            Thread.Sleep(4000);
                             await SelectedConfirm(context);
                            break;
                        case SettingsCardDialog.RRHHvoucher:
                            CardUtil.ShowRRHHvoucherCard(CategoryName, holiday);
                            Thread.Sleep(4000);
                             await SelectedConfirm(context);
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
            catch (Exception e)
            {
                Debug.WriteLine($"Error when searching for people: {e.Message}");
            }
         //   context.Done<object>(null);
        }
       
        private async Task SelectedConfirm(IDialogContext context)
        {
            PromptDialog.Confirm(context, Confirmed, "¿Desea realizar otra consulta RRHH?");
          //  return Task.CompletedTask;
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
                Session.Codigo = null;
                Session.Result = false;
                context.Done<object>(null);
              //  context.Call(new ScoreDialog(), ResumeAfterOptionDialog);
            }
        }
        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            context.Done<object>(null);
        }
    }
}