using System;

namespace Server.Items
{
    public class PinkFancyRugAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new PinkFancyRugDeed(); } }

        [Constructable]
        public PinkFancyRugAddon() : base()
        {
            AddComponent(new LocalizedAddonComponent(0xAEE, 1076590), 1, 1, 0);
            AddComponent(new LocalizedAddonComponent(0xAEF, 1076590), -1, -1, 0);
            AddComponent(new LocalizedAddonComponent(0xAF0, 1076590), -1, 1, 0);
            AddComponent(new LocalizedAddonComponent(0xAF1, 1076590), 1, -1, 0);
            AddComponent(new LocalizedAddonComponent(0xAF2, 1076590), -1, 0, 0);
            AddComponent(new LocalizedAddonComponent(0xAF3, 1076590), 0, -1, 0);
            AddComponent(new LocalizedAddonComponent(0xAF4, 1076590), 1, 0, 0);
            AddComponent(new LocalizedAddonComponent(0xAF5, 1076590), 0, 1, 0);
            AddComponent(new LocalizedAddonComponent(0xAEC, 1076590), 0, 0, 0);
        }

        public PinkFancyRugAddon(Serial serial) : base(serial)
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

    public class PinkFancyRugDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new PinkFancyRugAddon(); } }
        public override int LabelNumber { get { return 1076590; } } // Pink fancy rug

        [Constructable]
        public PinkFancyRugDeed() : base()
        {
            LootType = LootType.Blessed;
        }

        public PinkFancyRugDeed(Serial serial) : base(serial)
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
