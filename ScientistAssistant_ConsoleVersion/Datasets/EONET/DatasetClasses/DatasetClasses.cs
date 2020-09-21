using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace ScientistAssistant_ConsoleVersion.Datasets.EONET.DatasetClasses
{
    class Geometry
    {
        public string type { get; set; }
        public Newtonsoft.Json.Linq.JArray coordinates { get; set; }

        private string arrToString(Newtonsoft.Json.Linq.JArray arr)
        {
            if (arr.Count == 0) return "";

            List<string> parts = new List<string>();
            foreach(object item in arr)
            {
                if (item.GetType() == typeof(Newtonsoft.Json.Linq.JArray))
                    parts.Add(arrToString(item as Newtonsoft.Json.Linq.JArray));
                else
                    parts.Add(item.ToString());
            }

            return $"[{string.Join(", ", parts)}]";
        }

        public override string ToString()
        {
            string output = "";
            output += "{" + $"type: {type},\n";
            output += $"corrdinates: {arrToString(coordinates)}" + "}";
         
            return output;
        }
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

        public override string ToString()
        {
            string output = "-------------\n";
            output += $"id: {id}\n";
            output += $"title: {title}\n";
            output += $"description: {title}\n";
            output += $"link: {title}\n";

            output += "\n";
            output += "sources:\n";
            foreach (Source s in sources)
                output += "{" + $"id: {s.id}, url: {s.url}" + "}\n";

            output += "\n";
            output += "categories:\n";
            foreach (Category c in categories)
                output += "{" + $"id: {c.id}, title: {c.title}" + "}\n";

            output += "\n";
            output += "geometry: \n";
            foreach (Geometry g in geometry)
                output += $"{g}\n";
                
            return output;
        }
    }
}
