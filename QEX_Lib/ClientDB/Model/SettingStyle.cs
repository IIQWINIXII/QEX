using QEX_Lib.ClientDB.IModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEX_Lib.ClientDB.Model
{
    public class SettingStyle : IBaseModel
    {
        /// <summary>
        /// ID: Need to select setting preset
        /// </summary>
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Type { get; set; } = 0;
        public byte[] Value {get; set; } = Array.Empty<byte>();
        public void Load(ClientContext context)
        {

        }
        public void Save(ClientContext context)
        {

        }
    }
    public class SettingElementStyleList : IBaseModelList
    {
        public List<SettingStyle> Items { get; set; } = new List<SettingStyle>();
        public void Load(ClientContext context)
        {
            Items = context.SettingsStyles.Select(s => s).ToList();
        }
        public void Save(ClientContext context)
        {
        }
        public void Delete(ClientContext context)
        {
        }
    }
}
