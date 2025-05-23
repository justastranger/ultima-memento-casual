using System;

namespace Server.Items
{
    public class BluePlainRugAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new BluePlainRugDeed(); } }

        [Constructable]
        public BluePlainRugAddon() : base()
        {
            AddComponent(new LocalizedAddonComponent(0xAC2, 1076585), 1, 1, 0);
            AddComponent(new LocalizedAddonComponent(0xAC3, 1076585), -1, -1, 0);
            AddComponent(new LocalizedAddonComponent(0xAC4, 1076585), -1, 1, 0);
            AddComponent(new LocalizedAddonComponent(0xAC5, 1076585), 1, -1, 0);
            AddComponent(new LocalizedAddonComponent(0xAF6, 1076585), -1, 0, 0);
            AddComponent(new LocalizedAddonComponent(0xAF7, 1076585), 0, -1, 0);
            AddComponent(new LocalizedAddonComponent(0xAF8, 1076585), 1, 0, 0);
            AddComponent(new LocalizedAddonComponent(0xAF9, 1076585), 0, 1, 0);
            AddComponent(new LocalizedAddonComponent(0xAC0, 1076585), 0, 0, 0);
        }

        public BluePlainRugAddon(Serial serial) : base(serial)
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

    public class BluePlainRugDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new BluePlainRugAddon(); } }
        public override int LabelNumber { get { return 1076585; } } // Blue plain rug

        [Constructable]
        public BluePlainRugDeed() : base()
        {
            LootType = LootType.Blessed;
        }

        public BluePlainRugDeed(Serial serial) : base(serial)
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
