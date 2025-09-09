using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcApp.Core
{
    public interface IOperation
    {
        double Execute(double a, double b);
    }
}
