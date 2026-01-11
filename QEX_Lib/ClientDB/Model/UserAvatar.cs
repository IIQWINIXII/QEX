using QEX_Lib.ClientDB.IModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEX_Lib.ClientDB.Model
{
    public class UserAvatar : IBaseModel
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// ID_User: Need to show advanced profile
        /// </summary>
        public int ID_USR { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public byte[] IMG { get; set; } = [];
        
        public void Load(ClientContext context)
        {

        }
        public void Save(ClientContext context)
        {

        }
    }
    public class UserAvatarList : IBaseModelList
    {
        public List<UserAvatar> Items { get; set; } = new();
        public UserAvatar GetByID(int ID) =>
            Items.First(A => A.ID == ID);
        public string GetAvatarBase64ByID(int ID) =>
            Convert.ToBase64String(GetByID(ID).IMG);

        public string GetImageFromBase64ByID(int ID) =>
            $"data:image/{GetByID(ID).Type};base64,{GetAvatarBase64ByID(ID)}";

        public void Load(ClientContext context)
        {
            Items = context.UsersAvatars.ToList();
        }

        public void Save(ClientContext context)
        {
            throw new NotImplementedException();
        }

        public void Delete(ClientContext context)
        {
            throw new NotImplementedException();
        }
    }
}
