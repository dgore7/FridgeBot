//using System;
//using System.Net;
//using System.Web.Http;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using fridgeBot.DeserializationJson;

//using Microsoft.Bot.Connector;
//using Microsoft.Bot.Builder.Luis;
//using Microsoft.Bot.Builder.Dialogs;
//using Microsoft.Bot.Builder.Luis.Models;

//namespace fridgeBot
//{
//    [LuisModel("5ad69a7d-f722-4319-bc73-dbb89d65135f", "8226286cafeb4514963d9228ea3cc6bb")]
//    [Serializable]
//    public class FridgeDialog : LuisDialog<object>
//    {
//        [LuisIntent("")]
//        public async Task None(IDialogContext context, LuisResult result)
//        {
//            string message = $"I'm sorry. I didn't understand that. Try: \"What can we cook for dinner?\" or  \"What ingredients are we missing?\"";
//            await context.PostAsync(message);
//            context.Wait(MessageReceived);
//        }

//        [LuisIntent("Greeting")]
//        public async Task Greeting(IDialogContext context, LuisResult result)
//        {
//            string message = $"Hello! Feeling hungry and adventurous? Try: \"What can we cook for dinner?\" or  \"I'm hungry!\"";
//            await context.PostAsync(message);
//            context.Wait(MessageReceived);
//        }

//        [LuisIntent("FindRecipe")]
//        public async Task FindRecipe(IDialogContext context, LuisResult result)
//        {
//            string message = $"FindIngredients";
//            await context.PostAsync(message);
//            context.Wait(MessageReceived);
//        }

//        [LuisIntent("GetIngredients")]
//        public async Task GetIngredients(IDialogContext context, LuisResult result)
//        {
//            string message = $"GetIngredients";
//            await context.PostAsync(message);
//            context.Wait(MessageReceived);
//        }
//    }

//    [BotAuthentication]
//    public class MessagesController : ApiController
//    {
//        /// <summary>
//        /// POST: api/Messages
//        /// Receive a message from a user and reply to it
//        /// </summary>
//        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
//        {
//            // check if activity is of type message
//            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
//            {
//                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
//                activity.Text.Replace(",", "%2C");  // replace all commas with '%2C'
//                activity.Text.Replace(" ", "");     // remove all spaces if there any
//                var argIngredients = activity.Text;

//                await Conversation.SendAsync(activity, () => new FridgeDialog());
//            }
//            else
//            {
//                HandleSystemMessage(activity);
//            }
//            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);


//            if (activity.Type == ActivityTypes.Message)
//            {
//                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

//                // take in ingredients list and format correctly to pass into API url
//                activity.Text.Replace(",", "%2C");  // replace all commas with '%2C'
//                activity.Text.Replace(" ", "");     // remove all spaces if there any
//                var argIngredients = activity.Text;

//                var Caller = new CallAPI();

//                if (!Caller.isLimitHit())
//                {
//                    // Initialize recipeResult to hold json info
//                    // Call first API to obtain recipe from ingredients
//                    var recipeResult = new JsonRecipe();
//                    recipeResult = Caller.GetRecipe(argIngredients);


//                    // Initialize recipeResult to hold json info
//                    // Call second API to obtain recipe link from recipeId
//                    var recipeLink = new JsonLink();
//                    recipeLink = Caller.GetLink(recipeResult.id.ToString());


//                    // reply back to user with the recipe options
//                    Activity replyMessage = activity;
//                    Activity replyToConversation = replyMessage.CreateReply("May I interest you in..");
//                    replyToConversation.Recipient = replyMessage.From;
//                    replyToConversation.Type = "message";
//                    replyToConversation.Attachments = new List<Attachment>();
//                    List<CardImage> cardImages = new List<CardImage>();
//                    cardImages.Add(new CardImage(url: recipeResult.image));
//                    List<CardAction> cardButtons = new List<CardAction>();
//                    CardAction plButton = new CardAction()
//                    {
//                        Value = recipeLink.sourceUrl,
//                        Type = "openUrl",
//                        Title = "Let's Get Cooking!"
//                    };
//                    cardButtons.Add(plButton);
//                    HeroCard plCard = new HeroCard()
//                    {
//                        Title = recipeResult.title,
//                        Subtitle = "Recommended by " + recipeResult.likes.ToString() + " others!",
//                        Images = cardImages,
//                        Buttons = cardButtons
//                    };

//                    Attachment plAttachment = plCard.ToAttachment();
//                    replyToConversation.Attachments.Add(plAttachment);
//                    var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);
//                }
//                else
//                {
//                    //run out of calls.. try again tomorrow... order out?..pizza #
//                    Activity replyMessage = activity;
//                    Activity replyToConversation = replyMessage.CreateReply("Sorry! Kitchen is closed.. Time to order out? :)");
//                    var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);
//                }               
//            }
//            else
//            {
//                HandleSystemMessage(activity);
//            }
//            var response = Request.CreateResponse(HttpStatusCode.OK);
//            return response;
//        }

//        private Activity HandleSystemMessage(Activity message)
//        {
//            if (message.Type == ActivityTypes.DeleteUserData)
//            {
//                // Implement user deletion here
//                // If we handle user deletion, return a real message
//            }
//            else if (message.Type == ActivityTypes.ConversationUpdate)
//            {
//                // Handle conversation state changes, like members being added and removed
//                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
//                // Not available in all channels
//            }
//            else if (message.Type == ActivityTypes.ContactRelationUpdate)
//            {
//                // Handle add/remove from contact lists
//                // Activity.From + Activity.Action represent what happened
//            }
//            else if (message.Type == ActivityTypes.Typing)
//            {
//                // Handle knowing tha the user is typing
//            }
//            else if (message.Type == ActivityTypes.Ping)
//            {
//            }

//            return null;
//        }
//    }
//}