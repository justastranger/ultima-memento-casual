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
using Server.Regions;

namespace Server.Mobiles
{
    public class Healer : BaseHealer
    {
        public override bool CanTeach { get { return true; } }

        public override string TalkGumpTitle { get { return "Thou Art Going To Get Hurt"; } }
        public override string TalkGumpSubject { get { return "Healer"; } }

        public override bool CheckTeach(SkillName skill, Mobile from)
        {
            if (!base.CheckTeach(skill, from))
                return false;

            return (skill == SkillName.Forensics)
                || (skill == SkillName.Healing)
                || (skill == SkillName.Spiritualism)
                || (skill == SkillName.Swords);
        }

        [Constructable]
        public Healer()
        {
            Title = "the healer";

            if (!Core.AOS)
                NameHue = 0x35;

            SetSkill(SkillName.Forensics, 80.0, 100.0);
            SetSkill(SkillName.Spiritualism, 80.0, 100.0);
            SetSkill(SkillName.Swords, 80.0, 100.0);
        }

        public override bool IsActiveVendor { get { return true; } }

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
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Healer, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Healer, ItemSalesInfo.World.None, typeof(LesserHealPotion));
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Healer, ItemSalesInfo.World.None, typeof(Ginseng));
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Healer, ItemSalesInfo.World.None, typeof(Garlic));
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Healer, ItemSalesInfo.World.None, typeof(RefreshPotion));
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Healer, ItemSalesInfo.World.None, typeof(HealPotion));
                }
            }

            public class InternalSellInfo : GenericSellInfo
            {
                public InternalSellInfo()
                {
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Healer, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Healer, ItemSalesInfo.World.None, typeof(LesserHealPotion));
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Healer, ItemSalesInfo.World.None, typeof(Ginseng));
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Healer, ItemSalesInfo.World.None, typeof(Garlic));
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Healer, ItemSalesInfo.World.None, typeof(RefreshPotion));
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.All, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Healer, ItemSalesInfo.World.None, typeof(HealPotion));
                }
            }
        }

        public Healer(Serial serial) : base(serial)
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

            if (Core.AOS && NameHue == 0x35)
                NameHue = -1;
        }
    }
}