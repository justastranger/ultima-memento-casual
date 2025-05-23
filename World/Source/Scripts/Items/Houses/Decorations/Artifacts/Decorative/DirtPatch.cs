using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    public class DirtPatch : Item
    {
        [Constructable]
        public DirtPatch() : base(0x0913)
        {
        }

        public DirtPatch(Serial serial) : base(serial)
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
