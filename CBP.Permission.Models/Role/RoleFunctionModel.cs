using CBP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Permission.Models.Role
{
    public class RoleFunctionModel : BaseModel
    {
        public string RoleID { get; set; }

        public string FunctionID { get; set; }
    }
}
