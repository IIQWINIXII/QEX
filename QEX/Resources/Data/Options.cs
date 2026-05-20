using System;
using System.Collections.Generic;
using System.Text;
using QEX.Resources.Data.Classes;

namespace QEX.Resources.Data
{
    public class Options
    {
        private const string SettingFilePath = "";
        public ExtensionSetting? extensionSetting;
        public InterfaceSetting? interfaceSetting;
        public SecuritySetting? securitySetting;

        public void LoadSetting() // дописать загрузку
        {
            extensionSetting = new ExtensionSetting();
            interfaceSetting = new InterfaceSetting();
            securitySetting = new SecuritySetting();

            extensionSetting.LoadSetting(DataFilePath.OptionPath);
        }
        public void SaveSetting()
        {

        }
    }
}
