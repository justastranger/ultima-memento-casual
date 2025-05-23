using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SummonAirElementalScroll : SpellScroll
    {
        [Constructable]
        public SummonAirElementalScroll() : this(1)
        {
        }

        [Constructable]
        public SummonAirElementalScroll(int amount) : base(59, 0x1F68, amount)
        {
        }

        public SummonAirElementalScroll(Serial serial) : base(serial)
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