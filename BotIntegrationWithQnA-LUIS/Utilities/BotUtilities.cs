using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotIntegrationWithQnA_LUIS.Utilities
{
    public class BotUtilities
    {
        public static async Task DisplayMessage(Activity activity, string message)
        {
            var replyMessage = activity.CreateReply("");
            var client = new ConnectorClient(new Uri(activity.ServiceUrl));

            var card = new HeroCard();
            card.Title = message;

            string imgUrl = "https://c.s-microsoft.com/en-us/CMSImages/ImgTwo.jpg?version=2432BB03-C90E-EF03-A2BB-BFA093E1A899";
            var cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: imgUrl));
            card.Images = cardImages;

            replyMessage.Attachments.Add(card.ToAttachment());
            await client.Conversations.ReplyToActivityAsync(replyMessage);
        }
    }
}