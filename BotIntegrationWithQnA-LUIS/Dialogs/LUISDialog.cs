using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace BotIntegrationWithQnA_LUIS.Dialogs
{
    [Serializable]
    public class LUISDialog : LuisDialog<object>
    {
        public LUISDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisApplicationId"],
            ConfigurationManager.AppSettings["LuisSubscriptionKey"])))
        {

        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry! We couldn't understand you.");
            context.Done("None");
        }

        [LuisIntent("TurnOn")]
        public async Task TurnOnIntent(IDialogContext context, LuisResult result)
        {
            EntityRecommendation timeEntity;
            result.TryFindEntity("Time", out timeEntity);

            EntityRecommendation roomEntity;
            result.TryFindEntity("Room", out roomEntity);

            if (timeEntity != null)
                await context.PostAsync($"Ok, we'll turn on the {roomEntity.Entity} lights. at {timeEntity.Entity}");
            else await context.PostAsync($"Ok, we'll turn on the lights from the {roomEntity.Entity}");

            context.Done("TurnOn");
        }

        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            List<string> randomGreetings = new List<string> (){ "Hello", "Hi there", "Well hello, kind people", "I'm here for you! How can I help?"};
            Random ran = new Random();
            int position = ran.Next(0, randomGreetings.Count-1);

            await context.PostAsync(randomGreetings[position]);
            
            context.Done("TurnOn");
        }

        [LuisIntent("TurnOff")]
        public async Task TurnOffIntent(IDialogContext context, LuisResult result)
        {
            EntityRecommendation timeEntity;
            result.TryFindEntity("Time", out timeEntity);

            EntityRecommendation roomEntity;
            result.TryFindEntity("Room", out roomEntity);

            if (timeEntity != null)
                await context.PostAsync($"Ok, we'll turn off the {roomEntity.Entity} lights. at {timeEntity.Entity}");
            else await context.PostAsync($"Ok, we'll turn off the lights from the {roomEntity.Entity}");

            context.Done("TurnOff");
        }
    }
}