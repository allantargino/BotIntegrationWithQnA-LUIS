using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotIntegrationWithQnA_LUIS.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            try
            {
                QnADialog dialog = new QnADialog();
                await context.Forward(dialog, AfterQnADialog, activity, CancellationToken.None);
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
            }
            
        }

        private async Task AfterQnADialog(IDialogContext context, IAwaitable<object> result)
        {
            var message = context.Activity as Activity;
            bool foundResultInQnA = Convert.ToBoolean((await result as Activity).Text);

            if (foundResultInQnA)
            {
                await context.PostAsync("Hope I helped you!");
                context.EndConversation("EndedInQnA");
            }
            else
            {
                LUISDialog dialog = new LUISDialog();
                await context.Forward(dialog, AfterLUISDialog, message);
            }
        }

        private async Task AfterLUISDialog(IDialogContext context, IAwaitable<object> result)
        {
            var LUISIntentResult = await result;
            context.EndConversation("EndedInLUIS");
        }
    }
}