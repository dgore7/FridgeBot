//using System;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Http.Description;
//using Microsoft.Bot.Connector;
//using Newtonsoft.Json;

//namespace PhotonBot
//{
//    [BotAuthentication]
//    public class MessagesController : ApiController
//    {
//        /// <summary>
//        /// POST: api/Messages
//        /// Receive a message from a user and reply to it
//        /// </summary>
//        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
//        {
//            if (activity.Type == ActivityTypes.Message)
//            {
//                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
//                // calculate something for us to return
//                int length = (activity.Text ?? string.Empty).Length;

//                //// return our reply to the user
//                //Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
//                //await connector.Conversations.ReplyToActivityAsync(reply);

//                // Initialize all key items relevant for the API and returning to the user

//                string appliance = "";
//                string foodType = "food";
//                string cookTime = "0:00Hrs";
//                string cookTemp = "000F";

//                // search the string for key words
//                // mechanism for cooking: oven, bake, grill, stove, pan

//                if ((activity.Text.Contains("oven")) || (activity.Text.Contains("Oven")))
//                {
//                    appliance = "oven";

//                    // search for food type.. follows the word "cook "
//                    string searchTarget = "cook ";
//                    int foodStartIndex = activity.Text.IndexOf(searchTarget) + searchTarget.Length;
//                    //search for end of food type.. search for next " " or end of string
//                    int foodEndIndex = -1;
//                    foodEndIndex = activity.Text.IndexOf(" ", foodStartIndex);
//                    // if foodEndIndex = -1, no " " found... end of the string was hit or "?" found
//                    if (foodEndIndex != -1)
//                    {
//                        // " " found after foodType
//                        int foodLength = foodEndIndex - foodStartIndex;
//                        foodType = activity.Text.Substring(foodStartIndex, foodLength);
//                    }
//                    else
//                    {
//                        // test for "?"
//                        foodEndIndex = activity.Text.IndexOf("?", foodStartIndex);
//                        if (foodEndIndex != -1)
//                        {
//                            // "?" found after foodType
//                            int foodLength = foodEndIndex - foodStartIndex;
//                            foodType = activity.Text.Substring(foodStartIndex, foodLength);
//                        }
//                        else
//                        {
//                            // foodType is at the end of the string
//                            foodType = activity.Text.Substring(foodStartIndex);
//                        }
//                    }

//                    // search for cooking time
//                    cookTime = "0:00Hrs";

//                    // search for cooking temp
//                    cookTemp = "000F";

//                    Activity replyCookType = activity.CreateReply($"Cook {foodType} in the oven for {cookTime} @{cookTemp}");
//                    await connector.Conversations.ReplyToActivityAsync(replyCookType);
//                }
//                else if (activity.Text.Contains("bake") || activity.Text.Contains("Bake"))
//                {
//                    appliance = "bake";
//                    Activity replyCookType = activity.CreateReply($"Bake {foodType} for {cookTime} @{cookTemp}");
//                    await connector.Conversations.ReplyToActivityAsync(replyCookType);
//                }
//                else if (activity.Text.Contains("grill") || activity.Text.Contains("Grill"))
//                {
//                    appliance = "grill";
//                    Activity replyCookType = activity.CreateReply($"Grill {foodType} for {cookTime} @{cookTemp}");
//                    await connector.Conversations.ReplyToActivityAsync(replyCookType);
//                }
//                else if (activity.Text.Contains("stove") || activity.Text.Contains("Stove"))
//                {
//                    appliance = "stove";
//                    Activity replyCookType = activity.CreateReply($"Cook {foodType} on the stove for {cookTime} @{cookTemp}");
//                    await connector.Conversations.ReplyToActivityAsync(replyCookType);
//                }
//                else if (activity.Text.Contains("pan") || activity.Text.Contains("Pan"))
//                {
//                    appliance = "pan";
//                    Activity replyCookType = activity.CreateReply($"Cook {foodType} in a pan for {cookTime} @{cookTemp}");
//                    await connector.Conversations.ReplyToActivityAsync(replyCookType);
//                }
//                else if (activity.Text.Contains("thanks") || activity.Text.Contains("Thanks")
//                    || activity.Text.Contains("thank") || activity.Text.Contains("thanks"))
//                {
//                    Activity replyCookType = activity.CreateReply($"You're very welcome!");
//                    await connector.Conversations.ReplyToActivityAsync(replyCookType);
//                }
//                else if (activity.Text.Contains("hi") || activity.Text.Contains("Hi")
//                  || activity.Text.Contains("hello") || activity.Text.Contains("Hello"))
//                {
//                    Activity replyCookType = activity.CreateReply($"Hello, May I help you with any cooking time and temperatures?");
//                    await connector.Conversations.ReplyToActivityAsync(replyCookType);
//                }
//                else
//                {
//                    Activity replyCookType = activity.CreateReply($"Sorry, please include bake, grill, stove, oven or pan and what you are cooking!");
//                    await connector.Conversations.ReplyToActivityAsync(replyCookType);
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