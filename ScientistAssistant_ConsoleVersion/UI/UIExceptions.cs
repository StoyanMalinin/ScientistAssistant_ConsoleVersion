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
        : base($"{'"'}{dataset}{'"'} is not a valid dataset") {}
    }

    class InsufficientNumberOfFlagsException : CustomUIException
    {
        public InsufficientNumberOfFlagsException() 
        : base("Insufficient number of flags or misleading ones") { }
    }

    class WrongFlagException : CustomUIException
    {
        public WrongFlagException() { }
        public WrongFlagException(string flag)
        : base($"{'"'}{flag}{'"'} is not a valid flag for the current command") { }
    }

    class WrongFilterException : CustomUIException
    {
        public WrongFilterException() { }
        public WrongFilterException(string filter)
        : base($"{'"'}{filter}{'"'} is not a valid filter for the current command") { }
    }

    class WrongPropertyNameException : CustomUIException
    {
        public WrongPropertyNameException() { }
        public WrongPropertyNameException(string propertyName)
        : base($"{'"'}{propertyName}{'"'} is not a valid property name in the given context") { }
    }
}
