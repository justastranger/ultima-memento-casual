using System;

namespace Server.Items
{
    // [Flipable( 0x2B04, 0x2B05 )]
    public class RoyalCape : BaseCloak
    {
        [Constructable]
        public RoyalCape() : this(0)
        {
        }

        [Constructable]
        public RoyalCape(int hue) : base(0x2B04, hue)
        {
            Name = "royal cloak";
            Weight = 4.0;
        }

        public RoyalCape(Serial serial) : base(serial)
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