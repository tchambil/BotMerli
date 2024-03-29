﻿using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using System.Globalization;
using SimpleEchoBot.Extension;
using System.Threading;
using SimpleEchoBot.Model;

namespace SimpleEchoBot.Dialogs
{
    [Serializable]
    public class MainDialog : IDialog<object>
    {
               
        public async Task StartAsync(IDialogContext context)
        {
            Session.Codigo = null;
            Session.Result = false;
            if (!Session.Greet)
            {
                Session.Greet = true;
                var message = context.MakeMessage();                
                message.Text = $"Permíteme ayudarte en los siguientes temas:";
                await context.PostAsync(message);
                await ResumeAfter(context);
               
            }
            else
            {
                Session.Greet = false;
               // await StartAsync(context);
                context.Wait(this.MessageReceivedAsync);
               
            }
            
        }
        public async Task ResumeAfter(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            message.Attachments.Add(SettingsCardDialog.CardIntranet().ToAttachment());
            message.Attachments.Add(SettingsCardDialog.CardInfColaborador().ToAttachment());
            message.Attachments.Add(SettingsCardDialog.CardSolucionesTI().ToAttachment());
           // message.Text = $"Te puedo ayudar con las siguientes opciones.";
            await context.PostAsync(message);
            context.Wait(this.MessageReceivedAsync);

        }
        private async Task CardCarousel(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            message.Attachments.Add(SettingsCardDialog.CardIntranet().ToAttachment());
            message.Attachments.Add(SettingsCardDialog.CardInfColaborador().ToAttachment());
            message.Attachments.Add(SettingsCardDialog.CardSolucionesTI().ToAttachment());
            message.Text = $"Te puedo ayudar con las siguientes opciones.";
            await context.PostAsync(message);
            context.Wait(this.MessageReceivedAsync);

        }
        public async Task ProcessMessageReceived(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            string CategoryName = message.Text;
            if (CategoryName != null)
            {
               switch (CategoryName)
                {
                    case SettingsCardDialog.InTDocsfrecuent: 
                        await CardDoscFrecuent(context);
                        Thread.Sleep(4000);
                        await SelectedConfirm(context);
                        break;
                    case SettingsCardDialog.InTHelpDesk:
                        await SelectedInTHelpDesk(context);
                        Thread.Sleep(4000);
                        await CardPetitions(context);
                        Thread.Sleep(4000);
                        await SelectedConfirm(context);
                        break;
                    case SettingsCardDialog.PeopleAdm:
                        // context.Call(new PeopleAdmDialog(), ResumeAfterOptionDialog);
                        context.Call(new SearchPeopleAdmDialog(), ResumeAfterOptionDialog);                        
                        break;
                    case SettingsCardDialog.PeopleCentro:
                        context.Call(new SearchDocenteDialog(), ResumeAfterOptionDialog);
                    
                        break;                    
                    case SettingsCardDialog.ITOptions:
                        await SelectedITOptionsk(context);
                        
                        break;
                    case SettingsCardDialog.ITPreguntaFrecuentes:
                        context.Call(new QADialog(), ResumeAfterOptionDialog);
                        break;
                    default:
                        //  await context.PostAsync(string.Format(CultureInfo.CurrentCulture, "No entendí lo que quisiste decir.", CategoryName));
                        Session.Greet = false;
                        await StartAsync(context);
                        //context.Done(CategoryName);
                        break;

                } 
               // context.Done(CategoryName);
            }
            else
            {

                // await context.PostAsync(string.Format(CultureInfo.CurrentCulture, "No entendí lo que quisiste decir.", CategoryName));
                // context.Wait(this.MessageReceivedAsync);
                Session.Greet = false;
                await StartAsync(context);
                //context.Done(CategoryName);
            }
        }

        protected async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (message.Text.Equals("", StringComparison.InvariantCultureIgnoreCase))
            {
                await this.StartAsync(context);
            }
            else
            {
                await this.ProcessMessageReceived(context, result);
            }
        }
        private async Task SelectedConfirmTI(IDialogContext context)
        {
            PromptDialog.Confirm(context, ConfirmedTI, "¿Te puedo ayudar en algo más?");
        }
        private async Task SelectedConfirm(IDialogContext context)
        {
            PromptDialog.Confirm(context, Confirmed, "¿Te puedo ayudar en algo más?");

        }
        public async Task ConfirmedTI(IDialogContext context, IAwaitable<bool> argument)
        {
            var reply = context.MakeMessage();
            bool isCorrect = await argument;
            if (isCorrect)
            {
                await SelectedITOptionsk(context);
            }
            else
            {
                context.Call(new ScoreDialog(), ResumeAfterOptionDialog);

            }
        }
        public async Task Confirmed(IDialogContext context, IAwaitable<bool> argument)
        {
            var reply = context.MakeMessage();
            bool isCorrect = await argument;
            if (isCorrect)
            {
                await CardCarousel(context);
            }
            else
            {
                context.Call(new ScoreDialog(), ResumeAfterOptionDialog);
               
            }
        }
       
