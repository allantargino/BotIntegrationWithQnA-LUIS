using System;
using System.Threading.Tasks;
using BotIntegrationWithQnA_LUIS.Utilities;
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
            QnADialog dialog = new QnADialog();
            await context.Forward(dialog, AfterQnADialog, activity);
        }

        private async Task AfterQnADialog(IDialogContext context, IAwaitable<object> result)
        {
            var activity = context.Activity as Activity;
            if (BotUtilities.foundResultInQnA)
            {
                await context.PostAsync("Hope I helped you! \n\nExited QnA");
                context.Done(this);
            }
            else
            {
                LUISDialog dialog = new LUISDialog();
                await context.Forward(dialog, AfterLUISDialog, activity);
            }
        }

        private async Task AfterLUISDialog(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            await context.PostAsync("Exited LUIS dialog");
            context.Done(this);
        }
    }
}