using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class Guillotine : Item
    {
        [Constructable]
        public Guillotine()
            : base(4656)
        {
            Movable = false;
        }

        private DateTime m_NextUse;

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(this.GetWorldLocation(), 2) || !from.InLOS(this))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that
            }
            else if (Visible && (ItemID == 4656 || ItemID == 4702) && DateTime.Now >= m_NextUse)
            {
                Point3D p = this.GetWorldLocation();

                if (1 > Utility.Random(Math.Max(Math.Abs(from.X - p.X), Math.Abs(from.Y - p.Y))))
                {
                    Effects.PlaySound(from.Location, from.Map, from.GetHurtSound());
                    from.PublicOverheadMessage(MessageType.Regular, from.SpeechHue, true, "Ouch!");
                    Spells.SpellHelper.Damage(TimeSpan.FromSeconds(0.5), from, Utility.Dice(2, 10, 5));
                }

                Effects.PlaySound(this.GetWorldLocation(), this.Map, 0x387);

                Timer.DelayCall(TimeSpan.FromSeconds(0.25), new TimerCallback(Down1));
                Timer.DelayCall(TimeSpan.FromSeconds(0.50), new TimerCallback(Down2));

                Timer.DelayCall(TimeSpan.FromSeconds(5.00), new TimerCallback(BackUp));

                m_NextUse = DateTime.Now + TimeSpan.FromSeconds(10.0);
            }
        }

        private void Down1()
        {
            ItemID = (ItemID == 4656 ? 4678 : 4712);
        }

        private void Down2()
        {
            ItemID = (ItemID == 4678 ? 4679 : 4713);

            Point3D p = this.GetWorldLocation();
            Map f = this.Map;

            if (f == null)
                return;

            new Blood(4650).MoveToWorld(p, f);

            for (int i = 0; i < 4; ++i)
            {
                int x = p.X - 2 + Utility.Random(5);
                int y = p.Y - 2 + Utility.Random(5);
                int z = p.Z;

                if (!f.CanFit(x, y, z, 1, false, false, true))
                {
                    z = f.GetAverageZ(x, y);

                    if (!f.CanFit(x, y, z, 1, false, false, true))
                        continue;
                }

                new Blood().MoveToWorld(new Point3D(x, y, z), f);
            }
        }

        private void BackUp()
        {
            if (ItemID == 4678 || ItemID == 4679)
                ItemID = 4656;
            else if (ItemID == 4712 || ItemID == 4713)
                ItemID = 4702;
        }

        public Guillotine(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((byte)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadByte();

            if (ItemID == 4678 || ItemID == 4679)
                ItemID = 4656;
            else if (ItemID == 4712 || ItemID == 4713)
                ItemID = 4702;
        }
    }
}