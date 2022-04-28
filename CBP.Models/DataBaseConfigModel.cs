using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Models
{
    public class DataBaseConfigModel
    {
        public string SQLConnectionString { get; set; }

        public string OrclConnectionString { get; set; }

        public int DbType { get; set; }
    }
}
