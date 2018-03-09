using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Web.Http.Description;
using System.Net.Http;
using System;
using System.Linq;
using SimpleEchoBot.Dialogs;
using System.Collections.Generic;
using SimpleEchoBot.Extension;
using SimpleEchoBot.Model;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {


            // check if activity is of type message
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                Activity isTypingReply = activity.CreateReply();
                isTypingReply.Type = ActivityTypes.Typing;
                await connector.Conversations.ReplyToActivityAsync(isTypingReply);

                await Conversation.SendAsync(activity, () => new MainDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }

            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                IConversationUpdateActivity update = message;
                var client = new ConnectorClient(new Uri(message.ServiceUrl), new MicrosoftAppCredentials());
                if (update.MembersAdded != null && update.MembersAdded.Any())
                {
                    foreach (var newMember in update.MembersAdded)
                    {
                        if (newMember.Id != message.Recipient.Id)
                        {
                            if (!Session.Greet)
                            {
                                var init = new List<Attachment>()
                                {
                                     SettingsCardDialog.CardIntranet().ToAttachment(),
                                     SettingsCardDialog.CardInfColaborador().ToAttachment(),
                                     SettingsCardDialog.CardSolucionesTI().ToAttachment(),
                                };
                                var reply = message.CreateReply();
                                reply.Text = $"¡Hola, soy Merlí! Encantado de poder interactuar contigo.  Permíteme ayudarte en los siguientes temas:";
                                reply.Attachments = init;
                                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                                client.Conversations.ReplyToActivityAsync(reply);
                                Session.Greet = true;
                            }
                        }
                    }
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                //ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                //Activity reply = message.CreateReply();
                //reply.Type = ActivityTypes.Typing;
                //reply.Text = "...";
                //connector.Conversations.ReplyToActivityAsync(reply);
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {

            }

            return null;
        }
    }
}