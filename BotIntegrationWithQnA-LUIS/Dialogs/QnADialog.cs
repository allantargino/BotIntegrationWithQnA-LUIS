using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Linq;
using System.Threading.Tasks;
using conf = System.Configuration.ConfigurationManager;

namespace BotIntegrationWithQnA_LUIS.Dialogs
{
    [Serializable]
    public class QnADialog : QnAMakerDialog
    {
        public static bool FoundResultInQnA;
        public static string NoMatchMessage;
        public static double ConfidenceTreshold;
        public QnADialog() : base(
            new QnAMakerService (
                new QnAMakerAttribute(
                    conf.AppSettings["QnAMakerAuthKey"],
                    conf.AppSettings["QnAMakerKnowledgeBaseID"],
                    null, 
                    0, 
                    1,
                    conf.AppSettings["QnAEndpointHostName"])))
        {
            string confidence = conf.AppSettings["QnAConfidenceTreshold"];
            ConfidenceTreshold = double.Parse(confidence);
        }

        protected override bool IsConfidentAnswer(QnAMakerResults results)
        {
            FoundResultInQnA = base.IsConfidentAnswer(results);
            return true;
        }

        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            var qnaResult = result.Answers.First();
            var answer = qnaResult.Answer;

            if (qnaResult.Score < ConfidenceTreshold)
                FoundResultInQnA = false;

            else
            {
                FoundResultInQnA = true;
                await context.PostAsync(answer);
            }
        }

        protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            var newMessage = context.MakeMessage();
            newMessage.Text = FoundResultInQnA.ToString();
            context.Done<IMessageActivity>(newMessage);
        }
    }
}