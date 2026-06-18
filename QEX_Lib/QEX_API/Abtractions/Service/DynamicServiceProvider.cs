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
        private readonly Dictionary<Type, object> _singletons = new();

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
            // 1. Сначала проверяем — есть ли уже закешированный singleton
            if (_singletons.TryGetValue(type, out var instance))
                return instance;

            // 2. Если нет — создаём новый экземпляр через фабрику
            if (_factories.TryGetValue(type, out var factory))
                return factory(_fallback);

            // 3. Ничего нет — возвращаем null
            return null;
        }

        public T? GetService<T>() where T : class
            => GetService(typeof(T)) as T;

        /// <summary>
        /// Создаёт экземпляр через фабрику и кеширует его как singleton.
        /// Все последующие GetService вернут тот же объект.
        /// Если фабрика не зарегистрирована — выбрасывает InvalidOperationException.
        /// </summary>
        public T? SetService<T>() where T : class
        {
            var type = typeof(T);
            return SetService(type) as T;
        }

        /// <summary>
        /// Создаёт экземпляр через фабрику и кеширует его как singleton.
        /// Если singleton уже есть — возвращает существующий, не создавая новый.
        /// </summary>
        public object SetService(Type type)
        {
            // Если уже закеширован — возвращаем существующий
            if (_singletons.TryGetValue(type, out var existing))
                return existing;

            if (!_factories.TryGetValue(type, out var factory))
                throw new InvalidOperationException($"Service of type '{type.Name}' is not registered.");

            var instance = factory(_fallback);
            _singletons[type] = instance;
            return instance;
        }
    }
}
