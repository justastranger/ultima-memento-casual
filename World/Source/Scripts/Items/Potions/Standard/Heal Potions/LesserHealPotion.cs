using System;
using Server;

namespace Server.Items
{
    public class LesserHealPotion : BaseHealPotion
    {
        public override int MinHeal { get { return 6; } }
        public override int MaxHeal { get { return 8; } }
        public override double Delay { get { return 3.0; } }

        [Constructable]
        public LesserHealPotion() : base(PotionEffect.HealLesser)
        {
            ItemID = 0x25FD;
            Name = "lesser heal potion";
        }

        public LesserHealPotion(Serial serial) : base(serial)
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
            Name = "lesser heal potion";
        }
    }
}
