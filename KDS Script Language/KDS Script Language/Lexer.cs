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
using KDS_Script_Language.Tokens;

namespace KDS_Script_Language
{
    /// <summary>
    /// Creates a StringToken Stream Out of a Source Code "String".
    /// The Lexer follows the Keep It Simple Stupid Prinicle.
    /// It just detects unterminated Strings.
    /// It's not a class on it's own, it just extends the compiler 
    /// with Extension Method to reduce the complexity of the Compiler class.
    /// </summary>
    public static class Lexer
    {
        /// <summary>
        /// Terminates the token before
        /// </summary>
        private static char[] ignoredToken = { ' ', '\t', '\r' };

        /// <summary>
        /// A token always terminates the token before and is a token on its own
        /// </summary>
        private static char[] token = { '(', ')', '{', '}', '[', ']', ';', '.', ',' };

        /// <summary>
        /// Operators of our Language.
        /// They can occure next to each other example "==" or "+="
        /// </summary>
        private static char[] operators = { 
                                             '-', '+', '*', '/', '=', 
                                             '&', '|', '!', '%', '<', '>'
                                         };

        /// <summary>
        /// Defines the escape Sequenzes in strings
        /// </summary>
        private static char[,] escapeSequenzes = { 
                                                    {'n', '\n' },
                                                    {'\\', '\\' },
                                                    {'t', '\t' },
                                                    {'r', '\r' },
                                                    {'"', '\"' }
                                                };
        /// <summary>
        /// Defines the String Quote Character
        /// </summary>
        private static char stringQuote = '"';

        /// <summary>
        /// Defines the Escape Char
        /// </summary>
        private static char escapeChar = '\\';

        /// <summary>
        /// Defines the newLine Char
        /// </summary>
        private static char newLine = '\n';

