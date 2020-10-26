using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCompiler
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    class Identifier : Token
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public string Name { get; set; }
        public Identifier(string name, int line, int position)
        {
            Line = line;
            Position = position;
            Name = name;


            //you can add code here to identify invalid identifiers and throw an exception
           
            



        }
        public override bool Equals(object obj)
        {
            if (obj is Identifier)
            {
                Identifier t = (Identifier)obj;
                if (Name == t.Name)
                    return base.Equals(t);
            }
            return false;
        }
        public override string ToString()
        {
            return Name;
        }

       
       
    }
}
