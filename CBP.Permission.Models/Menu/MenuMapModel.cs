using CBP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Permission.Models.Menu
{
    public class MenuMapModel : BaseMapModel
    {
        public string ParentID { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }
    }
}
