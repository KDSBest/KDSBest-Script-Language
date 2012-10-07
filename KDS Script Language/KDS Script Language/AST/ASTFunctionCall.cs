using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDS_Script_Language.AST
{
    public class ASTFunctionCall : ASTNode
    {
        public string Scope;
        public string FunctionName;
        public List<ASTStatement> Parameter;

        public ASTFunctionCall()
        {
            Parameter = new List<ASTStatement>();
        }
    }
}
