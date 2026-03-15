using QEX_Lib.ClientDB.IModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using QEX_Lib.Classes;

namespace QEX_Lib.ClientDB.Model
{
    public class Extension : IBaseModel
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public byte[]? Icon { get; set; }
        public string? Author { get; set; }
        public string? Version { get; set; }
        public string? Type { get; set; }
        public string? Tag { get; set; }
        public string? Description { get; set; }
        public void Load(ClientContext context)
        {

        }
        public void Save(ClientContext context)
        {

        }
    }
    public class ExtensionList : IBaseModelList
    {
        public List<Extension> Items { get; set; } = [];
        public void Load(ClientContext context)
        {
            Items = [.. context.Extensions.Select(e => e)];
        }
        public void LoadByPath()
        {
            Items.Clear();
            //TODO: получение всех директорий для получения расширений и дальнейшая проверка всех указанных директорий
            string path = Path.Combine(AppContext.BaseDirectory, "Components", "Extensions", "Installed", "QEX_Writer");

            if (Directory.Exists(path))
            {
                string[] dirs = Directory.GetDirectories(path);

                foreach (var dir in dirs)
                {
                    string manifestPath = Path.Combine(dir, "manifest.json");
                    if (File.Exists(manifestPath))
                    {
                        string str = File.ReadAllText(manifestPath);
                        var man = JsonSerializer.Deserialize<ExtensionManifest>(str);
                        if (man != null)
                        {
                            Items.Add(new Extension
                            {
                                Name = man.Name,
                                Version = man.Version,
                                Description = man.Description

                            });
                            Items.Add(new Extension
                            {
                                Name = man.Name,
                                Version = man.Version,
                                Description = man.Description

                            });
                        }
                    }
                }
            }
        }
        public void Save(ClientContext context)
        {
        }
        public void Delete(ClientContext context)
        {
        }
       
    } 
}
