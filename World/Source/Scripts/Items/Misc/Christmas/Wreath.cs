using System;
using Server;
using Server.Gumps;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class WreathAddon : Item, IDyable, IAddon
    {
        [Constructable]
        public WreathAddon() : this(Utility.RandomDyedHue())
        {
        }

        [Constructable]
        public WreathAddon(int hue) : base(0x232C)
        {
            Hue = hue;
            Movable = false;
        }

        public WreathAddon(Serial serial) : base(serial)
        {
        }

        public bool CouldFit(IPoint3D p, Map map)
        {
            if (!map.CanFit(p.X, p.Y, p.Z, this.ItemData.Height))
                return false;

            if (this.ItemID == 0x232C)
                return BaseAddon.IsWall(p.X, p.Y - 1, p.Z, map); // North wall
            else
                return BaseAddon.IsWall(p.X - 1, p.Y, p.Z, map); // West wall
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

            Timer.DelayCall(TimeSpan.Zero, new TimerCallback(FixMovingCrate));
        }

        private void FixMovingCrate()
        {
            if (this.Deleted)
                return;

            if (this.Movable || this.IsLockedDown)
            {
                Item deed = this.Deed;

                if (this.Parent is Item)
                {
                    ((Item)this.Parent).AddItem(deed);
                    deed.Location = this.Location;
                }
                else
                {
                    deed.MoveToWorld(this.Location, this.Map);
                }

                Delete();
            }
        }

        public Item Deed
        {
            get { return new WreathDeed(this.Hue); }
        }

        public override void OnDoubleClick(Mobile from)
        {
            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (house != null && house.IsCoOwner(from))
            {
                if (from.InRange(this.GetWorldLocation(), 3))
                {
                    from.CloseGump(typeof(WreathAddonGump));
                    from.SendGump(new WreathAddonGump(from, this));
                }
                else
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                }
            }
        }

        public virtual bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;

            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (house != null && house.IsCoOwner(from))
            {
                if (from.InRange(GetWorldLocation(), 1))
                {
                    Hue = sender.DyedHue;
                    return true;
                }
                else
                {
                    from.SendLocalizedMessage(500295); // You are too far away to do that.
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private class WreathAddonGump : Gump
        {
            private Mobile m_From;
            private WreathAddon m_Addon;

            public WreathAddonGump(Mobile from, WreathAddon addon) : base(150, 50)
            {
                m_From = from;
                m_Addon = addon;

                AddPage(0);

                AddBackground(0, 0, 220, 170, 0x1453);
                AddBackground(10, 10, 200, 150, 0xBB8);
                AddHtmlLocalized(20, 30, 180, 60, 1062839, false, false); // Do you wish to re-deed this decoration?
                AddHtmlLocalized(55, 100, 160, 25, 1011011, false, false); // CONTINUE
                AddButton(20, 100, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
                AddHtmlLocalized(55, 125, 160, 25, 1011012, false, false); // CANCEL
                AddButton(20, 125, 0xFA5, 0xFA7, 0, GumpButtonType.Reply, 0);
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (m_Addon.Deleted)
                    return;

                if (info.ButtonID == 1)
                {
                    if (m_From.InRange(m_Addon.GetWorldLocation(), 3))
                    {
                        m_From.AddToBackpack(m_Addon.Deed);
                        m_Addon.Delete();
                    }
                    else
                    {
                        m_From.SendLocalizedMessage(500295); // You are too far away to do that.
                    }
                }
            }
        }
    }

    [Flipable(0x14F0, 0x14EF)]
    public class WreathDeed : Item
    {
        public override int LabelNumber { get { return 1062837; } } // holiday wreath deed

        [Constructable]
        public WreathDeed() : this(Utility.RandomDyedHue())
        {
        }

        [Constructable]
        public WreathDeed(int hue) : base(0x14F0)
        {
            Weight = 1.0;
            Hue = hue;
            LootType = LootType.Blessed;
        }

        public WreathDeed(Serial serial) : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                BaseHouse house = BaseHouse.FindHouseAt(from);

                if (house != null && house.IsCoOwner(from))
                {
                    from.SendLocalizedMessage(1062838); // Where would you like to place this decoration?
                    from.BeginTarget(-1, true, TargetFlags.None, new TargetStateCallback(Placement_OnTarget), null);
                }
                else
                {
                    from.SendLocalizedMessage(502092); // You must be in your house to do this.
                }
            }
            else
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
        }

        public void Placement_OnTarget(Mobile from, object targeted, object state)
        {
            IPoint3D p = targeted as IPoint3D;

            if (p == null)
                return;

            Point3D loc = new Point3D(p);

            BaseHouse house = BaseHouse.FindHouseAt(loc, from.Map, 16);

            if (house != null && house.IsCoOwner(from))
            {
                bool northWall = BaseAddon.IsWall(loc.X, loc.Y - 1, loc.Z, from.Map);
                bool westWall = BaseAddon.IsWall(loc.X - 1, loc.Y, loc.Z, from.Map);

                if (northWall && westWall)
                    from.SendGump(new WreathDeedGump(from, loc, this));
                else
                    PlaceAddon(from, loc, northWall, westWall);
            }
            else
            {
                from.SendLocalizedMessage(1042036); // That location is not in your house.
            }
        }

        private void PlaceAddon(Mobile from, Point3D loc, bool northWall, bool westWall)
        {
            if (Deleted)
                return;

            BaseHouse house = BaseHouse.FindHouseAt(loc, from.Map, 16);

            if (house == null || !house.IsCoOwner(from))
            {
                from.SendLocalizedMessage(1042036); // That location is not in your house.
                return;
            }

            int itemID = 0;

            if (northWall)
                itemID = 0x232C;
            else if (westWall)
                itemID = 0x232D;
            else
                from.SendLocalizedMessage(1062840); // The decoration must be placed next to a wall.

            if (itemID > 0)
            {
                Item addon = new WreathAddon(this.Hue);

                addon.ItemID = itemID;
                addon.MoveToWorld(loc, from.Map);

                house.Addons.Add(addon);
                Delete();
            }
        }

        private class WreathDeedGump : Gump
        {
            private Mobile m_From;
            private Point3D m_Loc;
            private WreathDeed m_Deed;

            public WreathDeedGump(Mobile from, Point3D loc, WreathDeed deed) : base(150, 50)
            {
                m_From = from;
                m_Loc = loc;
                m_Deed = deed;

                AddBackground(0, 0, 300, 150, 0xA28);

                AddPage(0);

                AddItem(90, 30, 0x232D);
                AddItem(180, 30, 0x232C);
                AddButton(50, 35, 0x868, 0x869, 1, GumpButtonType.Reply, 0);
                AddButton(145, 35, 0x868, 0x869, 2, GumpButtonType.Reply, 0);
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (m_Deed.Deleted)
                    return;

                switch (info.ButtonID)
                {
                    case 1:
                        m_Deed.PlaceAddon(m_From, m_Loc, false, true);
                        break;
                    case 2:
                        m_Deed.PlaceAddon(m_From, m_Loc, true, false);
                        break;
                }
            }
        }
    }
}