using System;
using Server;
using Server.Network;

namespace Server.Items
{
    public class RejuvinationAddonComponent : AddonComponent
    {
        public RejuvinationAddonComponent(int itemID) : base(itemID)
        {
        }

        public RejuvinationAddonComponent(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.BeginAction(typeof(RejuvinationAddonComponent)))
            {
                from.FixedEffect(0x373A, 1, 16);

                int random = Utility.Random(1, 4);

                if (random == 1 || random == 4)
                {
                    from.Hits = from.HitsMax;
                    SendLocalizedMessageTo(from, 500801); // A sense of warmth fills your body!
                }

                if (random == 2 || random == 4)
                {
                    from.Mana = from.ManaMax;
                    SendLocalizedMessageTo(from, 500802); // A feeling of power surges through your veins!
                }

                if (random == 3 || random == 4)
                {
                    from.Stam = from.StamMax;
                    SendLocalizedMessageTo(from, 500803); // You feel as though you've slept for days!
                }

                Timer.DelayCall(TimeSpan.FromHours(2.0), new TimerStateCallback(ReleaseUseLock_Callback), new object[] { from, random });
            }
        }

        public virtual void ReleaseUseLock_Callback(object state)
        {
            object[] states = (object[])state;

            Mobile from = (Mobile)states[0];
            int random = (int)states[1];

            from.EndAction(typeof(RejuvinationAddonComponent));

            if (random == 4)
            {
                from.Hits = from.HitsMax;
                from.Mana = from.ManaMax;
                from.Stam = from.StamMax;
                SendLocalizedMessageTo(from, 500807); // You feel completely rejuvinated!
            }
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

    public abstract class BaseRejuvinationAnkh : BaseAddon
    {
        public BaseRejuvinationAnkh()
        {
        }

        public override bool HandlesOnMovement { get { return true; } }

        private DateTime m_NextMessage;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m.Player && Utility.InRange(Location, m.Location, 3) && !Utility.InRange(Location, oldLocation, 3))
            {
                if (DateTime.Now >= m_NextMessage)
                {
                    if (Components.Count > 0)
                        ((AddonComponent)Components[0]).SendLocalizedMessageTo(m, 1010061); // An overwhelming sense of peace fills you.

                    m_NextMessage = DateTime.Now + TimeSpan.FromSeconds(25.0);
                }
            }
        }

        public BaseRejuvinationAnkh(Serial serial) : base(serial)
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

    public class RejuvinationAnkhWest : BaseRejuvinationAnkh
    {
        [Constructable]
        public RejuvinationAnkhWest()
        {
            AddComponent(new RejuvinationAddonComponent(0x3), 0, 0, 0);
            AddComponent(new RejuvinationAddonComponent(0x2), 0, 1, 0);
        }

        public RejuvinationAnkhWest(Serial serial) : base(serial)
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

    public class RejuvinationAnkhNorth : BaseRejuvinationAnkh
    {
        [Constructable]
        public RejuvinationAnkhNorth()
        {
            AddComponent(new RejuvinationAddonComponent(0x4), 0, 0, 0);
            AddComponent(new RejuvinationAddonComponent(0x5), 1, 0, 0);
        }

        public RejuvinationAnkhNorth(Serial serial) : base(serial)
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