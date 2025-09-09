using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcApp.Core;

namespace CalcApp.Operations
{
    public class SquareRoot : Operation
    {
        public override double Execute(double a, double b = 0) => Math.Sqrt(a);
    }
}
