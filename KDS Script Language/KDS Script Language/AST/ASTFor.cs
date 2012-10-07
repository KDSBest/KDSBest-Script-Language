using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDS_Script_Language.AST
{
    public class ASTFor : ASTNode
    {
        public ASTStatement Initialise;
        public ASTStatement Condition;
        public ASTStatement Count;
        public List<ASTNode> CodeBlock;
    }
}
