﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class NumericExpression : Expression
    {
        public int Value;

        public override void Parse(TokensStack sTokens)
        {
            Token tNum = sTokens.Pop();
            if (tNum is Number)
                Value = ((Number)tNum).Value;
            ToString();
        }

        public override string ToString()
        {
            return Value + "";
        }
    }
}
