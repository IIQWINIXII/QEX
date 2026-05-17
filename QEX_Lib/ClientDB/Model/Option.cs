using System;
using System.Collections.Generic;
using System.Text;

namespace QEX_Lib.ClientDB.Model
{
    public class Option
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// Type of value:
        ///     -1: null
        ///     0: integer
        ///     1: float
        ///     2: char
        ///     3: string
        ///     4: boolean
        /// </summary>
        public int Type { get; set; }
        public byte[] Value { get; set; } = [];
    }
}
