using Microsoft.JSInterop;
using System.Reflection;
using System.Text;

public static class JsModuleLoader
{
    public static async Task<IJSObjectReference> LoadAsync(
        IJSRuntime runtime,
        Assembly assembly,
        string resourceNameEnding)
    {
        // 1. Находим embedded resource по окончанию имени
        var resourceName = assembly.GetManifestResourceNames()
            .FirstOrDefault(n => n.EndsWith(resourceNameEnding, StringComparison.OrdinalIgnoreCase))
            ?? throw new FileNotFoundException(
                $"Embedded resource '{resourceNameEnding}' not found in {assembly.FullName}. " +
                $"Available: {string.Join(", ", assembly.GetManifestResourceNames())}");

        // 2. Читаем текст ресурса как строку
        await using var stream = assembly.GetManifestResourceStream(resourceName)!;
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var jsCode = await reader.ReadToEndAsync();

        // 3. Создаём Blob URL через JS-хелпер из index.html
        var url = await runtime.InvokeAsync<string>("qex.createModuleUrl", jsCode);

        // 4. Импортируем модуль по Blob URL
        var module = await runtime.InvokeAsync<IJSObjectReference>("import", url);

        // 5. Освобождаем Blob — модуль уже в памяти
        try { await runtime.InvokeVoidAsync("qex.revokeModuleUrl", url); } catch { }

        return module;
    }
}