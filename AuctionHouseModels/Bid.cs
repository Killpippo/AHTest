using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AuctionHouseModels
{
    [DataContract]
    public class Bid
    {
        [DataMember]
        public string ItemID { get; set; }

        [DataMember]
        public string BuyerID { get; set; }

        [DataMember]
        public DateTime CreateOn { get; private set; }

        [DataMember]
        public int BidValue { get; set; }

        [DataMember]
        public bool Anonymous { get; set; }

        public Bid()
        {
            ItemID = String.Empty;
            BuyerID = String.Empty;
            CreateOn = DateTime.Now;
            BidValue = 0;
            Anonymous = true;
        }
    }
}