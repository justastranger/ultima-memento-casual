/***************************************************************************
 *                                 Region.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id$
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Server.Network;
using Server.Targeting;

namespace Server
{
    public enum MusicName
    {
        Invalid = -1,
        Ultima = 0,
        Mines,
        Dragon,
        Scouting,
        Wrong,
        Hunting,
        Covetous,
        Deceit,
        Odyssey,
        CastleBritain,
        Britain,
        BucsDen,
        DevilGuard,
        CastleKnowledge,
        Adventure,
        Renika,
        Montor,
        Grey,
        Destard,
        FiresHell,
        SkaraBrae,
        Moon,
        Luna,
        TimeAwaits,
        Yew,
        Doom,
        Exodus,
        Traveling,
        Docks,
        Explore,
        Searching,
        Wandering,
        Sailing,
        Expedition,
        Tavern,
        Bar,
        Alehouse,
        Inn,
        Combat1,
        Combat2,
        Combat3,
        Catacombs,
        Death,
        Roaming,
        WizardDen,
        Fawn,
        Clues,
        DeathGulch,
        Elidor,
        Guild,
        MinesMorinia,
        Taiko,
        DardinsPit,
        City,
        PerinianDepths,
        Hythloth,
        Seeking,
        TimeLord,
        Cave,
        Quest,
        Grotto,
        Shame,
        DarkGuild,
        Despise,
        Pub,
        Combat4,
        Pirates,
        Dojo,
        CrashSite,
        JediCrypt,
        JediTomb,
        SythCrypt,
        SythTomb,
        Station1,
        Station2,
        Station3,
        Station4,
        Station5
    }

    public class Region : IComparable
    {
        private static List<Region> m_Regions = new List<Region>();

        public static List<Region> Regions { get { return m_Regions; } }

        public static Region Find(Point3D p, Map map)
        {
            if (map == null)
                return Map.Internal.DefaultRegion;

            Sector sector = map.GetSector(p);
            List<RegionRect> list = sector.RegionRects;

            for (int i = 0; i < list.Count; ++i)
            {
                RegionRect regRect = list[i];

                if (regRect.Contains(p))
                    return regRect.Region;
            }

            return map.DefaultRegion;
        }

        private static Type m_DefaultRegionType = typeof(Region);
        public static Type DefaultRegionType { get { return m_DefaultRegionType; } set { m_DefaultRegionType = value; } }

        private static TimeSpan m_StaffLogoutDelay = TimeSpan.FromSeconds(10.0);
        private static TimeSpan m_DefaultLogoutDelay = TimeSpan.FromMinutes(5.0);

        public static TimeSpan StaffLogoutDelay { get { return m_StaffLogoutDelay; } set { m_StaffLogoutDelay = value; } }
        public static TimeSpan DefaultLogoutDelay { get { return m_DefaultLogoutDelay; } set { m_DefaultLogoutDelay = value; } }

        public static readonly int DefaultPriority = 50;

        public static readonly int MinZ = sbyte.MinValue;
        public static readonly int MaxZ = sbyte.MaxValue + 1;

        public static Rectangle3D ConvertTo3D(Rectangle2D rect)
        {
            return new Rectangle3D(new Point3D(rect.Start, MinZ), new Point3D(rect.End, MaxZ));
        }

        public static Rectangle3D[] ConvertTo3D(Rectangle2D[] rects)
        {
            Rectangle3D[] ret = new Rectangle3D[rects.Length];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = ConvertTo3D(rects[i]);
            }

            return ret;
        }


        private string m_Name;
        private Map m_Map;
        private Region m_Parent;
        private List<Region> m_Children = new List<Region>();
        private Rectangle3D[] m_Area;
        private Sector[] m_Sectors;
        private bool m_Dynamic;
        private int m_Priority;
        private int m_ChildLevel;
        private bool m_Registered;

        private Point3D m_GoLocation;
        private MusicName m_Music;

        public string Name { get { return m_Name; } }
        public Map Map { get { return m_Map; } }
        public Region Parent { get { return m_Parent; } }
        public List<Region> Children { get { return m_Children; } }
        public Rectangle3D[] Area { get { return m_Area; } }
        public Sector[] Sectors { get { return m_Sectors; } }
        public bool Dynamic { get { return m_Dynamic; } }
        public int Priority { get { return m_Priority; } }
        public int ChildLevel { get { return m_ChildLevel; } }
        public bool Registered { get { return m_Registered; } }

        public Point3D GoLocation { get { return m_GoLocation; } set { m_GoLocation = value; } }
        public MusicName Music { get { return m_Music; } set { m_Music = value; } }

        public bool IsDefault { get { return m_Map.DefaultRegion == this; } }
        public virtual MusicName DefaultMusic { get { return m_Parent != null ? m_Parent.Music : MusicName.Invalid; } }

        public Region(string name, Map map, int priority, params Rectangle2D[] area) : this(name, map, priority, ConvertTo3D(area))
        {
        }

        public Region(string name, Map map, int priority, params Rectangle3D[] area) : this(name, map, null, area)
        {
            m_Priority = priority;
        }

        public Region(string name, Map map, Region parent, params Rectangle2D[] area) : this(name, map, parent, ConvertTo3D(area))
        {
        }

        public Region(string name, Map map, Region parent, params Rectangle3D[] area)
        {
            m_Name = name;
            m_Map = map;
            m_Parent = parent;
            m_Area = area;
            m_Dynamic = true;
            m_Music = this.DefaultMusic;

            if (m_Parent == null)
            {
                m_ChildLevel = 0;
                m_Priority = DefaultPriority;
            }
            else
            {
                m_ChildLevel = m_Parent.ChildLevel + 1;
                m_Priority = m_Parent.Priority;
            }
        }

        public void Register()
        {
            if (m_Registered)
                return;

            OnRegister();

            m_Registered = true;

            if (m_Parent != null)
            {
                m_Parent.m_Children.Add(this);
                m_Parent.OnChildAdded(this);
            }

            m_Regions.Add(this);

            m_Map.RegisterRegion(this);

            List<Sector> sectors = new List<Sector>();

            for (int i = 0; i < m_Area.Length; i++)
            {
                Rectangle3D rect = m_Area[i];

                Point2D start = m_Map.Bound(new Point2D(rect.Start));
                Point2D end = m_Map.Bound(new Point2D(rect.End));

                Sector startSector = m_Map.GetSector(start);
                Sector endSector = m_Map.GetSector(end);

                for (int x = startSector.X; x <= endSector.X; x++)
                {
                    for (int y = startSector.Y; y <= endSector.Y; y++)
                    {
                        Sector sector = m_Map.GetRealSector(x, y);

                        sector.OnEnter(this, rect);

                        if (!sectors.Contains(sector))
                            sectors.Add(sector);
                    }
                }
            }

            m_Sectors = sectors.ToArray();
        }

        public void Unregister()
        {
            if (!m_Registered)
                return;

            OnUnregister();

            m_Registered = false;

            if (m_Children.Count > 0)
                Console.WriteLine("Warning: Unregistering region '{0}' with children", this);

            if (m_Parent != null)
            {
                m_Parent.m_Children.Remove(this);
                m_Parent.OnChildRemoved(this);
            }

            m_Regions.Remove(this);

            m_Map.UnregisterRegion(this);

            if (m_Sectors != null)
            {
                for (int i = 0; i < m_Sectors.Length; i++)
                    m_Sectors[i].OnLeave(this);
            }

            m_Sectors = null;
        }

        public bool Contains(Point3D p)
        {
            for (int i = 0; i < m_Area.Length; i++)
            {
                Rectangle3D rect = m_Area[i];

                if (rect.Contains(p))
                    return true;
            }

            return false;
        }

        public bool IsChildOf(Region region)
        {
            if (region == null)
                return false;

            Region p = m_Parent;

            while (p != null)
            {
                if (p == region)
                    return true;

                p = p.m_Parent;
            }

            return false;
        }

        public Region GetRegion(Type regionType)
        {
            if (regionType == null)
                return null;

            Region r = this;

            do
            {
                if (regionType.IsAssignableFrom(r.GetType()))
                    return r;

                r = r.m_Parent;
            }
            while (r != null);

            return null;
        }

        public Region GetRegion(string regionName)
        {
            if (regionName == null)
                return null;

            Region r = this;

            do
            {
                if (r.m_Name == regionName)
                    return r;

                r = r.m_Parent;
            }
            while (r != null);

            return null;
        }

        public bool IsPartOf(Region region)
        {
            if (this == region)
                return true;

            return IsChildOf(region);
        }

        public bool IsPartOf(Type regionType)
        {
            return (GetRegion(regionType) != null);
        }

        public bool IsPartOf(string regionName)
        {
            return (GetRegion(regionName) != null);
        }

        public virtual bool AcceptsSpawnsFrom(Region region)
        {
            if (!AllowSpawn())
                return false;

            if (region == this)
                return true;

            if (m_Parent != null)
                return m_Parent.AcceptsSpawnsFrom(region);

            return false;
        }

        public List<Mobile> GetPlayers()
        {
            List<Mobile> list = new List<Mobile>();

            if (m_Sectors != null)
            {
                for (int i = 0; i < m_Sectors.Length; i++)
                {
                    Sector sector = m_Sectors[i];

                    foreach (Mobile player in sector.Players)
                    {
                        if (player.Region.IsPartOf(this))
                            list.Add(player);
                    }
                }
            }

            return list;
        }

        public int GetPlayerCount()
        {
            int count = 0;

            if (m_Sectors != null)
            {
                for (int i = 0; i < m_Sectors.Length; i++)
                {
                    Sector sector = m_Sectors[i];

                    foreach (Mobile player in sector.Players)
                    {
                        if (player.Region.IsPartOf(this))
                            count++;
                    }
                }
            }

            return count;
        }

        public List<Mobile> GetMobiles()
        {
            List<Mobile> list = new List<Mobile>();

            if (m_Sectors != null)
            {
                for (int i = 0; i < m_Sectors.Length; i++)
                {
                    Sector sector = m_Sectors[i];

                    foreach (Mobile mobile in sector.Mobiles)
                    {
                        if (mobile.Region.IsPartOf(this))
                            list.Add(mobile);
                    }
                }
            }

            return list;
        }

        public int GetMobileCount()
        {
            int count = 0;

            if (m_Sectors != null)
            {
                for (int i = 0; i < m_Sectors.Length; i++)
                {
                    Sector sector = m_Sectors[i];

                    foreach (Mobile mobile in sector.Mobiles)
                    {
                        if (mobile.Region.IsPartOf(this))
                            count++;
                    }
                }
            }

            return count;
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            Region reg = obj as Region;

            if (reg == null)
                throw new ArgumentException("obj is not a Region", "obj");

            // Dynamic regions go first
            if (this.Dynamic)
            {
                if (!reg.Dynamic)
                    return -1;
            }
            else if (reg.Dynamic)
            {
                return 1;
            }

            int thisPriority = this.Priority;
            int regPriority = reg.Priority;

            if (thisPriority != regPriority)
                return (regPriority - thisPriority);

            return (reg.ChildLevel - this.ChildLevel);
        }

        public override string ToString()
        {
            if (m_Name != null)
                return m_Name;
            else
                return GetType().Name;
        }


        public virtual void OnRegister()
        {
        }

        public virtual void OnUnregister()
        {
        }

        public virtual void OnChildAdded(Region child)
        {
        }

        public virtual void OnChildRemoved(Region child)
        {
        }

        public virtual bool OnMoveInto(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation)
        {
            return (m.WalkRegion == null || AcceptsSpawnsFrom(m.WalkRegion));
        }

        public virtual void OnEnter(Mobile m)
        {
        }

        public virtual void OnExit(Mobile m)
        {
        }

        public virtual void MakeGuard(Mobile focus)
        {
            if (m_Parent != null)
                m_Parent.MakeGuard(focus);
        }

        public virtual Type GetResource(Type type)
        {
            if (m_Parent != null)
                return m_Parent.GetResource(type);

            return type;
        }

        public virtual bool CanUseStuckMenu(Mobile m)
        {
            if (m_Parent != null)
                return m_Parent.CanUseStuckMenu(m);

            return true;
        }

        public virtual void OnAggressed(Mobile aggressor, Mobile aggressed, bool criminal)
        {
            if (m_Parent != null)
                m_Parent.OnAggressed(aggressor, aggressed, criminal);
        }

        public virtual void OnDidHarmful(Mobile harmer, Mobile harmed)
        {
            if (m_Parent != null)
                m_Parent.OnDidHarmful(harmer, harmed);
        }

        public virtual void OnGotHarmful(Mobile harmer, Mobile harmed)
        {
            if (m_Parent != null)
                m_Parent.OnGotHarmful(harmer, harmed);
        }

        public virtual void OnLocationChanged(Mobile m, Point3D oldLocation)
        {
            if (m_Parent != null)
                m_Parent.OnLocationChanged(m, oldLocation);
        }

        public virtual bool OnTarget(Mobile m, Target t, object o)
        {
            if (m_Parent != null)
                return m_Parent.OnTarget(m, t, o);

            return true;
        }

        public virtual bool OnCombatantChange(Mobile m, Mobile Old, Mobile New)
        {
            if (m_Parent != null)
                return m_Parent.OnCombatantChange(m, Old, New);

            return true;
        }

        public virtual bool AllowHousing(Mobile from, Point3D p)
        {
            if (m_Parent != null)
                return m_Parent.AllowHousing(from, p);

            return true;
        }

        public virtual bool SendInaccessibleMessage(Item item, Mobile from)
        {
            if (m_Parent != null)
                return m_Parent.SendInaccessibleMessage(item, from);

            return false;
        }

        public virtual bool CheckAccessibility(Item item, Mobile from)
        {
            if (m_Parent != null)
                return m_Parent.CheckAccessibility(item, from);

            return true;
        }

        public virtual bool OnDecay(Item item)
        {
            if (m_Parent != null)
                return m_Parent.OnDecay(item);

            return true;
        }

        public virtual bool AllowHarmful(Mobile from, Mobile target)
        {
            if (m_Parent != null)
                return m_Parent.AllowHarmful(from, target);

            if (Mobile.AllowHarmfulHandler != null)
                return Mobile.AllowHarmfulHandler(from, target);

            return true;
        }

        public virtual void OnCriminalAction(Mobile m, bool message)
        {
            if (m_Parent != null)
                m_Parent.OnCriminalAction(m, message);
            else if (message)
                m.SendLocalizedMessage(1005040); // You've committed a criminal act!!
        }

        public virtual bool AllowBeneficial(Mobile from, Mobile target)
        {
            if (m_Parent != null)
                return m_Parent.AllowBeneficial(from, target);

            if (Mobile.AllowBeneficialHandler != null)
                return Mobile.AllowBeneficialHandler(from, target);

            return true;
        }

        public virtual void OnBeneficialAction(Mobile helper, Mobile target)
        {
            if (m_Parent != null)
                m_Parent.OnBeneficialAction(helper, target);
        }

        public virtual void OnGotBeneficialAction(Mobile helper, Mobile target)
        {
            if (m_Parent != null)
                m_Parent.OnGotBeneficialAction(helper, target);
        }

        public virtual void SpellDamageScalar(Mobile caster, Mobile target, ref double damage)
        {
            if (m_Parent != null)
                m_Parent.SpellDamageScalar(caster, target, ref damage);
        }

        public virtual void OnSpeech(SpeechEventArgs args)
        {
            if (m_Parent != null)
                m_Parent.OnSpeech(args);
        }

        public virtual bool OnSkillUse(Mobile m, int Skill)
        {
            if (m_Parent != null)
                return m_Parent.OnSkillUse(m, Skill);

            return true;
        }

        public virtual bool OnBeginSpellCast(Mobile m, ISpell s)
        {
            if (m_Parent != null)
                return m_Parent.OnBeginSpellCast(m, s);

            return true;
        }

        public virtual void OnSpellCast(Mobile m, ISpell s)
        {
            if (m_Parent != null)
                m_Parent.OnSpellCast(m, s);
        }

        public virtual bool OnResurrect(Mobile m)
        {
            if (m_Parent != null)
                return m_Parent.OnResurrect(m);

            return true;
        }

        public virtual bool OnBeforeDeath(Mobile m)
        {
            if (m_Parent != null)
                return m_Parent.OnBeforeDeath(m);

            return true;
        }

        public virtual void OnDeath(Mobile m)
        {
            if (m_Parent != null)
                m_Parent.OnDeath(m);
        }

        public virtual bool OnDamage(Mobile m, ref int Damage)
        {
            if (m_Parent != null)
                return m_Parent.OnDamage(m, ref Damage);

            return true;
        }

        public virtual bool OnHeal(Mobile m, ref int Heal)
        {
            if (m_Parent != null)
                return m_Parent.OnHeal(m, ref Heal);

            return true;
        }

        public virtual bool OnDoubleClick(Mobile m, object o)
        {
            if (m_Parent != null)
                return m_Parent.OnDoubleClick(m, o);

            return true;
        }

        public virtual bool OnSingleClick(Mobile m, object o)
        {
            if (m_Parent != null)
                return m_Parent.OnSingleClick(m, o);

            return true;
        }

        public virtual bool AllowSpawn()
        {
            if (m_Parent != null)
                return m_Parent.AllowSpawn();

            return true;
        }

        public virtual void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            if (m_Parent != null)
                m_Parent.AlterLightLevel(m, ref global, ref personal);
        }

        public virtual TimeSpan GetLogoutDelay(Mobile m)
        {
            if (m_Parent != null)
                return m_Parent.GetLogoutDelay(m);
            else if (m.AccessLevel > AccessLevel.Counselor)
                return m_StaffLogoutDelay;
            else
                return m_DefaultLogoutDelay;
        }


        internal static bool CanMove(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation, Map map)
        {
            Region oldRegion = m.Region;
            Region newRegion = Find(newLocation, map);

            while (oldRegion != newRegion)
            {
                if (!newRegion.OnMoveInto(m, d, newLocation, oldLocation))
                    return false;

                if (newRegion.m_Parent == null)
                    return true;

                newRegion = newRegion.m_Parent;
            }

            return true;
        }

        internal static void OnRegionChange(Mobile m, Region oldRegion, Region newRegion)
        {
            if (newRegion != null && m.NetState != null)
            {
                m.CheckLightLevels(false);

                if (oldRegion == null || oldRegion.Music != newRegion.Music)
                {
                    m.Send(PlayMusic.GetInstance(newRegion.Music));
                }
            }

            Region oldR = oldRegion;
            Region newR = newRegion;

            while (oldR != newR)
            {
                int oldRChild = (oldR != null ? oldR.ChildLevel : -1);
                int newRChild = (newR != null ? newR.ChildLevel : -1);

                if (oldRChild >= newRChild)
                {
                    oldR.OnExit(m);
                    oldR = oldR.Parent;
                }

                if (newRChild >= oldRChild)
                {
                    newR.OnEnter(m);
                    newR = newR.Parent;
                }
            }
        }


        internal static void Load()
        {
            if (!System.IO.File.Exists("Data/System/XML/Regions.xml"))
            {
                Console.WriteLine("Error: Data/System/XML/Regions.xml does not exist");
                return;
            }

            //Console.Write( "Regions: Loading..." );

            XmlDocument doc = new XmlDocument();
            doc.Load(System.IO.Path.Combine(Core.BaseDirectory, "Data/System/XML/Regions.xml"));

            XmlElement root = doc["ServerRegions"];

            if (root == null)
            {
                Console.WriteLine("Could not find root element 'ServerRegions' in Regions.xml");
            }
            else
            {
                foreach (XmlElement facet in root.SelectNodes("Facet"))
                {
                    Map map = null;
                    if (ReadMap(facet, "name", ref map))
                    {
                        if (map == Map.Internal)
                            Console.WriteLine("Invalid internal map in a facet element");
                        else
                            LoadRegions(facet, map, null);
                    }
                }
            }

            //Console.WriteLine( "done" );
        }

        private static void LoadRegions(XmlElement xml, Map map, Region parent)
        {
            foreach (XmlElement xmlReg in xml.SelectNodes("region"))
            {
                Type type = DefaultRegionType;

                ReadType(xmlReg, "type", ref type, false);

                if (!typeof(Region).IsAssignableFrom(type))
                {
                    Console.WriteLine("Invalid region type '{0}' in regions.xml", type.FullName);
                    continue;
                }

                Region region = null;
                try
                {
                    region = (Region)Activator.CreateInstance(type, new object[] { xmlReg, map, parent });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during the creation of region type '{0}': {1}", type.FullName, ex);
                    continue;
                }

                region.Register();

                LoadRegions(xmlReg, map, region);
            }
        }

        public Region(XmlElement xml, Map map, Region parent)
        {
            m_Map = map;
            m_Parent = parent;
            m_Dynamic = false;

            if (m_Parent == null)
            {
                m_ChildLevel = 0;
                m_Priority = DefaultPriority;
            }
            else
            {
                m_ChildLevel = m_Parent.ChildLevel + 1;
                m_Priority = m_Parent.Priority;
            }

            ReadString(xml, "name", ref m_Name, false);

            if (parent == null)
                ReadInt32(xml, "priority", ref m_Priority, false);


            int minZ = MinZ;
            int maxZ = MaxZ;

            XmlElement zrange = xml["zrange"];
            ReadInt32(zrange, "min", ref minZ, false);
            ReadInt32(zrange, "max", ref maxZ, false);


            List<Rectangle3D> area = new List<Rectangle3D>();
            foreach (XmlElement xmlRect in xml.SelectNodes("rect"))
            {
                Rectangle3D rect = new Rectangle3D();
                if (ReadRectangle3D(xmlRect, minZ, maxZ, ref rect))
                    area.Add(rect);
            }

            m_Area = area.ToArray();

            if (m_Area.Length == 0)
                Console.WriteLine("Empty area for region '{0}'", this);


            if (!ReadPoint3D(xml["go"], map, ref m_GoLocation, false) && m_Area.Length > 0)
            {
                Point3D start = m_Area[0].Start;
                Point3D end = m_Area[0].End;

                int x = start.X + (end.X - start.X) / 2;
                int y = start.Y + (end.Y - start.Y) / 2;

                m_GoLocation = new Point3D(x, y, m_Map.GetAverageZ(x, y));
            }


            MusicName music = this.DefaultMusic;

            ReadEnum(xml["music"], "name", ref music, false);

            m_Music = music;
        }

        protected static string GetAttribute(XmlElement xml, string attribute, bool mandatory)
        {
            if (xml == null)
            {
                if (mandatory)
                    Console.WriteLine("Missing element for attribute '{0}'", attribute);

                return null;
            }
            else if (xml.HasAttribute(attribute))
            {
                return xml.GetAttribute(attribute);
            }
            else
            {
                if (mandatory)
                    Console.WriteLine("Missing attribute '{0}' in element '{1}'", attribute, xml.Name);

                return null;
            }
        }

        public static bool ReadString(XmlElement xml, string attribute, ref string value)
        {
            return ReadString(xml, attribute, ref value, true);
        }

        public static bool ReadString(XmlElement xml, string attribute, ref string value, bool mandatory)
        {
            string s = GetAttribute(xml, attribute, mandatory);

            if (s == null)
                return false;

            value = s;
            return true;
        }

        public static bool ReadInt32(XmlElement xml, string attribute, ref int value)
        {
            return ReadInt32(xml, attribute, ref value, true);
        }

        public static bool ReadInt32(XmlElement xml, string attribute, ref int value, bool mandatory)
        {
            string s = GetAttribute(xml, attribute, mandatory);

            if (s == null)
                return false;

            try
            {
                value = XmlConvert.ToInt32(s);
            }
            catch
            {
                Console.WriteLine("Could not parse integer attribute '{0}' in element '{1}'", attribute, xml.Name);
                return false;
            }

            return true;
        }

        public static bool ReadBoolean(XmlElement xml, string attribute, ref bool value)
        {
            return ReadBoolean(xml, attribute, ref value, true);
        }

        public static bool ReadBoolean(XmlElement xml, string attribute, ref bool value, bool mandatory)
        {
            string s = GetAttribute(xml, attribute, mandatory);

            if (s == null)
                return false;

            try
            {
                value = XmlConvert.ToBoolean(s);
            }
            catch
            {
                Console.WriteLine("Could not parse boolean attribute '{0}' in element '{1}'", attribute, xml.Name);
                return false;
            }

            return true;
        }

        public static bool ReadDateTime(XmlElement xml, string attribute, ref DateTime value)
        {
            return ReadDateTime(xml, attribute, ref value, true);
        }

        public static bool ReadDateTime(XmlElement xml, string attribute, ref DateTime value, bool mandatory)
        {
            string s = GetAttribute(xml, attribute, mandatory);

            if (s == null)
                return false;

            try
            {
                value = XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Local);
            }
            catch
            {
                Console.WriteLine("Could not parse DateTime attribute '{0}' in element '{1}'", attribute, xml.Name);
                return false;
            }

            return true;
        }

        public static bool ReadTimeSpan(XmlElement xml, string attribute, ref TimeSpan value)
        {
            return ReadTimeSpan(xml, attribute, ref value, true);
        }

        public static bool ReadTimeSpan(XmlElement xml, string attribute, ref TimeSpan value, bool mandatory)
        {
            string s = GetAttribute(xml, attribute, mandatory);

            if (s == null)
                return false;

            try
            {
                value = XmlConvert.ToTimeSpan(s);
            }
            catch
            {
                Console.WriteLine("Could not parse TimeSpan attribute '{0}' in element '{1}'", attribute, xml.Name);
                return false;
            }

            return true;
        }

        public static bool ReadEnum<T>(XmlElement xml, string attribute, ref T value)
        {
            return ReadEnum(xml, attribute, ref value, true);
        }

        public static bool ReadEnum<T>(XmlElement xml, string attribute, ref T value, bool mandatory)
        {
            string s = GetAttribute(xml, attribute, mandatory);

            if (s == null)
                return false;

            Type type = typeof(T);

            try
            {
                value = (T)Enum.Parse(type, s, true);
                //TODO: On .NET 4.0, use Enum.TryParse
            }
            catch
            {
                Console.WriteLine("Could not parse {0} enum attribute '{1}' in element '{2}'", type, attribute, xml.Name);
                return false;
            }

            return true;
        }

        public static bool ReadMap(XmlElement xml, string attribute, ref Map value)
        {
            return ReadMap(xml, attribute, ref value, true);
        }

        public static bool ReadMap(XmlElement xml, string attribute, ref Map value, bool mandatory)
        {
            string s = GetAttribute(xml, attribute, mandatory);

            if (s == null)
                return false;

            try
            {
                value = Map.Parse(s);
            }
            catch
            {
                Console.WriteLine("Could not parse Map attribute '{0}' in element '{1}'", attribute, xml.Name);
                return false;
            }

            return true;
        }

        public static bool ReadType(XmlElement xml, string attribute, ref Type value)
        {
            return ReadType(xml, attribute, ref value, true);
        }

        public static bool ReadType(XmlElement xml, string attribute, ref Type value, bool mandatory)
        {
            string s = GetAttribute(xml, attribute, mandatory);

            if (s == null)
                return false;

            Type type;
            try
            {
                type = ScriptCompiler.FindTypeByName(s, false);
            }
            catch
            {
                Console.WriteLine("Could not parse Type attribute '{0}' in element '{1}'", attribute, xml.Name);
                return false;
            }

            if (type == null)
            {
                Console.WriteLine("Could not find Type '{0}'", s);
                return false;
            }

            value = type;
            return true;
        }

        public static bool ReadPoint3D(XmlElement xml, Map map, ref Point3D value)
        {
            return ReadPoint3D(xml, map, ref value, true);
        }

        public static bool ReadPoint3D(XmlElement xml, Map map, ref Point3D value, bool mandatory)
        {
            int x = 0, y = 0, z = 0;

            bool xyOk = ReadInt32(xml, "x", ref x, mandatory) & ReadInt32(xml, "y", ref y, mandatory);
            bool zOk = ReadInt32(xml, "z", ref z, mandatory && map == null);

            if (xyOk && (zOk || map != null))
            {
                if (!zOk)
                    z = map.GetAverageZ(x, y);

                value = new Point3D(x, y, z);
                return true;
            }

            return false;
        }

        public static bool ReadRectangle3D(XmlElement xml, int defaultMinZ, int defaultMaxZ, ref Rectangle3D value)
        {
            return ReadRectangle3D(xml, defaultMinZ, defaultMaxZ, ref value, true);
        }

        public static bool ReadRectangle3D(XmlElement xml, int defaultMinZ, int defaultMaxZ, ref Rectangle3D value, bool mandatory)
        {
            int x1 = 0, y1 = 0, x2 = 0, y2 = 0;

            if (xml.HasAttribute("x"))
            {
                if (ReadInt32(xml, "x", ref x1, mandatory)
                    & ReadInt32(xml, "y", ref y1, mandatory)
                    & ReadInt32(xml, "width", ref x2, mandatory)
                    & ReadInt32(xml, "height", ref y2, mandatory))
                {
                    x2 += x1;
                    y2 += y1;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (!ReadInt32(xml, "x1", ref x1, mandatory)
                    | !ReadInt32(xml, "y1", ref y1, mandatory)
                    | !ReadInt32(xml, "x2", ref x2, mandatory)
                    | !ReadInt32(xml, "y2", ref y2, mandatory))
                {
                    return false;
                }
            }

            int z1 = defaultMinZ;
            int z2 = defaultMaxZ;

            ReadInt32(xml, "zmin", ref z1, false);
            ReadInt32(xml, "zmax", ref z2, false);

            value = new Rectangle3D(new Point3D(x1, y1, z1), new Point3D(x2, y2, z2));

            return true;
        }
    }
}