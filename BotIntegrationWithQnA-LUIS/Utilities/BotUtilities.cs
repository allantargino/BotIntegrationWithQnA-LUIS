using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotIntegrationWithQnA_LUIS.Utilities
{
    public class BotUtilities
    {
        public static bool foundResultInQnA;
        public static async Task DisplayWelcomeMessage(Activity activity, string message)
        {
            Activity replyMessage = activity.CreateReply(message);
            ConnectorClient client = new ConnectorClient(new Uri(activity.ServiceUrl));

            HeroCard card = new HeroCard();
            card.Title = message;

            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "https://c.s-microsoft.com/en-us/CMSImages/ImgTwo.jpg?version=2432BB03-C90E-EF03-A2BB-BFA093E1A899"));
            card.Images = cardImages;

            await client.Conversations.ReplyToActivityAsync(replyMessage);
        }
    }
}