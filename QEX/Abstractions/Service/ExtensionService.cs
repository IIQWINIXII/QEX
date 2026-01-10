using Microsoft.AspNetCore.Components;
using QEX.Abstractions.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEX.Abstractions.Service
{
    public class ExtensionService : IExtensionService
    {

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

        public void OpenExtension<TComponent>(Dictionary<string, object>? Parameters = null) where TComponent : ComponentBase
        {
            OpenExtension(typeof(TComponent), Parameters);
        }

        public void OpenExtension(Type ComponentType, Dictionary<string, object>? Parameters = null)
        {
            CurrentComponentType = ComponentType ?? throw new("Type is not ComponentBase");
            CurrentParameters = Parameters ?? [];
            IsVisible = true;

            OnOpenExtension?.Invoke(ComponentType, CurrentParameters);
        }
    }
}

