using System;
using System.Collections.Generic;
using System.Text;
using ScientistAssistant_ConsoleVersion.Datasets;
using ScientistAssistant_ConsoleVersion.CommunicatonModule;

namespace ScientistAssistant_ConsoleVersion.Datasets
{
    class EONETDataset
    {
        interface Query
        {
            public void execute();
        }

        class ReloadEventsQuery : Query
        {
            public void execute()
            {
                events = EONETConnector.getCurrentEvents();
            }
        }

        private static List<Event> events = new List<Event>();
    }
}
