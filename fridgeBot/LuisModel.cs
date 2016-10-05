﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace fridgeBot
{
    public class LUISClient
    {
        public static async Task<Rootobject> ParseUserInput(string strInput)
        {
            string strEscaped = Uri.EscapeDataString(strInput);

            using (var client = new HttpClient())
            {
                // TODO: put URI in config file
                // TODO: insert your LUIS URL here
                string luisURL = "https://api.projectoxford.ai/luis/v1/application?id=5ad69a7d-f722-4319-bc73-dbb89d65135f&subscription-key=8226286cafeb4514963d9228ea3cc6bb";
                string uri = luisURL + "&q=" + strEscaped;
                HttpResponseMessage msg = await client.GetAsync(uri);

                if (msg.IsSuccessStatusCode)
                {
                    var jsonResponse = await msg.Content.ReadAsStringAsync();
                    var _Data = JsonConvert.DeserializeObject<Rootobject>(jsonResponse);
                    return _Data;
                }
            }
            return null;
        }
    }

    public class Rootobject
    {
        public string query { get; set; }
        public Intent[] intents { get; set; }
        public Entity[] entities { get; set; }
    }

    public class Intent
    {
        public string intent { get; set; }
        public float score { get; set; }
    }

    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public float score { get; set; }
    }

}