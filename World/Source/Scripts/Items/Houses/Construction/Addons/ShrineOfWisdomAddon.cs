using System;
using Server;

namespace Server.Items
{
    public class ShrineOfWisdomAddon : BaseAddon
    {
        [Constructable]
        public ShrineOfWisdomAddon()
        {
            AddComponent(new ShrineOfWisdomComponent(0x14C3), 0, 0, 0);
            AddComponent(new ShrineOfWisdomComponent(0x14C6), 1, 0, 0);
            AddComponent(new ShrineOfWisdomComponent(0x14D4), 0, 1, 0);
            AddComponent(new ShrineOfWisdomComponent(0x14D5), 1, 1, 0);
            Hue = 0x47E;
        }

        public ShrineOfWisdomAddon(Serial serial) : base(serial)
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

    [Server.Engines.Craft.Forge]
    public class ShrineOfWisdomComponent : AddonComponent
    {
        public override int LabelNumber { get { return 1062046; } } // Shrine of Wisdom

        [Constructable]
        public ShrineOfWisdomComponent(int itemID) : base(itemID)
        {
        }

        public ShrineOfWisdomComponent(Serial serial) : base(serial)
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