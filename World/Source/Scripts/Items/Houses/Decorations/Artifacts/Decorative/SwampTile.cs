using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    public class SwampTile : Item
    {
        [Constructable]
        public SwampTile() : base(0x320D)
        {
        }

        public SwampTile(Serial serial) : base(serial)
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
