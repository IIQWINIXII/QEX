using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QEX.Resources.Data.Classes
{
    public class InterfaceSetting
    {
        //Theme add style select
        public const string _InterfaceSetting = "InterfaceSetting";
        public const string _Localization = "Localization";
        public const string _IsShowExtensionSysTray = "IsShowExtensionSysTray";
        public const string _IsUseHotKey = "IsUseHotKey";

        public string Theme = "Dark";
        public string Localization = "EN/en";
        public bool IsShowExtensionSysTray = false;
        public bool IsUseHotKey = true;
        /// <summary>
        /// Загружает настройки интерфейса из файла Data/Options
        /// </summary>
        /// <param name="FilePath">Путь к файлу вместе с именем файла</param>
        public void LoadSetting(string FilePath)
        {
            try
            {
                if (!Directory.Exists(FilePath))
                    throw new DirectoryNotFoundException($"Файл настроек не найден: {FilePath}");
                
                string json = File.ReadAllText(FilePath);
                
                var settingDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                if (settingDict == null)
                    return;

                if (settingDict.ContainsKey(_InterfaceSetting))
                {
                    var intSetting = JsonConvert.DeserializeObject<Dictionary<string, object>>(settingDict[_InterfaceSetting].ToString() ?? "");

                    if (intSetting != null)
                    {
                        if (intSetting.ContainsKey(_Localization))
                        {
                            var loc = intSetting[_Localization]?.ToString();
                            if (!string.IsNullOrEmpty(loc))
                                Localization = loc;
                        }
                        if (intSetting.ContainsKey(_IsShowExtensionSysTray))
                        {
                            bool.TryParse(intSetting[_IsShowExtensionSysTray]?.ToString(), out bool isShowExSysTray);
                            IsShowExtensionSysTray = isShowExSysTray;
                        }
                        if (intSetting.ContainsKey(_IsUseHotKey))
                        {
                            bool.TryParse(intSetting[_IsUseHotKey]?.ToString(), out bool isUseHotKey);
                            IsUseHotKey = isUseHotKey;
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Сохраняет настройки интерфейса в файл Data/Options
        /// </summary>
        /// <param name="FilePath">Путь к файлу с именем файла</param>
        public void SaveSetting(string FilePath)
        {
            try
            {
                string? directory = Path.GetDirectoryName(FilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    throw new DirectoryNotFoundException($"Файл найстроек не найден: {FilePath}");
                var settingDict = new Dictionary<string, object>()
                {
                    [_InterfaceSetting] = new Dictionary<string, object>
                    {
                        [_Localization] = Localization,
                        [_IsShowExtensionSysTray] = IsShowExtensionSysTray,
                        [_IsUseHotKey] = IsUseHotKey
                    },
                };
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
