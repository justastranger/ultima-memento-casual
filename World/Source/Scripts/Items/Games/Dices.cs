using System;
using Server;
using Server.Network;

namespace Server.Items
{
    public class Dices : Item, ITelekinesisable
    {
        [Constructable]
        public Dices() : base(0xFA7)
        {
            Weight = 1.0;
            Hue = 0x982;
        }

        public Dices(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(this.GetWorldLocation(), 2))
                return;

            Roll(from);
        }

        public void OnTelekinesis(Mobile from)
        {
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5022);
            Effects.PlaySound(Location, Map, 0x1F5);

            Roll(from);
        }

        public void Roll(Mobile from)
        {
            from.PublicOverheadMessage(MessageType.Regular, 0, false, string.Format("*{0} rolls a {1} and {2}*", from.Name, Utility.Random(1, 6), Utility.Random(1, 6)));
            from.PlaySound(0x34);
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
            if (Hue < 1) { Hue = 0x982; }
        }
    }
}