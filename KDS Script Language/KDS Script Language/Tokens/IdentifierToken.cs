using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDS_Script_Language.Tokens
{
    public class IdentifierToken : TokenBase
    {
        public IdentifierToken(string token, int lineNumber)
            : base(token, lineNumber)
        {
        }
    }
}
