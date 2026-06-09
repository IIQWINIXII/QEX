using System;
using System.Collections.Generic;
using System.Text;

namespace QEX_Lib.QEX_API.Abtractions.Interface
{
    public interface IDynamicServiceRegistry
    {
        void RegisterScoped<TService, TImplementation>()
            where TImplementation : TService;
    }
}
