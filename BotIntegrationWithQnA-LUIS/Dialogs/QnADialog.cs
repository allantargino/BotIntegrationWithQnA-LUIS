using BotIntegrationWithQnA_LUIS.Utilities;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace BotIntegrationWithQnA_LUIS.Dialogs
{
    [Serializable]
    public class QnADialog : QnAMakerDialog
    {
        public QnADialog() : base(
            new QnAMakerService (
                new QnAMakerAttribute(
                    ConfigurationManager.AppSettings["QnAMakerSubscriptionKey"],
                    ConfigurationManager.AppSettings["QnAMakerKnowledgeBaseID"],
                    "No good match found in the KB",
                    0)))
        {}

        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            var answer = result.Answers.First().Answer;

            if (answer == "No good match found in the KB")
                BotUtilities.foundResultInQnA = false;

            else
            {
                BotUtilities.foundResultInQnA = true;
                await context.PostAsync(answer);
            }
        }

        protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            context.Done<IMessageActivity>(null);
        }
    }
}