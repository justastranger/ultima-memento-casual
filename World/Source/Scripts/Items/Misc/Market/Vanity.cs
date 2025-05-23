using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
    public class VanityAddon : BaseAddonContainer
    {
        public override BaseAddonContainerDeed Deed { get { return new VanityDeed(); } }
        public override int LabelNumber { get { return 1074027; } } // Vanity
        public override int DefaultGumpID { get { return 0x51; } }
        public override int DefaultDropSound { get { return 0x42; } }

        [Constructable]
        public VanityAddon(bool east) : base(east ? 0xA44 : 0xA3C)
        {
            if (east) // east
            {
                AddComponent(new AddonContainerComponent(0xA45), 0, -1, 0);
            }
            else        // south
            {
                AddComponent(new AddonContainerComponent(0xA3D), -1, 0, 0);
            }
        }

        public VanityAddon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }

    public class VanityDeed : BaseAddonContainerDeed
    {
        public override BaseAddonContainer Addon { get { return new VanityAddon(m_East); } }
        public override int LabelNumber { get { return 1074027; } } // Vanity

        private bool m_East;

        [Constructable]
        public VanityDeed() : base()
        {
            LootType = LootType.Blessed;
        }

        public VanityDeed(Serial serial) : base(serial)
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
                from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.
        }

        private void SendTarget(Mobile m)
        {
            base.OnDoubleClick(m);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }

        private class InternalGump : Gump
        {
            private VanityDeed m_Deed;

            public InternalGump(VanityDeed deed) : base(60, 36)
            {
                m_Deed = deed;

                AddPage(0);

                AddBackground(0, 0, 273, 324, 0x1453);
                AddImageTiled(10, 10, 253, 20, 0xA40);
                AddImageTiled(10, 40, 253, 244, 0xA40);
                AddImageTiled(10, 294, 253, 20, 0xA40);
                AddAlphaRegion(10, 10, 253, 304);
                AddButton(10, 294, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0);
                AddHtmlLocalized(45, 296, 450, 20, 1060051, 0x7FFF, false, false); // CANCEL
                AddHtmlLocalized(14, 12, 273, 20, 1076744, 0x7FFF, false, false); // Please select your vanity position.

                AddPage(1);

                AddButton(19, 49, 0x845, 0x846, 1, GumpButtonType.Reply, 0);
                AddHtmlLocalized(44, 47, 213, 20, 1075386, 0x7FFF, false, false); // South
                AddButton(19, 73, 0x845, 0x846, 2, GumpButtonType.Reply, 0);
                AddHtmlLocalized(44, 71, 213, 20, 1075387, 0x7FFF, false, false); // East
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (m_Deed == null || m_Deed.Deleted || info.ButtonID == 0)
                    return;

                m_Deed.m_East = (info.ButtonID != 1);
                m_Deed.SendTarget(sender.Mobile);
            }
        }
    }
}
