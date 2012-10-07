using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDS_Script_Language.AST
{
    public class ASTClassParameter : ASTNode
    {
        public string Scope;
        public ASTStatement Parameter;
    }
}
