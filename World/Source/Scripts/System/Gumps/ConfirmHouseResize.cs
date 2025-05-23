using System;
using Server;
using Server.Items;
using Server.Multis;
using Server.Multis.Deeds;
using Server.Network;
using Server.Mobiles;

namespace Server.Gumps
{
    public class ConfirmHouseResize : Gump
    {
        private Mobile m_Mobile;
        private BaseHouse m_House;

        public ConfirmHouseResize(Mobile mobile, BaseHouse house) : base(110, 100)
        {
            m_Mobile = mobile;
            m_House = house;

            mobile.CloseGump(typeof(ConfirmHouseResize));

            Closable = false;

            AddPage(0);

            AddBackground(0, 0, 420, 280, 0x1453);
            AddImageTiled(10, 10, 400, 20, 0xA40);
            AddAlphaRegion(10, 10, 400, 20);
            AddHtmlLocalized(10, 10, 400, 20, 1060635, 0x7800, false, false); // <CENTER>WARNING</CENTER>
            AddImageTiled(10, 40, 400, 200, 0xA40);
            AddAlphaRegion(10, 40, 400, 200);

            /* You are attempting to resize your house. You will be refunded the house's 
			value directly to your bank box. All items in the house will *remain behind* 
			and can be *freely picked up by anyone*. Once the house is demolished, however, 
			only this account will be able to place on the land for one hour. This *will* 
			circumvent the normal 7-day waiting period (if it applies to you). This action 
			will not un-condemn any other houses on your account. If you have other, 
			grandfathered houses, this action *WILL* condemn them. Are you sure you wish 
			to continue?*/
            AddHtmlLocalized(10, 40, 400, 200, 1080196, 0x7F00, false, true);

            AddImageTiled(10, 250, 400, 20, 0xA40);
            AddAlphaRegion(10, 250, 400, 20);
            AddButton(10, 250, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
            AddButton(210, 250, 0xFA5, 0xFA7, 0, GumpButtonType.Reply, 0);
            AddHtmlLocalized(40, 250, 170, 20, 1011036, 0x7FFF, false, false); // OKAY
            AddHtmlLocalized(240, 250, 170, 20, 1011012, 0x7FFF, false, false); // CANCEL

        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info.ButtonID == 1 && !m_House.Deleted)
            {
                if (m_House.IsOwner(m_Mobile))
                {
                    if (m_House.MovingCrate != null || m_House.InternalizedVendors.Count > 0)
                    {
                        m_Mobile.SendLocalizedMessage(1080455); // You can not resize your house at this time. Please remove all items fom the moving crate and try again.
                        return;
                    }
                    else if (!Guilds.Guild.NewGuildSystem && m_House.FindGuildstone() != null)
                    {
                        m_Mobile.SendLocalizedMessage(501389); // You cannot redeed a house with a guildstone inside.
                        return;
                    }
                    else if (m_House.HasRentedVendors && m_House.VendorInventories.Count > 0)
                    {
                        m_Mobile.SendLocalizedMessage(1062679); // You cannot do that that while you still have contract vendors or unclaimed contract vendor inventory in your house.
                        return;
                    }
                    else if (m_House.HasRentedVendors)
                    {
                        m_Mobile.SendLocalizedMessage(1062680); // You cannot do that that while you still have contract vendors in your house.
                        return;
                    }
                    else if (m_House.VendorInventories.Count > 0)
                    {
                        m_Mobile.SendLocalizedMessage(1062681); // You cannot do that that while you still have unclaimed contract vendor inventory in your house.
                        return;
                    }

                    if (m_Mobile.AccessLevel >= AccessLevel.GameMaster)
                    {
                        m_Mobile.SendMessage("You do not get a refund for your house as you are not a player");
                        m_House.RemoveKeys(m_Mobile);
                        m_House.Delete();
                    }
                    else
                    {
                        Item toGive = null;

                        if (m_House.IsAosRules)
                        {
                            if (m_House.Price > 0)
                                toGive = new BankCheck(m_House.Price);
                            else
                                toGive = m_House.GetDeed();
                        }
                        else
                        {
                            toGive = m_House.GetDeed();

                            if (toGive == null && m_House.Price > 0)
                                toGive = new BankCheck(m_House.Price);
                        }

                        if (toGive != null)
                        {
                            BankBox box = m_Mobile.BankBox;

                            if (box.TryDropItem(m_Mobile, toGive, false))
                            {
                                if (toGive is BankCheck)
                                    m_Mobile.SendLocalizedMessage(1060397, ((BankCheck)toGive).Worth.ToString()); // ~1_AMOUNT~ gold has been deposited into your bank box.

                                m_House.RemoveKeys(m_Mobile);
                                new TempNoHousingRegion(m_House, m_Mobile);
                                m_House.Delete();
                            }
                            else
                            {
                                toGive.Delete();
                                m_Mobile.SendLocalizedMessage(500390); // Your bank box is full.
                            }
                        }
                        else
                        {
                            m_Mobile.SendMessage("Unable to refund house.");
                        }
                    }
                }
                else
                {
                    m_Mobile.SendLocalizedMessage(501320); // Only the house owner may do this.
                }
            }
            else if (info.ButtonID == 0)
            {
                m_Mobile.CloseGump(typeof(ConfirmHouseResize));
                m_Mobile.SendGump(new HouseGumpAOS(HouseGumpPageAOS.Customize, m_Mobile, m_House));
            }
        }
    }
}
