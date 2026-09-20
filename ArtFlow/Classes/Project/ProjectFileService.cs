using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace ArtFlow.Classes.Project;

/// <summary>
/// Сохраняет/загружает .artflow (ZIP с project.json, preview.png и PNG-слоями).
/// </summary>
public static class ProjectFileService
{
    public const string ProjectJsonEntry = "project.json";
    public const string PreviewEntry = "preview.png";

    // ---------- SAVE ----------

    public static async Task SaveAsync(
        Stream output,
        ArtFlowProject project,
        IDictionary<string, byte[]> layerPngBytes,
        byte[]? previewPng = null,
        CancellationToken ct = default)
    {
        project.Modified = DateTime.UtcNow;

        using var zip = new ZipArchive(output, ZipArchiveMode.Create, leaveOpen: true);

        // 1. project.json
        var jsonEntry = zip.CreateEntry(ProjectJsonEntry, CompressionLevel.Optimal);
        await using (var s = jsonEntry.Open())
        await using (var w = new StreamWriter(s, new UTF8Encoding(false)))
        {
            var json = JsonSerializer.Serialize(project, ArtFlowProject.JsonOptions);
            await w.WriteAsync(json.AsMemory(), ct);
        }

        // 2. preview.png
        if (previewPng is { Length: > 0 })
        {
            var pv = zip.CreateEntry(PreviewEntry, CompressionLevel.Optimal);
            await using var s = pv.Open();
            await s.WriteAsync(previewPng, ct);
        }

        // 3. PNG каждого слоя
        foreach (var layer in project.Layers)
        {
            if (!layerPngBytes.TryGetValue(layer.Id, out var png) || png.Length == 0)
                continue;

            var entryPath = layer.File;
            if (string.IsNullOrEmpty(entryPath))
            {
                entryPath = $"layers/{layer.Id}.png";
                layer.File = entryPath;
            }

            var entry = zip.CreateEntry(entryPath, CompressionLevel.Optimal);
            await using var s = entry.Open();
            await s.WriteAsync(png, ct);
        }
    }

    // ---------- LOAD ----------

    public sealed class LoadResult
    {
        public ArtFlowProject Project { get; init; } = new();
        public Dictionary<string, byte[]> LayerPngBytes { get; init; } = new();
        public byte[]? PreviewPng { get; init; }
    }

    public static async Task<LoadResult> LoadAsync(
        Stream input,
        CancellationToken ct = default)
    {
        using var zip = new ZipArchive(input, ZipArchiveMode.Read, leaveOpen: true);

        // 1. project.json
        var jsonEntry = zip.GetEntry(ProjectJsonEntry)
            ?? throw new InvalidDataException(
                $"Файл не является .artflow: отсутствует {ProjectJsonEntry}");

        ArtFlowProject project;
        await using (var s = jsonEntry.Open())
        {
            project = await JsonSerializer.DeserializeAsync<ArtFlowProject>(
                s, ArtFlowProject.JsonOptions, ct)
                ?? throw new InvalidDataException("project.json пуст или повреждён");
        }

        if (!string.Equals(project.Format, ArtFlowProject.FormatId, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException($"Неизвестный формат: {project.Format}");

        if (project.Version > ArtFlowProject.CurrentVersion)
            throw new InvalidDataException(
                $"Файл создан в более новой версии ArtFlow " +
                $"(v{project.Version} > v{ArtFlowProject.CurrentVersion}). " +
                $"Обновите приложение.");

        // 2. Слои
        var layerBytes = new Dictionary<string, byte[]>();
        foreach (var layer in project.Layers)
        {
            if (string.IsNullOrEmpty(layer.File)) continue;

            var e = zip.GetEntry(layer.File);
            if (e is null) continue;

            await using var s = e.Open();
            using var ms = new MemoryStream();
            await s.CopyToAsync(ms, ct);
            layerBytes[layer.Id] = ms.ToArray();
        }

        // 3. Превью
        byte[]? previewPng = null;
        var pv = zip.GetEntry(PreviewEntry);
        if (pv is not null)
        {
            await using var s = pv.Open();
            using var ms = new MemoryStream();
            await s.CopyToAsync(ms, ct);
            previewPng = ms.ToArray();
        }

        return new LoadResult
        {
            Project = project,
            LayerPngBytes = layerBytes,
            PreviewPng = previewPng
        };
    }

    // ---------- Helpers ----------

    public static string SuggestFileName(ArtFlowProject project)
    {
        var stamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        return $"artflow_{project.Canvas.Width}x{project.Canvas.Height}_{stamp}.artflow";
    }
}