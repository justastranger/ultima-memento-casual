using System;

namespace Server.Items
{
    public class Beeswax : Item
    {
        public override Catalogs DefaultCatalog { get { return Catalogs.Wax; } }

        [Constructable]
        public Beeswax() : this(1)
        {
        }

        [Constructable]
        public Beeswax(int amount) : base(0x1422)
        {
            Weight = 1.0;
            Stackable = true;
            Amount = amount;
            CoinPrice = 10;
        }

        public Beeswax(Serial serial) : base(serial)
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