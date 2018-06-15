﻿using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
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
        public QnADialog() : base(
            new QnAMakerService (
                new QnAMakerAttribute(
                    ConfigurationManager.AppSettings["QnAMakerSubscriptionKey"],
                    ConfigurationManager.AppSettings["QnAMakerKnowledgeBaseID"],
                    "No good match found in the KB",
                    0.3,
                    1,
                    ConfigurationManager.AppSettings["QnAEndpointHostName"])))
        {}

        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            var answer = result.Answers.First().Answer;

            if (answer == "No good match found in the KB")
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