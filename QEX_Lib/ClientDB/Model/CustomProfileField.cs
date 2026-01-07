using QEX_Lib.ClientDB.IModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEX_Lib.ClientDB.Model
{
    public class CustomProfileField : IBaseModel
    {
        public int ID { get; set; }
        /// <summary>
        /// ID_FieldPosition: FK to posiotion element
        /// </summary>
        public int ID_FP { get; set; }
        public string? Name { get; set; }
        public byte[] Caption { get; set; } = [];
        public string? Type { get; set; } 
        public void Load(ClientContext context)
        {

        }
        public void Save(ClientContext context)
        {

        }
    }
}
