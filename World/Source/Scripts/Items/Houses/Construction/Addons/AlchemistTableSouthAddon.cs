using System;
using Server;

namespace Server.Items
{
    public class AlchemistTableSouthAddon : BaseAddon
    {
        public override string AddonName { get { return "alchemy table"; } }

        public override BaseAddonDeed Deed { get { return new AlchemistTableSouthDeed(); } }

        [Constructable]
        public AlchemistTableSouthAddon()
        {
            AddComponent(new AddonComponent(0x2DD4), 0, 0, 0);
        }

        public AlchemistTableSouthAddon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }

    public class AlchemistTableSouthDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new AlchemistTableSouthAddon(); } }
        public override int LabelNumber { get { return 1073396; } } // alchemist table (south)

        [Constructable]
        public AlchemistTableSouthDeed()
        {
        }

        public AlchemistTableSouthDeed(Serial serial) : base(serial)
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