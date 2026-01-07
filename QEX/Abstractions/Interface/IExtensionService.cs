using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace QEX.Abstractions.Interface
{
    public interface IExtensionService
    {
        Type? CurrentComponentType { get; }
        Dictionary<string, object>? CurrentParameters { get; }
        bool IsVisible { get; }

        /// <summary>
        /// Событие открытия расширения
        /// </summary>
        event Action<Type, Dictionary<string, object>> OnOpenExtension;

        /// <summary>
        /// Событие закрытия расширения
        /// </summary>
        event Action OnCloseExtension;

        /// <summary>
        /// Открыть расширение с указанным компонентом
        /// </summary>
        /// <typeparam name="TComponent">Тип компонента Blazor</typeparam>
        /// <param name="Parameters">Параметры для компонента</param>
        void OpenExtension<TComponent>(Dictionary<string, object>? Parameters = null) 
            where TComponent : ComponentBase;

        /// <summary>
        /// Открыть раширение с указанным типом компонента
        /// </summary>
        void OpenExtension(Type ComponentType, Dictionary<string, object>? Parameters = null);

        /// <summary>
        /// Закрыть текущее расширение
        /// </summary>
        void CloseExtension();
    }
}
