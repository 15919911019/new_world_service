using CBP.Models;
using CBP.Permission.Models.Role;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Permission.Models.UserRole
{
    public class UserRoleMapModel : BaseMapModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserID { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public List<RoleMapModel> RoleList { get; set; }
    }
}
