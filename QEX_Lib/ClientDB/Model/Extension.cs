using System.Text.Json;
using QEX_Lib.ClientDB.IModel;

namespace QEX_Lib.ClientDB.Model
{
    public class Extension : IBaseModel
    {
        public int ID { get; set; }

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Version { get; set; } = "";
        public string Author { get; set; } = "";

        // Полный путь к папке расширения
        public string FolderPath { get; set; } = "";

        // Имя DLL
        public string AssemblyFile { get; set; } = "";

        // Полное имя компонента
        public string RootComponent { get; set; } = "";

        public void Load(ClientContext context) { }
        public void Save(ClientContext context) { }
    }
    public class ExtensionList : IBaseModelList
    {
        public List<Extension> Items { get; set; } = new();

        public void Load(ClientContext context)
        {
            // База нам больше не нужна
        }

        public void LoadByPath()
        {
            Items.Clear();

            string root = Path.Combine(
                AppContext.BaseDirectory,
                "Components", "Extensions", "Installed");

            if (!Directory.Exists(root))
                return;

            foreach (var dir in Directory.GetDirectories(root))
            {
                string manifestPath = Path.Combine(dir, "extension.manifest.json");
                if (!File.Exists(manifestPath))
                    continue;

                try
                {
                    //var json = File.ReadAllText(manifestPath);
                    //var man = JsonSerializer.Deserialize<ExtensionManifest>(json);

                    //if (man == null)
                    //    continue;

                    //Items.Add(new Extension
                    //{
                    //    Name = man.Name,
                    //    Description = man.Description,
                    //    Version = man.Version,
                    //    Author = man.Author ?? "",
                    //    FolderPath = dir,
                    //    AssemblyFile = man.AssemblyFile,
                    //    RootComponent = man.RootComponent
                    //});
                }
                catch
                {
                    // Игнорируем битые расширения
                }
            }
        }

        public void Save(ClientContext context) { }
        public void Delete(ClientContext context) { }
    }
}
