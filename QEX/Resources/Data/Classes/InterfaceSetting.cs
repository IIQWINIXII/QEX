using System;
using System.Collections.Generic;
using System.Text;

namespace QEX.Resources.Data.Classes
{
    public class InterfaceSetting
    {
        //Theme add style select
        public const string _InterfaceSetting = "InterfaceSetting";
        public const string _InterfaceSetting = "InterfaceSetting";
        public const string _Localization = "Localization";
        public const string _IsShowExtensionSysTray = "IsShowExtensionSysTray";
        public const string _IsUseHotKey = "IsUseHotKey";

        public string Localization = "EN/en";
        public bool IsShowExtensionSysTray = false;
        public bool IsUseHotKey = true;

        public void LoadSetting()
        {

        }
        public void SaveSetting()
        {

        }
    }
}
