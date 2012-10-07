using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDS_Script_Language.Exceptions
{
    /// <summary>
    /// LexerException Class
    /// </summary>
    public class LexerException : Exception
    {
        public int LineNumber;

        public LexerException(string message, int lineNumber)
            : base(message)
        {
            LineNumber = lineNumber;
        }
    }
}
