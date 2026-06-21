using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace QEX.Abstractions.Interface
{
    public interface IExtensionService
    {
        Type? CurrentComponentType { get; }
        Dictionary<string, object>? CurrentParameters { get; }
        bool IsVisible { get; }

        event Action<Type, Dictionary<string, object>> OnOpenExtension;
        event Action<Type, Dictionary<string, object>> OnOpenExtensionInNewWindow;
        event Action OnCloseExtension;

        void OpenExtension<TComponent>(Dictionary<string, object>? parameters = null)
            where TComponent : ComponentBase;

        void OpenExtension(Type componentType, Dictionary<string, object>? parameters = null);
        void OpenExtensionByName(string name, Dictionary<string, object>? parameters = null);
        public void OpenExtensionWindowByName(string name, Dictionary<string, object>? parameters = null);
        void OpenExtensionWindow(Type componentType, Dictionary<string, object>? parameters = null);
        void CloseExtension();

        void RegisterExtension(Type componentType, string tag);
    }
}
