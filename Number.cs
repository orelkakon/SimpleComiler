using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCompiler
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    class Number : Token
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public int Value { get; set; }
        public Number(string name, int line, int position)
        {
            Line = line;
            Position = position;
            Value = int.Parse(name);
        }
        public override bool Equals(object obj)
        {
            if (obj is Number)
            {
                Number t = (Number)obj;
                if (Value == t.Value)
                    return base.Equals(t);
            }
            return false;
        }
        public override string ToString()
        {
            return Value + "";
        }


    }
}
