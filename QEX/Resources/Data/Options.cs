using System;
using System.Collections.Generic;
using System.Text;
using QEX.Resources.Data.Classes;

namespace QEX.Resources.Data
{
    public class Options
    {
        private string SettingFilePath = DataFilePath.OptionPath;
        public ExtensionSetting? extensionSetting = new();
        public InterfaceSetting? interfaceSetting = new();
        public SecuritySetting? securitySetting = new();
        public Options()
        {
            //if (!File.Exists(SettingFilePath)) TODO: добавить проверку на наличие файла 
               // File.Create(SettingFilePath);          Тут или при инициализации приложения.
        }

        public void LoadSetting()
        {
            extensionSetting?.LoadSetting(SettingFilePath);
            interfaceSetting?.LoadSetting(SettingFilePath);
            securitySetting?.LoadSetting(SettingFilePath);
        }
        public void SaveSetting()
        {
            extensionSetting?.SaveSetting(SettingFilePath);
            interfaceSetting?.SaveSetting(SettingFilePath);
            securitySetting?.SaveSetting(SettingFilePath);
        }
    }
}
