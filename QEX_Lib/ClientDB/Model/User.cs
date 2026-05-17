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
        public int ID { get; set; }
        [MaxLength(128)]   
        public string? Name { get; set; }
        [MaxLength(128)]
        [Key]
        public string? Login { get; set; }
        public int CurrentAvatarID { get; set; }
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
    public class UserList : IBaseModelList
    {
        public List<User> Items { get; set; } = [];
        public void Load(ClientContext context)
        {
            Items = [.. context.Users.Select(u => u)];
        }
        public void Save(ClientContext context)
        {
            
        }
        public void Delete(ClientContext context)
        {
            
        }
    }
}
