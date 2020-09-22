using ScientistAssistant_ConsoleVersion.QueryTagLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace ScientistAssistant_ConsoleVersion.Datasets.EONET.DatasetClasses
{
    class Geometry
    {
        public string type { get; set; }
        public JArray coordinates { get; set; }

        private string arrToString(JArray arr)
        {
            if (arr.Count == 0) return "";

            List<string> parts = new List<string>();
            foreach(object item in arr)
            {
                if (item.GetType() == typeof(JArray))
                    parts.Add(arrToString(item as JArray));
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

        private bool checkInsideRectInternal(double minLatitude, double maxLatitude,
                                             double minLongtitude, double maxLongtitude,
                                             Newtonsoft.Json.Linq.JArray arr)
        {
            if (arr.Count == 0) return true;
            if(arr[0].GetType()==typeof(JArray))
            {
                bool fail = false;
                foreach(JArray item in arr)
                {
                    if(checkInsideRectInternal(minLatitude, maxLatitude, minLongtitude, maxLongtitude, item)==false)
                    {
                        fail = true;
                        break;
                    }
                }

                if (fail == false) return true;
                return false;
            }

            if(minLongtitude<=(double)arr[0] && (double)arr[0]<=maxLongtitude)
            {
                if (minLatitude <= (double)arr[1] && (double)arr[1] <= maxLatitude)
                {
                    return true;
                }
            }
            return false;
        }

        public bool checkInsideRect(double minLatitude, double maxLatitude,
                                    double minLongtitude, double maxLongtitude)
        {
            return checkInsideRectInternal(minLatitude, maxLatitude, minLongtitude, maxLongtitude, coordinates);
        }

        private double getAverageCoordinateInternal(JArray arr, int cInd)
        {
            if (arr.Count == 0) return 0;
            
            double sum = 0;
            if (arr[0].GetType() == typeof(JArray))
            {
                foreach (JArray item in arr)
                {
                    sum += getAverageCoordinateInternal(item, cInd);
                }

                return sum / arr.Count;
            }
            else
            {
                sum += (double)arr[cInd];
                return sum;
            }
        }

        public double getAverageCoordinate(int cInd)
        {
            return getAverageCoordinateInternal(coordinates, cInd);
        }
    }

    class Category : InformationObject
    {
        public string id { get; set; }
        public string title { get; set; }
    }

    class Source : InformationObject
    {
        public string id { get; set; }
        public string url { get; set; }
    }

    class Event : InformationObject
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string link { get; set; }

        public List<Source> sources { get; set; }
        public List<Geometry> geometry { get; set; }
        public List<Category> categories { get; set; }

        public double getAverageCoordinate(int cInd)
        {
            return geometry[0].getAverageCoordinate(cInd);
        }

        public bool checkInsideRect(double minLatitude, double maxLatitude, 
                                    double minLongtitude, double maxLongtitude, int cnt)
        {
            bool fail = false;
            for(int i = 0;i<Math.Min(geometry.Count, cnt);i++)
            {
                if(geometry[i].checkInsideRect(minLatitude, maxLatitude, minLongtitude, maxLongtitude)==false)
                {
                    fail = true;
                    break;
                }
            }

            if (fail == false) return true;
            return false;
        }

        public override string ToString()
        {
            string output = "----------------------------\n";
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
