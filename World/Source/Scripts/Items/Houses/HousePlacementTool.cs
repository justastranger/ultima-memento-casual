using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Multis;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class HousePlacementTool : Item
    {
        [Constructable]
        public HousePlacementTool() : base(0x14F0)
        {
            Weight = 1.0;
            Name = "construction contract";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!MySettings.S_AllowCustomHomes)
                from.SendGump(new HousePlacementListGump(from, HousePlacementEntry.ClassicHouses));
            else if (IsChildOf(from.Backpack))
                from.SendGump(new HousePlacementCategoryGump(from));
            else
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
        }

        public HousePlacementTool(Serial serial) : base(serial)
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

            if (Weight == 0.0)
                Weight = 3.0;
        }
    }

    public class HousePlacementCategoryGump : Gump
    {
        private Mobile m_From;

        private const int LabelColor = 0x7FFF;
        private const int LabelColorDisabled = 0x4210;

        public HousePlacementCategoryGump(Mobile from) : base(50, 50)
        {
            m_From = from;

            from.CloseGump(typeof(HousePlacementCategoryGump));
            from.CloseGump(typeof(HousePlacementListGump));

            AddPage(0);

            AddBackground(0, 0, 270, 145, 0x1453);

            AddImageTiled(10, 10, 250, 125, 2624);
            AddAlphaRegion(10, 10, 250, 125);

            AddHtmlLocalized(10, 10, 250, 20, 1060239, LabelColor, false, false); // <CENTER>CONSTRUCTION CONTRACT</CENTER>

            AddButton(10, 110, 4017, 4019, 0, GumpButtonType.Reply, 0);
            AddHtmlLocalized(45, 110, 150, 20, 3000363, LabelColor, false, false); // Close

            AddButton(10, 40, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddHtmlLocalized(45, 40, 200, 20, 1060390, LabelColor, false, false); // Classic Houses

            if (MySettings.S_AllowCustomHomes)
            {
                AddButton(10, 60, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddHtmlLocalized(45, 60, 200, 20, 1060391, LabelColor, false, false); // 2-Story Customizable Houses

                AddButton(10, 80, 4005, 4007, 3, GumpButtonType.Reply, 0);
                AddHtmlLocalized(45, 80, 200, 20, 1060392, LabelColor, false, false); // 3-Story Customizable Houses
            }
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            if (!m_From.CheckAlive() || m_From.Backpack == null || m_From.Backpack.FindItemByType(typeof(HousePlacementTool)) == null)
                return;

            switch (info.ButtonID)
            {
                case 1: // Classic Houses
                    {
                        m_From.SendGump(new HousePlacementListGump(m_From, HousePlacementEntry.ClassicHouses));
                        break;
                    }
                case 2: // 2-Story Customizable Houses
                    {
                        m_From.SendGump(new HousePlacementListGump(m_From, HousePlacementEntry.TwoStoryFoundations));
                        break;
                    }
                case 3: // 3-Story Customizable Houses
                    {
                        m_From.SendGump(new HousePlacementListGump(m_From, HousePlacementEntry.ThreeStoryFoundations));
                        break;
                    }
            }
        }
    }

    public class HousePlacementListGump : Gump
    {
        private Mobile m_From;
        private HousePlacementEntry[] m_Entries;

        private const int LabelColor = 0x7FFF;
        private const int LabelHue = 0x480;

        public HousePlacementListGump(Mobile from, HousePlacementEntry[] entries) : base(50, 50)
        {
            m_From = from;
            m_Entries = entries;

            from.CloseGump(typeof(HousePlacementCategoryGump));
            from.CloseGump(typeof(HousePlacementListGump));

            AddPage(0);

            AddBackground(0, 0, 520, 420, 0x1453);

            AddImageTiled(10, 10, 500, 20, 2624);
            AddAlphaRegion(10, 10, 500, 20);

            AddHtmlLocalized(10, 10, 500, 20, 1060239, LabelColor, false, false); // <CENTER>CONSTRUCTION CONTRACT</CENTER>

            AddImageTiled(10, 40, 500, 20, 2624);
            AddAlphaRegion(10, 40, 500, 20);

            AddHtmlLocalized(50, 40, 225, 20, 1060235, LabelColor, false, false); // House Description
            AddHtmlLocalized(275, 40, 75, 20, 1060236, LabelColor, false, false); // Storage
            AddHtmlLocalized(350, 40, 75, 20, 1060237, LabelColor, false, false); // Lockdowns
            AddHtmlLocalized(425, 40, 75, 20, 1060034, LabelColor, false, false); // Cost

            AddImageTiled(10, 70, 500, 280, 2624);
            AddAlphaRegion(10, 70, 500, 280);

            AddImageTiled(10, 360, 500, 20, 2624);
            AddAlphaRegion(10, 360, 500, 20);

            AddHtmlLocalized(10, 360, 250, 20, 1060645, LabelColor, false, false); // Bank Balance:
            AddLabel(250, 360, LabelHue, Banker.GetBalance(from).ToString());

            AddImageTiled(10, 390, 500, 20, 2624);
            AddAlphaRegion(10, 390, 500, 20);

            AddButton(10, 390, 4017, 4019, 0, GumpButtonType.Reply, 0);
            AddHtmlLocalized(50, 390, 100, 20, 3000363, LabelColor, false, false); // Close

            for (int i = 0; i < entries.Length; ++i)
            {
                int page = 1 + (i / 14);
                int index = i % 14;

                if (index == 0)
                {
                    if (page > 1)
                    {
                        AddButton(450, 390, 4005, 4007, 0, GumpButtonType.Page, page);
                        AddHtmlLocalized(400, 390, 100, 20, 3000406, LabelColor, false, false); // Next
                    }

                    AddPage(page);

                    if (page > 1)
                    {
                        AddButton(200, 390, 4014, 4016, 0, GumpButtonType.Page, page - 1);
                        AddHtmlLocalized(250, 390, 100, 20, 3000405, LabelColor, false, false); // Previous
                    }
                }

                HousePlacementEntry entry = entries[i];

                int y = 70 + (index * 20);

                AddButton(10, y, 4005, 4007, 1 + i, GumpButtonType.Reply, 0);
                AddHtmlLocalized(50, y, 225, 20, entry.Description, LabelColor, false, false);
                AddLabel(275, y, LabelHue, entry.Storage.ToString());
                AddLabel(350, y, LabelHue, entry.Lockdowns.ToString());
                AddLabel(425, y, LabelHue, entry.Cost.ToString());
            }
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            if (!m_From.CheckAlive() || m_From.Backpack == null || m_From.Backpack.FindItemByType(typeof(HousePlacementTool)) == null)
                return;

            int index = info.ButtonID - 1;

            if (index >= 0 && index < m_Entries.Length)
            {
                if (m_From.AccessLevel < AccessLevel.GameMaster && BaseHouse.HasAccountHouse(m_From))
                    m_From.SendLocalizedMessage(501271); // You already own a house, you may not place another!
                else
                    m_From.Target = new NewHousePlacementTarget(m_Entries, m_Entries[index]);
            }
            else
            {
                if (MySettings.S_AllowCustomHomes)
                    m_From.SendGump(new HousePlacementCategoryGump(m_From));
            }
        }
    }

    public class NewHousePlacementTarget : MultiTarget
    {
        private HousePlacementEntry m_Entry;
        private HousePlacementEntry[] m_Entries;

        private bool m_Placed;

        public NewHousePlacementTarget(HousePlacementEntry[] entries, HousePlacementEntry entry) : base(entry.MultiID, entry.Offset)
        {
            Range = 14;

            m_Entries = entries;
            m_Entry = entry;
        }

        protected override void OnTarget(Mobile from, object o)
        {
            if (!from.CheckAlive() || from.Backpack == null || from.Backpack.FindItemByType(typeof(HousePlacementTool)) == null)
                return;

            IPoint3D ip = o as IPoint3D;

            if (ip != null)
            {
                if (ip is Item)
                    ip = ((Item)ip).GetWorldTop();

                Point3D p = new Point3D(ip);

                Region reg = Region.Find(new Point3D(p), from.Map);

                if (from.AccessLevel >= AccessLevel.GameMaster || reg.AllowHousing(from, p))
                    m_Placed = m_Entry.OnPlacement(from, p);
                else
                    from.SendLocalizedMessage(501265); // Housing can not be created in this area.
            }
        }

        protected override void OnTargetFinish(Mobile from)
        {
            if (!from.CheckAlive() || from.Backpack == null || from.Backpack.FindItemByType(typeof(HousePlacementTool)) == null)
                return;

            if (!m_Placed)
                from.SendGump(new HousePlacementListGump(from, m_Entries));
        }
    }

    public class HousePlacementEntry
    {
        private Type m_Type;
        private int m_Description;
        private int m_Storage;
        private int m_Lockdowns;
        private int m_NewStorage;
        private int m_NewLockdowns;
        private int m_Vendors;
        private int m_Cost;
        private int m_MultiID;
        private Point3D m_Offset;

        public Type Type { get { return m_Type; } }

        public int Description { get { return m_Description; } }
        public int Storage { get { return BaseHouse.NewVendorSystem ? m_NewStorage : m_Storage; } }
        public int Lockdowns { get { return BaseHouse.NewVendorSystem ? m_NewLockdowns : m_Lockdowns; } }
        public int Vendors { get { return m_Vendors; } }
        public int Cost { get { return m_Cost; } }

        public int MultiID { get { return m_MultiID; } }
        public Point3D Offset { get { return m_Offset; } }

        public HousePlacementEntry(Type type, int description, int storage, int lockdowns, int newStorage, int newLockdowns, int vendors, int cost, int xOffset, int yOffset, int zOffset, int multiID)
        {
            m_Type = type;
            m_Description = description;
            m_Storage = storage;
            m_Lockdowns = lockdowns;
            m_NewStorage = newStorage;
            m_NewLockdowns = newLockdowns;
            m_Vendors = vendors;
            m_Cost = cost;

            m_Offset = new Point3D(xOffset, yOffset, zOffset);

            m_MultiID = multiID;
        }

        public BaseHouse ConstructHouse(Mobile from)
        {
            try
            {
                object[] args;

                if (m_Type == typeof(HouseFoundation))
                    args = new object[4] { from, m_MultiID, m_Storage, m_Lockdowns };
                else if (m_Type == typeof(SmallOldHouse) || m_Type == typeof(SmallShop) || m_Type == typeof(TwoStoryHouse))
                    args = new object[2] { from, m_MultiID };
                else
                    args = new object[1] { from };

                return Activator.CreateInstance(m_Type, args) as BaseHouse;
            }
            catch
            {
            }

            return null;
        }

        public void PlacementWarning_Callback(Mobile from, bool okay, object state)
        {
            if (!from.CheckAlive() || from.Backpack == null || from.Backpack.FindItemByType(typeof(HousePlacementTool)) == null)
                return;

            PreviewHouse prevHouse = (PreviewHouse)state;

            if (!okay)
            {
                prevHouse.Delete();
                return;
            }

            if (prevHouse.Deleted)
            {
                /* Too much time has passed and the test house you created has been deleted.
				 * Please try again!
				 */
                from.SendGump(new NoticeGump(1060637, 30720, 1060647, 32512, 320, 180, null, null));

                return;
            }

            Point3D center = prevHouse.Location;
            Map map = prevHouse.Map;

            prevHouse.Delete();

            ArrayList toMove;
            //Point3D center = new Point3D( p.X - m_Offset.X, p.Y - m_Offset.Y, p.Z - m_Offset.Z );
            HousePlacementResult res = HousePlacement.Check(from, m_MultiID, center, out toMove);

            switch (res)
            {
                case HousePlacementResult.Valid:
                    {
                        if (from.AccessLevel < AccessLevel.GameMaster && BaseHouse.HasAccountHouse(from))
                        {
                            from.SendLocalizedMessage(501271); // You already own a house, you may not place another!
                        }
                        else
                        {
                            BaseHouse house = ConstructHouse(from);

                            if (house == null)
                                return;

                            house.Price = m_Cost;

                            if (!(house is HouseFoundation))
                            {
                                Item contract = from.Backpack.FindItemByType(typeof(HousePlacementTool));
                                if (contract.Hue == 1) { contract.Hue = 0x497; }
                                if (MySettings.S_AllowHouseDyes) { house.Hue = contract.Hue; }
                            }

                            if (from.AccessLevel >= AccessLevel.GameMaster)
                            {
                                from.SendMessage("{0} gold would have been withdrawn from your bank if you were not a GM.", m_Cost.ToString());
                            }
                            else
                            {
                                if (Banker.Withdraw(from, m_Cost))
                                {
                                    from.SendLocalizedMessage(1060398, m_Cost.ToString()); // ~1_AMOUNT~ gold has been withdrawn from your bank box.
                                }
                                else
                                {
                                    house.RemoveKeys(from);
                                    house.Delete();
                                    from.SendLocalizedMessage(1060646); // You do not have the funds available in your bank box to purchase this house.  Try placing a smaller house, or adding gold or checks to your bank box.
                                    return;
                                }
                            }

                            house.MoveToWorld(center, from.Map);

                            for (int i = 0; i < toMove.Count; ++i)
                            {
                                object o = toMove[i];

                                if (o is Mobile)
                                    ((Mobile)o).Location = house.BanLocation;
                                else if (o is Item)
                                    ((Item)o).Location = house.BanLocation;
                            }
                        }

                        break;
                    }
                case HousePlacementResult.BadItem:
                case HousePlacementResult.BadLand:
                case HousePlacementResult.BadStatic:
                case HousePlacementResult.BadRegionHidden:
                case HousePlacementResult.NoSurface:
                    {
                        from.SendLocalizedMessage(1043287); // The house could not be created here.  Either something is blocking the house, or the house would not be on valid terrain.
                        break;
                    }
                case HousePlacementResult.BadRegion:
                    {
                        from.SendLocalizedMessage(501265); // Housing cannot be created in this area.
                        break;
                    }
                case HousePlacementResult.BadRegionTemp:
                    {
                        from.SendLocalizedMessage(501270); // Lord British has decreed a 'no build' period, thus you cannot build this house at this time.
                        break;
                    }
                case HousePlacementResult.InvalidCastleKeep:
                    {
                        from.SendLocalizedMessage(1061122); // Castles and keeps cannot be created here.
                        break;
                    }
            }
        }

        public bool OnPlacement(Mobile from, Point3D p)
        {
            if (!from.CheckAlive() || from.Backpack == null || from.Backpack.FindItemByType(typeof(HousePlacementTool)) == null)
                return false;

            ArrayList toMove;
            Point3D center = new Point3D(p.X - m_Offset.X, p.Y - m_Offset.Y, p.Z - m_Offset.Z);
            HousePlacementResult res = HousePlacement.Check(from, m_MultiID, center, out toMove);

            switch (res)
            {
                case HousePlacementResult.Valid:
                    {
                        if (from.AccessLevel < AccessLevel.GameMaster && BaseHouse.HasAccountHouse(from))
                        {
                            from.SendLocalizedMessage(501271); // You already own a house, you may not place another!
                        }
                        else
                        {
                            from.SendLocalizedMessage(1011576); // This is a valid location.

                            PreviewHouse prev = new PreviewHouse(m_MultiID);

                            if (!((prev.GetType()).IsAssignableFrom(typeof(HouseFoundation))))
                            {
                                Item contract = from.Backpack.FindItemByType(typeof(HousePlacementTool));
                                if (contract.Hue == 1) { contract.Hue = 0x497; }
                                if (MySettings.S_AllowHouseDyes) { prev.Hue = contract.Hue; }
                            }

                            MultiComponentList mcl = prev.Components;

                            Point3D banLoc = new Point3D(center.X + mcl.Min.X, center.Y + mcl.Max.Y + 1, center.Z);

                            for (int i = 0; i < mcl.List.Length; ++i)
                            {
                                MultiTileEntry entry = mcl.List[i];

                                int itemID = entry.m_ItemID;

                                if (itemID >= 0xBA3 && itemID <= 0xC0E)
                                {
                                    banLoc = new Point3D(center.X + entry.m_OffsetX, center.Y + entry.m_OffsetY, center.Z);
                                    break;
                                }
                            }

                            for (int i = 0; i < toMove.Count; ++i)
                            {
                                object o = toMove[i];

                                if (o is Mobile)
                                    ((Mobile)o).Location = banLoc;
                                else if (o is Item)
                                    ((Item)o).Location = banLoc;
                            }

                            prev.MoveToWorld(center, from.Map);

                            /* You are about to place a new house.
                             * Placing this house will condemn any and all of your other houses that you may have.
                             * All of your houses on all shards will be affected.
                             * 
                             * In addition, you will not be able to place another house or have one transferred to you for one (1) real-life week.
                             * 
                             * Once you accept these terms, these effects cannot be reversed.
                             * Re-deeding or transferring your new house will not uncondemn your other house(s) nor will the one week timer be removed.
                             * 
                             * If you are absolutely certain you wish to proceed, click the button next to OKAY below.
                             * If you do not wish to trade for this house, click CANCEL.
                             */
                            from.SendGump(new WarningGump(1060635, 30720, 1049583, 32512, 420, 280, new WarningGumpCallback(PlacementWarning_Callback), prev));

                            return true;
                        }

                        break;
                    }
                case HousePlacementResult.BadItem:
                case HousePlacementResult.BadLand:
                case HousePlacementResult.BadStatic:
                case HousePlacementResult.BadRegionHidden:
                case HousePlacementResult.NoSurface:
                    {
                        from.SendLocalizedMessage(1043287); // The house could not be created here.  Either something is blocking the house, or the house would not be on valid terrain.
                        break;
                    }
                case HousePlacementResult.BadRegion:
                    {
                        from.SendLocalizedMessage(501265); // Housing cannot be created in this area.
                        break;
                    }
                case HousePlacementResult.BadRegionTemp:
                    {
                        from.SendLocalizedMessage(501270); //Lord British has decreed a 'no build' period, thus you cannot build this house at this time.
                        break;
                    }
                case HousePlacementResult.InvalidCastleKeep:
                    {
                        from.SendLocalizedMessage(1061122); // Castles and keeps cannot be created here.
                        break;
                    }
            }

            return false;
        }

        private static Hashtable m_Table;

        static HousePlacementEntry()
        {
            m_Table = new Hashtable();

            FillTable(m_ClassicHouses);
            FillTable(m_TwoStoryFoundations);
            FillTable(m_ThreeStoryFoundations);
        }

        public static HousePlacementEntry Find(BaseHouse house)
        {
            object obj = m_Table[house.GetType()];

            if (obj is HousePlacementEntry)
            {
                return ((HousePlacementEntry)obj);
            }
            else if (obj is ArrayList)
            {
                ArrayList list = (ArrayList)obj;

                for (int i = 0; i < list.Count; ++i)
                {
                    HousePlacementEntry e = (HousePlacementEntry)list[i];

                    if (e.m_MultiID == house.ItemID)
                        return e;
                }
            }
            else if (obj is Hashtable)
            {
                Hashtable table = (Hashtable)obj;

                obj = table[house.ItemID];

                if (obj is HousePlacementEntry)
                    return (HousePlacementEntry)obj;
            }

            return null;
        }

        private static void FillTable(HousePlacementEntry[] entries)
        {
            for (int i = 0; i < entries.Length; ++i)
            {
                HousePlacementEntry e = entries[i];

                object obj = m_Table[e.m_Type];

                if (obj == null)
                {
                    m_Table[e.m_Type] = e;
                }
                else if (obj is HousePlacementEntry)
                {
                    ArrayList list = new ArrayList();

                    list.Add(obj);
                    list.Add(e);

                    m_Table[e.m_Type] = list;
                }
                else if (obj is ArrayList)
                {
                    ArrayList list = (ArrayList)obj;

                    if (list.Count == 8)
                    {
                        Hashtable table = new Hashtable();

                        for (int j = 0; j < list.Count; ++j)
                            table[((HousePlacementEntry)list[j]).m_MultiID] = list[j];

                        table[e.m_MultiID] = e;

                        m_Table[e.m_Type] = table;
                    }
                    else
                    {
                        list.Add(e);
                    }
                }
                else if (obj is Hashtable)
                {
                    ((Hashtable)obj)[e.m_MultiID] = e;
                }
            }
        }

        private static HousePlacementEntry[] m_ClassicHouses = new HousePlacementEntry[]
            {
                new HousePlacementEntry( typeof( BlueTent ),                            1041217,    351,    81,     351,    81,     1,  15000,      0,  4,  0,  0x70),
                new HousePlacementEntry( typeof( GreenTent ),                           1041218,    351,    81,     351,    81,     1,  15000,      0,  4,  0,  0x72),
                new HousePlacementEntry( typeof( SmallLogCabinSouth ),                  1030871,    382,    112,    382,    112,    2,  30000,      0,  4,  0,  0x99),
                new HousePlacementEntry( typeof( SmallLogCabinEast ),                   1030871,    382,    112,    382,    112,    2,  30000,      0,  4,  0,  0x97),
                new HousePlacementEntry( typeof( NewSmallStoneHomeEast ),               1030845,    382,    112,    382,    112,    2,  30000,      4,  -2, 0,  0x5C),
                new HousePlacementEntry( typeof( NewSmallStoneHouseEast ),              1030848,    382,    112,    382,    112,    2,  30000,      4,  -2, 0,  0x5F),
                new HousePlacementEntry( typeof( SmallOldHouse ),                       1011303,    382,    112,    382,    112,    2,  30000,      0,  4,  0,  0x64),
                new HousePlacementEntry( typeof( SmallOldHouse ),                       1011304,    382,    112,    382,    112,    2,  30000,      0,  4,  0,  0x66),
                new HousePlacementEntry( typeof( SmallOldHouse ),                       1011305,    382,    112,    382,    112,    2,  30000,      0,  4,  0,  0x68),
                new HousePlacementEntry( typeof( SmallOldHouse ),                       1011306,    382,    112,    382,    112,    2,  30000,      0,  4,  0,  0x6A),
                new HousePlacementEntry( typeof( SmallOldHouse ),                       1011307,    382,    112,    382,    112,    2,  30000,      0,  4,  0,  0x6C),
                new HousePlacementEntry( typeof( SmallOldHouse ),                       1011308,    382,    112,    382,    112,    2,  30000,      0,  4,  0,  0x6E),
                new HousePlacementEntry( typeof( NewSmallStoneStoreFront ),             1030844,    410,    140,    410,    140,    3,  45000,      0,  4,  0,  0x5B),
                new HousePlacementEntry( typeof( NewSmallWoodenShackPorch ),            1030849,    410,    140,    410,    140,    3,  45000,      -3, 4,  0,  0x60),
                new HousePlacementEntry( typeof( SmallShop ),                           1011321,    444,    174,    444,    174,    4,  60000,      -1, 4,  0,  0xA0),
                new HousePlacementEntry( typeof( SmallShop ),                           1011322,    444,    174,    444,    174,    4,  60000,      0,  4,  0,  0xA2),
                new HousePlacementEntry( typeof( NewPlainStoneHouse ),                  1030851,    456,    186,    456,    186,    5,  65000,      -5, 6,  0,  0x62),
                new HousePlacementEntry( typeof( NewSmallLogCabinWithDeck ),            1030860,    449,    179,    449,    179,    4,  65000,      1,  4,  0,  0x88),
                new HousePlacementEntry( typeof( NewPlainPlasterHouse ),                1030850,    463,    193,    463,    193,    5,  70000,      -5, 4,  0,  0x61),
                new HousePlacementEntry( typeof( NewSmallSandstoneWorkshop ),           1030857,    458,    188,    458,    188,    5,  70000,      4,  4,  0,  0x84),
                new HousePlacementEntry( typeof( NewTwoStorySmallPlasterDwelling ),     1030855,    470,    200,    470,    200,    5,  75000,      3,  3,  0,  0x82),
                new HousePlacementEntry( typeof( Wagon ),                               1030870,    470,    200,    470,    200,    5,  75000,      0,  0,  0,  0x94),
                new HousePlacementEntry( typeof( NewTwoStorySmallStoneDwelling ),       1030841,    470,    200,    470,    200,    5,  75000,      3,  3,  0,  0x58),
                new HousePlacementEntry( typeof( NewTwoStorySmallStoneHome ),           1030839,    470,    200,    470,    200,    5,  75000,      3,  3,  0,  0x56),
                new HousePlacementEntry( typeof( NewTwoStorySmallStoneHouse ),          1030840,    470,    200,    470,    200,    5,  75000,      3,  3,  0,  0x57),
                new HousePlacementEntry( typeof( NewTwoStorySmallWoodenDwelling ),      1030842,    470,    200,    470,    200,    5,  75000,      3,  3,  0,  0x59),
                new HousePlacementEntry( typeof( LogCabin ),                            1011318,    478,    208,    478,    208,    5,  80000,      1,  6,  0,  0x9A),
                new HousePlacementEntry( typeof( NewLogCabin ),                         1030859,    488,    218,    488,    218,    6,  80000,      2,  5,  0,  0x86),
                new HousePlacementEntry( typeof( NewSmallStoneShoppe ),                 1030835,    478,    208,    478,    208,    5,  80000,      -5, 6,  0,  0x52),
                new HousePlacementEntry( typeof( NewWoodenHomePorch ),                  1030836,    487,    217,    487,    217,    6,  80000,      2,  5,  0,  0x53),
                new HousePlacementEntry( typeof( SmallTower ),                          1011317,    500,    230,    500,    230,    6,  85000,      3,  4,  0,  0x98),
                new HousePlacementEntry( typeof( NewSmallStoneTemple ),                 1030856,    504,    234,    504,    234,    6,  90000,      4,  -3, 0,  0x83),
                new HousePlacementEntry( typeof( NewBrickHomeWithFrontDeck ),           1030867,    518,    248,    518,    248,    7,  95000,      0,  7,  0,  0x91),
                new HousePlacementEntry( typeof( NewPlasterHousePictureWindow ),        1030832,    515,    245,    515,    245,    7,  95000,      7,  -6, 0,  0x4F),
                new HousePlacementEntry( typeof( NewStoneHomeWithEnclosedPatio ),       1030858,    516,    246,    516,    246,    7,  95000,      7,  0,  0,  0x85),
                new HousePlacementEntry( typeof( SandStonePatio ),                      1011320,    520,    250,    520,    250,    7,  95000,      -1, 4,  0,  0x9C),
                new HousePlacementEntry( typeof( NewOldStoneHomeShoppe ),               1030864,    526,    256,    526,    256,    7,  100000,     8,  -5, 0,  0x8E),
                new HousePlacementEntry( typeof( NewBrickHomeWithLargePorch ),          1030869,    533,    263,    533,    263,    7,  105000,     -6, 6,  0,  0x93),
                new HousePlacementEntry( typeof( GuildHouse ),                          1011309,    544,    274,    544,    274,    7,  110000,     -1, 7,  0,  0x74),
                new HousePlacementEntry( typeof( LargePatioHouse ),                     1011315,    546,    276,    546,    276,    8,  110000,     -4, 7,  0,  0x8C),
                new HousePlacementEntry( typeof( NewTwoStoryWoodenHomeWithPorch ),      1030834,    562,    292,    562,    292,    8,  115000,     6,  4,  0,  0x51),
                new HousePlacementEntry( typeof( TwoStoryVilla ),                       1011319,    560,    290,    560,    290,    8,  115000,     3,  6,  0,  0x9E),
                new HousePlacementEntry( typeof( NewTwoStoryBrickHouse ),               1030831,    568,    298,    568,    298,    8,  120000,     -4, 5,  0,  0x4E),
                new HousePlacementEntry( typeof( NewFancyStoneWoodHome ),               1030846,    580,    310,    580,    310,    9,  125000,     -4, 5,  0,  0x5D),
                new HousePlacementEntry( typeof( NewTwoStoryStoneVilla ),               1030854,    589,    319,    589,    319,    9,  130000,     4,  8,  0,  0x81),
                new HousePlacementEntry( typeof( NewWoodenHomeUpperDeck ),              1030853,    590,    320,    590,    320,    9,  130000,     -4, 5,  0,  0x80),
                new HousePlacementEntry( typeof( NewBrickArena ),                       1030862,    608,    338,    608,    338,    10, 140000,     -8, 11, 0,  0x8A),
                new HousePlacementEntry( typeof( NewMarbleShoppe ),                     1030868,    608,    338,    608,    338,    10, 140000,     -5, 6,  0,  0x92),
                new HousePlacementEntry( typeof( NewPlasterHomeDirtDeck ),              1030852,    622,    352,    622,    352,    10, 145000,     -2, 7,  0,  0x63),
                new HousePlacementEntry( typeof( NewTwoStoryBrickHome ),                1030833,    625,    355,    625,    355,    10, 150000,     -3, 7,  0,  0x50),
                new HousePlacementEntry( typeof( NewBrickHouseWithSteeple ),            1030830,    654,    384,    654,    384,    11, 160000,     0,  6,  0,  0x4D),
                new HousePlacementEntry( typeof( NewFancyWoodenStoneHouse ),            1030847,    653,    383,    653,    383,    11, 160000,     6,  -4, 0,  0x5E),
                new HousePlacementEntry( typeof( TwoStoryHouse ),                       1011310,    694,    424,    694,    424,    12, 180000,     -3, 7,  0,  0x76),
                new HousePlacementEntry( typeof( TwoStoryHouse ),                       1011311,    694,    424,    694,    424,    12, 180000,     -3, 7,  0,  0x78),
                new HousePlacementEntry( typeof( LargeMarbleHouse ),                    1011316,    700,    430,    700,    430,    13, 185000,     -4, 7,  0,  0x96),
                new HousePlacementEntry( typeof( NewTwoStorySandstoneHouse ),           1030829,    725,    455,    725,    455,    14, 195000,     7,  -4, 0,  0x4C),
                new HousePlacementEntry( typeof( NewSmallStoneTower ),                  1030837,    731,    461,    731,    461,    14, 200000,     -2, 6,  0,  0x54),
                new HousePlacementEntry( typeof( NewSmallBrickCastle ),                 1030865,    743,    473,    743,    473,    14, 205000,     -5, 6,  0,  0x8F),
                new HousePlacementEntry( typeof( NewStoneFort ),                        1030863,    750,    480,    750,    480,    14, 210000,     -5, 7,  0,  0x8B),
                new HousePlacementEntry( typeof( CastleTower ),                         1024781,    839,    569,    839,    569,    17, 250000,     5,  7,  0,  0x4A),
                new HousePlacementEntry( typeof( NewRaisedBrickHome ),                  1030861,    866,    596,    866,    596,    18, 265000,     3,  7,  0,  0x89),
                new HousePlacementEntry( typeof( NewSmallWizardTower ),                 1030866,    869,    599,    869,    599,    18, 270000,     -2, 6,  0,  0x90),
                new HousePlacementEntry( typeof( NewWoodenMansion ),                    1030843,    920,    650,    920,    650,    20, 290000,     6,  7,  0,  0x5A),
                new HousePlacementEntry( typeof( NewThreeStoryStoneVilla ),             1030838,    965,    695,    965,    695,    22, 310000,     -6, 7,  0,  0x55),
                new HousePlacementEntry( typeof( Tower ),                               1011312,    1476,   1206,   1476,   1206,   24, 560000,     0,  7,  0,  0x7A),
                new HousePlacementEntry( typeof( LargeTent ),                           1024851,    1572,   1302,   1572,   1302,   28, 610000,     1,  13, 0,  0x49),
                new HousePlacementEntry( typeof( Keep ),                                1011313,    1847,   1577,   1847,   1577,   30, 740000,     0,  11, 0,  0x7C),
                new HousePlacementEntry( typeof( Pyramid ),                             1024788,    1856,   1586,   1856,   1586,   32, 750000,     3,  16, 0,  0x48),
                new HousePlacementEntry( typeof( LogMansion ),                          1024875,    2777,   2507,   2777,   2507,   34, 800000,     13, 13, 0,  0x95),
                new HousePlacementEntry( typeof( Castle ),                              1011314,    2777,   2507,   2777,   2507,   34, 800000,     0,  16, 0,  0x7E),
                new HousePlacementEntry( typeof( Fortress ),                            1024869,    4448,   4178,   4448,   4178,   36, 900000,     4,  16, 0,  0x4B)
            };

        public static HousePlacementEntry[] ClassicHouses { get { return m_ClassicHouses; } }


        private static HousePlacementEntry[] m_TwoStoryFoundations = new HousePlacementEntry[]
            {
                new HousePlacementEntry( typeof( HouseFoundation ),     1060241,    425,    212,    489,    244,    10, 30500,      0,  4,  0,  0x13EC  ), // 7x7 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060242,    580,    290,    667,    333,    14, 34500,      0,  5,  0,  0x13ED  ), // 7x8 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060243,    650,    325,    748,    374,    16, 38500,      0,  5,  0,  0x13EE  ), // 7x9 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060244,    700,    350,    805,    402,    16, 42500,      0,  6,  0,  0x13EF  ), // 7x10 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060245,    750,    375,    863,    431,    16, 46500,      0,  6,  0,  0x13F0  ), // 7x11 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060246,    800,    400,    920,    460,    18, 50500,      0,  7,  0,  0x13F1  ), // 7x12 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060253,    580,    290,    667,    333,    14, 34500,      0,  4,  0,  0x13F8  ), // 8x7 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060254,    650,    325,    748,    374,    16, 39000,      0,  5,  0,  0x13F9  ), // 8x8 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060255,    700,    350,    805,    402,    16, 43500,      0,  5,  0,  0x13FA  ), // 8x9 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060256,    750,    375,    863,    431,    16, 48000,      0,  6,  0,  0x13FB  ), // 8x10 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060257,    800,    400,    920,    460,    18, 52500,      0,  6,  0,  0x13FC  ), // 8x11 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060258,    850,    425,    1265,   632,    24, 57000,      0,  7,  0,  0x13FD  ), // 8x12 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060259,    1100,   550,    1265,   632,    24, 61500,      0,  7,  0,  0x13FE  ), // 8x13 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060265,    650,    325,    748,    374,    16, 38500,      0,  4,  0,  0x1404  ), // 9x7 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060266,    700,    350,    805,    402,    16, 43500,      0,  5,  0,  0x1405  ), // 9x8 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060267,    750,    375,    863,    431,    16, 48500,      0,  5,  0,  0x1406  ), // 9x9 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060268,    800,    400,    920,    460,    18, 53500,      0,  6,  0,  0x1407  ), // 9x10 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060269,    850,    425,    1265,   632,    24, 58500,      0,  6,  0,  0x1408  ), // 9x11 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060270,    1100,   550,    1265,   632,    24, 63500,      0,  7,  0,  0x1409  ), // 9x12 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060271,    1100,   550,    1265,   632,    24, 68500,      0,  7,  0,  0x140A  ), // 9x13 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060277,    700,    350,    805,    402,    16, 42500,      0,  4,  0,  0x1410  ), // 10x7 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060278,    750,    375,    863,    431,    16, 48000,      0,  5,  0,  0x1411  ), // 10x8 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060279,    800,    400,    920,    460,    18, 53500,      0,  5,  0,  0x1412  ), // 10x9 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060280,    850,    425,    1265,   632,    24, 59000,      0,  6,  0,  0x1413  ), // 10x10 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060281,    1100,   550,    1265,   632,    24, 64500,      0,  6,  0,  0x1414  ), // 10x11 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060282,    1100,   550,    1265,   632,    24, 70000,      0,  7,  0,  0x1415  ), // 10x12 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060283,    1150,   575,    1323,   661,    24, 75500,      0,  7,  0,  0x1416  ), // 10x13 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060289,    750,    375,    863,    431,    16, 46500,      0,  4,  0,  0x141C  ), // 11x7 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060290,    800,    400,    920,    460,    18, 52500,      0,  5,  0,  0x141D  ), // 11x8 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060291,    850,    425,    1265,   632,    24, 58500,      0,  5,  0,  0x141E  ), // 11x9 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060292,    1100,   550,    1265,   632,    24, 64500,      0,  6,  0,  0x141F  ), // 11x10 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060293,    1100,   550,    1265,   632,    24, 70500,      0,  6,  0,  0x1420  ), // 11x11 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060294,    1150,   575,    1323,   661,    24, 76500,      0,  7,  0,  0x1421  ), // 11x12 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060295,    1200,   600,    1380,   690,    26, 82500,      0,  7,  0,  0x1422  ), // 11x13 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060301,    800,    400,    920,    460,    18, 50500,      0,  4,  0,  0x1428  ), // 12x7 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060302,    850,    425,    1265,   632,    24, 57000,      0,  5,  0,  0x1429  ), // 12x8 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060303,    1100,   550,    1265,   632,    24, 63500,      0,  5,  0,  0x142A  ), // 12x9 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060304,    1100,   550,    1265,   632,    24, 70000,      0,  6,  0,  0x142B  ), // 12x10 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060305,    1150,   575,    1323,   661,    24, 76500,      0,  6,  0,  0x142C  ), // 12x11 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060306,    1200,   600,    1380,   690,    26, 83000,      0,  7,  0,  0x142D  ), // 12x12 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060307,    1250,   625,    1438,   719,    26, 89500,      0,  7,  0,  0x142E  ), // 12x13 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060314,    1100,   550,    1265,   632,    24, 61500,      0,  5,  0,  0x1435  ), // 13x8 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060315,    1100,   550,    1265,   632,    24, 68500,      0,  5,  0,  0x1436  ), // 13x9 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060316,    1150,   575,    1323,   661,    24, 75500,      0,  6,  0,  0x1437  ), // 13x10 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060317,    1200,   600,    1380,   690,    26, 82500,      0,  6,  0,  0x1438  ), // 13x11 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060318,    1250,   625,    1438,   719,    26, 89500,      0,  7,  0,  0x1439  ), // 13x12 2-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060319,    1300,   650,    1495,   747,    28, 96500,      0,  7,  0,  0x143A  )  // 13x13 2-Story Customizable House
			};

        public static HousePlacementEntry[] TwoStoryFoundations { get { return m_TwoStoryFoundations; } }


        private static HousePlacementEntry[] m_ThreeStoryFoundations = new HousePlacementEntry[]
            {
                new HousePlacementEntry( typeof( HouseFoundation ),     1060272,    1150,   575,    1323,   661,    24, 73500,      0,  8,  0,  0x140B  ), // 9x14 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060284,    1200,   600,    1380,   690,    26, 81000,      0,  8,  0,  0x1417  ), // 10x14 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060285,    1250,   625,    1438,   719,    26, 86500,      0,  8,  0,  0x1418  ), // 10x15 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060296,    1250,   625,    1438,   719,    26, 88500,      0,  8,  0,  0x1423  ), // 11x14 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060297,    1300,   650,    1495,   747,    28, 94500,      0,  8,  0,  0x1424  ), // 11x15 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060298,    1350,   675,    1553,   776,    28, 100500,     0,  9,  0,  0x1425  ), // 11x16 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060308,    1300,   650,    1495,   747,    28, 96000,      0,  8,  0,  0x142F  ), // 12x14 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060309,    1350,   675,    1553,   776,    28, 102500,     0,  8,  0,  0x1430  ), // 12x15 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060310,    1370,   685,    1576,   788,    28, 109000,     0,  9,  0,  0x1431  ), // 12x16 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060311,    1370,   685,    1576,   788,    28, 115500,     0,  9,  0,  0x1432  ), // 12x17 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060320,    1350,   675,    1553,   776,    28, 103500,     0,  8,  0,  0x143B  ), // 13x14 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060321,    1370,   685,    1576,   788,    28, 110500,     0,  8,  0,  0x143C  ), // 13x15 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060322,    1370,   685,    1576,   788,    28, 117500,     0,  9,  0,  0x143D  ), // 13x16 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060323,    2119,   1059,   2437,   1218,   42, 124500,     0,  9,  0,  0x143E  ), // 13x17 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060324,    2119,   1059,   2437,   1218,   42, 131500,     0,  10, 0,  0x143F  ), // 13x18 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060327,    1150,   575,    1323,   661,    24, 73500,      0,  5,  0,  0x1442  ), // 14x9 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060328,    1200,   600,    1380,   690,    26, 81000,      0,  6,  0,  0x1443  ), // 14x10 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060329,    1250,   625,    1438,   719,    26, 88500,      0,  6,  0,  0x1444  ), // 14x11 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060330,    1300,   650,    1495,   747,    28, 96000,      0,  7,  0,  0x1445  ), // 14x12 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060331,    1350,   675,    1553,   776,    28, 103500,     0,  7,  0,  0x1446  ), // 14x13 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060332,    1370,   685,    1576,   788,    28, 111000,     0,  8,  0,  0x1447  ), // 14x14 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060333,    1370,   685,    1576,   788,    28, 118500,     0,  8,  0,  0x1448  ), // 14x15 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060334,    2119,   1059,   2437,   1218,   42, 126000,     0,  9,  0,  0x1449  ), // 14x16 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060335,    2119,   1059,   2437,   1218,   42, 133500,     0,  9,  0,  0x144A  ), // 14x17 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060336,    2119,   1059,   2437,   1218,   42, 141000,     0,  10, 0,  0x144B  ), // 14x18 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060340,    1250,   625,    1438,   719,    26, 86500,      0,  6,  0,  0x144F  ), // 15x10 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060341,    1300,   650,    1495,   747,    28, 94500,      0,  6,  0,  0x1450  ), // 15x11 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060342,    1350,   675,    1553,   776,    28, 102500,     0,  7,  0,  0x1451  ), // 15x12 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060343,    1370,   685,    1576,   788,    28, 110500,     0,  7,  0,  0x1452  ), // 15x13 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060344,    1370,   685,    1576,   788,    28, 118500,     0,  8,  0,  0x1453  ), // 15x14 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060345,    2119,   1059,   2437,   1218,   42, 126500,     0,  8,  0,  0x1454  ), // 15x15 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060346,    2119,   1059,   2437,   1218,   42, 134500,     0,  9,  0,  0x1455  ), // 15x16 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060347,    2119,   1059,   2437,   1218,   42, 142500,     0,  9,  0,  0x1456  ), // 15x17 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060348,    2119,   1059,   2437,   1218,   42, 150500,     0,  10, 0,  0x1457  ), // 15x18 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060353,    1350,   675,    1553,   776,    28, 100500,     0,  6,  0,  0x145C  ), // 16x11 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060354,    1370,   685,    1576,   788,    28, 109000,     0,  7,  0,  0x145D  ), // 16x12 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060355,    1370,   685,    1576,   788,    28, 117500,     0,  7,  0,  0x145E  ), // 16x13 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060356,    2119,   1059,   2437,   1218,   42, 126000,     0,  8,  0,  0x145F  ), // 16x14 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060357,    2119,   1059,   2437,   1218,   42, 134500,     0,  8,  0,  0x1460  ), // 16x15 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060358,    2119,   1059,   2437,   1218,   42, 143000,     0,  9,  0,  0x1461  ), // 16x16 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060359,    2119,   1059,   2437,   1218,   42, 151500,     0,  9,  0,  0x1462  ), // 16x17 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060360,    2119,   1059,   2437,   1218,   42, 160000,     0,  10, 0,  0x1463  ), // 16x18 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060366,    1370,   685,    1576,   788,    28, 115500,     0,  7,  0,  0x1469  ), // 17x12 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060367,    2119,   1059,   2437,   1218,   42, 124500,     0,  7,  0,  0x146A  ), // 17x13 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060368,    2119,   1059,   2437,   1218,   42, 133500,     0,  8,  0,  0x146B  ), // 17x14 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060369,    2119,   1059,   2437,   1218,   42, 142500,     0,  8,  0,  0x146C  ), // 17x15 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060370,    2119,   1059,   2437,   1218,   42, 151500,     0,  9,  0,  0x146D  ), // 17x16 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060371,    2119,   1059,   2437,   1218,   42, 160500,     0,  9,  0,  0x146E  ), // 17x17 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060372,    2119,   1059,   2437,   1218,   42, 169500,     0,  10, 0,  0x146F  ), // 17x18 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060379,    2119,   1059,   2437,   1218,   42, 131500,     0,  7,  0,  0x1476  ), // 18x13 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060380,    2119,   1059,   2437,   1218,   42, 141000,     0,  8,  0,  0x1477  ), // 18x14 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060381,    2119,   1059,   2437,   1218,   42, 150500,     0,  8,  0,  0x1478  ), // 18x15 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060382,    2119,   1059,   2437,   1218,   42, 160000,     0,  9,  0,  0x1479  ), // 18x16 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060383,    2119,   1059,   2437,   1218,   42, 169500,     0,  9,  0,  0x147A  ), // 18x17 3-Story Customizable House
				new HousePlacementEntry( typeof( HouseFoundation ),     1060384,    2119,   1059,   2437,   1218,   42, 179000,     0,  10, 0,  0x147B  )  // 18x18 3-Story Customizable House
			};

        public static HousePlacementEntry[] ThreeStoryFoundations { get { return m_ThreeStoryFoundations; } }
    }
}