using CBP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.UserModels.User
{
    public class UserMapModel : BaseMapModel
    {
        public string UserName { get; set; }

        public string Mobile { get; set; }

        public string PassWord { get; set; }
    }
}
