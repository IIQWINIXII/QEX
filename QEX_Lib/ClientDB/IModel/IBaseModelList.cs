using System;
using System.Collections.Generic;
using System.Text;

namespace QEX_Lib.ClientDB.IModel
{
    public interface IBaseModelList
    {
        public void Load(ClientContext context);
        public void Save(ClientContext context);
        public void Delete(ClientContext context);
    }
}
