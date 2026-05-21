using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using QEX.Resources.Data;

namespace QEX.Resources.Data.Classes
{
    public class ExtensionSetting
    {
        private const string _ExtensionSetting = "ExtensionSetting";
        private const string _ExtensionPath = "ExtensionPath";
        private const string _IsAutoScanDir = "IsScanDir";
        private const string _IsAutoUpdate = "IsAutoUpdate";


        public List<string> ExtensionPath = [];
        public bool IsAutoScanDir = false;
        public bool IsAutoUpdate = false;
        /// <summary>
        /// Загружает настройки расширений из файла Data/Options 
        /// </summary>
        /// <param name="FilePath">Путь к файлу вместе с именем файла </param>
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

                if (settingDict.ContainsKey(_ExtensionSetting))
                {
                    var extSetting = JsonConvert.DeserializeObject<Dictionary<string, object>>(settingDict[_ExtensionSetting].ToString() ?? "");

                    if (extSetting != null)
                    {
                        if (extSetting.ContainsKey(_ExtensionPath))
                        {
                            var Path = extSetting[_ExtensionPath]?.ToString();
                            if (!string.IsNullOrEmpty(Path))
                                ExtensionPath = JsonConvert.DeserializeObject<List<string>>(Path) ?? [];
                        }
                        if (extSetting.ContainsKey(_IsAutoScanDir))
                        {
                            bool.TryParse(extSetting[_IsAutoScanDir]?.ToString(), out bool isScan);
                            IsAutoScanDir = isScan;
                        }
                        if (extSetting.ContainsKey(_IsAutoUpdate))
                        {
                            bool.TryParse(extSetting[_IsAutoUpdate]?.ToString(), out bool isUpdate);
                            IsAutoUpdate = isUpdate;
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
        /// Сохраняет настройки расширений в файл Data/Options
        /// </summary>
        /// <param name="FilePath">Путь к файлу вместе с именем файла</param>
        public void SaveSetting(string FilePath)
        {
            try
            {
                string? directory = Path.GetDirectoryName(FilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    throw new DirectoryNotFoundException($"Файл настроек не найден: {FilePath}");
                var settingDict = new Dictionary<string, object>()
                {
                    [_ExtensionSetting] = new Dictionary<string, object>
                    {
                        [_ExtensionPath] = ExtensionPath,
                        [_IsAutoScanDir] = IsAutoScanDir,
                        [_IsAutoUpdate] = IsAutoUpdate
                    },
                };
                string json = JsonConvert.SerializeObject(settingDict, Formatting.Indented);
                File.WriteAllText(FilePath, json);

            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }



    //public class ExtensionSettingList  TODO: MB Add settings preset
    //{
    //    List<ExtensionSetting> Items = new List<ExtensionSetting>();

    //}
}
