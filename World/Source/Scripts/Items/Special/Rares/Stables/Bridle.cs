using System;

namespace Server.Items
{
    public class DecoBridle : Item
    {

        [Constructable]
        public DecoBridle() : base(0x1374)
        {
            Movable = true;
            Stackable = false;
        }

        public DecoBridle(Serial serial) : base(serial)
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
