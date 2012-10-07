using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDS_Script_Language.AST
{
    public class ASTClass : ASTNode
    {
        public string Name;
        public List<ASTConstructor> Constructors;
        public List<ASTClassAttribute> Attributes;
        public List<ASTFunction> Functions;
    }
}
