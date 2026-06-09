using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using QEX_Lib.QEX_API.Abtractions.Interface;
using QNums.Abstractions.Interfaces;
using QNums.Abstractions.Service;
using QNums.Components.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Module
{
    internal class Module
    {
        public static void Register(IDynamicServiceRegistry registry)
        {
            registry.RegisterScoped<ICalculationService, CalculationService>();
        }
    }
}
