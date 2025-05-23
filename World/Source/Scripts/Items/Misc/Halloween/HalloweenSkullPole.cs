using System;

namespace Server.Items
{
    [Furniture]
    public class HalloweenSkullPole : Item
    {
        [Constructable]
        public HalloweenSkullPole() : base(0x2204)
        {
            Weight = 1.0;
            Name = "Skull Pole";
        }

        public HalloweenSkullPole(Serial serial) : base(serial)
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