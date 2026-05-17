using Microsoft.AspNetCore.Components;
using QEX.Abstractions.Interface;
using System;
using System.Collections.Generic;

namespace QEX.Abstractions.Service
{
    public class ExtensionService : IExtensionService
    {
        private readonly Dictionary<string, Type> _extensions = new();

        public Type? CurrentComponentType { get; private set; }
        public Dictionary<string, object>? CurrentParameters { get; private set; }
        public bool IsVisible { get; private set; }

        public event Action<Type, Dictionary<string, object>>? OnOpenExtension;
        public event Action? OnCloseExtension;

        public void CloseExtension()
        {
            CurrentComponentType = null;
            CurrentParameters = null;
            IsVisible = false;
            OnCloseExtension?.Invoke();
        }

        public void OpenExtension<TComponent>(Dictionary<string, object>? parameters = null)
            where TComponent : ComponentBase
            => OpenExtension(typeof(TComponent), parameters);

        public void OpenExtension(Type componentType, Dictionary<string, object>? parameters = null)
        {
            if (!typeof(ComponentBase).IsAssignableFrom(componentType))
                throw new("Type is not ComponentBase");

            CurrentComponentType = componentType;
            CurrentParameters = parameters ?? new();
            IsVisible = true;

            OnOpenExtension?.Invoke(componentType, CurrentParameters);
        }

        public void OpenExtensionByName(string name, Dictionary<string, object>? parameters)
        {
            if (!_extensions.TryGetValue(name, out var type))
                throw new Exception($"Extension '{name}' not registered.");

            OpenExtension(type, parameters);
        }

        public void RegisterExtension(Type componentType, string tag)
        {
            _extensions[tag] = componentType;
        }
    }
}
