using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcApp.Core;

namespace CalcApp.Operations
{
    public class Addition : Operation
    {
        public override double Execute(double a, double b) => a + b;
    }
}
