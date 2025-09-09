using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcApp.Core
{
    public abstract class Operation : IOperation
    {
        public abstract double Execute(double a, double b);
    }
}
