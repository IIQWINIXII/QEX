using QEX_Lib.ClientDB.IModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEX_Lib.ClientDB.Model
{
    public class CustomFieldPosition : IBaseModel
    {
        public string? ID { get; set; }
        public string? Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        /// <summary>
        /// Entering is parameter need to indecate field in inner panel
        /// </summary>
        public int Entering {  get; set; }
        public void Load(ClientContext context)
        {

        }

        public void Save(ClientContext context)
        {

        }
    }
    public class CustomFieldPositionList : IBaseModelList
    {
        public List<CustomFieldPosition> Items { get; set; } = new List<CustomFieldPosition>();
        public void Load(ClientContext context)
        {
            Items = context.CustomFields.Select(c => c).ToList();
        }
        public void Save(ClientContext context)
        {
        }
        public void Delete(ClientContext context)
        {
        }
    }
}
