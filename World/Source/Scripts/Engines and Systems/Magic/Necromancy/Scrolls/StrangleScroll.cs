using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class StrangleScroll : SpellScroll
    {
        public override string DefaultDescription { get { return NecromancerSpellbook.SpellDescription(110); } }

        [Constructable]
        public StrangleScroll() : this(1)
        {
        }

        [Constructable]
        public StrangleScroll(int amount) : base(110, 0x226A, amount)
        {
            Name = "strangle scroll";
        }

        public StrangleScroll(Serial serial) : base(serial)
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
            Name = "strangle scroll";
        }
    }
}