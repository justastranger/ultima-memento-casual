using System;
using Server;

namespace Server.Items
{
    public class StoneFireplaceSouthAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new StoneFireplaceSouthDeed(); } }

        [Constructable]
        public StoneFireplaceSouthAddon()
        {
            AddComponent(new AddonComponent(0x967), -1, 0, 0);
            AddComponent(new AddonComponent(0x961), 0, 0, 0);
        }

        public StoneFireplaceSouthAddon(Serial serial) : base(serial)
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

    public class StoneFireplaceSouthDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new StoneFireplaceSouthAddon(); } }
        public override int LabelNumber { get { return 1061849; } } // stone fireplace (south)

        [Constructable]
        public StoneFireplaceSouthDeed()
        {
        }

        public StoneFireplaceSouthDeed(Serial serial) : base(serial)
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