using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using ScientistAssistant_ConsoleVersion.QueryTagLogic;
using ScientistAssistant_ConsoleVersion.Datasets.EONET.DatasetClasses;
using System.Data;
using ScientistAssistant_ConsoleVersion.UI;

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
            if (flags.Count > 0)
            {
                string printType = flags[0];
                flags.RemoveAt(0);

                if (queries.ContainsKey(printType) == true)
                {
                    queries[printType].execute(flags);
                }
                else
                {
                    throw new WrongFlagException(printType);
                }
            }
            else
            {
                throw new InsufficientNumberOfFlagsException();
            }
        }
    }

    class PrintEventsQuery : IPrintQuery
    {
        FilteringFunctionDictionary<Event> mp = new FilteringFunctionDictionary<Event>();

        public PrintEventsQuery()
        {
            mp.addFun("properties", GenericOperations.filterListByProperties);
        }

        private void printList(List <Event> l)
        {
            foreach (Event e in l)
                Console.WriteLine(e.ToString());
        }

        public void execute(List<string> flags)
        {
            List<Event> matching = EONETDataset.events;
            matching = GenericOperations.filterList(matching, flags, mp);

            printList(matching);
            Console.WriteLine($"MatchingCount: {matching.Count}");
        }
    }
}
