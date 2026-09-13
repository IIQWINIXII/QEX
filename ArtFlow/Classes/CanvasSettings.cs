using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtFlow.Classes
{
    public class CanvasSettings
    {
        public int Width { get; set; } = 800;
        public int Height { get; set; } = 500;
        public string Background { get; set; } = "#FFFFFF";

        /// <summary>Пресеты размеров для быстрого выбора.</summary>
        public static readonly (string Name, int W, int H)[] Presets =
        {
            ("Малый 640×480",       640, 480),
            ("Средний 800×600",     800, 600),
            ("HD 1280×720",        1280, 720),
            ("Full HD 1920×1080",  1920, 1080),
            ("Квадрат 1000×1000",  1000, 1000),
            ("A4 300dpi 2480×3508",2480, 3508),
        };

        public CanvasSettings Clone() => new()
        {
            Width = Width,
            Height = Height,
            Background = Background
        };
    }

    /// <summary>Один слой документа.</summary>
    public class LayerInfo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; } = "Слой";
        public bool Visible { get; set; } = true;

        /// <summary>0..1</summary>
        public double Opacity { get; set; } = 1.0;

        /// <summary>Один из Canvas2D globalCompositeOperation.</summary>
        public string BlendMode { get; set; } = "source-over";

        public LayerInfo Clone(string? newName = null) => new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = newName ?? (Name + " копия"),
            Visible = Visible,
            Opacity = Opacity,
            BlendMode = BlendMode
        };
    }

    /// <summary>Действие над слоями из UI.</summary>
    public enum LayerActionKind
    {
        Select, Add, Duplicate, Delete, MoveUp, MoveDown,
        ToggleVisible, SetBlend, SetOpacity, Rename
    }

    /// <summary>Событие от LayersPanel в родитель.</summary>
    public record LayerAction(
        LayerActionKind Kind,
        string? Id = null,
        int? Index = null,
        bool? Visible = null,
        string? BlendMode = null,
        double? Opacity = null,
        string? Name = null);

    /// <summary>Доступные режимы смешивания (совпадают с Canvas2D).</summary>
    public static class BlendModes
    {
        public static readonly string[] All =
        {
            "source-over", "multiply", "screen", "overlay",
            "darken", "lighten", "color-dodge", "color-burn",
            "hard-light", "soft-light", "difference", "exclusion",
            "hue", "saturation", "color", "luminosity"
        };
    }
}