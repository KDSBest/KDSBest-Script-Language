using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDS_Script_Language.AST
{
    public class ASTOperator : ASTNode
    {
        public ASTNode Left;
        public ASTNode Right;
        public string Operator;
        public int Precedence;
    }
}
