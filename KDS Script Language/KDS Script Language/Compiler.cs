using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDS_Script_Language
{
    /// <summary>
    /// Compiler of the KDS Script Language
    /// </summary>
    public class Compiler
    {
        public Compiler(string SourceCode)
        {
            this._sourceCode = SourceCode;
        }

        private string _sourceCode;
        public string SourceCode
        {
            get
            {
                return this._sourceCode;
            }
        }
    }
}
