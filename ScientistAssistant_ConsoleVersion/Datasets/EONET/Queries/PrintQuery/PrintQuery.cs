using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

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
            foreach (DatasetClasses.Event e in l)
                Console.WriteLine(e.ToString());
        }

        public void execute(List<string> flags)
        {
            if(flags.Count==0)
            {
                Console.WriteLine("All events:");
                printList(EONETDataset.events);
            }
            else
            {

            }
        }


    }
}
