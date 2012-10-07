using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDS_Script_Language.VM;

namespace KDS_Script_Language.AST
{
    public class ASTFunctionParameter : ASTNode
    {
        public string Name;
        public VMType Type;
    }
}
