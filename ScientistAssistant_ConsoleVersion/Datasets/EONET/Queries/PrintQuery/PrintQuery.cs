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
using System.Net.Http.Headers;

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
            mp.addFun("position", GenericOperations.filterListByPosition);
        }

        public void execute(List<string> flags)
        {
            flags = GenericOperations.removeRepeatingElements(flags).ToList();
            bool fullInfo = GenericOperations.checkForFullInfo(flags);
            
            List<Event> matching = EONETDataset.events;
            matching = GenericOperations.filterList(matching, flags, mp);

            GenericOperations.printList(matching, fullInfo);
            Console.WriteLine($"MatchingCount: {matching.Count}");
        }
    }
}
