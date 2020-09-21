using System;
using System.Collections.Generic;
using System.Text;
using ScientistAssistant_ConsoleVersion.Datasets;
using ScientistAssistant_ConsoleVersion.CommunicatonModule;
using System.Net.Http.Headers;

namespace ScientistAssistant_ConsoleVersion.Datasets
{
    class EONETDataset : IDataset
    {
        Dictionary<string, UI.Queries.IQuery> queries = new Dictionary<string, UI.Queries.IQuery>();

        public EONETDataset()
        {
            queries["reload"] = new UI.Queries.ReloadQuery.ReloadQuery();  
            
        }

        public static List<Event> events = new List<Event>();

        public void processQuery(List<string> flags)
        {
            string queryType = flags[0];
            flags.RemoveAt(0);

            if(queries.ContainsKey(queryType)==true)
            {
                queries[queryType].execute(flags);
            }
        }
    }
}
