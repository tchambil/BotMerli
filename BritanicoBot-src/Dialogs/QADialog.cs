using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Azure;

namespace SimpleEchoBot.Dialogs
{
    [Serializable]
    public class QADialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            /* Wait until the first message is received from the conversation and call MessageReceviedAsync 
            *  to process that message. */
            var message = context.MakeMessage();
            message.Text = $"Por el momento puedo responderte las preguntas frecuentes que se encuentran en https://www.britanico.edu.pe/preguntas-frecuentes/ y dime cual es tu pregunta:";
            await context.PostAsync(message);
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            /* When MessageReceivedAsync is called, it's passed an IAwaitable<IMessageActivity>. To get the message,
            *  await the result. */
            var message = await result;

            var qnaSubscriptionKey = ConfigurationManager.AppSettings["QnASubscriptionKey"];
            var qnaKBId = ConfigurationManager.AppSettings["QnAKnowledgebaseId"];
            //var qnaSubscriptionKey = Utils.GetAppSetting("QnASubscriptionKey");
            // var qnaKBId = Utils.GetAppSetting("QnAKnowledgebaseId");

            // QnA Subscription Key and KnowledgeBase Id null verification
            if (!string.IsNullOrEmpty(qnaSubscriptionKey) && !string.IsNullOrEmpty(qnaKBId))
            {
                await context.Forward(new BasicQnAMakerDialog(), AfterAnswerAsync, message, CancellationToken.None);
            }
            else
            {
                await context.PostAsync("Please set QnAKnowledgebaseId and QnASubscriptionKey in App Settings. Get them at https://qnamaker.ai.");
            }

        }
        private async Task SelectedConfirm(IDialogContext context)
        {
          

        }
        public async Task Confirmed(IDialogContext context, IAwaitable<bool> argument)
        {
            var reply = context.MakeMessage();
            bool isCorrect = await argument;
            if (isCorrect)
            {
                var message = context.MakeMessage();
                message.Text = $"¡Muy Bien! Pregúntame lo que deseas saber:";
                await context.PostAsync(message);
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                context.Done<object>(null);
            }
        }
        private async Task AfterAnswerAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            // wait for the next user message
            PromptDialog.Confirm(context, Confirmed, "¿Tienes alguna pregunta más?");
         
        }
    }
    [Serializable]
    public class BasicQnAMakerDialog : QnAMakerDialog
    {
        // Go to https://qnamaker.ai and feed data, train & publish your QnA Knowledgebase.        
        // Parameters to QnAMakerService are:
        // Required: subscriptionKey, knowledgebaseId, 
        // Optional: defaultMessage, scoreThreshold[Range 0.0 – 1.0]
        //public BasicQnAMakerDialog() : base(new QnAMakerService(new QnAMakerAttribute(Utils.GetAppSetting("QnASubscriptionKey"), Utils.GetAppSetting("QnAKnowledgebaseId"), "No good match in FAQ.", 0.5)))


        public BasicQnAMakerDialog() : base(new QnAMakerService(new QnAMakerAttribute(ConfigurationManager.AppSettings["QnASubscriptionKey"], ConfigurationManager.AppSettings["QnAKnowledgebaseId"], "No entendí lo que quisiste decir.", 0.5)))
        { }
    }
}