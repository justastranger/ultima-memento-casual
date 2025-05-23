using System;

namespace Server.Items
{
    [FlipableAttribute(0xFAF, 0xFB0)]
    [Server.Engines.Craft.Anvil]
    public class Anvil : Item
    {
        [Constructable]
        public Anvil() : base(0xFAF)
        {
            Weight = 100;
        }

        public Anvil(Serial serial) : base(serial)
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
    public class Forge : Item
    {
        [Constructable]
        public Forge() : base(0xFB1)
        {
            Movable = false;
            Light = LightType.Circle225;
        }

        public Forge(Serial serial) : base(serial)
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