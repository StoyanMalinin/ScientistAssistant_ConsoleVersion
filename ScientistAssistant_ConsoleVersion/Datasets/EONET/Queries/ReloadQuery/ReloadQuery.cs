using System;
using System.Collections.Generic;
using System.Text;
using ScientistAssistant_ConsoleVersion.Datasets;
using ScientistAssistant_ConsoleVersion.Datasets.EONET.DatasetClasses;
using ScientistAssistant_ConsoleVersion.Datasets.EONET.Queries;

namespace ScientistAssistant_ConsoleVersion.Datasets.EONET.Queries.ReloadQuery
{
    interface IReloadQuery : IQuery
    {

    }

    class ReloadQuery : IReloadQuery
    {
        Dictionary<string, IReloadQuery> queries = new Dictionary<string, IReloadQuery>();

        public ReloadQuery()
        {
            queries["events"] = new ReloadEventsQuery();
        }

        public void execute(List<string> flags)
        {
            string reloadType = flags[0];
            flags.RemoveAt(0);

            if (queries.ContainsKey(reloadType) == true)
            {
                queries[reloadType].execute(flags);
            }
        }
    }

    class ReloadEventsQuery : IReloadQuery
    {
        public void execute(List<string> flags)
        {
            EONETDataset.events = EONETConnector.getCurrentEvents();
        }
    }
}
