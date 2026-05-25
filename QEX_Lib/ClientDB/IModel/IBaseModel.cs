using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEX_Lib.ClientDB.IModel
{
    public interface IBaseModel
    {
        public string ID { get; set; }
        public string? Name { get; set; }
        public void Load(ClientContext context);
        public void Save(ClientContext context);
    }
}
