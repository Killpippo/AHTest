using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using AuctionHouseModels;

namespace AuctionHouseService
{
    [ServiceContract]
    public interface IAHService
    {
        #region Buyer Management

        /// <summary>
        /// Return the list of all buyers
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "buyers")]
        List<Buyer> QueryBuyers();

        /// <summary>
        /// return the specified buyer
        /// </summary>
        /// <param name="_szID"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "buyers/{_szID}")]
        Buyer QueryBuyer(string _szID);

        /// <summary>
        /// return the specified buyer
        /// </summary>
        /// <param name="_szID"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "buyers/{_szUsername}/{_szPassword}")]
        Buyer VerifyBuyer(string _szUsername,string _szPassword);

        /// <summary>
        /// Add a new buyer
        /// </summary>
        /// <param name="_oBuyer"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "buyers/add")]
        ResultCode AddBuyer(Buyer _oBuyer);

        /// <summary>
        /// Modify the specified buyer
        /// </summary>
        /// <param name="_oBuyer"></param>
        /// <param name="_szID"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "PUT",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "buyers/modify")]
        ResultCode ModifyBuyer(Buyer _oBuyer);

        #endregion

        #region Item Management

        [OperationContract]
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "items")]
        List<Item> QueryItems();
                
        [OperationContract]
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "items/{_szID}")]
        Item QueryItem(string _szID);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "items/add")]
        ResultCode AddItem( Item _oItem );
                
        [OperationContract]
        [WebInvoke(Method = "PUT",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "items/modify")]
        ResultCode ModifyItem(Item _oItem);

        #endregion

        #region Bid Management

        [OperationContract]
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "bid/item/{_szItemID}")]
        List<Bid> QueryItemBids(string _szItemID);

        [OperationContract]
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "bid/buyer/{_szBuyerID}")]
        List<Bid> QueryBuyerBids(string _szBuyerID);

        //[OperationContract]
        //[WebInvoke(Method = "GET",
        //            ResponseFormat = WebMessageFormat.Json,
        //            BodyStyle = WebMessageBodyStyle.Bare,
        //            UriTemplate = "bid/buyer/outbids/{_szBuyerID}")]
        //List<Item> QueryBuyerOutbidItems(string _szBuyerID);

        [OperationContract]
        [WebInvoke(Method = "POST",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "bid/{_szItemID}/{_szBuyerID}/{_szPrice}/{_szAnonymous}")]
        ResultCode AddBid(string _szItemID, string _szBuyerID, string _szPrice, string _szAnonymous );

        #endregion
    }
}
