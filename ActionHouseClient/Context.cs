using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionHouseModels;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Web;
using System.Net;

namespace ActionHouseClient
{
    public class Context
    {        
        #region Internal methods

        private string ProcessServiceRequest( string _szRoutedRequest, string _szMethod, string _szInputParams )
        {
            HttpWebRequest oHttpRequest = (HttpWebRequest)WebRequest.Create(ServiceEndPoint + "/" + _szRoutedRequest );
            oHttpRequest.Method = _szMethod;

            if (!String.IsNullOrEmpty(_szInputParams))
            {
                oHttpRequest.ContentType = "application/json";

                using (StreamWriter oWStream = new StreamWriter(oHttpRequest.GetRequestStream()))
                {
                    oWStream.Write(_szInputParams);
                }
            }

            HttpWebResponse oHttpResponse = (HttpWebResponse)oHttpRequest.GetResponse();

            string szResponseResult;

            using (System.IO.StreamReader oRStream = new System.IO.StreamReader(oHttpResponse.GetResponseStream()))
            {
                szResponseResult = oRStream.ReadToEnd();
            }

            oHttpResponse.Close();

            return szResponseResult;
        }

        #endregion

        #region Members

        public string ServiceEndPoint { get; private set; }

        #endregion

        #region Constructor
        public Context( string _szServiceEndpoint )
        {
            ServiceEndPoint = _szServiceEndpoint;
        }
        #endregion

        #region Public Methods

        public List<Buyer> QueryBuyers()
        {
            if (String.IsNullOrEmpty(ServiceEndPoint)) throw new Exception("Missing Service Endpoint in Context");

            string szResponseResult = ProcessServiceRequest( "buyers", "GET", String.Empty );

            return Helper.Deserialize<List<Buyer>>(szResponseResult);
        }

        public Buyer QueryBuyer( string _szID )
        {
            if (String.IsNullOrEmpty(ServiceEndPoint)) throw new Exception("Missing Service Endpoint in Context");

            string szResponseResult = ProcessServiceRequest("buyers/" + _szID, "GET", String.Empty);

            return Helper.Deserialize<Buyer>(szResponseResult);
        }

        public Buyer VerifyBuyer(string _szUsername, string _szPassword)
        {
            if (String.IsNullOrEmpty(ServiceEndPoint)) throw new Exception("Missing Service Endpoint in Context");

            string szResponseResult = ProcessServiceRequest("buyers/" + HttpUtility.UrlEncode( _szUsername ) + "/" + HttpUtility.UrlEncode( _szPassword ), "GET", String.Empty);

            return Helper.Deserialize<Buyer>(szResponseResult);
        }

        public ResultCode ModifyBuyer( Buyer _oBuyer )
        {
            if (String.IsNullOrEmpty(ServiceEndPoint)) throw new Exception("Missing Service Endpoint in Context");

            string szResponseResult = ProcessServiceRequest("buyers/modify", "PUT", Helper.Serialize<Buyer>(_oBuyer));

            return Helper.Deserialize<ResultCode>(szResponseResult);
        }

        public ResultCode AddBuyer(Buyer _oBuyer)
        {
            if (String.IsNullOrEmpty(ServiceEndPoint)) throw new Exception("Missing Service Endpoint in Context");

            string szResponseResult = ProcessServiceRequest("buyers/add", "POST", Helper.Serialize<Buyer>(_oBuyer));

            return Helper.Deserialize<ResultCode>(szResponseResult);
        }

        public List<Item> QueryItems()
        {
            if (String.IsNullOrEmpty(ServiceEndPoint)) throw new Exception("Missing Service Endpoint in Context");

            string szResponseResult = ProcessServiceRequest("items", "GET", String.Empty );

            return Helper.Deserialize<List<Item>>(szResponseResult);
        }

        public Item QueryItem( string _szID )
        {
            if (String.IsNullOrEmpty(ServiceEndPoint)) throw new Exception("Missing Service Endpoint in Context");

            string szResponseResult = ProcessServiceRequest("items/" + _szID, "GET", String.Empty);

            return Helper.Deserialize<Item>(szResponseResult);
        }

        public ResultCode AddItem(Item _oItem)
        {
            if (String.IsNullOrEmpty(ServiceEndPoint)) throw new Exception("Missing Service Endpoint in Context");

            string szResponseResult = ProcessServiceRequest("items/add", "POST", Helper.Serialize<Item>(_oItem));

            return Helper.Deserialize<ResultCode>(szResponseResult);
        }

        public ResultCode ModifyItem(Item _oItem)
        {
            if (String.IsNullOrEmpty(ServiceEndPoint)) throw new Exception("Missing Service Endpoint in Context");

            string szResponseResult = ProcessServiceRequest("items/modify", "PUT", Helper.Serialize<Item>(_oItem));

            return Helper.Deserialize<ResultCode>(szResponseResult);
        }
        
        public List<Bid> QueryItemBids( string _szItemID )
        {
            if (String.IsNullOrEmpty(ServiceEndPoint)) throw new Exception("Missing Service Endpoint in Context");

            string szResponseResult = ProcessServiceRequest("bid/item/" + _szItemID.ToString(), "GET", String.Empty);

            return Helper.Deserialize<List<Bid>>(szResponseResult);
        }

        public List<Bid> QueryBuyerBids(string _szBuyerID)
        {
            if (String.IsNullOrEmpty(ServiceEndPoint)) throw new Exception("Missing Service Endpoint in Context");

            string szResponseResult = ProcessServiceRequest("bid/buyer/" + _szBuyerID, "GET", String.Empty);

            return Helper.Deserialize<List<Bid>>(szResponseResult);
        }

        //public List<Item> QueryBuyerOutbidItems(string _szBuyerID)
        //{
        //    if (String.IsNullOrEmpty(ServiceEndPoint)) throw new Exception("Missing Service Endpoint in Context");

        //    string szResponseResult = ProcessServiceRequest("bid/buyer/outbids/" + _szBuyerID, "GET", String.Empty);

        //    return Helper.Deserialize<List<Item>>(szResponseResult);
        //}

        //public ResultCode AddBid(string _szItemID, string _szBuyerID, string _szPrice, string _szAnonymous)
        public ResultCode AddBid( string _szItemID, string _szBuyerID, int _iBidValue, bool _bAnonymous )
        {
            if (String.IsNullOrEmpty(ServiceEndPoint)) throw new Exception("Missing Service Endpoint in Context");

            string szResponseResult = ProcessServiceRequest( "bid/" +
                                                                _szItemID.ToString() + "/" +
                                                                _szBuyerID.ToString() + "/" +
                                                                _iBidValue.ToString() + "/" +
                                                                (_bAnonymous ? "1" : "0") , "POST", " " );

            return Helper.Deserialize<ResultCode>(szResponseResult);
        }
        #endregion
    }
}
