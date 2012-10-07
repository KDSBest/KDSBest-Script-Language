using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDS_Script_Language.Exceptions
{
    /// <summary>
    /// LexerException Class
    /// </summary>
    public class ParserException : Exception
    {
        public int LineNumber;

        public ParserException(string message, int lineNumber)
            : base(message)
        {
            LineNumber = lineNumber;
        }
    }
}
