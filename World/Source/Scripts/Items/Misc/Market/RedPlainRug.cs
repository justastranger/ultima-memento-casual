using System;

namespace Server.Items
{
    public class RedPlainRugAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new RedPlainRugDeed(); } }

        [Constructable]
        public RedPlainRugAddon() : base()
        {
            AddComponent(new LocalizedAddonComponent(0xAC9, 1076588), 1, 1, 0);
            AddComponent(new LocalizedAddonComponent(0xACA, 1076588), -1, -1, 0);
            AddComponent(new LocalizedAddonComponent(0xACB, 1076588), -1, 1, 0);
            AddComponent(new LocalizedAddonComponent(0xACC, 1076588), 1, -1, 0);
            AddComponent(new LocalizedAddonComponent(0xACD, 1076588), -1, 0, 0);
            AddComponent(new LocalizedAddonComponent(0xACE, 1076588), 0, -1, 0);
            AddComponent(new LocalizedAddonComponent(0xACF, 1076588), 1, 0, 0);
            AddComponent(new LocalizedAddonComponent(0xAD0, 1076588), 0, 1, 0);
            AddComponent(new LocalizedAddonComponent(0xAC6, 1076588), 0, 0, 0);
        }

        public RedPlainRugAddon(Serial serial) : base(serial)
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

    public class RedPlainRugDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new RedPlainRugAddon(); } }
        public override int LabelNumber { get { return 1076588; } } // Red plain rug

        [Constructable]
        public RedPlainRugDeed() : base()
        {
            LootType = LootType.Blessed;
        }

        public RedPlainRugDeed(Serial serial) : base(serial)
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
