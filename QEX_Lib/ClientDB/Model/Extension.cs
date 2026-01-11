using QEX_Lib.ClientDB.IModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEX_Lib.ClientDB.Model
{
    public class Extension : IBaseModel
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Tag { get; set; }
        public string? Description { get; set; }
        public string? ExtensionPath { get; set; }
        public bool IsInstalled { get; set; }
        public void Load(ClientContext context)
        {

        }
        public void Save(ClientContext context)
        {

        }
    }
    public class ExtensionList : IBaseModelList
    {
        public List<Extension> Items { get; set; } = new List<Extension>();
        public void Load(ClientContext context)
        {
            Items = context.Extensions.Select(e => e).ToList();
        }
        public void Save(ClientContext context)
        {
        }
        public void Delete(ClientContext context)
        {
        }
} 
}
