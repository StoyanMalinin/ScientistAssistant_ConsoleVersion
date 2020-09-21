using System;
using System.Collections.Generic;
using System.Text;

namespace ScientistAssistant_ConsoleVersion.UI
{
    abstract class CustomUIException : Exception
    {
        public CustomUIException() : base() { }
        public CustomUIException(string msg) : base(msg) { }
    }

    class InvalidDatasetException : CustomUIException
    {
        public InvalidDatasetException() { }
        public InvalidDatasetException(string dataset) 
            : base($"{dataset} is not a valid dataset") {}
    }
}
