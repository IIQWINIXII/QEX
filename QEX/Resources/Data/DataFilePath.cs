using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace QEX.Resources.Data
{
    public static class DataFilePath
    {
        public static string OptionPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Data", "Options.json");
        // TODO: Добавить сохранение для дополнительнызх файлов из расширений.
    }
}
