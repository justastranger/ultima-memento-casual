using System;

namespace Server.Items
{
    [Flipable(0x3D8E, 0x3D8F)]
    public class LargeFishingNetComponent : AddonComponent
    {
        public override int LabelNumber { get { return 1076285; } } // Large Fish Net

        public LargeFishingNetComponent() : base(0x3D8E)
        {
        }

        public LargeFishingNetComponent(Serial serial) : base(serial)
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

    public class LargeFishingNetAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new LargeFishingNetDeed(); } }

        [Constructable]
        public LargeFishingNetAddon() : base()
        {
            AddComponent(new LargeFishingNetComponent(), 0, 0, 0);
        }

        public LargeFishingNetAddon(Serial serial) : base(serial)
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

    public class LargeFishingNetDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new LargeFishingNetAddon(); } }
        public override int LabelNumber { get { return 1076285; } } // Large Fish Net

        [Constructable]
        public LargeFishingNetDeed() : base()
        {
            LootType = LootType.Blessed;
        }

        public LargeFishingNetDeed(Serial serial) : base(serial)
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
