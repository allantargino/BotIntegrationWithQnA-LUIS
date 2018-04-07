using System;
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
            LUISDialog dialog = new LUISDialog();
            await context.Forward(dialog, AfterLUISDialog, activity);
        }

        private async Task AfterLUISDialog(IDialogContext context, IAwaitable<object> result)
        {
            // Something to do after LUIS exits
            //var activity = await result as Activity;
            //await context.PostAsync("Exited LUIS dialog");
            context.Done(this);
        }
    }
}