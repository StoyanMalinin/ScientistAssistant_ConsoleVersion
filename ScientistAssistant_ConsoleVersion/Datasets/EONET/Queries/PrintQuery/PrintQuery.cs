using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace ScientistAssistant_ConsoleVersion.UI.Queries
{
    interface IPrintQuery : IQuery
    {

    }

    class PrintQuery : IPrintQuery
    {
        Dictionary<string, IPrintQuery> queries = new Dictionary<string, IPrintQuery>();

        public void execute(List<string> flags)
        {

        }
    }
}
