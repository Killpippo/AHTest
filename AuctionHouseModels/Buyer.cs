using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AuctionHouseModels
{
    [DataContract]
    public class Buyer
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string ContactInfo { get; set; }

        public Buyer()
        {
            ID = System.Guid.NewGuid().ToString();
            Username = String.Empty;
            Password = String.Empty;
            ContactInfo = String.Empty;
        }
    }
}