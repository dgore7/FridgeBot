using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using unirest_net.http;
using System.Text;

namespace fridgeBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;

                //// return our reply to the user
                //Activity reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
                //await connector.Conversations.ReplyToActivityAsync(reply);

                // respond to basic "hello" and "thank you" responses from user
                // if not, assume the user is passing in a list of ingredients seperated by ", "

                //if (activity.Text.Contains("hi") || activity.Text.Contains("Hi")
                //  || activity.Text.Contains("hello") || activity.Text.Contains("Hello"))
                //{
                //    Activity replyGreeting = activity.CreateReply($"Hello! If you give me ingredients, I can recommend a recipe for you. (ie. apples,sugar,flower)");
                //    await connector.Conversations.ReplyToActivityAsync(replyGreeting);
                //} else if (activity.Text.Contains("thanks") || activity.Text.Contains("Thanks")
                //    || activity.Text.Contains("thank") || activity.Text.Contains("thanks"))
                //{
                //    Activity replyThanks = activity.CreateReply($"You're very welcome! Enjoy your meal!");
                //    await connector.Conversations.ReplyToActivityAsync(replyThanks);
                //} else
                //{
                //    // remove " " in bewtween ingredients - format for API: spoonacular (GET Find By Ingredients)
                //    activity.Text = activity.Text.Replace(" ", string.Empty);
                //    Activity replyAcknowledge = activity.CreateReply($"Let me find recipes for {activity.Text}...");
                //    await connector.Conversations.ReplyToActivityAsync(replyAcknowledge);


                // take in ingredients list and format correctly to pass into API url

                activity.Text.Replace(",", "%2C");  // replace all commas with '%2C'
                activity.Text.Replace(" ", "");     // remove all spaces if there any

                string argIngredients = activity.Text;

                // create url by passing in parameters
                string url = "https://spoonacular-recipe-food-nutrition-v1.p.mashape.com/recipes/findByIngredients?";

                string param_fillIngredients = "false";
                string param_ingredients = argIngredients;
                string param_limitLicense = "false";
                string param_number = "3";
                string param_ranking = "1";

                url = url + "fillIngredients=" + param_fillIngredients + "&";
                url = url + "ingredients=" + param_ingredients + "&";
                url = url + "limitLicense=" + param_limitLicense + "&";
                url = url + "number=" + param_number + "&";
                url = url + "ranking=" + param_ranking;


                // "https://spoonacular-recipe-food-nutrition-v1.p.mashape.com/recipes/findByIngredients?fillIngredients=false&ingredients=apples%2Cflour%2Csugar&limitLicense=false&number=5&ranking=1"
                // These code snippets use an open-source library.
                HttpResponse<MemoryStream> responseAPI = Unirest.get(url)
                .header("X-Mashape-Key", "API-KEY")
                .header("Accept", "application/json")
                //.field("fillIngredients", "false")
                //.field("ingredients", "apples,flour,sugar")
                //.field("limitLicense", "false")
                //.field("number", "1")
                //.field("ranking", "1")
                .asJson<MemoryStream>();

                // Convert MemoryStream into a json array
                string json = Encoding.Default.GetString(responseAPI.Body.ToArray());


                // reply back to user with the three recipe options
                Activity replyRecipe = activity.CreateReply(json);
                await connector.Conversations.ReplyToActivityAsync(replyRecipe);

                //}

            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}