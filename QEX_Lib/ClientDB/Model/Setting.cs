using QEX_Lib.ClientDB.IModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEX_Lib.ClientDB.Model
{
    public class Setting : IBaseModel
    {
        [Key]
        public int ID { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// ID_User: User identificator
        /// </summary>
        public int ID_USR { get; set; }
        /// <summary>
        /// ID_Theme: Theme identificator
        /// </summary>
        public int ID_THM { get; set; }
        /// <summary>
        /// ID_Style: Style identificator
        /// </summary>
        public int ID_STL { get; set; }
        /// <summary>
        /// UI Language
        /// </summary>
        public string Language { get; set; } = "eng";

        public void Load(ClientContext context)
        {

        }

        public void Save(ClientContext context)
        {

        }
    }
}
