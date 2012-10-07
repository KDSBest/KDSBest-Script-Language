using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDS_Script_Language.Tokens
{
    /// <summary>
    /// Base Class for Token
    /// </summary>
    public abstract class TokenBase
    {
        public string Token;
        public int LineNumber;

        public TokenBase(string token, int lineNumber)
        {
            Token = token;
            LineNumber = lineNumber;
        }
    }
}
