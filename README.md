# How to use QnA Maker and LUIS in the same bot #
This is a sample on how to integrate LUIS and QnA Maker services from Microsoft Azure with the Azure Bot Service

><b>Disclosure</b>. This repo asumes that you have already worked with Bots, QnA and/or LUIS before. If you haven't then these tutorials will sure be useful as the basic parts of such services are not covered here. Please check them first and come back here to learn how to use both services in the same bot: [Azure Bot Service documentation](https://azure.microsoft.com/en-us/services/bot-service/), [Create your first LUIS App](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-get-started-create-app) and [Create your first QnA Maker service](https://qnamaker.ai/Documentation/CreateKb)


## Quick summary ##
We're constantly faced with scenarios where we need both LUIS and QnA working together and while it should be easy there are not many examples out there. Here's a quick way to do it calling a QnA service first and if the service doesn't have any answer then it calls LUIS to get intents and answer the user.

## The staged scenario ##
For this example, the chat bot will use LUIS to help a user to:
- "turn on" and "turn off" the lights from different rooms of a house and also give random greetings everytime the user says hi to the bot.

At the same time, the bot will use QnA Maker to answer basic questions like:
- "Are you a bot?"
- "What rooms are available to turn on or off?"
- "Can you add a new room to the service?"

### Creating the QnA Maker service ###
>NOTICE. This example does not use <b><i>the ultimate</i></b> QnA Maker service altough nothing stops you to get creative. 

For simplicity purposes the QnA Maker service used has only a handful of questions, just to see that the bot is connecting to the service and retrieving some answers. Here's a screenshot the questions and answers:

![Image 1. Notice that this QnA Maker service only has 6 pairs of questions and answers](images/QnAMaker.png)

That's it. Just quick steps:
- Created a new QnA Service
- Created 6 pairs of questions and answers
- Clicked on "Save and retrain" the service
- Published the service
- Saved the <i>QnA Maker SubscriptionKey</i> and <i>QnA Maker Knowdledge Base ID</i> to my bot webconfig file. If you don't remember how / where to save these values just go again to the [QnA Maker website](https://qnamaker.ai/Home/MyServices) and you'll see a list of all the QnAs you have created. You'll see a list of Services like this one:

![Image 2. A published QnA Maker service](images/QnAMakerServices.png)  

Just click on the <i>pencil icon</i> and once you are on our service click on <i>"Settings"</i> at the left menu. Once there you'll see a <i>"Deployment details"</i> section almost at the bottom of the page and there you'll see your service ID and subscription key:

![Image 3. Where to locate the QnA Maker subscription key and knowledge base ID](images/QnAMakerKeys.png)

