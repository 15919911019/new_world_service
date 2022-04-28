using CBP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Permission.Models.Role
{
    public class RoleFunctionMapModel : BaseMapModel
    {
        public string RoleID { get; set; }

        public string RoleName { get; set; }

        public string FunctionID { get; set; }

        public string FunctionName { get; set; }

        public string FunctionValue { get; set; }
    }
}
