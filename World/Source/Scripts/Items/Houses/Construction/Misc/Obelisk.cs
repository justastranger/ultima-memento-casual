using System;

namespace Server.Items
{
    public class Obelisk : Item
    {
        public override int LabelNumber { get { return 1016474; } } // an obelisk

        [Constructable]
        public Obelisk() : base(0x1184)
        {
            Movable = false;
        }

        public Obelisk(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}