using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.ContextMenus;

namespace Server.Items
{
    public class Ankhs
    {
        public const int ResurrectRange = 4;
        public const int TitheRange = 4;
        public const int LockRange = 4;

        public static void GetContextMenuEntries(Mobile from, Item item, List<ContextMenuEntry> list)
        {
            if (from is PlayerMobile)
                list.Add(new LockKarmaEntry((PlayerMobile)from));

            list.Add(new ResurrectEntry(from, item));
            list.Add(new TitheEntry(from));
        }

        public static void Resurrect(Mobile m, Item item)
        {
            if (m.Alive)
                return;

            if (!m.InRange(item.GetWorldLocation(), ResurrectRange))
                m.SendLocalizedMessage(500446); // That is too far away.
            else if (m.Map != null && m.Map.CanFit(m.Location, 16, false, false))
            {
                int shrineType = 2;
                if (item is AltarDaemon)
                {
                    shrineType = 3;
                    if (item.ItemID == 0x6393 || item.ItemID == 0x6394) { shrineType = 10; }
                }
                else if (item is AltarEvil) { shrineType = 4; }
                else if (item is AltarSea)
                {
                    shrineType = 5;
                    if (item.ItemID == 0x4FB1 || item.ItemID == 0x4FB2) { shrineType = 9; }
                    else if (item.ItemID == 0x6395) { shrineType = 8; }
                }
                else if (item is AltarWizard) { shrineType = 6; }
                else if (item is AltarDurama) { shrineType = 11; }
                else if (item is AltarDryad) { shrineType = 12; }
                else if (item is AltarGargoyle) { shrineType = 7; }
                m.CloseGump(typeof(ResurrectCostGump));
                m.SendGump(new ResurrectCostGump(m, shrineType));
            }
            else
                m.SendLocalizedMessage(502391); // Thou can not be resurrected there!
        }

        private class ResurrectEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;
            private Item m_Item;

            public ResurrectEntry(Mobile mobile, Item item) : base(6195, ResurrectRange)
            {
                m_Mobile = mobile;
                m_Item = item;

                Enabled = !m_Mobile.Alive;
            }

            public override void OnClick()
            {
                Resurrect(m_Mobile, m_Item);
            }
        }

        private class LockKarmaEntry : ContextMenuEntry
        {
            private PlayerMobile m_Mobile;

            public LockKarmaEntry(PlayerMobile mobile) : base(mobile.KarmaLocked ? 6197 : 6196, LockRange)
            {
                m_Mobile = mobile;
            }

            public override void OnClick()
            {
                m_Mobile.KarmaLocked = !m_Mobile.KarmaLocked;

                if (m_Mobile.KarmaLocked)
                    m_Mobile.SendLocalizedMessage(1060192); // Your karma has been locked. Your karma can no longer be raised.
                else
                    m_Mobile.SendLocalizedMessage(1060191); // Your karma has been unlocked. Your karma can be raised again.
            }
        }

        private class TitheEntry : ContextMenuEntry
        {
            private Mobile m_Mobile;

            public TitheEntry(Mobile mobile) : base(6198, TitheRange)
            {
                m_Mobile = mobile;

                Enabled = m_Mobile.Alive;
            }

