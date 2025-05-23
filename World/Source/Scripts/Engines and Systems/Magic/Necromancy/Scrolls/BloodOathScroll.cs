using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class BloodOathScroll : SpellScroll
    {
        public override string DefaultDescription { get { return NecromancerSpellbook.SpellDescription(101); } }

        [Constructable]
        public BloodOathScroll() : this(1)
        {
        }

        [Constructable]
        public BloodOathScroll(int amount) : base(101, 0x2261, amount)
        {
            Name = "blood oath scroll";
        }

        public BloodOathScroll(Serial serial) : base(serial)
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
            Name = "blood oath scroll";
        }
    }
}