# How to use QnA Maker and LUIS in the same bot #
This is a sample on how to integrate LUIS and QnA Maker services from Microsoft Azure with the Azure Bot Service

><b>Disclosure</b>. This repo asumes that you have already worked with Bots, QnA and/or LUIS before. If you haven't then these tutorials will sure be useful as we won't cover the basic parts of such services. Please check them first and come back here to learn how to use both services in the same bot: [Azure Bot Service documentation](https://azure.microsoft.com/en-us/services/bot-service/), [Create your first LUIS App](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-get-started-create-app) and [Create your first QnA Maker service](https://qnamaker.ai/Documentation/CreateKb)


## Quick summary ##
We're constantly faced with scenarios where we need both LUIS and QnA working together and while it should be easy I haven't seen many examples out there. Here's a quick way to do it calling a QnA service first and if the service doesn't have any response then it calls LUIS to get intents and answer the user

## Architecture ##
//Insert architecture here

## The staged scenario ##
For this example, the chat bot will use LUIS to help a user to:
- "turn on" and "turn off" the lights from different rooms of a house and also give random greetings everytime the user says hi to the bot.

At the same time, the bot will use QnA Maker to answer basic questions like:
- "Are you a bot?"
- "What rooms are available to turn on or off?"
- "Can you add a new room to the service?"

### Creating the QnA Maker service ###
>NOTICE. For this example we won't be creating <b><i>the ultimate</i></b> QnA Maker service altough nothing stops you to get creative. 

For simplicity purposes I created a QnA Maker service wit a handful of questions, just to see that my bot is connecting to the service and retrieving some answers. Here's a screenshot of what I did:

![Image 1. Notice that my QnA Maker only has 6 pairs of questions and answers](images/QnAMaker.png)

That's it. Just quick steps:
- Created a new QnA Service
- Created 6 pairs of questions and answers
- Clicked on "Save and retrain" the service
- Published the service
- Saved the <i>QnA Maker SubscriptionKey</i> and <i>QnA Maker Knowdledge Base ID</i> to my bot webconfig file. If you don't remember how / where to save these values just go again to the [QnA Maker website](https://qnamaker.ai/Home/MyServices) and you'll see a list of all the QnAs you have created. You'll see a list of Services like this one:

![Image 2. My recently published QnA Maker service](images/QnAMakerServices.png)  

Just click on the <i>pencil icon</i> and once you are on our service click on <i>"Settings"</i> at the left menu. Once there you'll see a <i>"Deployment details"</i> section almost at the bottom of the page and there you'll see your service ID and subscription key:

![Image 3. We have located our QnA Maker subscription key and knowledge base ID](images/QnAMakerKeys.png)

Now that you have you credentials go to your Bot code and locate the file Web.Config (usually it's the last file at the project). Open it and locate the <i><appSettings></i> section. Within create two new keys for your QnA credentials. You should have something like this:

```xml
<appSettings>
    <add key="BotId" value="YourBotId" />
    <add key="MicrosoftAppId" value="" />
    <add key="MicrosoftAppPassword" value="" />
    <add key="QnAMakerSubscriptionKey" value="YOUR_QNA_SUBSCRIPTION_KEY_GOES_HERE" />
    <add key="QnAMakerKnowledgeBaseId" value="YOUR_QNA_KNOWLEDGE_BASE_ID_GOES_HERE" />
  </appSettings>
```
You're set! At least from a service and credentials perspective. Don't worry about the code as we'll touch it later.
### Creating the LUIS App ###
Same as with QnA. The LUIS app we'll create is not about its complexity regarding # of intents or entities. We'll stick to basics defining:
- 3 intents: "Greeting", "TurnOn" and "TurnOff
- 2 entities: "Time" and "Date"

>TIP. You don't have to follow this repo through and through. Instead of playing with turning the lights on and off you could bring your own intents and entities

#### Analizing the intents ####
##### Greeting #####
This intent has nothing but samples of different ways to greet a person. If the user writes things like "hi", "good morning", "hello there" and similars then we'll take them as all as greetings

###### TurnOn #####
 Takes utterances that involve keywords that are invoke the same action as turning the lights:
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
Did you notice we had blank spaces in the previous statements?
- "Turn off the <i>[room]</i> at <i>[time]</i>
- "Please start the lights from the <i>[room]</i> at <i> [time]</i>

Well, those are our two entities:
- Room: Will help us identify the rooms named in the TurnOn and TurnOff intents
- Time: Will help us identify the hour/date that our user wants the bot to turn on/off the light

Again, it's not the most complex LUIS scenario but this was on purpose to just grab a functional QnA Service and functional LUIS App and mix them

#### Training the LUIS App ####
It shouldn't take us more than 30 minutes to train our LUIS App with more than a handful of utterances. These are some the utterances I used to train my app:

##### Greeting intent #####
```console
good morning
top of the morning to ya
good afternon
hello there
good afternoon
```
##### Greeting TurnOn #####
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
##### Greeting TurnOff #####
```console
turn off the [Room] lights
power off the [Room]
can you please stop the lights from the [Room]
kill the [Room] lights
please turn off the lights from the [Room] at [Time]
stop the [Room] lights at [Time]
kill the [Room] lights at [Time] 
```

## Findings ##

## Next steps ##

## References ##