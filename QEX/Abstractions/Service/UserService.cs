using QEX_Lib.ClientDB.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QEX.Abstractions.Service
{
    public class UserService
    {
        public User? CurrentUser { get; private set; }

        public void SetUser(User user)
        {
            CurrentUser = user;
        }
    }
}
