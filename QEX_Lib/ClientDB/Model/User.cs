using QEX_Lib.ClientDB.IModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEX_Lib.ClientDB.Model
{
    public class User : IBaseModel
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(128)]   
        public string? Name { get; set; }
        [MaxLength(128)]
        public string? Login { get; set; }
        public byte[] CurrentAvatar { get; set; } = [];
        [MaxLength(128)]
        public string? Password { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public void Save(ClientContext context)
        {
            
        }
        public void Load(ClientContext context)
        {

        }
    }
}
