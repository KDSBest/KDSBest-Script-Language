/*
    KDSBest Script Language/Engine
    Copyright (C) 2012  KDSBest (www.kdsbest.com)

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

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
