using System;
using System.Collections.Generic;
using Server;
using System.Collections;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class Baker : BaseVendor
    {
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

        public override string TalkGumpTitle { get { return "The Art of Cooking"; } }
        public override string TalkGumpSubject { get { return "Cook"; } }

        public override NpcGuild NpcGuild { get { return NpcGuild.CulinariansGuild; } }

        [Constructable]
        public Baker() : base("the baker")
        {
            SetSkill(SkillName.Cooking, 75.0, 98.0);
            SetSkill(SkillName.Tasting, 36.0, 68.0);
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
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.Tavern, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Mill, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.None, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Mill, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Cook, ItemSalesInfo.World.None, null);
                }
            }

            public class InternalSellInfo : GenericSellInfo
            {
                public InternalSellInfo()
                {
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.Tavern, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Mill, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.None, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Mill, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.None, ItemSalesInfo.Market.Cook, ItemSalesInfo.World.None, null);
                }
            }
        }

        public Baker(Serial serial) : base(serial)
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