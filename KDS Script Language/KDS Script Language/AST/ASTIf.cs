using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDS_Script_Language.AST
{
    public class ASTIf : ASTNode
    {
        public ASTStatement Condition;
        public List<ASTNode> CodeBlock;
        public List<ASTNode> ElseCodeBlock;
    }
}
