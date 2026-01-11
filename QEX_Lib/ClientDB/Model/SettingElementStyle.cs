using QEX_Lib.ClientDB.IModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEX_Lib.ClientDB.Model
{
    public class SettingElementStyle : IBaseModel
    {
        public int ID { get; set; }
        /// <summary>
        /// ID_Setting: Need to select setting preset
        /// </summary>
        public int ID_S { get; set; }
        public string? Name { get; set; }
        public void Load(ClientContext context)
        {

        }
        public void Save(ClientContext context)
        {

        }
    }
    public class SettingElementStyleList : IBaseModelList
    {
        public List<SettingElementStyle> Items { get; set; } = new List<SettingElementStyle>();
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
