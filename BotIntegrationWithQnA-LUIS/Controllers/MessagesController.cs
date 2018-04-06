using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BotIntegrationWithQnA_LUIS.Utilities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;


namespace BotIntegrationWithQnA_LUIS
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            switch (activity.Type)
            {
                case ActivityTypes.Message:
                    await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
                    break;
                case ActivityTypes.ConversationUpdate:
                    if (activity.MembersAdded.Any(o => o.Id == activity.Recipient.Id))
                        await BotUtilities.DisplayWelcomeMessage(activity, "Welcome! I'll be your bot guide");
                    break;
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}