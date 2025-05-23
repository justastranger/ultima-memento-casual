using System;
using Server;

namespace Server.Items
{
    public class ElvenStoveSouthAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new ElvenStoveSouthDeed(); } }

        [Constructable]
        public ElvenStoveSouthAddon()
        {
            AddComponent(new AddonComponent(0x2DDC), 0, 0, 0);
        }

        public ElvenStoveSouthAddon(Serial serial) : base(serial)
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

    public class ElvenStoveSouthDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new ElvenStoveSouthAddon(); } }
        public override int LabelNumber { get { return 1073394; } } // elven oven (south)

        [Constructable]
        public ElvenStoveSouthDeed()
        {
        }

        public ElvenStoveSouthDeed(Serial serial) : base(serial)
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
}