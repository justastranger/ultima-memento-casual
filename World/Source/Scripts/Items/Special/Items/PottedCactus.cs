using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
    public class RewardPottedCactus : Item
    {
        public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

        private bool m_IsRewardItem;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsRewardItem
        {
            get { return m_IsRewardItem; }
            set { m_IsRewardItem = value; InvalidateProperties(); }
        }

        [Constructable]
        public RewardPottedCactus() : this(Utility.RandomMinMax(0x1E0F, 0x1E14))
        {
        }

        [Constructable]
        public RewardPottedCactus(int itemID) : base(itemID)
        {
            Weight = 5.0;
        }

        public RewardPottedCactus(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(1); // version

            writer.Write((bool)m_IsRewardItem);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            switch (version)
            {
                case 1:
                    m_IsRewardItem = reader.ReadBool();
                    break;
            }

        }
    }

    public class PottedCactusDeed : Item
    {
        public override int LabelNumber { get { return 1080407; } } // Potted Cactus Deed

        private bool m_IsRewardItem;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsRewardItem
        {
            get { return m_IsRewardItem; }
            set { m_IsRewardItem = value; InvalidateProperties(); }
        }

        [Constructable]
        public PottedCactusDeed() : base(0x14F0)
        {
            Weight = 1.0;
        }

        public PottedCactusDeed(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.CloseGump(typeof(InternalGump));
                from.SendGump(new InternalGump(this));
            }
            else
                from.SendLocalizedMessage(1042038); // You must have the object in your backpack to use it.
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version

            writer.Write((bool)m_IsRewardItem);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            m_IsRewardItem = reader.ReadBool();
        }

        private class InternalGump : Gump
        {
            private PottedCactusDeed m_Cactus;

            public InternalGump(PottedCactusDeed cactus) : base(100, 200)
            {
                m_Cactus = cactus;

                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;

                AddPage(0);
                AddBackground(0, 0, 425, 250, 0xA28);

                AddPage(1);
                AddLabel(45, 15, 0, "Choose a Potted Cactus:");

                AddItem(45, 75, 0x1E0F);
                AddButton(55, 50, 0x845, 0x846, 0x1E0F, GumpButtonType.Reply, 0);

                AddItem(105, 75, 0x1E10);
                AddButton(115, 50, 0x845, 0x846, 0x1E10, GumpButtonType.Reply, 0);

                AddItem(160, 75, 0x1E14);
                AddButton(175, 50, 0x845, 0x846, 0x1E14, GumpButtonType.Reply, 0);

                AddItem(220, 75, 0x1E11);
                AddButton(235, 50, 0x845, 0x846, 0x1E11, GumpButtonType.Reply, 0);

                AddItem(280, 75, 0x1E12);
                AddButton(295, 50, 0x845, 0x846, 0x1E12, GumpButtonType.Reply, 0);

                AddItem(340, 75, 0x1E13);
                AddButton(355, 50, 0x845, 0x846, 0x1E13, GumpButtonType.Reply, 0);
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (m_Cactus == null | m_Cactus.Deleted)
                    return;

                Mobile m = sender.Mobile;

                if (info.ButtonID >= 0x1E0F && info.ButtonID <= 0x1E14)
                {
                    RewardPottedCactus cactus = new RewardPottedCactus(info.ButtonID);
                    cactus.IsRewardItem = m_Cactus.IsRewardItem;

                    if (!m.PlaceInBackpack(cactus))
                    {
                        cactus.Delete();
                        m.SendLocalizedMessage(1078837); // Your backpack is full! Please make room and try again.
                    }
                    else
                        m_Cactus.Delete();
                }
            }
        }
    }
}
