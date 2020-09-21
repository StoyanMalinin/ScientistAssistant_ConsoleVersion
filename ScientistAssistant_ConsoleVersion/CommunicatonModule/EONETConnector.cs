﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace ScientistAssistant_ConsoleVersion.CommunicatonModule
{
    class Geometry
    {
        public string type { get; set; }
        public Newtonsoft.Json.Linq.JArray coordinates { get; set; }
    }

    class Category
    {
        public string id { get; set; }
        public string title { get; set; }
    }

    class Source
    {
        public string id { get; set; }
        public string url { get; set; }
    }

    class Event
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string link { get; set; }

        public List<Source> sources { get; set; }
        public List<Geometry> geometry { get; set; }
        public List<Category> categories { get; set; }
    }

    static class EONETConnector
    {
        class EventsWrapper  
        {
            public Event[] events { get; set; }
        }

        private static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(T);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jtr);
                return searchResult;
            }
        }

        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }

        private static async Task<T> getJSONObject<T>(CancellationToken cancellationToken, string url) where T: class
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                    return DeserializeJsonFromStream<T>(stream);
            }

            return null;
        }

        private static string apiV3EventsUrl = "https://eonet.sci.gsfc.nasa.gov/api/v3/events";

        public static List <Event> getCurrentEvents()
        {
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            //watch.Stop();

            EventsWrapper ew = getJSONObject<EventsWrapper>(new CancellationToken(), apiV3EventsUrl).Result;
            return ew.events.ToList();
        }
    }
}
