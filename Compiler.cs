using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class Compiler
    {


        public Compiler()
        {

        }


        public List<VarDeclaration> ParseVarDeclarations(List<string> lVarLines)
        {
            List<VarDeclaration> lVars = new List<VarDeclaration>();
            for (int i = 0; i < lVarLines.Count; i++)
            {
                List<Token> lTokens = Tokenize(lVarLines[i], i);
                TokensStack stack = new TokensStack(lTokens);
                VarDeclaration var = new VarDeclaration();
                var.Parse(stack);
                lVars.Add(var);
            }
            return lVars;
        }


        public List<LetStatement> ParseAssignments(List<string> lLines)
        {
            List<LetStatement> lParsed = new List<LetStatement>();
            List<Token> lTokens = Tokenize(lLines);
            TokensStack sTokens = new TokensStack();
            for (int i = lTokens.Count - 1; i >= 0; i--)
                sTokens.Push(lTokens[i]);
            while (sTokens.Count > 0)
            {
                LetStatement ls = new LetStatement();
                ls.Parse(sTokens);
                lParsed.Add(ls);

            }
            return lParsed;
        }



        public List<string> GenerateCode(LetStatement aSimple, Dictionary<string, int> dSymbolTable)
        {
            List<string> lAssembly = new List<string>();
            if (!dSymbolTable.ContainsKey(aSimple.Variable))
                throw new SyntaxErrorException("the var isn't exist", new Token());
            if(aSimple.Value is BinaryOperationExpression)
            {
                //case 1: number,number-
                if( AllIsNumeric(((BinaryOperationExpression)aSimple.Value).Operand1+"") && AllIsNumeric(((BinaryOperationExpression)aSimple.Value).Operand2 + ""))
                {
                    CopyToOperand1(((BinaryOperationExpression)aSimple.Value).Operand1 + "", lAssembly);
                    CopyToOperand2(((BinaryOperationExpression)aSimple.Value).Operand2 + "", lAssembly);
                    lAssembly.Add("D=M");
                    lAssembly.Add("@OPERAND1");
                    if (((BinaryOperationExpression)aSimple.Value).Operator == "+")
                    {
                        lAssembly.Add("D=D+M");
                    }
                    else
                    {
                        lAssembly.Add("D=M-D");
                    }
                    lAssembly.Add("@RESULT");
                    lAssembly.Add("M=D");
                }
                //case 2: variable,number-
                if(AllStringIsChars((((BinaryOperationExpression)aSimple.Value).Operand1 + "")[0]+"")&&AllStringIsCharsOr_OrNumber((((BinaryOperationExpression)aSimple.Value).Operand1 + "").Remove(0,1)) && AllIsNumeric(((BinaryOperationExpression)aSimple.Value).Operand2 + ""))
                {
                    CopyToOperand1(((BinaryOperationExpression)aSimple.Value).Operand1 + "", lAssembly);
                    CopyVarToOperand2(((BinaryOperationExpression)aSimple.Value).Operand2 + "", dSymbolTable, lAssembly);
                    lAssembly.Add("D=M");
                    lAssembly.Add("@OPERAND1");
                    if (((BinaryOperationExpression)aSimple.Value).Operator == "+")
                    {
                        lAssembly.Add("D=D+M");
                    }
                    else
                    {
                        lAssembly.Add("D=M-D");
                    }
                    lAssembly.Add("@RESULT");
                    lAssembly.Add("M=D");
                }
                //case 3: number,variable-
                if (AllStringIsChars((((BinaryOperationExpression)aSimple.Value).Operand2 + "")[0] + "") && AllStringIsCharsOr_OrNumber((((BinaryOperationExpression)aSimple.Value).Operand2 + "").Remove(0, 1)) && AllIsNumeric(((BinaryOperationExpression)aSimple.Value).Operand1 + ""))
                {
                    CopyVarToOperand1(((BinaryOperationExpression)aSimple.Value).Operand1 + "", dSymbolTable, lAssembly);
                    CopyToOperand2(((BinaryOperationExpression)aSimple.Value).Operand2 + "", lAssembly);
                    lAssembly.Add("D=M");
                    lAssembly.Add("@OPERAND1");
                    if (((BinaryOperationExpression)aSimple.Value).Operator == "+")
                    {
                        lAssembly.Add("D=D+M");
                    }
                    else
                    {
                        lAssembly.Add("D=M-D");
                    }
                    lAssembly.Add("@RESULT");
                    lAssembly.Add("M=D");
                }
                //case 4: variable,variable-
                if (AllStringIsChars((((BinaryOperationExpression)aSimple.Value).Operand2 + "")[0] + "") && AllStringIsCharsOr_OrNumber((((BinaryOperationExpression)aSimple.Value).Operand2 + "").Remove(0, 1)) && AllStringIsChars((((BinaryOperationExpression)aSimple.Value).Operand1 + "")[0] + "") && AllStringIsCharsOr_OrNumber((((BinaryOperationExpression)aSimple.Value).Operand1 + "").Remove(0, 1)))
                {
                    CopyVarToOperand1(((BinaryOperationExpression)aSimple.Value).Operand1 + "", dSymbolTable, lAssembly);
                    CopyVarToOperand2(((BinaryOperationExpression)aSimple.Value).Operand2 + "", dSymbolTable, lAssembly);
                    lAssembly.Add("D=M");
                    lAssembly.Add("@OPERAND1");
                    if (((BinaryOperationExpression)aSimple.Value).Operator == "+")
                    {
                        lAssembly.Add("D=D+M");
                    }
                    else
                    {
                        lAssembly.Add("D=M-D");
                    }
                    lAssembly.Add("@RESULT");
                    lAssembly.Add("M=D");
                }
            }

            return lAssembly;
        }
        private void CopyToOperand1(string num, List<string> lAssembly)
        {
            lAssembly.Add("@" + num);
            lAssembly.Add("D=A");
            lAssembly.Add("@OPERAND1");
            lAssembly.Add("M=D");
        }
        private void CopyToOperand2(string num, List<string> lAssembly)
        {
            lAssembly.Add("@" + num);
            lAssembly.Add("D=A");
            lAssembly.Add("@OPERAND2");
            lAssembly.Add("M=D");
        }
        private void CopyVarToOperand1(string operand, Dictionary<string, int> dSymbolTable, List<string> lAssembly)
        {

            lAssembly.Add("@" + dSymbolTable[operand]);
            lAssembly.Add("D=A");
            lAssembly.Add("@" + "LCL");
            lAssembly.Add("D=M+D");
            lAssembly.Add("@ADDRESS");
            lAssembly.Add("M=D");

            lAssembly.Add("A=M");
            lAssembly.Add("D=M");
            lAssembly.Add("@OPERAND1");
            lAssembly.Add("M=D");
        }
        private void CopyVarToOperand2(string operand, Dictionary<string, int> dSymbolTable, List<string> lAssembly)
        {

            lAssembly.Add("@" + dSymbolTable[operand]);
            lAssembly.Add("D=A");
            lAssembly.Add("@" + "LCL");
            lAssembly.Add("D=M+D");
            lAssembly.Add("@ADDRESS");
            lAssembly.Add("M=D");

            lAssembly.Add("A=M");
            lAssembly.Add("D=M");
            lAssembly.Add("@OPERAND2");
            lAssembly.Add("M=D");
        }
        public Dictionary<string, int> ComputeSymbolTable(List<VarDeclaration> lDeclerations)
        {
            Dictionary<string, int> dTable = new Dictionary<string, int>();
            List<string> temp = new List<string>();
            int counter = 0;
            for (int i = 0; i < lDeclerations.Count; i++)
            {
                if (dTable.ContainsKey(lDeclerations[i].Name) || temp.Contains(lDeclerations[i].Name))
                {
                    //throw new SyntaxErrorException("same name is defined more than once", new Token());
                }
                else
                {
                    if (lDeclerations[i].Name[0].Equals("_"))
                    {
                        temp.Add(lDeclerations[i].Name);
                    }
                    else
                    {
                        dTable.Add(lDeclerations[i].Name, counter);
                        counter++;
                    }
                }
            }
            for (int x = 0; x < temp.Count; x++)
            {
                dTable.Add(temp[x], counter);
                counter++;
            }

            //add here code to comptue a symbol table for the given var declarations
            //real vars should come before (lower indexes) than artificial vars (starting with _), and their indexes must be by order of appearance.
            //for example, given the declarations:
            //var int x;
            //var int _1;
            //var int y;
            //the resulting table should be x=0,y=1,_1=2
            //throw an exception if a var with the same name is defined more than once
            return dTable;
        }


        public List<string> GenerateCode(List<LetStatement> lSimpleAssignments, List<VarDeclaration> lVars)
        {
            List<string> lAssembly = new List<string>();
            Dictionary<string, int> dSymbolTable = ComputeSymbolTable(lVars);
            foreach (LetStatement aSimple in lSimpleAssignments)
                lAssembly.AddRange(GenerateCode(aSimple, dSymbolTable));
            return lAssembly;
        }

        public List<LetStatement> SimplifyExpressions(LetStatement s, List<VarDeclaration> lVars)
        {
            //add here code to simply expressins in a statement. 
            List<LetStatement> lState1 = new List<LetStatement>();
            if (s.Value is BinaryOperationExpression)
            {
                Simplfy(lState1,1,s,lVars);
            }
            else
            {
                lState1.Add(s);
            }
            return lState1;

                //add var declarations for artificial variables
        }

        private void Simplfy(List<LetStatement> lState1, int funcName, LetStatement s, List<VarDeclaration> lVars)
        {
            bool Op = false;
            if (((BinaryOperationExpression)s.Value).Operand1 is BinaryOperationExpression)
            {
                Op = true;
                LetStatement x = new LetStatement();
                x.Value = ((BinaryOperationExpression)s.Value).Operand1;
                x.Variable = "_" + funcName;
                VarDeclaration vd = new VarDeclaration("int", x.Variable);
                lVars.Add(vd);
                funcName++;
                Simplfy(lState1, funcName, x, lVars);
            }
            if (((BinaryOperationExpression)s.Value).Operand2 is BinaryOperationExpression)
            {
                Op = true;
                LetStatement y = new LetStatement();
                y.Value = ((BinaryOperationExpression)s.Value).Operand2;
                y.Variable = "_" + funcName;
                VarDeclaration vd = new VarDeclaration("int", y.Variable);
                lVars.Add(vd);
                funcName++;
                Simplfy(lState1, funcName, y, lVars);
            }
            if (!Op)
            {
                lState1.Add(s);
            }


        }
        public List<LetStatement> SimplifyExpressions(List<LetStatement> ls, List<VarDeclaration> lVars)
        {
            List<LetStatement> lSimplified = new List<LetStatement>();
            foreach (LetStatement s in ls)
                lSimplified.AddRange(SimplifyExpressions(s, lVars));
            return lSimplified;
        }

 
        public LetStatement ParseStatement(List<Token> lTokens)
        {
            TokensStack sTokens = new TokensStack();
            for (int i = lTokens.Count - 1; i >= 0; i--)
                sTokens.Push(lTokens[i]);
            LetStatement s = new LetStatement();
            s.Parse(sTokens);
            return s;
        }
        private bool Contains(string[] a, string s)
        {
            foreach (string s1 in a)
                if (s1 == s)
                    return true;
            return false;
        }
        private bool Contains(char[] a, char c)
        {
            foreach (char c1 in a)
                if (c1 == c)
                    return true;
            return false;
        }
        private List<string> Split(string s, char[] aDelimiters)
        {
            List<string> lTokens = new List<string>();
            while (s.Length > 0)
            {
                string sToken = "";
                int i = 0;
                for (i = 0; i < s.Length; i++)
                {
                    if (Contains(aDelimiters, s[i]))
                    {
                        if (sToken.Length > 0)
                            lTokens.Add(sToken);
                        lTokens.Add(s[i] + "");
                        break;
                    }
                    else
                        sToken += s[i];
                }
                if (i == s.Length)
                {
                    lTokens.Add(sToken);
                    s = "";
                }
                else
                    s = s.Substring(i + 1);
            }
            return lTokens;
        }


        public List<Token> Tokenize(string sLine, int iLine)
        {
            List<Token> lTokens = new List<Token>(); 
            char[] allSymbols = { '*', '+', '-', '/', '<', '>', '&', '=', '|', '~', '(', ')', '[', ']', '{', '}', ',', ';', ' ', '\t' };
            char[] specialSymbols = { ' ', '\t' };

            if (sLine.Contains("//"))
            {
                string x = sLine.Remove(sLine.IndexOf('/'));
                sLine = x;
            }

            List<string> AfterSplit = Split(sLine, allSymbols);
            int indexer = 0;
            for (int j = 0; j < AfterSplit.Count; j++)
            {
                if (IsNumeric(AfterSplit[j][0]) && AllStringIsChars(AfterSplit[j].Substring(1)))
                {
                    throw new SyntaxErrorException("not AlphaNumeric expression", new Token());
                }
                if (!InvalidChars(AfterSplit[j]) && !ValidChars(AfterSplit[j]) && !ValidType(AfterSplit[j]))
                {
                    throw new SyntaxErrorException("char not legal", new Token());
                }
                if (Token.Statements.Contains(AfterSplit[j]))
                {
                    lTokens.Add(new Statement(AfterSplit[j],iLine, indexer));
                    indexer = indexer + AfterSplit[j].Length;
                    continue;
                }
                if (Token.VarTypes.Contains(AfterSplit[j]))
                {
                    lTokens.Add(new VarType(AfterSplit[j], iLine, indexer));
                    indexer = indexer + AfterSplit[j].Length;
                    continue;
                }
                if (Token.Constants.Contains(AfterSplit[j]))
                {
                    lTokens.Add(new Constant(AfterSplit[j], iLine, indexer));
                    indexer = indexer + AfterSplit[j].Length;
                    continue;
                }
                try
                {
                    int num = Int32.Parse(AfterSplit[j]);
                    lTokens.Add(new Number(num + "", iLine, indexer));
                    indexer = indexer + (num + "").Length;
                    continue;
                }
                catch (Exception e)
                {

                }
                if (AfterSplit[j].Length == 1)
                {
                    char nowChar = AfterSplit[j][0];
                    if (Token.Operators.Contains(nowChar))
                    {
                        lTokens.Add(new Operator(nowChar, iLine, indexer));
                        indexer = indexer + 1;
                        continue;
                    }
                    if (Token.Parentheses.Contains(nowChar))
                    {
                        lTokens.Add(new Parentheses(nowChar, iLine, indexer));
                        indexer = indexer + 1;
                        continue;
                    }
                    if (Token.Separators.Contains(nowChar))
                    {
                        lTokens.Add(new Separator(nowChar, iLine, indexer));
                        indexer = indexer + 1;
                        continue;
                    }
                    if (specialSymbols.Contains(nowChar))
                    {
                        indexer = indexer + 1;
                        continue;
                    }
                    if (AllStringIsChars(nowChar + ""))
                    {
                        lTokens.Add(new Identifier(nowChar + "", iLine, indexer));
                        indexer = indexer + 1;
                        continue;
                    }
                }
                if (AllStringIsCharsOr_OrNumber(AfterSplit[j].Substring(1)) && AllStringIsChars(AfterSplit[j][0] + ""))
                {
                    lTokens.Add(new Identifier(AfterSplit[j], iLine, indexer));
                    indexer = indexer + AfterSplit[j].Length;
                    continue;
                }
            }

            return lTokens;
        }

        public List<Token> Tokenize(List<string> lCodeLines)
        {
            List<Token> lTokens = new List<Token>();
            for (int i = 0; i < lCodeLines.Count; i++)
            {
                string sLine = lCodeLines[i];
                List<Token> lLineTokens = Tokenize(sLine, i);
                lTokens.AddRange(lLineTokens);
            }
            return lTokens;
        }
        private bool AllStringIsCharsOr_OrNumber(string x)
        {
            return Regex.IsMatch(x, @"^[a-zA-Z0-9_]+$");
        }
        private bool AllStringIsChars(string x)
        {
            return Regex.IsMatch(x, @"^[a-zA-Z]+$");
        }
        private bool InvalidChars(string x)
        {
            return Regex.IsMatch(x, @"^[a-zA-Z0-9_]+$");
        }
        private bool ValidChars(string x)
        {
            char[] allSymbols = { '*', '+', '-', '/', '<', '>', '&', '=', '|', '~', '(', ')', '[', ']', '{', '}', ',', ';', ' ', '\t' };
            for (int i = 0; i < x.Length; i++)
            {
                if (!allSymbols.Contains(x[i]))
                {
                    return false;
                }
            }
            return true;
        }
        private bool ValidType(string x)
        {
            string[] listWord = { "function", "var", "let", "while", "if", "else", "return", "int", "char", "boolean", "array", "true", "false", "null" };
            for (int i = 0; i < x.Length; i++)
            {
                if (!listWord.Contains(x))
                {
                    return false;
                }
            }
            return true;
        }
        private bool IsNumeric(char x)
        {
            return Regex.IsMatch(x + "", @"^[0-9]+$");
        }
        private bool AllIsNumeric(string x)
        {
            for(int i = 0; i < x.Length; i++)
            {
                if (!IsNumeric(x[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }

}    