using System;
using Server;
using Server.Network;
using System.Text;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Regions;
using System.Collections.Generic;
using System.Collections;
using Server.Commands;

namespace Server.Items
{
    public class ThruDoor : BaseDoor
    {
        public Point3D m_PointDest;
        public Map m_MapDest;
        public int m_Rules;

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D PointDest
        {
            get { return m_PointDest; }
            set { m_PointDest = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map MapDest
        {
            get { return m_MapDest; }
            set { m_MapDest = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Rules
        {
            get { return m_Rules; }
            set { m_Rules = value; InvalidateProperties(); }
        }

        [Constructable]
        public ThruDoor() : this(DoorFacing.WestCW)
        {
            Name = "door";
            m_PointDest = new Point3D(0, 0, 0);
            m_MapDest = null;
            m_Rules = 0;
        }

        [Constructable]
        public ThruDoor(DoorFacing WestCW) : base(1661, 1661, 0xEC, 0xEC, BaseDoor.GetOffset(WestCW))
        {
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m is PlayerMobile)
                Use(m);

            return false;
        }

        public Point3D loc(Item door, Mobile from)
        {
            Point3D d = door.Location;
            Point3D p = d;

            if (from.Y > door.Y)
                d = new Point3D(p.X, p.Y - 1, p.Z);
            else if (from.Y < door.Y)
                d = new Point3D(p.X, p.Y + 1, p.Z);
            else if (from.X < door.X)
                d = new Point3D(p.X + 1, p.Y, p.Z);
            else if (from.X > door.X)
                d = new Point3D(p.X - 1, p.Y, p.Z);

            return d;
        }

        public static int DoorSound(int door)
        {
            int sound = 234;
            if (door == 0x12DC || door == 0x12E6 || door == 0x608 || door == 0x6727 || door == 0x6728) { sound = 0x057; } // TENT
            else if (door >= 0x675 && door <= 0x694) { sound = 236; }
            else if (door >= 8172 && door <= 8188) { sound = 236; }
            else if (door == 8197) { sound = 236; }
            else if (door >= 0x6C5 && door <= 0x6D4) { sound = 236; }
            else if (door == 0x695 || door == 0x697 || door == 0x69F || door == 0x69D) { sound = 235; }
            else if (door >= 0x1ED9 && door <= 0x1EFC) { sound = 0x1FE; }
            else if (door == 0x3F38) { sound = 0x1FE; }

            return sound;
        }

        public override void Use(Mobile from)
        {
            if (MapDest == null)
                MapDest = this.Map;

            if (from is PlayerMobile && !((PlayerMobile)from).PauseDoor)
            {
                if (Rules == 1 && !GetPlayerInfo.EvilPlayer(from))
                {
                    from.SendMessage("This door has an evil aura and doesn't seem to budge.");
                    return;
                }

                if (Rules == 1) // RAVENDARK
                {
                    Point3D d = loc(this, from);
                    Server.Mobiles.BaseCreature.TeleportPets(from, d, from.Map);
                    from.MoveToWorld(d, from.Map);
                }
                else if (Rules == 2) // TELEPORTER DOORS
                {
                    Server.Mobiles.BaseCreature.TeleportPets(from, PointDest, MapDest);
                    from.MoveToWorld(PointDest, MapDest);
                }
                else if (Rules == 3) // DOORS FROM ONE SIDE TO ANOTHER
                {
                    Point3D d = loc(this, from);
                    Server.Mobiles.BaseCreature.TeleportPets(from, d, from.Map);
                    from.MoveToWorld(d, from.Map);
                }
                else
                {
                    DoPublicDoor(from);
                }

                ((PlayerMobile)from).PauseDoor = true;
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(UnPause), from);
                from.PlaySound(DoorSound(this.ItemID));
            }
        }

        private void UnPause(object state)
        {
            Mobile from = state as Mobile;

            if (from is PlayerMobile)
                ((PlayerMobile)from).PauseDoor = false;
        }

        public void DoPublicDoor(Mobile m)
        {
            string sPublicDoor = "";
            int mX = 0;
            int mY = 0;
            int mZ = 0;
            Map mWorld = null;
            string mZone = "";

            if (m is PlayerMobile)
            {
                PlayerMobile pc = (PlayerMobile)m;

                sPublicDoor = ((PlayerMobile)m).CharacterPublicDoor;

                if (sPublicDoor != null)
                {
                    string[] sPublicDoors = sPublicDoor.Split('#');
                    int nEntry = 1;
                    foreach (string exits in sPublicDoors)
                    {
                        if (nEntry == 1) { mX = Convert.ToInt32(exits); }
                        else if (nEntry == 2) { mY = Convert.ToInt32(exits); }
                        else if (nEntry == 3) { mZ = Convert.ToInt32(exits); }
                        else if (nEntry == 4) { try { mWorld = Map.Parse(exits); } catch { } if (mWorld == null) { mWorld = Map.Sosaria; } }
                        else if (nEntry == 5) { mZone = exits; }
                        nEntry++;
                    }
                }

                if (Server.Misc.Worlds.GetRegionName(m.Map, m.Location) == "the Dojo" || Server.Misc.Worlds.GetRegionName(m.Map, m.Location) == "the Thieves Guild" || Server.Misc.Worlds.GetRegionName(m.Map, m.Location) == "the Camping Tent" || Server.Misc.Worlds.GetRegionName(m.Map, m.Location) == "the Dungeon Room" || Server.Misc.Worlds.GetRegionName(m.Map, m.Location) == "the Black Magic Guild")
                {
                    m.Hidden = true;
                }

                if (Server.Misc.Worlds.GetRegionName(m.Map, m.Location) == "the Basement" && m.Region is PublicRegion && Server.Items.BasementDoor.HatchAtOtherEnd(m))
                {
                    ((PlayerMobile)m).CharacterPublicDoor = null;
                    Point3D loc = new Point3D(mX, mY, mZ);
                    PublicTeleport(m, loc, mWorld, mZone, "exit");
                }
                else if (Server.Misc.Worlds.GetRegionName(m.Map, m.Location) != "the Basement" && m.Region is PublicRegion && sPublicDoor != null)
                {
                    ((PlayerMobile)m).CharacterPublicDoor = null;
                    Point3D loc = new Point3D(mX, mY, mZ);
                    PublicTeleport(m, loc, mWorld, mZone, "exit");
                }
                else if (m.Region is PublicRegion) // FAIL SAFE
                {
                    ((PlayerMobile)m).CharacterPublicDoor = null;
                    Point3D loc = new Point3D(1832, 755, 0);

                    string failPlace = "the Building";
                    Map failMap = Map.Sosaria;

                    if (Server.Misc.Worlds.GetRegionName(m.Map, m.Location) == "the Bank") { loc = new Point3D(1830, 768, 0); failMap = Map.Sosaria; failPlace = "the Bank"; }
                    else if (Server.Misc.Worlds.GetRegionName(m.Map, m.Location) == "the Tavern") { loc = new Point3D(1831, 758, 12); failMap = Map.Sosaria; failPlace = "the Inn"; }
                    else if (Server.Misc.Worlds.GetRegionName(m.Map, m.Location) == "the Dojo") { loc = new Point3D(1831, 758, 12); failMap = Map.Sosaria; failPlace = "the Inn"; }
                    else if (Server.Misc.Worlds.GetRegionName(m.Map, m.Location) == "the Port") { loc = new Point3D(1831, 758, 12); failMap = Map.Sosaria; failPlace = "the PortThat "; }
                    else if (Server.Misc.Worlds.GetRegionName(m.Map, m.Location) == "the Black Magic Guild") { loc = new Point3D(2243, 251, 0); failMap = Map.Sosaria; failPlace = "the Black Magic Guild"; }
                    else if (Server.Misc.Worlds.GetRegionName(m.Map, m.Location) == "the Wizards Guild") { loc = new Point3D(2832, 1873, 55); failMap = Map.Sosaria; failPlace = "the Wizards Guild"; }
                    else if (Server.Misc.Worlds.GetRegionName(m.Map, m.Location) == "the Thieves Guild") { loc = new Point3D(3315, 2059, 40); failMap = Map.Sosaria; failPlace = "the Thieves Guild"; }
                    else if (Server.Misc.Worlds.GetRegionName(m.Map, m.Location) == "the Basement") { loc = new Point3D(1831, 758, 12); failMap = Map.Sosaria; failPlace = "the Basement"; }

                    PublicTeleport(m, loc, failMap, failPlace, "exit");
                }
                else if (this.Name == "the Wizards Guild" && pc.NpcGuild != NpcGuild.MagesGuild)
                {
                    m.SendMessage("Only those of the Wizards Guild may enter!");
                }
                else
                {
                    string sX = m.X.ToString();
                    string sY = m.Y.ToString();
                    string sZ = m.Z.ToString();
                    string sMap = Worlds.GetMyMapString(m.Map);
                    string sZone = this.Name;
                    if (this.Name == "oak shelf") { sZone = "the Thieves Guild"; }
                    else if (this.Name == "trapdoor") { sZone = "the Thieves Guild"; }
                    else if (this.Name == "camping tent") { sZone = "the Camping Tent"; }

                    ((PlayerMobile)m).CharacterPublicDoor = sX + "#" + sY + "#" + sZ + "#" + sMap + "#" + sZone;

                    PublicTeleport(m, m_PointDest, m_MapDest, sZone, "enter");
                }
            }
        }

        public static void PublicTeleport(Mobile m, Point3D loc, Map map, string zone, string direction)
        {
            BaseCreature.TeleportPets(m, loc, map, false);
            m.MoveToWorld(loc, map);
            LoggingFunctions.LogRegions(m, zone, direction);
        }

        public ThruDoor(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_PointDest);
            writer.Write(m_MapDest);
            writer.Write(m_Rules);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_PointDest = reader.ReadPoint3D();
            m_MapDest = reader.ReadMap();
            m_Rules = reader.ReadInt();
        }
    }
}