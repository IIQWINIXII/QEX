using System;
using System.Collections.Generic;
using System.Text;

namespace QEX_Lib.QEX_API.Abtractions.Service
{
    public class CombinedServiceProvider : IServiceProvider
    {
        private readonly IServiceProvider _maui;
        private readonly DynamicServiceProvider _dynamic;

        public CombinedServiceProvider(IServiceProvider maui, DynamicServiceProvider dynamic)
        {
            _maui = maui;
            _dynamic = dynamic;
        }

        public object? GetService(Type serviceType)
        {
            var dyn = _dynamic.GetService(serviceType);
            if (dyn != null)
                return dyn;

            return _maui.GetService(serviceType);
        }
    }


}
