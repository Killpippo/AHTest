using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AuctionHouseModels
{
    [DataContract]
    public class Item
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool AuctionEnabled { get; set; }

        [DataMember]
        public DateTime AuctionExpiry { get; set; }

        [DataMember]
        public string Base64Img { get; set; }

        public Item()
        {
            ID = System.Guid.NewGuid().ToString();
            Name = String.Empty;
            Description = String.Empty;
            AuctionEnabled = false;
            AuctionExpiry = new DateTime(2100, 1, 1);
            Base64Img = String.Empty;
        }
    }
}