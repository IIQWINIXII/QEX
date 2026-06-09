using System;
using System.Collections.Generic;
using System.Text;

namespace QNums.Abstractions.Interfaces
{
    public interface ICalculationService
    {
        double Evaluate(string expression);
    }
}
