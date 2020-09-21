using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Headers;
using ScientistAssistant_ConsoleVersion.Datasets.EONET.DatasetClasses;
using ScientistAssistant_ConsoleVersion.Datasets.EONET.Queries.PrintQuery;
using ScientistAssistant_ConsoleVersion.Datasets.EONET.Queries.ReloadQuery;
using ScientistAssistant_ConsoleVersion.UI;

namespace ScientistAssistant_ConsoleVersion.Datasets.EONET
{
    class EONETDataset : IDataset
    {
        Dictionary<string, Queries.IQuery> queries = new Dictionary<string, Queries.IQuery>();

        public EONETDataset()
        {
            queries["reload"] = new ReloadQuery();  
            queries["print"] = new PrintQuery();  
        }

        public static List<Event> events = new List<Event>();

        public void processQuery(List<string> flags)
        {
            if (flags.Count > 0)
            {
                string queryType = flags[0];
                flags.RemoveAt(0);

                if (queries.ContainsKey(queryType) == true)
                {
                    queries[queryType].execute(flags);
                }
                else
                {
                    throw new WrongFlagException(queryType);
                }
            }
            else
            {
                
                throw new InsufficientNumberOfFlagsException();
            }
        }
    }
}
