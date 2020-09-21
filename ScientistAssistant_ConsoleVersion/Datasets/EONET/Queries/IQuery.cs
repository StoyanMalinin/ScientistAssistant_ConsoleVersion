using System;
using System.Collections.Generic;
using System.Text;

namespace ScientistAssistant_ConsoleVersion.Datasets.EONET.Queries
{
    interface IQuery
    {
        public void execute(List<string> flags);
    }
}
