using System;
using Server;

namespace Server.Items
{
    public class FancyElvenTableSouthAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new FancyElvenTableSouthDeed(); } }

        [Constructable]
        public FancyElvenTableSouthAddon()
        {
            AddComponent(new AddonComponent(0x3095), 0, 1, 0);
            AddComponent(new AddonComponent(0x3096), 0, 0, 0);
            AddComponent(new AddonComponent(0x3097), 0, -1, 0);
        }

        public FancyElvenTableSouthAddon(Serial serial) : base(serial)
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

    public class FancyElvenTableSouthDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new FancyElvenTableSouthAddon(); } }
        public override int LabelNumber { get { return 1073385; } } // hardwood table (south)

        [Constructable]
        public FancyElvenTableSouthDeed()
        {
        }

        public FancyElvenTableSouthDeed(Serial serial) : base(serial)
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