        /// <summary>
        /// Checks if a given Character is in the Token List
        /// </summary>
        /// <param name="c">Character to check</param>
        /// <param name="tokens">Token List</param>
        /// <returns>true if character is one of the token</returns>
        private static bool IsCharInTokenList(char c, char[] tokens)
        {
            foreach (char t in tokens)
            {
                if (c == t)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if Character is in ignoredToken List
        /// </summary>
        /// <param name="c">Character to check</param>
        /// <returns>true if character is an ignoredToken</returns>
        private static bool IsIgnoredToken(char c)
        {
            return IsCharInTokenList(c, ignoredToken);
        }

        /// <summary>
        /// Checks if Character is in token List
        /// </summary>
        /// <param name="c">Character to check</param>
        /// <returns>true if character is an token</returns>
        private static bool IsSpecialToken(char c)
        {
            return IsCharInTokenList(c, token);
        }

        /// <summary>
        /// Checks if Character is in operator List
        /// </summary>
        /// <param name="c">Character to check</param>
        /// <returns>true if character is an operator</returns>
        private static bool IsOperator(char c)
        {
            return IsCharInTokenList(c, operators);
        }

        /// <summary>
        /// Gives back the defined escape Sequenze of an given Character
        /// </summary>
        /// <param name="c">Character that defines the escape Sequenz</param>
        /// <returns>Escape Sequenzed char</returns>
        private static char GetEscapreSequenze(char c)
        {
            for (int i = 0; i < escapeSequenzes.Length; i++)
            {
                if (c == escapeSequenzes[i, 0])
                    return escapeSequenzes[i, 1];
            }

            throw new LexerException("Escape Sequenz is unknown.", lineNumber);
        }

        /// <summary>
        /// Current Line Number, so other functions can Throw Exceptions with a Line Number
        /// </summary>
        private static int lineNumber;

        /// <summary>
        /// Single Line Comment (has to be build of 2 operators)
        /// </summary>
        private static string singleLineComment = "//";

        /// <summary>
        /// Multi Line Comment Start (has to be build of 2 operators)
        /// </summary>
        private static string multiLineCommentStart = "/*";

        /// <summary>
        /// Multi Line Comment End (has to be build of 2 operators)
        /// </summary>
        private static string multiLineCommentEnd = "*/";

        /// <summary>
        /// First Stage of the Lexing Procedure
        /// </summary>
        /// <param name="compiler"></param>
        /// <returns></returns>
        private static List<LexerToken> SplitSourceCode(Compiler compiler)
        {
            List<LexerToken> stringToken = new List<LexerToken>();

            string sourceCode = compiler.SourceCode;

            string currentToken = "";
            lineNumber = 1;

            // Traverse Source Code by char
            for (int i = 0; i < sourceCode.Length; i++)
            {
                if (IsSpecialToken(sourceCode[i]))
                {
                    if (currentToken != "")
                    {
                        stringToken.Add(new LexerToken(currentToken, lineNumber));
                        currentToken = "";
                    }
                    stringToken.Add(new LexerToken("" + sourceCode[i], lineNumber));
                }
                else if (IsIgnoredToken(sourceCode[i]))
                {
                    if (currentToken != "")
                    {
                        stringToken.Add(new LexerToken(currentToken, lineNumber));
                        currentToken = "";
                    }
                }
                else if (IsOperator(sourceCode[i]))
                {
                    if(currentToken != "")
                        stringToken.Add(new LexerToken(currentToken, lineNumber));
                    currentToken = "" + sourceCode[i];

                    int ii = i + 1;
                    if (ii < sourceCode.Length)
                    {
                        if (IsOperator(sourceCode[ii]))
                        {
                            // ignore next character it is an operator so we got a 2 char operator
                            i++;

                            currentToken += sourceCode[ii];

                            // Remove Single Line Comment
                            if (currentToken == singleLineComment)
                            {
                                for (++i; i < sourceCode.Length; i++)
                                {
                                    if (sourceCode[i] == newLine)
                                        break;
                                }

                                // Komment ends on new line preserve the new Line
                                lineNumber++;
                            }
                            else if (currentToken == multiLineCommentStart)
                            {
                                for (++i; i < sourceCode.Length - 1; i++)
                                {
                                    if (sourceCode[i] == multiLineCommentEnd[0] && sourceCode[i + 1] == multiLineCommentEnd[1])
                                    {
                                        // ignore the multiLineCommentEnd Part 2
                                        i++;
                                        break;
                                    }
                                    if (sourceCode[i] == '\n')
                                        lineNumber++;
                                }
                            }
                            else
                            {
                                // add the operator to tokens
                                stringToken.Add(new LexerToken(currentToken, lineNumber));
                            }
                        }
                        else
                        {
                            // 1 char operator we add it
                            stringToken.Add(new LexerToken(currentToken, lineNumber));
                        }
                        currentToken = "";
                    }
                    else
                    {
                        throw new LexerException("Source Code can't end on an operator", lineNumber);
                    }
                }
                else if (sourceCode[i] == newLine)
                {
                    if (currentToken != "")
                    {
                        stringToken.Add(new LexerToken(currentToken, lineNumber));
                        currentToken = "";
                    }
                    lineNumber++;
                }
                else if (sourceCode[i] == stringQuote)
                {
                    if (currentToken != "")
                        stringToken.Add(new LexerToken(currentToken, lineNumber));
                    // Note: 
                    // we copy the "" in the string token, 
                    // so the 2. stage lexer can identify a string or an identifier
                    currentToken = "" + stringQuote;
                    int ii = i + 1;
                    for (; ii < sourceCode.Length && sourceCode[ii] != '"'; ii++)
                    {
                        if (sourceCode[ii] == newLine)
                        {
                            throw new LexerException("String is not terminated.", lineNumber);
                        }
                        else if (sourceCode[ii] == escapeChar)
                        {
                            int iii = ii + 1;
                            if (iii < sourceCode.Length)
                            {
                                currentToken += "" + GetEscapreSequenze(sourceCode[iii]);
                                ii++;
                            }
                            else
                            {
                                throw new LexerException("Source Code ends in an escape sequenz.", lineNumber);
                            }
                        }
                        else
                        {
                            currentToken += "" + sourceCode[ii];
                        }
                    }

                    // Check if we just reached the end of the source code.
                    if (ii == sourceCode.Length)
                        throw new LexerException("String is not terminated.", lineNumber);

                    currentToken += stringQuote;

                    // We point to the end of the string the '"'
                    i = ii;


                    stringToken.Add(new LexerToken(currentToken, lineNumber));
                    currentToken = "";
                }
                else
                {
                    currentToken += sourceCode[i];
                }
            }

            return stringToken;
        }

        /// <summary>
        /// Checks if a given character is a digit
        /// </summary>
        /// <param name="c">char to check</param>
        /// <returns>true = Digit</returns>
        private static bool IsCharDigit(char c)
        {
            if (c != '0' && c != '1' && c != '2' && c != '3' && c != '4' && c != '5' && 
                c != '6' && c != '7' && c != '8' && c != '9')
                return false;
            return true;
        }

        /// <summary>
        /// Checks if token is a valid Number else an Exception is raised
        /// </summary>
        /// <param name="token">Token to Check</param>
        private static void ValidateNumber(LexerToken token)
        {
            bool dot = false;

            for (int i = 0; i < token.Token.Length; i++)
            {
                if (token.Token[i] == '.')
                {
                    if (dot)
                        throw new LexerException(string.Format("Invalid Number {0}.", token.Token), token.LineNumber);
                    else
                        dot = true;
                }
                else if(!IsCharDigit(token.Token[i]))
                    throw new LexerException(string.Format("Invalid Number {0}.", token.Token), token.LineNumber);
            }
        }

        /// <summary>
        /// Checks if token is a valid Number else an Exception is raised
        /// </summary>
        /// <param name="token">Token to Check</param>
        private static void ValidateIdentifier(LexerToken token)
        {
            string lowerToken = token.Token.ToLower();
            for (int i = 0; i < lowerToken.Length; i++)
            {
                if (lowerToken[i] != '_' && lowerToken[i] != 'a' && lowerToken[i] != 'b' && lowerToken[i] != 'c' &&
                     lowerToken[i] != 'd' && lowerToken[i] != 'e' && lowerToken[i] != 'f' && lowerToken[i] != 'g' &&
                     lowerToken[i] != 'h' && lowerToken[i] != 'i' && lowerToken[i] != 'j' && lowerToken[i] != 'k' &&
                     lowerToken[i] != 'l' && lowerToken[i] != 'm' && lowerToken[i] != 'n' && lowerToken[i] != 'o' &&
                     lowerToken[i] != 'p' && lowerToken[i] != 'q' && lowerToken[i] != 'r' && lowerToken[i] != 's' &&
                     lowerToken[i] != 't' && lowerToken[i] != 'u' && lowerToken[i] != 'v' && lowerToken[i] != 'w' &&
                     lowerToken[i] != 'x' && lowerToken[i] != 'y' && lowerToken[i] != 'z' && !IsCharDigit(lowerToken[i]))
                    throw new LexerException(string.Format("Identifier {0} has an invalid Name.", token.Token), token.LineNumber);
            }
        }

        private static string[] reservedWords = { "function", "class", 
                                                  "string", "number", "object", 
                                                  "new",
                                                  "this", "VM",
                                                  "if", "while", "for", "return" };

        /// <summary>
        /// Checks if LexerToken is a reserved word
        /// </summary>
        /// <param name="token">Token to Check</param>
        /// <returns>true = reserved word</returns>
        private static bool IsReservedWord(LexerToken token)
        {
            foreach (string word in reservedWords)
            {
                if (word == token.Token)
                {
                    return true;
                }
            }
            return false;
        }

        public static List<TokenBase> Lex(this Compiler compiler)
        {
            List<LexerToken> stringToken = SplitSourceCode(compiler);
            List<TokenBase> token = new List<TokenBase>();

            for (int i = 0; i < stringToken.Count; i++)
            {
                LexerToken sToken = stringToken[i];

                if (IsReservedWord(sToken))
                {
                    token.Add(new ReservedWordToken(sToken.Token, sToken.LineNumber));
                }
                else if (IsOperator(sToken.Token[0]))
                {
                    token.Add(new OperatorToken(sToken.Token, sToken.LineNumber));
                }
                else if (IsCharDigit(sToken.Token[0]))
                {
                    if (stringToken.Count > i + 2)
                    {
                        // if digit + "." + digit, we have a double number
                        if (stringToken[i + 1].Token == "." && IsCharDigit(stringToken[i + 2].Token[0]))
                        {
                            sToken.Token += stringToken[i + 1].Token + stringToken[i + 2].Token;
                            i += 2;
                        }
                    }
                    ValidateNumber(sToken);
                    token.Add(new NumberToken(sToken.Token, sToken.LineNumber));
                }
                else if (sToken.Token[0] == '"')
                {
                    token.Add(new StringToken(sToken.Token.Substring(1, sToken.Token.Length - 2), sToken.LineNumber));
                }
                else if (IsSpecialToken(sToken.Token[0]))
                {
                    token.Add(new SpecialToken(sToken.Token, sToken.LineNumber));
                }
                else
                {
                    ValidateIdentifier(sToken);
                    token.Add(new IdentifierToken(sToken.Token, sToken.LineNumber));
                }
            }

            return token;
        }    
    }
}
