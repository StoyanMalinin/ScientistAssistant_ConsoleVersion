using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using ScientistAssistant_ConsoleVersion.QueryTagLogic;
using ScientistAssistant_ConsoleVersion.Datasets.EONET.DatasetClasses;

namespace ScientistAssistant_ConsoleVersion.Datasets.EONET.Queries.PrintQuery
{
    interface IPrintQuery : IQuery
    {

    }

    class PrintQuery : IPrintQuery
    {
        Dictionary<string, IPrintQuery> queries = new Dictionary<string, IPrintQuery>();

        public PrintQuery()
        {
            queries["events"] = new PrintEventsQuery();
        }

        public void execute(List<string> flags)
        {
            string printType = flags[0];
            flags.RemoveAt(0);

            if(queries.ContainsKey(printType)==true)
            {
                queries[printType].execute(flags);
            }
        }
    }

    class PrintEventsQuery : IPrintQuery
    {
        private void printList(List <DatasetClasses.Event> l)
        {
            foreach (Event e in l)
                Console.WriteLine(e.ToString());
        }

        public void execute(List<string> flags)
        {
            if (flags.Count == 0)
            {
                Console.WriteLine("All events:");
                printList(EONETDataset.events);
            }
            else
            {
                List<string> filters = flags[0].Split('-').ToList();

                List<LogicNode> requirements = new List<LogicNode>();
                foreach(string f in filters)
                {
                    Console.Write($"Requiremets for {f}: ");
                    string s = Console.ReadLine();

                    requirements.Add(LogicConstructor.constructLogicTree(s));
                    Console.WriteLine($"A more explicit version: {requirements.Last()}");
                }

                List<Event> matching = new List<Event>();
                foreach(Event e in EONETDataset.events)
                {
                    bool fail = false;
                    for(int i = 0;i<filters.Count;i++)
                    {
                        if(requirements[i].checkObject<Event>(e, filters[i])==false)
                        {
                            fail = true;
                            break;
                        }
                        
                    }

                    if (fail == false) matching.Add(e);
                }

                Console.WriteLine("Matching events:");
                printList(matching);
            }
        }


    }
}
