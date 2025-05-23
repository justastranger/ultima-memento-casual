using System;
using Server;
using Server.Targeting;
using Server.Commands;

namespace Server.Items
{
    [FlipableAttribute(0x1f14, 0x1f15, 0x1f16, 0x1f17)]
    public class WayPoint : Item
    {
        public static void Initialize()
        {
            CommandSystem.Register("WayPointSeq", AccessLevel.GameMaster, new CommandEventHandler(WayPointSeq_OnCommand));
        }

        public static void WayPointSeq_OnCommand(CommandEventArgs arg)
        {
            arg.Mobile.SendMessage("Target the position of the first way point.");
            arg.Mobile.Target = new WayPointSeqTarget(null);
        }

        private WayPoint m_Next;

        public override string DefaultName
        {
            get { return "AI Way Point"; }
        }

        [Constructable]
        public WayPoint() : base(0x1f14)
        {
            this.Hue = 0x498;
            this.Visible = false;
            //this.Movable = false;
        }

        public WayPoint(WayPoint prev) : this()
        {
            if (prev != null)
                prev.NextPoint = this;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public WayPoint NextPoint
        {
            get
            {
                return m_Next;
            }
            set
            {
                if (m_Next != this)
                    m_Next = value;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel >= AccessLevel.GameMaster)
            {
                from.SendMessage("Target the next way point in the sequence.");

                from.Target = new NextPointTarget(this);
            }
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            if (m_Next == null)
                LabelTo(from, "(Unlinked)");
            else
                LabelTo(from, "(Linked: {0})", m_Next.Location);
        }

        public WayPoint(Serial serial) : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Next = reader.ReadItem() as WayPoint;
                        break;
                    }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write(m_Next);
        }
    }

    public class NextPointTarget : Target
    {
        private WayPoint m_Point;

        public NextPointTarget(WayPoint pt) : base(-1, false, TargetFlags.None)
        {
            m_Point = pt;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (target is WayPoint && m_Point != null)
            {
                m_Point.NextPoint = (WayPoint)target;
            }
            else
            {
                from.SendMessage("Target a way point.");
            }
        }
    }

    public class WayPointSeqTarget : Target
    {
        private WayPoint m_Last;

        public WayPointSeqTarget(WayPoint last) : base(-1, true, TargetFlags.None)
        {
            m_Last = last;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is WayPoint)
            {
                if (m_Last != null)
                    m_Last.NextPoint = (WayPoint)targeted;
            }
            else if (targeted is IPoint3D)
            {
                Point3D p = new Point3D((IPoint3D)targeted);

                WayPoint point = new WayPoint(m_Last);
                point.MoveToWorld(p, from.Map);

                from.Target = new WayPointSeqTarget(point);
                from.SendMessage("Target the position of the next way point in the sequence, or target a way point link the newest way point to.");
            }
            else
            {
                from.SendMessage("Target a position, or another way point.");
            }
        }
    }
}
