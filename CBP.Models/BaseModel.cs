using System;

namespace CBP.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            this.RecordID = Public.Tools.Tool.GuidTo16String();
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
            this.DelMarker = false;
            this.IsEnable = true;
        }

        public string RecordID { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public bool DelMarker { get; set; }

        public bool IsEnable { get; set; }

        public string OperationPerson { get; set; }
    }
}
