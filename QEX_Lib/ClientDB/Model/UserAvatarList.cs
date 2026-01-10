using QEX_Lib.ClientDB.IModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace QEX_Lib.ClientDB.Model
{
    public class UserAvatarList
    {
        public List<UserAvatar> Avatars { get; set; } = new();
        public UserAvatar GetByID(int ID) => 
            Avatars.First(A => A.ID == ID);
        public string GetAvatarBase64ByID(int ID) =>
            Convert.ToBase64String(GetByID(ID).IMG);

        public string GetImageFromBase64ByID(int ID) =>
            $"data:image/{GetByID(ID).Type};base64,{GetAvatarBase64ByID(ID)}";
        

    }
}