Now that the credentials have been identified it is time to go the Bot code and locate the file Web.Config (usually it's the last file at the project). Once there the credentials should go on the <i><appSettings></i> section. The result should go like this:

```xml
<appSettings>
    <add key="BotId" value="YourBotId" />
    <add key="MicrosoftAppId" value="" />
    <add key="MicrosoftAppPassword" value="" />
    <add key="QnAMakerSubscriptionKey" value="YOUR_QNA_SUBSCRIPTION_KEY_GOES_HERE" />
    <add key="QnAMakerKnowledgeBaseId" value="YOUR_QNA_KNOWLEDGE_BASE_ID_GOES_HERE" />
  </appSettings>
```
You're set! At least from a service and credentials perspective.

### Creating the LUIS App ###
Same as with QnA. The LUIS app used for this example is not about its complexity regarding the number of intents or entities. Only this entities and intents are defined:
- 3 intents: "Greeting", "TurnOn" and "TurnOff
- 2 entities: "Time" and "Date"

>TIP. You don't have to follow this repo through and through. Instead of playing with turning the lights on and off you could bring your own intents and entities

#### Analizing the intents ####
##### Greeting #####
This intent has nothing but samples of different ways to greet a person. If the user writes things like "hi", "good morning", "hello there" and similars then we'll take them as all as greetings

###### TurnOn #####
 Takes utterances that involve keywords that invoke the same action as turning the lights:
 - "Turn the lights from the [room]"
 - Power up the [room] lights
 - Can you start the lights at the [room]?
 - Please, help me starting the [room] lights
 - Start the [room] lights at [time]
 - etc.
###### TurnOff #####
Opposite of the TurnOn intent. Is composed of utterances that follow these similar patterns:
- Turn off the [room] lights
- Stop the [room] lights
- Please turn off the lights at the [room]
- Power off the [room] at [time]
- Can you please shut the [room] lights at [time]?

#### Analizing the entities ####
Notice the blank spaces in the previous statements?
- "Turn off the <i>[room]</i> at <i>[time]</i>
- "Please start the lights from the <i>[room]</i> at <i> [time]</i>

Well, those are two entities:
- Room: Will help the bot to identify the rooms named in the TurnOn and TurnOff intents
- Time: Will help the bot to identify the hour/date that our user wants the bot to turn on/off the light

Again, it's not the most complex LUIS scenario but this was on purpose to just grab a functional QnA Service and functional LUIS App and mix them.

#### Training the LUIS App ####
It shouldn't take more than 30 minutes to train the LUIS App with more than a handful of utterances. These are some the utterances used to train the app:

##### Greeting intent #####
```console
good morning
top of the morning to ya
good afternon
hello there
good afternoon
```
##### TurnOn intent #####
``` console
please turn on the [Room] lights
turn the [Room] lights on
start the [Room] lights
can you turn on the [Room] lights ?
please start up the [Room]
get the Room lights [going]
cna you turn un the lights from the [Room]?
please turn on the [Room] lights
turn on the [Room] lights at [Time]
please start the [Room] lights at [Time] 
```
##### TurnOff intent #####
```console
turn off the [Room] lights
power off the [Room]
can you please stop the lights from the [Room]
kill the [Room] lights
please turn off the lights from the [Room] at [Time]
stop the [Room] lights at [Time]
kill the [Room] lights at [Time] 
```
#### Setting the LUIS credentials into our bot ####
Don't forget to add your LUIS credentials to the webconfig file. You'll find your App ID and Subscription key on your [LUIS app's site](https://www.luis.ai/applications) at the <i>Publish</i> tab on the upper right side of the page. Once there, you'll see a <i>"Resources and Keys"</i> section and you'll find your credentials in a query string:

![Image 4. Your LUIS credentials are in a query string](images/LUISKeys.png)

Again, the <b>webconfig</b> file needs to be updated it with the LUIS App credentials:

```xml
<appSettings>
    <add key="BotId" value="YourBotId" />
    <add key="MicrosoftAppId" value="" />
    <add key="MicrosoftAppPassword" value="" />
    <add key="LuisSubscriptionKey" value="YOUR_LUIS_SUBSCRIPTION_KEY_GOES_HERE" />
    <add key="LuisApplicationId" value="YOUR_LUIS_APPLICATION_ID_GOES_HERE" />
    <add key="QnAMakerSubscriptionKey" value="YOUR_QNA_SUBSCRIPTION_KEY_GOES_HERE" />
    <add key="QnAMakerKnowledgeBaseId" value="YOUR_QNA_KNOWLEDGE_BASE_ID_GOES_HERE" />
  </appSettings>
```
## Coding time ##
### Setting up the bot: MessagesController class ###
This is a personal take on the MessageController class to keep it clean. This demo only considers two types of activities: <b>Message</b> and <b>ConversationUpdate</b>. The MessagesController code goes like this:
```csharp
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
```
Simply put: Everytime a user joins to chat with the bot it will see a welcome message. And after that everytime the user writes something that message will re route to our RootDialog class.

### Utilities folder ###
The Utilities folder includes things that are useful through the bot journey such as a DisplayWelcomeMessage method that could also serve to show a menu everytime a user types in keywords like "Start", "Menu", "Main Menu" and others. Those keywords could be catched at the Post method in MessagesController.cs and then reroute the user to a main menu. But that is work for other day.
```csharp
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotIntegrationWithQnA_LUIS.Utilities
{
    public class BotUtilities
    {
        public static async Task DisplayWelcomeMessage(Activity activity, string message)
        {
            Activity replyMessage = activity.CreateReply("");
            ConnectorClient client = new ConnectorClient(new Uri(activity.ServiceUrl));

            HeroCard card = new HeroCard();
            card.Title = message;

            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "https://c.s-microsoft.com/en-us/CMSImages/ImgTwo.jpg?version=2432BB03-C90E-EF03-A2BB-BFA093E1A899"));
            card.Images = cardImages;

            replyMessage.Attachments.Add(card.ToAttachment());
            await client.Conversations.ReplyToActivityAsync(replyMessage);
        }
    }
}
```
The DisplayWelcomeMessage method just displays a HeroCard with an image and a message for the user. There's a <b>message</b> parameter that contains the text that will be displayed to the user and a default image displayed (the logo from Microsoft)

### QnA Dialog ###
Bot Framework provides an easy way to use QnA services through the [<b>QnAMakerDialog class</b>](https://github.com/Microsoft/BotBuilder-CognitiveServices/blob/master/CSharp/Library/QnAMaker/QnAMaker/QnAMakerDialog.cs). This class has a constructor that primarily receives the credentials from an existing QnAService and then proceeds to handle the conversation trhough a series of methods in the next order:
- StartAsync: Starts the dialog
- MessageReceivedAsync: 
    - Receives a message from the user and sends it to QnA using the private method <b>QueryServiceAsync</b>
    - If it receives an answer from the QnAService it checks if it is a valid answer with the method <b>IsConfidentAnswer</b>
    - If it is a confident answer, posts the answer to the user via the method <b>RespondFromQnAMakerResultAsync</b>
    - After, it finalizes with the interaction with method <b>DefaultWaitNextMessageAsync</b>
    - If it is not a confident answer it calls the method <b>QnAFeedbackStepAsync</b>
    - Then returns to conversate with the user through the method <b>ResumeAndPostAnswer</b>

The QnAMakerDialog does all this in behalf of us devs so in most cases initializing a new QnADialog should be more than enough to integrate QnA with our bot:
```csharp
[Serializable]
public class QnADialog : QnAMakerDialog
{
    public QnADialog() : base(
        new QnAMakerService (
            new QnAMakerAttribute(
                ConfigurationManager.AppSettings["QnAMakerSubscriptionKey"],
                ConfigurationManager.AppSettings["QnAMakerKnowledgeBaseID"])))
    {}
}
```
But this integration scenario with LUIS is not a common scenario. So an  override of some of the methods we mentioned earlier is needed in order for the bot to play nice between the two services:
```csharp
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
                0)))
    {}
}
```

Two things:
- First a boolean called <b>foundResultInQnA</b> is added that helps to  know if QnA got an answer or not 
- The "confidence" treshold is overrided to 0 in the dialog constructor and provide a default "no answer" reply for the user even though this reply will not be used but it needs to be defined to override the confidence treshold.

This is neeeded because the <b>IsConfidentAnswer</b> method returns a FALSE value when it detects that the best answer acquired has a confidence score below 0.99 OR if the difference between the confidences from the best answer and second best answer is greater than 0.20.

So what? If the <b>IsConfidentAnswer</b> method returns a false value then after a series of steps it will answer the user with a "No match found" message and what is neeeded is the bot to navigate to LUIS when this happens.

So, now that the confidence threshold goes to 0 then the QnAMakerDialog will always go to the <b>RespondFromQnAMakerResultAsync</b> method and now it is time to override its code:
```csharp
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
```
The original code from this method is more complex but for this example it is ok to simply ask if the QnA Service has an anwer or not. By default, when a QnA Service doesn't find an answer on its knowledge base it returns a "No good match found in the KB" message and since confidence threshold is dropped to 0 that means the bot will always get this answer if my QnA is not completely confident. 
If the bot has an answer then the boolean <b>foundResultInQnA</b> gets a TRUE value and the QnAMakerDialog will spost the answer straight to the user. If the QnA Service did not have an answer on his knowledge base then the boolean <b>foundResultInQnA</b> will get a false value and no answer to the user will be written (for now).
>NOTE. For this scenario, ditching the confidence threshold to 0 is fine but you might want to play with some different values and take different approaches depending on the bot you are writing.

```csharp
protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
{
    IMessageActivity newMessage = context.MakeMessage();
    newMessage.Text = foundResultInQnA.ToString();
    context.Done<IMessageActivity>(newMessage);
}
```
The <b>DefaultWaitNextMessageAsync</b> method is also overrided to return the value from the boolean <b>foundResultInQnA</b>. This means that the bot main dialog will receive this result and will get to decide if the user has an answer to the question posted or if it should rise a LUIS Dialog.

### LUIS Dialog ###
A new LUISDialog class that inherits its behavior from the original <b>LuisDialog</b> class is created:
```csharp
[Serializable]
public class LUISDialog : LuisDialog<object>
{
    public LUISDialog() : base(new LuisService(new LuisModelAttribute(
        ConfigurationManager.AppSettings["LuisApplicationId"],
        ConfigurationManager.AppSettings["LuisSubscriptionKey"])))
    {}
}
```
Next, it is time to define a method for each intent defined before. Remember there are 4 intents:
- A default 'None' intent when LUIS does not identify a entity we defined.
- Greeting intent
- TurnOn intent
- TurnOff intent

#### None intent ####
This method just sends a message to the user stating that the bot could not understand what the user said and internally returns a "None" message to the conversation flow.

```csharp
[LuisIntent("None")]
public async Task NoneIntent(IDialogContext context, LuisResult result)
{
    await context.PostAsync("Sorry! We couldn't understand you.");
    context.Done("None");
}
```

#### Greeting intent ####
In case the user says hi to the bot, the latest will reply with a random greeting and later return an internal "Greeting" message to the conversation flow:
```csharp
[LuisIntent("Greeting")]
public async Task GreetingIntent(IDialogContext context, LuisResult result)
{
    List<string> randomGreetings = new List<string>() { "Hello", "Hi there", "Well hello, kind people", "I'm here for you! How can I help?" };
    Random ran = new Random();
    int position = ran.Next(0, randomGreetings.Count - 1);

    await context.PostAsync(randomGreetings[position]);

    context.Done("Greeting");
}
```
#### TurnOn intent ####
When a user wants to turn on a light from a room the bot will simply reply saying that it will turn on the lights. In a real IoT scenario this would be the part when there is a call to a service such as EventHubs, IoTHub, or others to take care of the instructions and send an action to an IoT device. After it sends the confirmation message to the user the bot returns a "TurnOn" text to the conversation flow:

```csharp
[LuisIntent("TurnOn")]
public async Task TurnOnIntent(IDialogContext context, LuisResult result)
{
    EntityRecommendation timeEntity;
    result.TryFindEntity("Time", out timeEntity);

    EntityRecommendation roomEntity;
    result.TryFindEntity("Room", out roomEntity);

    if (timeEntity != null)
        await context.PostAsync($"Ok, we'll turn on the {roomEntity.Entity} lights. at {timeEntity.Entity}");
    else
        await context.PostAsync($"Ok, we'll turn on the lights from the {roomEntity.Entity}");
        
    context.Done("TurnOn");
}
```
#### TurnOff intent ####
Almost identical to the TurnOn scenario, the only difference is the reply message from the bot to the user and that it internally replies to the conversation flow with a "TurnOff" message:
```csharp
[LuisIntent("TurnOff")]
public async Task TurnOffIntent(IDialogContext context, LuisResult result)
{
    EntityRecommendation timeEntity;
    result.TryFindEntity("Time", out timeEntity);

    EntityRecommendation roomEntity;
    result.TryFindEntity("Room", out roomEntity);

    if (timeEntity != null)
        await context.PostAsync($"Ok, we'll turn off the {roomEntity.Entity} lights. at {timeEntity.Entity}");
    else
        await context.PostAsync($"Ok, we'll turn off the lights from the {roomEntity.Entity}");

    context.Done("TurnOff");
}
```
### Root Dialog ###
Now that both LUIS and QnA dialogs are set up it is just a matter of calling them from the root dialog. Let's look at the code.

First of all, the StartAsync method doesn't change at all there are only 3 remaining methods in the code to focus on:
- MessageReceivedAsync
- AfterQnADialog
- AfterLUISDialog

#### MessageReceivedAsync method ####
Once the user's message arrives here in the <b>result</b> parameter, this method grabs the text value of it and sends it to the QnA Dialog which handles the conversation flow and returns a result in the AfterQnADialog method: 
```csharp
private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
{
    var activity = await result as Activity;
    QnADialog dialog = new QnADialog();
    await context.Forward(dialog, AfterQnADialog, activity, CancellationToken.None);
}
```

#### AfterQnADialog method ####
This is the part when the code becomes interesting. Lets recall that a message arrives in the <b>result</b> parameter of a conversation method.
That means that this line:
```csharp
bool foundResultInQnA = Convert.ToBoolean((await result as Activity).Text);
```
will return a boolean that defines if QnA Dialog found an answer or not.
>NOTE. While initially a boolean defines the finding of an answer in the QnADialog it is not possible to return a boolean as a message. So an IMessageActivity object needs to be created with the boolean turned to a string and said string becomes a boolean again in the AfterQnADialog

Now the bot knows if a call to the LUIS dialog is needed, but it needs the initial message that the user wrote. Said message is in the <b>context</b> parameter of the method:
```csharp
var message = context.Activity as Activity;
```
The complete code of the AfterQnADialog looks like this:
```csharp
private async Task AfterQnADialog(IDialogContext context, IAwaitable<object> result)
{
    var message = context.Activity as Activity;
    bool foundResultInQnA = Convert.ToBoolean((await result as Activity).Text);

    if (foundResultInQnA)
    {
        await context.PostAsync("Hope I helped you!");
        context.EndConversation("EndedInQnA");
    }
    else
    {
        LUISDialog dialog = new LUISDialog();
        await context.Forward(dialog, AfterLUISDialog, message);
    }
}
```
#### AfterLUISDialog method ####
Not a lot of magic, the AfterLUISDialog just ends the conversation. In practice, code here should handle what to do if LUIS did not find an answer.
```csharp
private async Task AfterLUISDialog(IDialogContext context, IAwaitable<object> result)
{
    var LUISIntentResult = await result;
    context.EndConversation("EndedInLUIS");
}
```

## References ##
1. [<b>UPDATED QnAMakerDialog documentation</b>](https://github.com/Microsoft/BotBuilder-CognitiveServices/blob/master/CSharp/Samples/QnAMaker/README.md) Really useful to understand what happens within the QnAMakerDialog class methods we often overlook in favor of a simple QnA implementation for our bot.
