using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AuctionHouseModels;

namespace AuctionHouseService
{
    public class AHService : IAHService
    {
        #region Data Management

        private static object ms_oSyncRoot = new object();
        private static bool ms_bDataLoaded = false;

        private static List<Buyer> ms_aBuyers = new List<Buyer>();
        private static List<Item> ms_aItems = new List<Item>();
        private static List<Bid> ms_aBids = new List<Bid>();

        public static List<Buyer> Buyers {get {lock(ms_oSyncRoot) {return ms_aBuyers;}}}
        public static List<Item> Items { get { lock (ms_oSyncRoot) { return ms_aItems; } } }
        public static List<Bid> Bids { get { lock (ms_oSyncRoot) { return ms_aBids; } } }

        //System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath

        static AHService()
        {
            lock (ms_oSyncRoot)
            {
                if (ms_bDataLoaded) return;

                ms_bDataLoaded = true;

                try
                {
                    // load buyers
                    string szData = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "App_Data\\buyer.json";

                    if (System.IO.File.Exists( szData ))
                    {
                        using (System.IO.StreamReader oFStream = new System.IO.StreamReader( szData, Encoding.UTF8 ))
                        {
                            string szValue = oFStream.ReadToEnd();

                            ms_aBuyers = Helper.Deserialize<List<Buyer>>(szValue);
                        }
                    }

                    // load items
                    szData = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "App_Data\\item.json";

                    if (System.IO.File.Exists(szData))
                    {
                        using (System.IO.StreamReader oFStream = new System.IO.StreamReader(szData, Encoding.UTF8))
                        {
                            string szValue = oFStream.ReadToEnd();

                            ms_aItems = Helper.Deserialize<List<Item>>(szValue);
                        }
                    }

                    // load bids
                    szData = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "App_Data\\bid.json";

                    if (System.IO.File.Exists(szData))
                    {
                        using (System.IO.StreamReader oFStream = new System.IO.StreamReader(szData, Encoding.UTF8))
                        {
                            string szValue = oFStream.ReadToEnd();

                            ms_aBids = Helper.Deserialize<List<Bid>>(szValue);
                        }
                    }
                }
                // temporary ignore error
                catch (Exception exc)
                {
                }
            }
        }

        private static void SaveAHData()
        {
            lock (ms_oSyncRoot)
            {
                try
                {
                    // buyer
                    string szData = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "App_Data\\buyer.json";

                    if (ms_aBuyers.Count > 0)
                    {
                        using (System.IO.StreamWriter oFStream = new System.IO.StreamWriter(szData, false, Encoding.UTF8 ))
                        {
                            oFStream.Write(Helper.Serialize<List<Buyer>>(ms_aBuyers));
                        }
                    }

                    // items
                    szData = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "App_Data\\item.json";

                    if (ms_aItems.Count > 0)
                    {
                        using (System.IO.StreamWriter oFStream = new System.IO.StreamWriter(szData, false, Encoding.UTF8))
                        {
                            oFStream.Write(Helper.Serialize<List<Item>>(ms_aItems));
                        }
                    }

                    // bids
                    szData = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "App_Data\\bid.json";

                    if (ms_aBids.Count > 0)
                    {
                        using (System.IO.StreamWriter oFStream = new System.IO.StreamWriter(szData, false, Encoding.UTF8))
                        {
                            oFStream.Write(Helper.Serialize<List<Bid>>(ms_aBids));
                        }
                    }
                }
                // temporary ignore error
                catch { }
            }
        }

        #endregion

        #region Buyer Management

        public List<Buyer> QueryBuyers()
        {
            return Buyers;
        }

        public Buyer QueryBuyer(string _szID)
        {
            try
            {
                for ( int i=0; i<Buyers.Count; i++ )
                {
                    if (Buyers[i].ID == _szID) return Buyers[i];
                }
            }
            catch (Exception exc)
            {
                // TODO register error
            }

            return null;
        }

        public Buyer VerifyBuyer(string _szUsername, string _szPassword)
        {
            try
            {
                for (int i = 0; i < Buyers.Count; i++)
                {
                    if (String.Compare(Buyers[i].Username, _szUsername, true) == 0 &&
                        String.Compare(Buyers[i].Password, _szPassword, true) == 0) return Buyers[i];
                }
            }
            catch (Exception exc)
            {
                // TODO register error
            }

            return null;
        }

        public ResultCode AddBuyer(Buyer _oBuyer)
        {
            try
            {
                for (int i = 0; i < Buyers.Count; i++)
                {
                    if (String.Compare(Buyers[i].Username, _oBuyer.Username, true) == 0)
                    {
                        // TODO map error code
                        return new ResultCode() { Code = -2, ContextID = String.Empty, Message = "Username already used" };
                    }
                }

                Buyers.Add(_oBuyer);

                SaveAHData();

                return new ResultCode() { Code = 0, ContextID = _oBuyer.ID, Message = "Success" };
            }
            catch (Exception exc)
            {
                // TODO register error

                return new ResultCode() { Code = -1, ContextID = String.Empty, Message = "EXC: " + exc.Message };
            }
        }

