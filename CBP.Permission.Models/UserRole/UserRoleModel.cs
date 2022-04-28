using CBP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Permission.Models.UserRole
{
    public class UserRoleModel : BaseModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public string RoleID { get; set; }
    }
}
