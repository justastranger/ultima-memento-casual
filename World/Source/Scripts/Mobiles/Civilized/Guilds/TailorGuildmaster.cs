using System;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class TailorGuildmaster : BaseGuildmaster
    {
        public override NpcGuild NpcGuild { get { return NpcGuild.TailorsGuild; } }

        [Constructable]
        public TailorGuildmaster() : base("tailor")
        {
            SetSkill(SkillName.Tailoring, 90.0, 100.0);
        }

        public override void InitSBInfo(Mobile m)
        {
            m_Merchant = m;
            SBInfos.Add(new MyStock());
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
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.None, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Tailor, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.None, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Leather, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.None, ItemSalesInfo.Material.Cloth, ItemSalesInfo.Market.Tailor, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.Armor, ItemSalesInfo.Material.Leather, ItemSalesInfo.Market.All, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.Weapon, ItemSalesInfo.Material.Leather, ItemSalesInfo.Market.All, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.Rare, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Tailor, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.Resource, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Tailor, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.Resource, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Leather, ItemSalesInfo.World.None, null);
                }
            }

            public class InternalSellInfo : GenericSellInfo
            {
                public InternalSellInfo()
                {
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.None, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Tailor, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.None, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Leather, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.None, ItemSalesInfo.Material.Cloth, ItemSalesInfo.Market.Tailor, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.Armor, ItemSalesInfo.Material.Leather, ItemSalesInfo.Market.All, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.Weapon, ItemSalesInfo.Material.Leather, ItemSalesInfo.Market.All, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.Rare, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Tailor, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.Resource, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Tailor, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.Resource, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Leather, ItemSalesInfo.World.None, null);
                }
            }
        }




        public TailorGuildmaster(Serial serial) : base(serial)
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