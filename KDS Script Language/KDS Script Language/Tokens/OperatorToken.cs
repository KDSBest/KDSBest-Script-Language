using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KDS_Script_Language.Exceptions;

namespace KDS_Script_Language.Tokens
{
    public class OperatorToken : TokenBase
    {
        public int Precedence;

        public OperatorToken(string token, int lineNumber)
            : base(token, lineNumber)
        {
            switch (token)
            {
                case "!":
                case "++":
                case "--":
                    Precedence = 1;
                    break;
                case "*":
                case "/":
                case "%":
                    Precedence = 2;
                    break;
                case "-":
                case "+":
                    Precedence = 3;
                    break;
                case "<":
                case ">":
                case "<=":
                case ">=":
                    Precedence = 4;
                    break;
                case "==":
                case "!=":
                    Precedence = 5;
                    break;
                case "&":
                    Precedence = 6;
                    break;
                case "|":
                    Precedence = 7;
                    break;
                case "&&":
                    Precedence = 8;
                    break;
                case "||":
                    Precedence = 9;
                    break;
                case "=":
                case "|=":
                case "&=":
                case "*=":
                case "/=":
                case "+=":
                case "-=":
                case "%=":
                    Precedence = 10;
                    break;
                default:
                    throw new LexerException(string.Format("Operator {0} unknown.", token), lineNumber);
            }
        }
    }
}
