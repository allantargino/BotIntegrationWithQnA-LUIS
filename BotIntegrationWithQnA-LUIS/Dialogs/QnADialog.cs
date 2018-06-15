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
        public static bool foundResultInQnA;
        public static string NoMatchMessage = "No good match found in KB.";
        public QnADialog() : base(
            new QnAMakerService (
                new QnAMakerAttribute(
                    ConfigurationManager.AppSettings["QnAMakerAuthKey"],
                    ConfigurationManager.AppSettings["QnAMakerKnowledgeBaseID"],
                    NoMatchMessage,
                    0,
                    2,
                    ConfigurationManager.AppSettings["QnAEndpointHostName"])))
        {}

        protected override bool IsConfidentAnswer(QnAMakerResults qnaMakerResults)
        {
            foundResultInQnA = true;
            return true;
        }

        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            var answer = result.Answers.First().Answer;

            if (answer == NoMatchMessage)
                foundResultInQnA = false;

            else
            {
                foundResultInQnA = true;
                await context.PostAsync(answer);
            }
        }

        protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            IMessageActivity newMessage = context.MakeMessage();
            newMessage.Text = foundResultInQnA.ToString();
            context.Done<IMessageActivity>(newMessage);
        }
    }
}