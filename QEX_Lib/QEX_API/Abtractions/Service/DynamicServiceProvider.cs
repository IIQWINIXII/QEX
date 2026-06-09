using Microsoft.Extensions.DependencyInjection;
using QEX_Lib.QEX_API.Abtractions.Interface;
using System;
using System.Collections.Generic;

namespace QEX_Lib.QEX_API.Abtractions.Service
{
    public class DynamicServiceProvider : IDynamicServiceRegistry, IPluginServiceProvider
    {
        private readonly IServiceProvider _fallback;
        private readonly Dictionary<Type, Func<IServiceProvider, object>> _factories = new();

        public DynamicServiceProvider(IServiceProvider fallback)
        {
            _fallback = fallback;
        }

        public void RegisterScoped<TService, TImplementation>()
            where TImplementation : TService
        {
            _factories[typeof(TService)] = sp =>
                ActivatorUtilities.CreateInstance<TImplementation>(_fallback);
        }

        public object? GetService(Type type)
        {
            if (_factories.TryGetValue(type, out var factory))
                return factory(_fallback);

            return null;
        }

        public T? GetService<T>() where T : class
            => GetService(typeof(T)) as T;
    }
}
