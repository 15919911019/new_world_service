using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBP.Models
{
    [BsonIgnoreExtraElements]
    public class TokenModel
    {
        public string UserID { get; set; }

        public string TokenGuid { get; set; } 
    }
}
