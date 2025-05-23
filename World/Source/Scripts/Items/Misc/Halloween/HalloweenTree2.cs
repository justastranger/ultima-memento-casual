using System;

namespace Server.Items
{
    [Furniture]
    public class HalloweenTree2 : Item
    {
        [Constructable]
        public HalloweenTree2() : base(0xD94)
        {
            Weight = 1.0;
            Name = "Tree";
            Hue = 0x2C5;
        }

        public HalloweenTree2(Serial serial) : base(serial)
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

            if (Weight == 4.0)
                Weight = 1.0;
        }
    }
}