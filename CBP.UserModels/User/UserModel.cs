using CBP.Models;
using System;

namespace CBP.UserModels.User
{
    public class UserModel : BaseModel
    {
        public string UserName { get; set; }

        public string Mobile { get; set; }

        public string Password { get; set; }
    }
}