        public ResultCode ModifyBuyer(Buyer _oBuyer)
        {
            try
            {
                for (int i = 0; i < Buyers.Count; i++)
                {
                    if (Buyers[i].ID == _oBuyer.ID)
                    {
                        Buyers[i] = _oBuyer;

                        break;
                    }
                }

                SaveAHData();

                return new ResultCode() { Code = 0, ContextID = _oBuyer.ID, Message = "Success" };
            }
            catch (Exception exc)
            {
                // TODO register error

                return new ResultCode() { Code = -1, ContextID = String.Empty, Message = "EXC: " + exc.Message };
            }
        }

        #endregion

        #region Item Management

        public List<Item> QueryItems()
        {
            return Items;
        }
                
        public Item QueryItem(string _szID)
        {
            try
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].ID == _szID) return Items[i];
                }
            }
            // TODO handle error
            catch { }

            return null;
        }

        public ResultCode AddItem(Item _oItem)
        {
            try
            {
                Items.Add(_oItem);

                SaveAHData();

                return new ResultCode() { Code = 0, ContextID = _oItem.ID, Message = "Success" };
            }
            catch (Exception exc)
            {
                // TODO register error

                return new ResultCode() { Code = -1, ContextID = String.Empty, Message = "EXC: " + exc.Message };
            }
        }

        public ResultCode ModifyItem(Item _oItem)
        {
            try
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].ID == _oItem.ID)
                    {
                        // TODO image change (from client)
                        Items[i].Name = _oItem.Name;
                        Items[i].Description = _oItem.Description;

                        if (!Items[i].AuctionEnabled)
                        {
                            Items[i].AuctionEnabled = _oItem.AuctionEnabled;
                            Items[i].AuctionExpiry = _oItem.AuctionExpiry;
                        }

                        break;
                    }
                }

                SaveAHData();

                return new ResultCode() { Code = 0, ContextID = _oItem.ID, Message = "Success" };
            }
            catch  (Exception exc)
            {
                // TODO register error

                return new ResultCode() { Code = -1, ContextID = String.Empty, Message = "EXC: " + exc.Message };
            }
        }

        #endregion

        #region Bid Management

        public List<Bid> QueryItemBids(string _szItemID)
        {
            List<Bid> aBids = new List<Bid>();

            try
            {
                for (int i = 0; i < Bids.Count; i++)
                {
                    if (Bids[i].ItemID == _szItemID) aBids.Add( Bids[i] );
                }
            }
            // TODO handle error
            catch { }

            aBids.Sort(
                delegate(Bid o1, Bid o2)
                {
                    if (o1 == o2) return 0;

                    if (o1.BidValue > o2.BidValue)
                    {
                        return -1;
                    }
                    else if (o1.BidValue < o2.BidValue)
                    {
                        return 1;
                    }
                    else
                    {
                        return o1.CreateOn.CompareTo( o2.CreateOn );
                    }
                });
                        
            return aBids;
        }

        public List<Bid> QueryBuyerBids(string _szBuyerID)
        {
            List<Bid> aBids = new List<Bid>();

            try
            {
                for (int i = 0; i < Bids.Count; i++)
                {
                    if (Bids[i].BuyerID == _szBuyerID) aBids.Add(Bids[i]);
                }
            }
            // TODO handle error
            catch { }

            aBids.Sort(
                delegate(Bid o1, Bid o2)
                {
                    if (o1 == o2) return 0;

                    return o1.CreateOn.CompareTo(o2.CreateOn);
                });

            return aBids;
        }

        //public List<Item> QueryBuyerOutbidItems(string _szBuyerID)
        //{
        //    return new List<Item>();
        //}

        public ResultCode AddBid(string _szItemID, string _szBuyerID, string _szPrice, string _szAnonymous)
        {
            int iBidValue = -1;

            try
            {
                iBidValue = Convert.ToInt32(_szPrice);
            }
            catch {}

            if (iBidValue <= 0)
                return new ResultCode() { Code = -5, ContextID = String.Empty, Message = "Invalid params" };

            // validate the specified bid
            // check buyer
            if (QueryBuyer( _szBuyerID ) == null)
                return new ResultCode() { Code = -6, ContextID = String.Empty, Message = "Unknown buyer" };

            // check item
            Item oItem = QueryItem( _szItemID );

            if (oItem == null)
                return new ResultCode() { Code = -6, ContextID = String.Empty, Message = "Unknown item" };

            // check auction started
            if (!oItem.AuctionEnabled)
                return new ResultCode() { Code = -7, ContextID = String.Empty, Message = "Auction not started yet" };

            // check auction expired
            if (oItem.AuctionExpiry < DateTime.Now)
                return new ResultCode() { Code = -10, ContextID = String.Empty, Message = "Auction has expired" };
            
            List<Bid> aBids = QueryItemBids(_szItemID);

            if (aBids.Count > 0)
            {
                // check if the winning bid outbid the specified value
                if (aBids[0].BidValue > iBidValue)
                    return new ResultCode() { Code = -8, ContextID = String.Empty, Message = "Bid not high enought" };

                if (aBids[0].BuyerID == _szBuyerID)
                    return new ResultCode() { Code = -9, ContextID = String.Empty, Message = "Buyer owns the current higher bid" };
            }

            Bids.Add(new Bid() { ItemID = _szItemID, Anonymous = (_szAnonymous == "1"), BidValue = iBidValue, BuyerID = _szBuyerID });

            SaveAHData();

            return new ResultCode() { Code = 0, ContextID = String.Empty, Message = "Success" };
        }

        #endregion
    }
}
