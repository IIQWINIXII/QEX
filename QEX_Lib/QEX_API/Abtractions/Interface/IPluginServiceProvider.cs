using System;
using System.Collections.Generic;
using System.Text;

namespace QEX_Lib.QEX_API.Abtractions.Interface
{
    public interface IPluginServiceProvider
    {
        T? GetService<T>() where T : class;
        object? GetService(Type type);
    }
}
