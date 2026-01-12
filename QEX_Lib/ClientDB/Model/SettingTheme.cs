using QEX_Lib.ClientDB.IModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEX_Lib.ClientDB.Model
{
    /// <summary>
    /// ID_Setting: Need to select setting preset
    /// </summary>
    public class SettingTheme : IBaseModel
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? BackgroundColor { get; set; }
        public string? FirstColor { get; set; }
        public string? SecondColor { get; set; }
        public string? BorderColor { get; set; }
        public void Load(ClientContext context)
        {

        }
        public void Save(ClientContext context)
        {

        }
    }
    public class SettingThemeList : IBaseModelList
    {
        public List<SettingTheme> Items { get; set; } = new List<SettingTheme>();
        public void Load(ClientContext context)
        {
            Items = context.SettingsThemes.Select(s => s).ToList();
        }
        public void Save(ClientContext context)
        {

        }
        public void Delete(ClientContext context)
        {
        }
    }
}
