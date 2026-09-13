using System;
using System.Collections.Generic;
using System.Text;

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

    /// <summary>Заготовка под слои — на будущее.</summary>
    public class Layer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; } = "Слой";
        public bool Visible { get; set; } = true;
        public double Opacity { get; set; } = 1.0;
        public string BlendMode { get; set; } = "source-over";
    }
}
