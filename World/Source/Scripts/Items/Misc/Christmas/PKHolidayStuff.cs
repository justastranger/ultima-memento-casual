using System;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Items
{
    public class Coal : Item
    {
        public override string DefaultName { get { return "Coal"; } }

        [Constructable]
        public Coal() : base(0x19b9)
        {
            Stackable = false;
            LootType = LootType.Blessed;
            Hue = 0x965;
        }

        public Coal(Serial serial) : base(serial)
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

    public class BadCard : Item
    {
        public override int LabelNumber { get { return 1041428; } } // Maybe next year youll get a better...

        [Constructable]
        public BadCard() : base(0x14ef)
        {
            int[] m_CardHues = new int[] { 0x45, 0x27, 0x3d0 };
            Hue = m_CardHues[Utility.Random(m_CardHues.Length)];
            Stackable = false;
            LootType = LootType.Blessed;
            Movable = true;
        }

        public BadCard(Serial serial) : base(serial)
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

    public class Spam : Food
    {
        [Constructable]
        public Spam() : base(0x1044)
        {
            Stackable = false;
            LootType = LootType.Blessed;
        }

        public Spam(Serial serial) : base(serial)
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
