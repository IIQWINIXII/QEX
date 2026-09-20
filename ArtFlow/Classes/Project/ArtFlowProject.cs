using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArtFlow.Classes.Project;

/// <summary>
/// Модель файла .artflow (сохраняется в ZIP как project.json).
/// </summary>
public sealed class ArtFlowProject
{
    public const string FormatId = "artflow";
    public const int CurrentVersion = 1;
    public const string FileExtension = ".artflow";

    [JsonPropertyName("format")]
    public string Format { get; set; } = FormatId;

    [JsonPropertyName("version")]
    public int Version { get; set; } = CurrentVersion;

    [JsonPropertyName("created")]
    public DateTime Created { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("modified")]
    public DateTime Modified { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("canvas")]
    public ProjectCanvas Canvas { get; set; } = new();

    [JsonPropertyName("layers")]
    public List<ProjectLayer> Layers { get; set; } = new();

    [JsonPropertyName("activeLayerId")]
    public string? ActiveLayerId { get; set; }

    [JsonPropertyName("tool")]
    public string Tool { get; set; } = "pencil";

    [JsonPropertyName("color")]
    public string Color { get; set; } = "#000000";

    [JsonPropertyName("size")]
    public int Size { get; set; } = 3;

    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}

public sealed class ProjectCanvas
{
    [JsonPropertyName("width")]
    public int Width { get; set; } = 800;

    [JsonPropertyName("height")]
    public int Height { get; set; } = 600;

    [JsonPropertyName("background")]
    public string Background { get; set; } = "#ffffff";
}

public sealed class ProjectLayer
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [JsonPropertyName("name")]
    public string Name { get; set; } = "Слой";

    [JsonPropertyName("visible")]
    public bool Visible { get; set; } = true;

    [JsonPropertyName("opacity")]
    public double Opacity { get; set; } = 1.0;

    [JsonPropertyName("blendMode")]
    public string BlendMode { get; set; } = "source-over";

    [JsonPropertyName("file")]
    public string File { get; set; } = "";
}