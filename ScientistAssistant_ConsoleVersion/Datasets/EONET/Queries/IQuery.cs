using System;
using System.Collections.Generic;
using System.Text;

namespace ScientistAssistant_ConsoleVersion.UI.Queries
{
    interface IQuery
    {
        public void execute(List<string> flags);
    }
}
