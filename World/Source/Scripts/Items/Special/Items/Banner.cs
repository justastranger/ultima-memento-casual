using System;
using Server;
using Server.Gumps;
using Server.Multis;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class Banner : Item, IAddon, IDyable
    {
        public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

        public Item Deed
        {
            get
            {
                BannerDeed deed = new BannerDeed();
                deed.IsRewardItem = m_IsRewardItem;

                return deed;
            }
        }

        private bool m_IsRewardItem;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsRewardItem
        {
            get { return m_IsRewardItem; }
            set { m_IsRewardItem = value; InvalidateProperties(); }
        }

        public bool FacingSouth
        {
            get { return (ItemID & 0x1) == 0; }
        }

        [Constructable]
        public Banner(int itemID) : base(itemID)
        {
        }

        public Banner(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version

            writer.Write((bool)m_IsRewardItem);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            m_IsRewardItem = reader.ReadBool();
        }

        public bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted)
                return false;

            Hue = sender.DyedHue;

            return true;
        }

        public bool CouldFit(IPoint3D p, Map map)
        {
            if (map == null || !map.CanFit(p.X, p.Y, p.Z, ItemData.Height))
                return false;

            if (FacingSouth)
                return BaseAddon.IsWall(p.X, p.Y - 1, p.Z, map); // north wall
            else
                return BaseAddon.IsWall(p.X - 1, p.Y, p.Z, map); // west wall
        }
    }

    public class BannerDeed : Item
    {
        public override int LabelNumber { get { return 1041007; } } // a banner deed

        private bool m_IsRewardItem;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsRewardItem
        {
            get { return m_IsRewardItem; }
            set { m_IsRewardItem = value; InvalidateProperties(); }
        }
        [Constructable]
        public BannerDeed() : base(0x14F0)
        {
            LootType = LootType.Blessed;
            Weight = 1.0;
        }

        public BannerDeed(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                BaseHouse house = BaseHouse.FindHouseAt(from);

                if (house != null && house.IsOwner(from))
                {
                    from.CloseGump(typeof(InternalGump));
                    from.SendGump(new InternalGump(this));
                }
                else
                    from.SendLocalizedMessage(502092); // You must be in your house to do this.
            }
            else
                from.SendLocalizedMessage(1042038); // You must have the object in your backpack to use it.          	
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version

            writer.Write((bool)m_IsRewardItem);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            m_IsRewardItem = reader.ReadBool();
        }

        private class InternalGump : Gump
        {
            public const int Start = 0x15AE;
            public const int End = 0x15F4;

            private BannerDeed m_Banner;

            public InternalGump(BannerDeed banner) : base(100, 200)
            {
                m_Banner = banner;

                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;

                AddPage(0);

                AddBackground(25, 0, 520, 230, 0xA28);
                AddLabel(70, 12, 0x3E3, "Choose a Banner:");


                int itemID = Start;

                for (int i = 1; i <= 4; i++)
                {
                    AddPage(i);

                    for (int j = 0; j < 8; j++, itemID += 2)
                    {
                        AddItem(50 + 60 * j, 70, itemID);
                        AddButton(50 + 60 * j, 50, 0x845, 0x846, itemID, GumpButtonType.Reply, 0);
                    }

                    if (i > 1)
                        AddButton(75, 198, 0x8AF, 0x8AF, 0, GumpButtonType.Page, i - 1);

                    if (i < 4)
                        AddButton(475, 198, 0x8B0, 0x8B0, 0, GumpButtonType.Page, i + 1);
                }
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (m_Banner == null | m_Banner.Deleted)
                    return;

                Mobile m = sender.Mobile;

                if (info.ButtonID >= Start && info.ButtonID <= End)
                {
                    if ((info.ButtonID & 0x1) == 0)
                    {
                        m.SendLocalizedMessage(1042037); // Where would you like to place this banner?
                        m.Target = new InternalTarget(m_Banner, info.ButtonID);
                    }
                }
            }
        }

        private class InternalTarget : Target
        {
            private BannerDeed m_Banner;
            private int m_ItemID;

            public InternalTarget(BannerDeed banner, int itemID) : base(-1, true, TargetFlags.None)
            {
                m_Banner = banner;
                m_ItemID = itemID;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Banner == null || m_Banner.Deleted)
                    return;

                if (m_Banner.IsChildOf(from.Backpack))
                {
                    BaseHouse house = BaseHouse.FindHouseAt(from);

                    if (house != null && house.IsOwner(from))
                    {
                        IPoint3D p = targeted as IPoint3D;
                        Map map = from.Map;

                        if (p == null || map == null)
                            return;

                        Point3D p3d = new Point3D(p);
                        ItemData id = TileData.ItemTable[m_ItemID & TileData.MaxItemValue];

                        if (map.CanFit(p3d, id.Height))
                        {
                            house = BaseHouse.FindHouseAt(p3d, map, id.Height);

                            if (house != null && house.IsOwner(from))
                            {
                                bool north = BaseAddon.IsWall(p3d.X, p3d.Y - 1, p3d.Z, map);
                                bool west = BaseAddon.IsWall(p3d.X - 1, p3d.Y, p3d.Z, map);

                                if (north && west)
                                {
                                    from.CloseGump(typeof(FacingGump));
                                    from.SendGump(new FacingGump(m_Banner, m_ItemID, p3d, house));
                                }
                                else if (north || west)
                                {
                                    Banner banner = null;

                                    if (north)
                                        banner = new Banner(m_ItemID);
                                    else if (west)
                                        banner = new Banner(m_ItemID + 1);

                                    house.Addons.Add(banner);

                                    banner.IsRewardItem = m_Banner.IsRewardItem;
                                    banner.MoveToWorld(p3d, map);

                                    m_Banner.Delete();
                                }
                                else
                                    from.SendLocalizedMessage(1042039); // The banner must be placed next to a wall.								
                            }
                            else
                                from.SendLocalizedMessage(1042036); // That location is not in your house.
                        }
                        else
                            from.SendLocalizedMessage(500269); // You cannot build that there.		
                    }
                    else
                        from.SendLocalizedMessage(502092); // You must be in your house to do this.
                }
                else
                    from.SendLocalizedMessage(1042038); // You must have the object in your backpack to use it.     
            }

            private class FacingGump : Gump
            {
                private BannerDeed m_Banner;
                private int m_ItemID;
                private Point3D m_Location;
                private BaseHouse m_House;

                private enum Buttons
                {
                    Cancel,
                    East,
                    South
                }

                public FacingGump(BannerDeed banner, int itemID, Point3D location, BaseHouse house) : base(150, 50)
                {
                    m_Banner = banner;
                    m_ItemID = itemID;
                    m_Location = location;
                    m_House = house;

                    Closable = true;
                    Disposable = true;
                    Dragable = true;
                    Resizable = false;

                    AddPage(0);

                    AddBackground(0, 0, 300, 150, 0xA28);

                    AddItem(90, 30, itemID + 1);
                    AddItem(180, 30, itemID);

                    AddButton(50, 35, 0x868, 0x869, (int)Buttons.East, GumpButtonType.Reply, 0);
                    AddButton(145, 35, 0x868, 0x869, (int)Buttons.South, GumpButtonType.Reply, 0);
                }

                public override void OnResponse(NetState sender, RelayInfo info)
                {
                    if (m_Banner == null || m_Banner.Deleted || m_House == null)
                        return;

                    Banner banner = null;

                    if (info.ButtonID == (int)Buttons.East)
                        banner = new Banner(m_ItemID + 1);
                    if (info.ButtonID == (int)Buttons.South)
                        banner = new Banner(m_ItemID);

                    if (banner != null)
                    {
                        m_House.Addons.Add(banner);

                        banner.IsRewardItem = m_Banner.IsRewardItem;
                        banner.MoveToWorld(m_Location, sender.Mobile.Map);

                        m_Banner.Delete();
                    }
                }
            }
        }
    }
}
