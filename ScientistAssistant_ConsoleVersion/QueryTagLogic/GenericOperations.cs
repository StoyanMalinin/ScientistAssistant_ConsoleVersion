using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ScientistAssistant_ConsoleVersion.Datasets.EONET.DatasetClasses;
using ScientistAssistant_ConsoleVersion.Datasets.EONET;
using System.Runtime.InteropServices.ComTypes;

namespace ScientistAssistant_ConsoleVersion.QueryTagLogic
{
    class FilteringFunctionDictionary<T> where T: class
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
        public static List<T> filterListByProperties<T>(List<T> all, List<string> properties) where T: class
        {
            List<LogicNode> requirements = new List<LogicNode>();
            foreach (string f in properties)
            {
                Console.Write($"Requiremets for {f}: ");
                string s = Console.ReadLine();

                requirements.Add(LogicConstructor.constructLogicTree(s));
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
    }
}
