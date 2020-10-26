﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCompiler
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    class VarType : Keyword
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public VarType(string name, int line, int position)
        {
            Line = line;
            Position = position;
            Name = name;
        }
        public override bool Equals(object obj)
        {
            if (obj is VarType)
            {
                return base.Equals(obj);
            }
            return false;
        }


    }
}
