using QEX.Abstractions.Interface;
using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Text;
using System.Text.Json;

namespace QEX.Components.Extensions.Classes
{
    public class ExtensionLoader
    {
        private readonly IExtensionService _extensionService;

        // Пока путь задаём строкой
        private readonly string _extensionsRoot =
            //@"C:\QEX\QEX\Extension\Writer\bin\Debug";
            @"C:\QEX\QEX\ArtFlow\bin\Debug";

        private readonly List<string> extensionaRoots = [];
        public ExtensionLoader(IExtensionService extensionService)
        {
            _extensionService = extensionService;
            extensionaRoots = [];
        }

        public List<ExtensionInfo> LoadInstalled()
        {
            var result = new List<ExtensionInfo>();

            if (!Directory.Exists(_extensionsRoot))
                return result;

            foreach (var dir in Directory.GetDirectories(_extensionsRoot))
            {
                var manifestPath = Path.Combine(dir, "manifest.json");
                if (!File.Exists(manifestPath))
                    continue;

                var json = File.ReadAllText(manifestPath);
                var manifest = JsonSerializer.Deserialize<ExtensionManifest>(json);
                if (manifest == null)
                    continue;

                var dllPath = Path.Combine(dir, manifest.AssemblyFile);

                var asm = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath);
                var type = asm.GetType(manifest.RootComponent);
                if (type == null)
                    continue;

                _extensionService.RegisterExtension(type, manifest.Name);

                result.Add(new ExtensionInfo
                {
                    Manifest = manifest,
                    FolderPath = dir,
                    ComponentType = type
                });
            }

            return result;
        }
    }
}
