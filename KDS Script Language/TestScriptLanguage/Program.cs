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
using System.IO;
using KDS_Script_Language;
using KDS_Script_Language.Tokens;
using KDS_Script_Language.AST;
using System.Globalization;
using KDS_Script_Language.VM;

namespace TestScriptLanguage
{
    class Program
    {
        private static StreamWriter sw;

        private static void PrintNode(ASTNode node, int incident)
        {
            if(node == null)
                return;
            for(int i = 0; i < incident; i++)
                sw.Write("\t");
            if (node is ASTString)
            {
                sw.WriteLine("ASTString: " + ((ASTString)node).String.Replace("\n", "\\n"));
            }
            else if (node is ASTNumber)
            {
                sw.WriteLine("ASTNumber: " + ((ASTNumber)node).Number);
            }
            else if (node is ASTIdentifier)
            {
                sw.WriteLine("ASTIdentifier: " + ((ASTIdentifier)node).Identifier);
            }
            else if (node is ASTOperator)
            {
                ASTOperator op = (ASTOperator)node;
                sw.WriteLine("ASTOperator: " + op.Operator);
                PrintNode(op.Left, incident + 1);
                PrintNode(op.Right, incident + 1);
            }
            else if (node is ASTStatement)
            {
                ASTStatement stm = (ASTStatement)node;
                sw.WriteLine("ASTStatement: ");
                PrintNode(stm.Statement, incident + 1);
            }
            else if (node is ASTFunctionCall)
            {
                ASTFunctionCall call = (ASTFunctionCall)node;
                sw.WriteLine("ASTFunctionCall: " + call.Scope + "." + call.FunctionName);
                foreach(ASTNode param in call.Parameter)
                    PrintNode(param, incident + 1);
            }
            else if (node is ASTClassParameter)
            {
                ASTClassParameter classP = (ASTClassParameter) node;
                sw.WriteLine("ASTClassParameter: " + classP.Scope);
                PrintNode(classP.Parameter, incident + 1);
            }
            else if (node is ASTIf)
            {
                ASTIf ifNode = (ASTIf)node;
                sw.WriteLine("ASTIf:");
                for (int i = 0; i < incident; i++)
                    sw.Write("\t"); 
                sw.WriteLine("\tCondition:");
                PrintNode(ifNode.Condition, incident + 2);
                for (int i = 0; i < incident; i++)
                    sw.Write("\t"); 
                sw.WriteLine("\tCodeBlock:");
                foreach (ASTNode codeBlock in ifNode.CodeBlock)
                    PrintNode(codeBlock, incident + 2);
                if (ifNode.ElseCodeBlock != null)
                {
                    for (int i = 0; i < incident; i++)
                        sw.Write("\t"); 
                    sw.WriteLine("\tElse:");
                    foreach (ASTNode codeBlock in ifNode.ElseCodeBlock)
                        PrintNode(codeBlock, incident + 2);
                }
            }
            else if (node is ASTFor)
            {
                ASTFor forNode = (ASTFor)node;
                sw.WriteLine("ASTFor:");
                for (int i = 0; i < incident; i++)
                    sw.Write("\t"); 
                sw.WriteLine("\tInit:");
                PrintNode(forNode.Initialise, incident + 2);
                for (int i = 0; i < incident; i++)
                    sw.Write("\t"); 
                sw.WriteLine("\tCondition:");
                PrintNode(forNode.Condition, incident + 2);
                for (int i = 0; i < incident; i++)
                    sw.Write("\t"); 
                sw.WriteLine("\tCounter:");
                PrintNode(forNode.Count, incident + 2);
                for (int i = 0; i < incident; i++)
                    sw.Write("\t"); 
                sw.WriteLine("\tCodeBlock:");
                foreach (ASTNode codeBlock in forNode.CodeBlock)
                    PrintNode(codeBlock, incident + 2);
            }
            else if (node is ASTWhile)
            {
                ASTWhile wNode = (ASTWhile)node;
                sw.WriteLine("ASTWhile:");
                for (int i = 0; i < incident; i++)
                    sw.Write("\t"); 
                sw.WriteLine("\tCondition:");
                PrintNode(wNode.Condition, incident + 2);
                for (int i = 0; i < incident; i++)
                    sw.Write("\t"); 
                sw.WriteLine("\tCodeBlock:");
                foreach (ASTNode codeBlock in wNode.CodeBlock)
                    PrintNode(codeBlock, incident + 2);
            }
            else if (node is ASTFunction)
            {
                ASTFunction func = (ASTFunction)node;
                if(node is ASTConstructor)
                    sw.WriteLine("ASTConstructor:");
                else
                    sw.WriteLine("ASTFunction: " + func.ReturnValue.ToString() + " " + func.Name);
                for (int i = 0; i < incident; i++)
                    sw.Write("\t"); 
                sw.WriteLine("\tParameter:");
                foreach (ASTNode n in func.Parameter)
                    PrintNode(n, incident + 2);
                for (int i = 0; i < incident; i++)
                    sw.Write("\t"); 
                sw.WriteLine("\tCodeBlock:");
                foreach (ASTNode codeBlock in func.CodeBlock)
                    PrintNode(codeBlock, incident + 2);
            }
            else if (node is ASTFunctionParameter)
            {
                ASTFunctionParameter param = (ASTFunctionParameter)node;
                switch (param.Type)
                {
                    case VMType.Object:
                        sw.WriteLine("Object: " + param.Name);
                        break;
                    case VMType.String:
                        sw.WriteLine("String: " + param.Name);
                        break;
                    case VMType.Number:
                        sw.WriteLine("Number: " + param.Name);
                        break;
                }
            }
            else if (node is ASTLocalVariable)
            {
                ASTLocalVariable param = (ASTLocalVariable)node;
                switch (param.Type)
                {
                    case VMType.Object:
                        sw.WriteLine("var Object: " + param.Name);
                        break;
                    case VMType.String:
                        sw.WriteLine("var String: " + param.Name);
                        break;
                    case VMType.Number:
                        sw.WriteLine("var Number: " + param.Name);
                        break;
                }
            }
            else if (node is ASTReturn)
            {
                sw.WriteLine("ASTReturn:");
                PrintNode(((ASTReturn)node).Return, incident + 1);
            }
            else if (node is ASTClass)
            {
                ASTClass cNode = (ASTClass) node;
                sw.WriteLine("ASTClass: " + cNode.Name);
                sw.WriteLine("\tAttributes:");
                foreach (ASTNode n in cNode.Attributes)
                    PrintNode(n, incident + 2);
                sw.WriteLine("\tConstructors:");
                foreach (ASTNode n in cNode.Constructors)
                    PrintNode(n, incident + 2);
                sw.WriteLine("\tFunctions:");
                foreach (ASTNode n in cNode.Functions)
                    PrintNode(n, incident + 2);
            }
        }

