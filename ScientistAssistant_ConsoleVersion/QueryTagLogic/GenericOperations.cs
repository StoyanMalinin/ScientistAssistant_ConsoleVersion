using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ScientistAssistant_ConsoleVersion.Datasets.EONET.DatasetClasses;
using ScientistAssistant_ConsoleVersion.Datasets.EONET;
using System.Runtime.InteropServices.ComTypes;
using ScientistAssistant_ConsoleVersion.UI;
using System.Collections;
using System.Globalization;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;

namespace ScientistAssistant_ConsoleVersion.QueryTagLogic
{
    interface InformationObject
    {
        public string id { get; set; }
    }

    class FilteringFunctionDictionary<T> where T: InformationObject
    {
        Dictionary<string, Func<List<T>, List<string>, List<T>>> mp;

        public FilteringFunctionDictionary()
        {
            this.mp = new Dictionary<string, Func<List<T>, List<string>, List<T>>>();
        }

        public void addFun(string name, Func<List<T>, List<string>, List<T>> f)
        {
            mp[name] = f;
        }

        public bool checkKey(string name)
        {
            return mp.ContainsKey(name);
        }

        public Func<List<T>, List<string>, List<T>> getFunction(string name)
        {
            return mp[name];
        }
    }

    static class GenericOperations
    {
        public static List<T> filterList<T>(List<T> all, List<string> flags, FilteringFunctionDictionary<T> mp) where T: InformationObject
        {
            List<T> matching = all;
            foreach (string s in flags)
            {
                List<string> elements = s.Split('-').Where(x => x!="").ToList();

                string type = elements[0];
                elements.RemoveAt(0);

                if (mp.checkKey(type) == true)
                {
                    matching = mp.getFunction(type)(matching, elements);
                }
                else
                {
                    throw new WrongFilterException(type);
                }
            }

            return matching;
        }

        public static ICollection<T> removeRepeatingElements<T>(ICollection <T> all) where T : class
        {
            return (new HashSet<T>(all));
        }

        public static bool checkForFullInfo(List <string> flags)
        {
            bool res = flags.Contains("full-info");
            flags.RemoveAll(s => s=="full-info");

            return res;
        }

        public static void printList<T>(List<T> l, bool fullInfo) where T: InformationObject
        {
            foreach (T item in l)
            {
                if (fullInfo == false) Console.WriteLine(item.id);
                else Console.WriteLine(item);
            }
        }

        public static List<T> filterListByProperties<T>(List<T> all, List<string> properties) where T: InformationObject
        {
            List<LogicNode> requirements = new List<LogicNode>();
            foreach (string f in properties)
            {
                LogicNode logicTree = null;
                
                while(true)
                {
                    try
                    {
                        Console.Write($"Requiremets for {f}: ");
                        string s = Console.ReadLine();

                        logicTree = LogicConstructor.constructLogicTree(s);
                    }
                    catch(ExpressionException e)
                    {
                        Console.WriteLine("Invalid expression");
                        Console.WriteLine(e.Message);

                        continue;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid expression");
                        Console.WriteLine("Reason - unknown");

                        continue;
                    }

                    break;
                }

                requirements.Add(logicTree);
                Console.WriteLine($"A more explicit version: {requirements.Last()}");
            }

            List<T> matching = new List<T>();
            foreach (T item in all)
            {
                bool fail = false;
                for (int i = 0; i < properties.Count; i++)
                {
                    if (requirements[i].checkObject(item, properties[i]) == false)
                    {
                        fail = true;
                        break;
                    }
                }

                if (fail == false) matching.Add(item);
            }

            return matching;
        }

        public static List<Event> filterListByPosition(List<Event> all, List<string> flags)
        {
            static double readDouble(string txt)
            {
                double x;
                while(true)
                {
                    try
                    {
                        Console.Write(txt);
                        x = double.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Invalid number");
                        continue;
                    }

                    break;
                }

                return x;
            }

            int cnt = 1;
            if(flags.Count>0)
            {
                if (flags.Count != 1) throw new WrongFlagException();
                
                try
                {
                    cnt = int.Parse(flags[0]);
                }
                catch
                {
                    throw new WrongFlagException();
                }
            }

            double minLatitude = readDouble("Minimum latitude: ");
            double maxLatitude = readDouble("Maximum latitude: ");

            double minLongtitude = readDouble("Minimum longtitude: ");
            double maxLongtitude = readDouble("Maximum longtitude: ");

            List<Event> matcing = new List<Event>();
            foreach(Event e in all)
            {
                if(e.checkInsideRect(minLatitude, maxLatitude, minLongtitude, maxLongtitude, cnt)==true)
                {
                    matcing.Add(e);
                }
            }

            return matcing;
        }
    }
}
