using CBP.Models;
using CBP.Permission.Models.Function;
using CBP.Permission.Models.Menu;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Permission.Models.Role
{
    public class RoleMapModel : BaseMapModel
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public string RoleID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 功能集合
        /// </summary>
        public List<FunctionMapModel> FunctionList { get; set; }

    }
}
