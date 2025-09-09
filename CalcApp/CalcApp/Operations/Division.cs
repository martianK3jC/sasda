using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcApp.Core;
namespace CalcApp.Operations
{
    public class Division : Operation
    {
        public override double Execute(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Cannot divide by zero.");
            return a / b;
        }
    }
}
