using System;
using System.Collections.Generic;
using System.Text;

namespace QEX.Components.Extensions.Classes
{
    public class ExtensionInfo
    {
        public ExtensionManifest Manifest { get; set; } = new();
        public string FolderPath { get; set; } = "";
        public Type? ComponentType { get; set; }
    }

}
