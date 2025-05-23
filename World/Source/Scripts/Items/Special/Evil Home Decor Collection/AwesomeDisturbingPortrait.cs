using System;
using Server;
using Server.Network;

namespace Server.Items
{
    [Flipable(0x2A5D, 0x2A61)]
    public class AwesomeDisturbingPortraitComponent : AddonComponent
    {
        public override int LabelNumber { get { return 1074479; } } // Disturbing portrait
        public bool FacingSouth { get { return ItemID < 0x2A61; } }

        private InternalTimer m_Timer;

        public AwesomeDisturbingPortraitComponent() : base(0x2A5D)
        {
            m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(1));
            m_Timer.Start();
        }

        public AwesomeDisturbingPortraitComponent(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Utility.InRange(Location, from.Location, 2))
            {
                int hours;
                int minutes;

                Clock.GetTime(Map, X, Y, out hours, out minutes);

                if (hours < 4 || hours > 20)
                    Effects.PlaySound(Location, Map, 0x569);

                UpdateImage();
            }
            else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (m_Timer != null && m_Timer.Running)
                m_Timer.Stop();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            m_Timer = new InternalTimer(this, TimeSpan.Zero);
            m_Timer.Start();
        }

        private void UpdateImage()
        {
            int hours;
            int minutes;

            Clock.GetTime(Map, X, Y, out hours, out minutes);

            if (FacingSouth)
            {
                if (hours < 4)
                    ItemID = 0x2A60;
                else if (hours < 6)
                    ItemID = 0x2A5F;
                else if (hours < 8)
                    ItemID = 0x2A5E;
                else if (hours < 16)
                    ItemID = 0x2A5D;
                else if (hours < 18)
                    ItemID = 0x2A5E;
                else if (hours < 20)
                    ItemID = 0x2A5F;
                else
                    ItemID = 0x2A60;
            }
            else
            {
                if (hours < 4)
                    ItemID = 0x2A64;
                else if (hours < 6)
                    ItemID = 0x2A63;
                else if (hours < 8)
                    ItemID = 0x2A62;
                else if (hours < 16)
                    ItemID = 0x2A61;
                else if (hours < 18)
                    ItemID = 0x2A62;
                else if (hours < 20)
                    ItemID = 0x2A63;
                else
                    ItemID = 0x2A64;
            }
        }

        private class InternalTimer : Timer
        {
            private AwesomeDisturbingPortraitComponent m_Component;

            public InternalTimer(AwesomeDisturbingPortraitComponent c, TimeSpan delay) : base(delay, TimeSpan.FromMinutes(10))
            {
                m_Component = c;

                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                if (m_Component != null && !m_Component.Deleted)
                    m_Component.UpdateImage();
            }
        }
    }

    public class AwesomeDisturbingPortraitAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new AwesomeDisturbingPortraitDeed(); } }

        [Constructable]
        public AwesomeDisturbingPortraitAddon() : base()
        {
            AddComponent(new AwesomeDisturbingPortraitComponent(), 0, 0, 0);
        }

        public AwesomeDisturbingPortraitAddon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }

    public class AwesomeDisturbingPortraitDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new AwesomeDisturbingPortraitAddon(); } }

        [Constructable]
        public AwesomeDisturbingPortraitDeed() : base()
        {
            Name = "box containing a disturbing portrait";
            ItemID = Utility.RandomList(0x3420, 0x3425);
            Hue = Utility.RandomEvilHue();
            Weight = 5.0;
        }

        public AwesomeDisturbingPortraitDeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            if (ItemID != 0x3420 && ItemID != 0x3425) { ItemID = 0x3425; }
        }
    }
}
