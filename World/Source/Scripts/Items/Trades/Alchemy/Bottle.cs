using System;

namespace Server.Items
{
    public class Bottle : Item
    {
        public override string DefaultDescription { get { return "These bottles are often used by alchemists to store potions in."; } }

        [Constructable]
        public Bottle() : this(1)
        {
        }

        [Constructable]
        public Bottle(int amount) : base(0xF0E)
        {
            Stackable = true;
            Weight = 1.0;
            Amount = amount;
            Built = true;
        }

        public Bottle(Serial serial) : base(serial)
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

            Built = true;
        }
    }
}