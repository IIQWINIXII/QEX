using QEX.Abstractions.Interface;
using QEX.Resources.Data;
using QEX_Lib.QEX_API.Abtractions.Interface;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;

namespace QEX.Components.Extensions.Classes
{
    public class ExtensionLoader
    {
        private readonly IExtensionService _extensionService;
        private readonly IDynamicServiceRegistry _registry;
        private Options _options = new();

        // Пока путь задаём строкой (пока что потом переписать на получение путей из настроек)
        //private readonly string _extensionsRoot =
            //@"C:\QEX\QEX\Extension\Writer\bin\Debug";
           // @"C:\QEX\QEX\ArtFlow\bin\Debug";

        private readonly List<string>? ExtensionsRoots = [];
        public ExtensionLoader(IExtensionService extensionService, IDynamicServiceRegistry registry)
        {
            _extensionService = extensionService;
            _options.LoadSetting();
            _registry = registry;
            //extensionaRoots = _options?.extensionSetting?.ExtensionPath ?? []; // переписать структуру полчения 
        }                                                               // примерно черз чтение каждого пути

        public List<ExtensionInfo> LoadInstalled()
        {
            var result = new List<ExtensionInfo>();

            
            foreach (var extension in _options.extensionSetting?.ExtensionPath ?? [])
            {
                if (!Directory.Exists(extension))
                    return result;
                foreach (var dir in Directory.GetDirectories(extension))
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

                    var moduleType = asm.GetTypes()
                        .FirstOrDefault(t => t.Name == "Module");

                    var registerMethod = moduleType?
                        .GetMethod("Register", BindingFlags.Public | BindingFlags.Static);

                    if (registerMethod != null)
                    {
                        registerMethod.Invoke(null, new object[] { _registry });
                    }

                    _extensionService.RegisterExtension(type, manifest.Name);

                    result.Add(new ExtensionInfo
                    {
                        Manifest = manifest,
                        FolderPath = dir,
                        ComponentType = type
                    });
                }
            }
            

            return result;
        }
    }
}
