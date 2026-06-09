using QNums.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace QNums.Abstractions.Service
{
    public class CalculationService : ICalculationService
    {
        public double Evaluate(string expression)
        {
            try
            {
                return Convert.ToDouble(new DataTable().Compute(expression, null));
            }
            catch
            {
                return double.NaN;
            }
        }
    }
}