        static void Main(string[] args)
        {
            StreamReader sr = File.OpenText("test.kds");
            string source = sr.ReadToEnd();
            sr.Close();
            if (File.Exists("log.txt"))
                File.Delete("log.txt");
            sw = new StreamWriter(File.OpenWrite("log.txt"));
            Compiler comp = new Compiler(source);

            sw.WriteLine("LEXER:\n");
            List<TokenBase> tokens = comp.Lex();
            foreach (TokenBase token in tokens)
            {
                if (token is IdentifierToken)
                    sw.WriteLine("Identifier: " + token.Token);
                else if (token is NumberToken)
                    sw.WriteLine("Number: " + double.Parse(token.Token, CultureInfo.GetCultureInfo("en")));
                else if (token is OperatorToken)
                    sw.WriteLine("Operator: " + token.Token);
                else if (token is ReservedWordToken)
                    sw.WriteLine("Reserved Word: " + token.Token);
                else if (token is StringToken)
                    sw.WriteLine("String: " + token.Token);
                else if (token is SpecialToken)
                    sw.WriteLine("Special: " + token.Token);
            }

            sw.WriteLine("\n\nParser:\n");
            List<ASTClass> ast = comp.Parse(tokens);
            foreach(ASTClass node in ast)
                PrintNode(node, 0);
            sw.Close();
            Console.WriteLine("DONE! Open log.txt to see the result.");
            Console.ReadLine();
        }
    }
}
