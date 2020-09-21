using System;
using System.Collections.Generic;
using System.Text;

namespace ScientistAssistant_ConsoleVersion.Datasets
{
    interface IDataset
    {
        void processQuery(List<string> flags);
    }
}
