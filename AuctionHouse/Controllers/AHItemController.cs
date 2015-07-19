using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuctionHouse.Controllers
{
    public class AHItemController : Controller
    {
        public enum eBuyerBidStatus
        {
            NoBuyerBid = 0,
            BuyerOutbidded,
            BuyerHighestBid,
            OtherBuyerBid
        }

        // GET: AHItem
        public ActionResult Index()
        {
            ActionHouseClient.Context oAHContext = new ActionHouseClient.Context(System.Configuration.ConfigurationManager.AppSettings["AHServiceAddress"]);

            List<AuctionHouseModels.Item> aItems = oAHContext.QueryItems();

            for (int i = 0; i < aItems.Count; i++ )
            {
                List<AuctionHouseModels.Bid> aBids = oAHContext.QueryItemBids(aItems[i].ID);

                ViewData["COUNT_" + aItems[i].ID] = aBids.Count.ToString();
                ViewData["STATUS_" + aItems[i].ID] = (aBids.Count == 0) ? eBuyerBidStatus.NoBuyerBid.ToString() : eBuyerBidStatus.OtherBuyerBid.ToString();
                ViewData["BG_" + aItems[i].ID] = "#FFFFFF";

                // check if the buyer has a bid on the item and if it's still the highest
                if (Session["BuyerID"] != null)
                {
                    for ( int j=0; j<aBids.Count; j++ )
                    {
                        if (aBids[j].BuyerID == Session["BuyerID"].ToString())
                        {
                            if (j == 0)
                            {
                                ViewData["BG_" + aItems[i].ID] = "#0BCF0B";
                                ViewData["STATUS_" + aItems[i].ID] = eBuyerBidStatus.BuyerHighestBid.ToString();
                            }
                            else
                            {
                                ViewData["BG_" + aItems[i].ID] = "#CFCF0B";
                                ViewData["STATUS_" + aItems[i].ID] = eBuyerBidStatus.BuyerOutbidded.ToString();
                            }

                            break;
                        }
                    }
                }
            }

            return View(aItems);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(AuctionHouseModels.Item _oItem)
        {
            // TODO notify operation denied
            if (Session["BuyerID"] != null) return RedirectToAction("Index", "AHItem");

            if (ModelState.IsValid)
            {
                // upload image file
                HttpPostedFileBase oHttpFile = Request.Files[0] as HttpPostedFileBase;

                if (oHttpFile.ContentLength == 0)
                {
                    ModelState.AddModelError(String.Empty, "Select an image for this item first");

                    return View();
                }

                try
                {
                    using (System.IO.MemoryStream oMStream = new System.IO.MemoryStream())
                    {
                        oHttpFile.InputStream.CopyTo(oMStream);

                        _oItem.Base64Img = Convert.ToBase64String(oMStream.ToArray());
                    }
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(String.Empty, "Image upload failed: " + exc.Message);

                    return View();
                }

                ActionHouseClient.Context oAHContext = new ActionHouseClient.Context(System.Configuration.ConfigurationManager.AppSettings["AHServiceAddress"]);

                AuctionHouseModels.ResultCode oRes = oAHContext.AddItem(_oItem);

                // TODO encode error
                if (oRes.Code != 0)
                {
                    ModelState.AddModelError(String.Empty, "Item creation failed: " + oRes.Message );

                    return View();
                }

                return RedirectToAction( "Index", "AHItem" );
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit( string id )
        {
            // TODO notify operation denied
            if (Session["BuyerID"] != null) return RedirectToAction("Index", "AHItem");

            ActionHouseClient.Context oAHContext = new ActionHouseClient.Context(System.Configuration.ConfigurationManager.AppSettings["AHServiceAddress"]);

            AuctionHouseModels.Item oItem = oAHContext.QueryItem(id);

            return View( oItem );
        }

        [HttpPost]
        public ActionResult Edit(AuctionHouseModels.Item _oItem)
        {
            // TODO notify operation denied
            if (Session["BuyerID"] != null) return RedirectToAction("Index", "AHItem");

            if (ModelState.IsValid)
            {
                ActionHouseClient.Context oAHContext = new ActionHouseClient.Context(System.Configuration.ConfigurationManager.AppSettings["AHServiceAddress"]);

                AuctionHouseModels.ResultCode oRes = oAHContext.ModifyItem(_oItem);

                // TODO encode error
                if (oRes.Code != 0)
                {
                    ModelState.AddModelError(String.Empty, "Item creation failed: " + oRes.Message);

                    return View();
                }

                return RedirectToAction("Index", "AHItem");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            // TODO notify operation denied
            if (Session["BuyerID"] == null) return RedirectToAction("Index", "AHItem");

            ActionHouseClient.Context oAHContext = new ActionHouseClient.Context(System.Configuration.ConfigurationManager.AppSettings["AHServiceAddress"]);

            if (!String.IsNullOrEmpty( Request.QueryString["bidValue"] ))
            {
                int iBidValue = 0;
                bool bAnonymous = (Request.QueryString["anonymous"] != null && Request.QueryString["anonymous"] == "1");

                try
                {
                    iBidValue = Convert.ToInt32(Request.QueryString["bidValue"]);

                    AuctionHouseModels.ResultCode oRes = oAHContext.AddBid(id, Session["BuyerID"].ToString(), iBidValue, bAnonymous);

                    // TODO encode the error
                    if (oRes.Code != 0)
                    {
                        ModelState.AddModelError(String.Empty, "Bid refused: " + oRes.Message);
                    }
                    else
                    {
                        Response.Redirect(Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.IndexOf("?")));
                    }
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(String.Empty, "Invalid bid request");
                }
            }

            AuctionHouseModels.Item oItem = oAHContext.QueryItem(id);

            if (oItem != null)
            {
                List<AuctionHouseModels.Bid> aBids = oAHContext.QueryItemBids(oItem.ID);

                int iMinBid = 0;

                if (aBids.Count == 0)
                {
                    ViewData["BUYERBID"] = "No bids on this item at the moment";
                }
                else
                {
                    iMinBid = aBids[0].BidValue;

                    // buyer with the current highest bid
                    if (aBids[0].BuyerID == Session["BuyerID"].ToString())
                    {
                        ViewData["BUYERBID"] = String.Format("Your current bid of {0} is the highest", aBids[0].BidValue.ToString());
                    }
                    else
                    {
                        AuctionHouseModels.Buyer oBuyer = null;

                        if (!aBids[0].Anonymous)
                            oBuyer = oAHContext.QueryBuyer(aBids[0].BuyerID);
                            
                        if (oBuyer != null)
                            ViewData["BUYERBID"] = String.Format("A bid of {0} from '{1}' is the highest", aBids[0].BidValue.ToString(), oBuyer.Username);
                        else
                            ViewData["BUYERBID"] = String.Format("A bid of {0} from unknown buyer is the highest", aBids[0].BidValue.ToString());
                    }
                }

                // no bids or the winning bid is not of the buyer connected: enable bid submission
                ViewData["BIDENABLED"] = (oItem.AuctionEnabled && (aBids.Count == 0 || aBids[0].BuyerID != Session["BuyerID"].ToString())) ? "1" : "0";
                ViewData["MINBID"] = ++iMinBid;
            }
            // TODO notify unknown item
            else
            {
                return RedirectToAction("Index", "AHItem");
            }

            return View(oItem);
        }
    }
}