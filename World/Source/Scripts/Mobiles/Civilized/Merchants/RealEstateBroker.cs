using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Multis;
using Server.Multis.Deeds;
using Server.Targeting;
using Server.Network;

namespace Server.Mobiles
{
    public class RealEstateBroker : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        public override NpcGuild NpcGuild { get { return NpcGuild.MerchantsGuild; } }

        [Constructable]
        public RealEstateBroker() : base("the architect")
        {
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.Alive && from.InRange(this, 3))
                return true;

            return base.HandlesOnSpeech(from);
        }

        private DateTime m_NextCheckPack;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (DateTime.Now > m_NextCheckPack && InRange(m, 4) && !InRange(oldLocation, 4) && m.Player)
            {
                Container pack = m.Backpack;

                if (pack != null)
                {
                    m_NextCheckPack = DateTime.Now + TimeSpan.FromSeconds(2.0);

                    Item deed = pack.FindItemByType(typeof(HouseDeed), false);

                    if (deed != null)
                    {
                        // If you have a deed, I can appraise it or buy it from you...
                        PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500605, m.NetState);

                        // Simply hand me a deed to sell it.
                        PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500606, m.NetState);
                    }
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (!e.Handled && e.Mobile.Alive && e.HasKeyword(0x38)) // *appraise*
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, 500608); // Which deed would you like appraised?
                e.Mobile.BeginTarget(12, false, TargetFlags.None, new TargetCallback(Appraise_OnTarget));
                e.Handled = true;
            }

            base.OnSpeech(e);
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is HouseDeed)
            {
                HouseDeed deed = (HouseDeed)dropped;
                int price = ComputePriceFor(deed);

                if (price > 0)
                {
                    if (Banker.Deposit(from, price))
                    {
                        // For the deed I have placed gold in your bankbox : 
                        PublicOverheadMessage(MessageType.Regular, 0x3B2, 1008000, AffixType.Append, price.ToString(), "");

                        deed.Delete();
                        return true;
                    }
                    else
                    {
                        PublicOverheadMessage(MessageType.Regular, 0x3B2, 500390); // Your bank box is full.
                        return false;
                    }
                }
                else
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, 500607); // I'm not interested in that.
                    return false;
                }
            }

            return base.OnDragDrop(from, dropped);
        }

        public void Appraise_OnTarget(Mobile from, object obj)
        {
            if (obj is HouseDeed)
            {
                HouseDeed deed = (HouseDeed)obj;
                int price = ComputePriceFor(deed);

                if (price > 0)
                {
                    // I will pay you gold for this deed : 
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, 1008001, AffixType.Append, price.ToString(), "");

                    PublicOverheadMessage(MessageType.Regular, 0x3B2, 500610); // Simply hand me the deed if you wish to sell it.
                }
                else
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, 500607); // I'm not interested in that.
                }
            }
            else
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, 500609); // I can't appraise things I know nothing about...
            }
        }

        public int ComputePriceFor(HouseDeed deed)
        {
            int price = 0;

            if (deed is SmallBrickHouseDeed || deed is StonePlasterHouseDeed || deed is FieldStoneHouseDeed || deed is SmallBrickHouseDeed || deed is WoodHouseDeed || deed is WoodPlasterHouseDeed || deed is ThatchedRoofCottageDeed)
                price = 43800;
            else if (deed is BrickHouseDeed)
                price = 144500;
            else if (deed is TwoStoryWoodPlasterHouseDeed || deed is TwoStoryStonePlasterHouseDeed)
                price = 192400;
            else if (deed is TowerDeed)
                price = 433200;
            else if (deed is KeepDeed)
                price = 665200;
            else if (deed is CastleDeed)
                price = 1022800;
            else if (deed is LargePatioDeed)
                price = 152800;
            else if (deed is LargeMarbleDeed)
                price = 192800;
            else if (deed is SmallTowerDeed)
                price = 88500;
            else if (deed is LogCabinDeed)
                price = 97800;
            else if (deed is SandstonePatioDeed)
                price = 90900;
            else if (deed is VillaDeed)
                price = 136500;
            else if (deed is StoneWorkshopDeed)
                price = 60600;
            else if (deed is MarbleWorkshopDeed)
                price = 60300;

            return AOS.Scale(price, 80); // refunds 80% of the purchase price
        }

        public override void InitSBInfo(Mobile m)
        {
            m_Merchant = m;
            m_SBInfos.Add(new MyStock());
        }

        public class MyStock : SBInfo
        {
            private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
            private IShopSellInfo m_SellInfo = new InternalSellInfo();

            public MyStock()
            {
            }

            public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
            public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

            public class InternalBuyInfo : List<GenericBuyInfo>
            {
                public InternalBuyInfo()
                {
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Home, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.Christmas, ItemSalesInfo.Material.All, ItemSalesInfo.Market.All, ItemSalesInfo.World.None, null);
                }
            }

            public class InternalSellInfo : GenericSellInfo
            {
                public InternalSellInfo()
                {
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Home, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.Christmas, ItemSalesInfo.Material.All, ItemSalesInfo.Market.All, ItemSalesInfo.World.None, null);
                }
            }
        }

        public RealEstateBroker(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}