        private async Task SelectedInTHelpDesk(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            message.Attachments.Add(SettingsCardDialog.CardHelpDesk().ToAttachment());
            message.Text = $"¡GENIAL...! Con esta guía podrás realizar tus peticiones de manera más sencilla.";
            await context.PostAsync(message);
        }
        private async Task CardDoscFrecuent(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            message.Attachments.Add(SettingsCardDialog.CardDocsFrencuent().ToAttachment());
            message.Text = $"¡CORRECTO...! Te presento los documentos más solicitados. Elige el que deseas descargar.";
            await context.PostAsync(message);

        }
        private async Task SelectedITOptionsk(IDialogContext context)
        {
            PromptDialog.Choice(context, AfterMenuSelection, new List<string>()
            { SettingsCardDialog.InTSearchDocuments,
                SettingsCardDialog.OPComputer,
                SettingsCardDialog.OPPcPrint,
                SettingsCardDialog.OPTicket,
                SettingsCardDialog.OPProyector,
                SettingsCardDialog.OPConnectivity,
                SettingsCardDialog.OPEmailOutlook                

            },
                "¡CORRECTO...! Con estas opciones podrás solucionar de manera rápida tu problema.");

        }      
        private async Task AfterMenuSelection(IDialogContext context, IAwaitable<string> result)
        {
            var message = context.MakeMessage();
            var optionSelected = await result;
            switch (optionSelected)
            {
                case SettingsCardDialog.InTSearchDocuments:
                    CardUtil.ShowSearchDocumentCard(context);
                    Thread.Sleep(4000);
                    await SelectedConfirmTI(context);
                    break;
                case SettingsCardDialog.OPComputer:
                      message.Attachments.Add(SettingsCardDialog.CardComputerOptions().ToAttachment());
                    await context.PostAsync(message);
                    Thread.Sleep(4000);
                    await SelectedConfirmTI(context);
                    break;
                case SettingsCardDialog.OPPcPrint:
                    message.Attachments.Add(SettingsCardDialog.CardPCPrintOptions().ToAttachment());
                    await context.PostAsync(message);
                    Thread.Sleep(4000);
                    await SelectedConfirmTI(context);
                    break;

                case SettingsCardDialog.OPTicket:
                       message.Attachments.Add(SettingsCardDialog.CardTickesOptions().ToAttachment());
                    await context.PostAsync(message);
                    Thread.Sleep(4000);
                    await SelectedConfirmTI(context);
                    break;
                case SettingsCardDialog.OPProyector:
                     message.Attachments.Add(SettingsCardDialog.CardProyectorOptions().ToAttachment());
                    await context.PostAsync(message);
                    Thread.Sleep(4000);
                    await SelectedConfirmTI(context);
                    break;
                case SettingsCardDialog.OPConnectivity:
                    message.Attachments.Add(SettingsCardDialog.CardConnectivityOptions().ToAttachment());
                    await context.PostAsync(message);
                    Thread.Sleep(4000);
                    await SelectedConfirmTI(context);
                    break;
                case SettingsCardDialog.OPEmailOutlook:
                    message.Attachments.Add(SettingsCardDialog.CardEmailOutlookOptions().ToAttachment());
                    await context.PostAsync(message);
                    Thread.Sleep(4000);
                    await SelectedConfirmTI(context);
                    break;
                
                default:
                    await context.PostAsync(string.Format(CultureInfo.CurrentCulture, "La opción {0} no es válida. Por favor intente de nuevo", optionSelected ));
                 await SelectedITOptionsk(context);
                    break;
               
            }
        }
        private async Task CardPetitions(IDialogContext context)
        {
            var message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            message.Attachments.Add(SettingsCardDialog.CardPeticionesHelpDesk().ToAttachment()); 
            message.Text = $"Si deseas, puedes empezar aquí. Elige tu opción y se abrirá la página correspondiente.";
            await context.PostAsync(message);
            

        }
        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
          
            await CardCarousel(context);
        }
    }

}