            public override void OnClick()
            {
                if (m_Mobile.CheckAlive())
                    m_Mobile.SendGump(new TithingGump(m_Mobile, 0));
            }
        }
    }

    public class AnkhWest : Item
    {
        private InternalItem m_Item;

        [Constructable]
        public AnkhWest() : this(false)
        {
        }

        [Constructable]
        public AnkhWest(bool bloodied) : base(bloodied ? 0x1D98 : 0x3)
        {
            Movable = false;

            m_Item = new InternalItem(bloodied, this);
        }

        public AnkhWest(Serial serial) : base(serial)
        {
        }

        public override bool HandlesOnMovement { get { return true; } } // Tell the core that we implement OnMovement

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (Parent == null && Utility.InRange(Location, m.Location, 1) && !Utility.InRange(Location, oldLocation, 1))
                Ankhs.Resurrect(m, this);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            Ankhs.GetContextMenuEntries(from, this, list);
        }

        [Hue, CommandProperty(AccessLevel.GameMaster)]
        public override int Hue
        {
            get { return base.Hue; }
            set { base.Hue = value; if (m_Item.Hue != value) m_Item.Hue = value; }
        }

        public override void OnDoubleClickDead(Mobile m)
        {
            Ankhs.Resurrect(m, this);
        }

        public override void OnLocationChange(Point3D oldLocation)
        {
            if (m_Item != null)
                m_Item.Location = new Point3D(X, Y + 1, Z);
        }

        public override void OnMapChange()
        {
            if (m_Item != null)
                m_Item.Map = Map;
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (m_Item != null)
                m_Item.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_Item);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Item = reader.ReadItem() as InternalItem;
        }

        private class InternalItem : Item
        {
            private AnkhWest m_Item;

            public InternalItem(bool bloodied, AnkhWest item) : base(bloodied ? 0x1D97 : 0x2)
            {
                Movable = false;

                m_Item = item;
            }

            public InternalItem(Serial serial) : base(serial)
            {
            }

            public override void OnLocationChange(Point3D oldLocation)
            {
                if (m_Item != null)
                    m_Item.Location = new Point3D(X, Y - 1, Z);
            }

            public override void OnMapChange()
            {
                if (m_Item != null)
                    m_Item.Map = Map;
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();

                if (m_Item != null)
                    m_Item.Delete();
            }

            public override bool HandlesOnMovement { get { return true; } } // Tell the core that we implement OnMovement

            public override void OnMovement(Mobile m, Point3D oldLocation)
            {
                if (Parent == null && Utility.InRange(Location, m.Location, 1) && !Utility.InRange(Location, oldLocation, 1))
                    Ankhs.Resurrect(m, this);
            }

            public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
            {
                base.GetContextMenuEntries(from, list);
                Ankhs.GetContextMenuEntries(from, this, list);
            }

            [Hue, CommandProperty(AccessLevel.GameMaster)]
            public override int Hue
            {
                get { return base.Hue; }
                set { base.Hue = value; if (m_Item.Hue != value) m_Item.Hue = value; }
            }

            public override void OnDoubleClickDead(Mobile m)
            {
                Ankhs.Resurrect(m, this);
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);

                writer.Write((int)0); // version

                writer.Write(m_Item);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);

                int version = reader.ReadInt();

                m_Item = reader.ReadItem() as AnkhWest;
            }
        }
    }

    [TypeAlias("Server.Items.AnkhEast")]
    public class AnkhNorth : Item
    {
        private InternalItem m_Item;

        [Constructable]
        public AnkhNorth() : this(false)
        {
        }

        [Constructable]
        public AnkhNorth(bool bloodied) : base(bloodied ? 0x1E5D : 0x4)
        {
            Movable = false;

            m_Item = new InternalItem(bloodied, this);
        }

        public AnkhNorth(Serial serial)
            : base(serial)
        {
        }

        public override bool HandlesOnMovement { get { return true; } } // Tell the core that we implement OnMovement

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (Parent == null && Utility.InRange(Location, m.Location, 1) && !Utility.InRange(Location, oldLocation, 1))
                Ankhs.Resurrect(m, this);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            Ankhs.GetContextMenuEntries(from, this, list);
        }

        [Hue, CommandProperty(AccessLevel.GameMaster)]
        public override int Hue
        {
            get { return base.Hue; }
            set { base.Hue = value; if (m_Item.Hue != value) m_Item.Hue = value; }
        }

        public override void OnDoubleClickDead(Mobile m)
        {
            Ankhs.Resurrect(m, this);
        }

        public override void OnLocationChange(Point3D oldLocation)
        {
            if (m_Item != null)
                m_Item.Location = new Point3D(X + 1, Y, Z);
        }

        public override void OnMapChange()
        {
            if (m_Item != null)
                m_Item.Map = Map;
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            if (m_Item != null)
                m_Item.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_Item);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Item = reader.ReadItem() as InternalItem;
        }

        [TypeAlias("Server.Items.AnkhEast+InternalItem")]
        private class InternalItem : Item
        {
            private AnkhNorth m_Item;

            public InternalItem(bool bloodied, AnkhNorth item)
                : base(bloodied ? 0x1E5C : 0x5)
            {
                Movable = false;

                m_Item = item;
            }

            public InternalItem(Serial serial) : base(serial)
            {
            }

            public override void OnLocationChange(Point3D oldLocation)
            {
                if (m_Item != null)
                    m_Item.Location = new Point3D(X - 1, Y, Z);
            }

            public override void OnMapChange()
            {
                if (m_Item != null)
                    m_Item.Map = Map;
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();

                if (m_Item != null)
                    m_Item.Delete();
            }

            public override bool HandlesOnMovement { get { return true; } } // Tell the core that we implement OnMovement

            public override void OnMovement(Mobile m, Point3D oldLocation)
            {
                if (Parent == null && Utility.InRange(Location, m.Location, 1) && !Utility.InRange(Location, oldLocation, 1))
                    Ankhs.Resurrect(m, this);
            }

            public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
            {
                base.GetContextMenuEntries(from, list);
                Ankhs.GetContextMenuEntries(from, this, list);
            }

            [Hue, CommandProperty(AccessLevel.GameMaster)]
            public override int Hue
            {
                get { return base.Hue; }
                set { base.Hue = value; if (m_Item.Hue != value) m_Item.Hue = value; }
            }

            public override void OnDoubleClickDead(Mobile m)
            {
                Ankhs.Resurrect(m, this);
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);

                writer.Write((int)0); // version

                writer.Write(m_Item);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);

                int version = reader.ReadInt();

                m_Item = reader.ReadItem() as AnkhNorth;
            }
        }
    }
}
