using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AuctionHouse.Controllers
{
    public class RegisterAHBuyerController : Controller
    {
        //
        // GET: /RegisterAHBuyer/
        public ActionResult Index()
        {
            return View();
        }

		[HttpGet]
		public ActionResult CreateBuyer()
		{
			return View();
		}

		[HttpPost]
		public ActionResult CreateBuyer( AuctionHouse.Models.RegisterAHBuyerModel _oNewBuyer )
		{
			if (ModelState.IsValid)
			{
				try
				{
                    ActionHouseClient.Context oAHContext = new ActionHouseClient.Context(System.Configuration.ConfigurationManager.AppSettings["AHServiceAddress"]);

                    AuctionHouseModels.Buyer oAHBuyer = new AuctionHouseModels.Buyer() { Username = _oNewBuyer.UserName, Password = _oNewBuyer.Password, ContactInfo = _oNewBuyer.ContactInfo };

                    AuctionHouseModels.ResultCode oRes = oAHContext.AddBuyer(oAHBuyer);

                    // TODO encode the error
                    if (oRes.Code != 0)
                    {
                        ModelState.AddModelError(String.Empty, "Register user failed: " + oRes.Message);

                        return View();
                    }

                    // register in session
                    Session["BuyerID"] = oRes.ContextID;

					FormsAuthentication.SetAuthCookie( _oNewBuyer.UserName, false );

					return RedirectToAction( "Index", "Home" );					
				}
				catch (Exception exc)
				{
					ModelState.AddModelError( String.Empty, "An error occurs during the registration: " + exc.Message );
				}
			}

			return View();
		}
	}
}