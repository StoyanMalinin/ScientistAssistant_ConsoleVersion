using System;
using System.Collections.Generic;
using System.Text;

namespace ScientistAssistant_ConsoleVersion.QueryTagLogic
{
    abstract class ExpressionException : Exception
    {
        public ExpressionException() : base() { }
        public ExpressionException(string msg) : base(msg) { }
    }

    class InvalidBracketSequenceExeption : ExpressionException
    {
        public InvalidBracketSequenceExeption()
        : base("Ivalid bracket sequence") { }
    }

    class BinaryOperatorCannotTakeTwoArgumentsException : ExpressionException
    {
        public BinaryOperatorCannotTakeTwoArgumentsException()
        : base("A binary operator is not able to take two arguments") { }
    }

    class UnaryOperatorCannotTakeOneArgumentException : ExpressionException
    {
        public UnaryOperatorCannotTakeOneArgumentException()
        : base("A unary operator is not able to take one argument") { }
    }

    class UnaryOperatorTakesTwoArgumentsException : ExpressionException
    {
        public UnaryOperatorTakesTwoArgumentsException()
        : base("A unary operator takes two arguments, but it must not") { }
    }
}
