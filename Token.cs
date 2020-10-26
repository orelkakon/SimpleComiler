using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCompiler
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Token
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public static string[] Statements = { "function", "var", "let", "while", "if", "else", "return" };
        public static string[] VarTypes = { "int", "char", "boolean", "array" };
        public static string[] Constants = { "true", "false", "null" };
        public static char[] Operators = new char[] { '*', '+', '-', '/', '<', '>', '&', '=', '|', '~' };
        public static char[] Parentheses = new char[] { '(', ')', '[', ']', '{', '}' };
        public static char[] Separators = new char[] {  ',', ';'};


        public int Line { get; set; }
        public int Position { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is Token)
            {
                Token t = (Token)obj;
                return t.Line == Line && t.Position == Position;
            }
            return false;
        }

    }
}
