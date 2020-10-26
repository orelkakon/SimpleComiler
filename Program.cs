﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    class Program
    {

        static void InitLCL(List<string> lAssembly)
        {
            lAssembly.Insert(0, "@20");
            lAssembly.Insert(1, "D=A");
            lAssembly.Insert(2, "@LCL");
            lAssembly.Insert(3, "M=D");

        }
        static void Test1()
        {
            Compiler c = new Compiler();
            List<string> lVars = new List<string>();
            lVars.Add("var int x;");
            List<VarDeclaration> vars = c.ParseVarDeclarations(lVars);

            string s = "let x = 5;";
            List<Token> lTokens = c.Tokenize(s, 0);
            LetStatement assignment = c.ParseStatement(lTokens);
            if(assignment.ToString() != s)
                Console.WriteLine("BUGBUG");


            List<LetStatement> l = new List<LetStatement>();
            l.Add(assignment);
            List<string> lAssembly = c.GenerateCode(l, vars);
            CPUEmulator cpu = new CPUEmulator();
            InitLCL(lAssembly);
            cpu.Code = lAssembly;
            cpu.Run(1000, false);
            if (cpu.M[20] != 5)
                Console.WriteLine("BUGBUG");
        }

        static void Test2()
        {
            Compiler c = new Compiler();
            List<string> lVars = new List<string>();
            lVars.Add("var int x;");
            lVars.Add("var int y;");
            lVars.Add("var int z;");
            List<VarDeclaration> vars = c.ParseVarDeclarations(lVars);

            List<string> lAssignments = new List<string>();
            lAssignments.Add("let x = 10;");
            lAssignments.Add("let y = 15;");
            lAssignments.Add("let z = (+ x y);");

            List<LetStatement> ls = c.ParseAssignments(lAssignments);


            List<string> lAssembly = c.GenerateCode(ls, vars);
            CPUEmulator cpu = new CPUEmulator();
            InitLCL(lAssembly);
            cpu.Code = lAssembly;
            cpu.Run(1000, false);
            if (cpu.M[22] != 25)
                Console.WriteLine("BUGBUG");
        }
        static void Test3()
        {
            Compiler c = new Compiler();
            List<string> lVars = new List<string>();
            lVars.Add("var int x;");
            lVars.Add("var int y;");
            lVars.Add("var int z;");
            List<VarDeclaration> vars = c.ParseVarDeclarations(lVars);

            string s = "let x = (+ (+ x 5) (- y z));";
            List<Token> lTokens = c.Tokenize(s,0);
            LetStatement assignment = c.ParseStatement(lTokens);

            List<LetStatement> lSimple = c.SimplifyExpressions(assignment, vars);
            List<string> lAssembly = c.GenerateCode(lSimple, vars);

            CPUEmulator cpu = new CPUEmulator();
            InitLCL(lAssembly);
            cpu.Code = lAssembly;
            cpu.Run(1000, false);
            if (cpu.M[20] != 5)
                Console.WriteLine("BGUBGU");
        }

        static void Test4()
        {
            Compiler c = new Compiler();

            List<string> lVars = new List<string>();
            lVars.Add("var int x;");
            lVars.Add("var int y;");
            lVars.Add("var int z;");
            List<VarDeclaration> vars = c.ParseVarDeclarations(lVars);

            List<string> lAssignments = new List<string>();
            lAssignments.Add("let x = (+ (- 53 12) (- 467 3));");
            lAssignments.Add("let y = 3;");
            lAssignments.Add("let z = (+ (- x 12) (+ y 3));");

            List<LetStatement> ls = c.ParseAssignments(lAssignments);
            Dictionary<string, int> dValues = new Dictionary<string, int>();
            dValues["x"] = 0;
            dValues["y"] = 0;
            dValues["z"] = 0;

            CPUEmulator cpu = new CPUEmulator();
            cpu.Compute(ls, dValues);

            List<LetStatement> lSimple = c.SimplifyExpressions(ls, vars);

            Dictionary<string, int> dValues2 = new Dictionary<string, int>();
            dValues2["x"] = 0;
            dValues2["y"] = 0;
            dValues2["z"] = 0;

            cpu.Compute(lSimple, dValues2);

            foreach (string sKey in dValues.Keys)
                if (dValues[sKey] != dValues2[sKey])
                    Console.WriteLine("BGUBGU");

            List<string> lAssembly = c.GenerateCode(lSimple, vars);

            InitLCL(lAssembly);
            cpu.Code = lAssembly;
            cpu.Run(1000, false);
            if (cpu.M[20] != dValues2["x"])
                Console.WriteLine("BGUBGU");

        }




        static void Main(string[] args)
        {
            Test1();
            Test2();
            Test3();
            Test4();
            //TestParseAndErrors();
        }

        public static void WriteResults(string sMsgs, int cErrors)
        {
            bool bDone = false;
            while (!bDone)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter("results.txt"))
                    {
                        sw.WriteLine(sMsgs + ", errors = " + cErrors);
                        Console.WriteLine(sMsgs + ", errors = " + cErrors);
                        bDone = true;
                    }
                }
                catch (Exception e)
                {

                }
            }
        }
 
        public static string GetName(Token t)
        {
            if (t is Identifier)
            {
                return ((Identifier)t).Name;
            }
            if (t is Keyword)
            {
                return ((Keyword)t).Name;
            }
            if (t is Symbol)
            {
                return ((Symbol)t).Name + "";
            }
            if (t is Number)
            {
                return ((Number)t).Value +"";
            }
            return "";
        }

     }
}
