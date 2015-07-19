using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AuctionHouse.Controllers
{
    public class AHBuyerController : Controller
    {		
        // GET: AHBuyer
        public ActionResult Index()
        {
            return View();
        }

		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Login( AuctionHouse.Models.AHBuyer _oBuyer )
		{
			if (ModelState.IsValid)
			{
                ActionHouseClient.Context oAHContext = new ActionHouseClient.Context( System.Configuration.ConfigurationManager.AppSettings["AHServiceAddress"] );

                AuctionHouseModels.Buyer oVerifiedBuyer = oAHContext.VerifyBuyer(_oBuyer.UserName, _oBuyer.Password);

                if (oVerifiedBuyer != null)
				{
                    // register in session
                    Session["BuyerID"] = oVerifiedBuyer.ID;
                    
					FormsAuthentication.SetAuthCookie( _oBuyer.UserName, false );

                    return RedirectToAction( "Index", "Home" );
				}
				else
				{
					ModelState.AddModelError( String.Empty, "Incorrect login data" );
				}
			}

			return View();
		}

		public ActionResult Logout()
		{
            // unregister ID from session
            Session["BuyerID"] = null;

			FormsAuthentication.SignOut();

			return RedirectToAction( "Index", "Home" );
		}
    }
}