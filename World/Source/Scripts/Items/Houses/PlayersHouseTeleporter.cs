using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Misc;
using Server.Prompts;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Multis;
using Server.Regions;
using Server.Spells;

namespace Server.Items
{
    public class PlayersHouseTeleporter : Item, IDyable, ISecurable
    {
        private bool m_Active, m_Creatures, m_CombatCheck;
        private Point3D m_PointDest;
        private Map m_MapDest;
        private bool m_SourceEffect;
        private bool m_DestEffect;
        private int m_SoundID;
        private TimeSpan m_Delay;
        private Mobile m_Owner;
        private SecureLevel m_Level;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool SourceEffect
        {
            get { return m_SourceEffect; }
            set { m_SourceEffect = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DestEffect
        {
            get { return m_DestEffect; }
            set { m_DestEffect = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SoundID
        {
            get { return m_SoundID; }
            set { m_SoundID = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan Delay
        {
            get { return m_Delay; }
            set { m_Delay = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active
        {
            get { return m_Active; }
            set { m_Active = value; InvalidateProperties(); }
        }

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
        public bool Creatures
        {
            get { return m_Creatures; }
            set { m_Creatures = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool CombatCheck
        {
            get { return m_CombatCheck; }
            set { m_CombatCheck = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SecureLevel Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        public override int LabelNumber { get { return 1026095; } } // teleporter

        [Constructable]
        public PlayersHouseTeleporter() : this(new Point3D(0, 0, 0), null, false)
        {
        }

        [Constructable]
        public PlayersHouseTeleporter(Point3D pointDest, Map mapDest) : this(pointDest, mapDest, false)
        {
        }

        [Constructable]
        public PlayersHouseTeleporter(Point3D pointDest, Map mapDest, bool creatures) : base(0x181D)
        {
            Movable = true;
            Visible = true;
            Name = "House Teleporter";

            m_Active = true;
            m_PointDest = pointDest;
            m_MapDest = mapDest;
            m_Creatures = creatures;

            m_SourceEffect = true;
            m_DestEffect = true;
            m_SoundID = 0x1FE;

            m_CombatCheck = false;
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            SetSecureLevelEntry.AddTo(from, this, list);
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Worlds.RegionAllowedTeleport(from.Map, from.Location, from.X, from.Y) == false)
            {
                from.SendMessage("This does not seem to have any magical properties in this place!");
                m_Owner = null;
                m_MapDest = null;
                m_PointDest = new Point3D(0, 0, 0);
            }
            else if (IsChildOf(from.Backpack))
            {
                from.SendMessage("You change the appearance of the teleporter but it must be secured in a house to mark it. This teleporter can be dyed as well.");
                this.ItemID = this.ItemID + 1;
                if (this.ItemID > 6184) { this.ItemID = 6173; }
            }
            else if (this.Movable)
            {
                from.SendMessage("The destination has been cleared. This must be secured in a house to mark it or in your backpack to change the appearance! This teleporter can be dyed as well.");
                m_Owner = null;
                m_MapDest = null;
                m_PointDest = new Point3D(0, 0, 0);
            }
            else
            {
                if (m_Owner != null)
                {
                    from.SendMessage("This teleporter has been marked already.");
                    return;
                }
                else
                {
                    m_Owner = from;
                    m_MapDest = this.Map;
                    m_PointDest = this.Location;
                    from.SendMessage("You have marked the teleporter.  You can now unlock it and move it to the house you wish to teleport from. It must be locked down in that house to use it.");
                }
            }
            return;
        }

        public bool CheckAccess(Mobile m)
        {
            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (house != null && (house.Public ? house.IsBanned(m) : !house.HasAccess(m)))
                return false;

            return (house != null && house.HasSecureAccess(m, m_Level));
        }

        public virtual void StartTeleport(Mobile m)
        {
            if (Worlds.RegionAllowedTeleport(m_MapDest, m_PointDest, m_PointDest.X, m_PointDest.Y) == false)
            {
                m.SendMessage("The destination seems magically unreachable.");
            }
            else if (m_Delay == TimeSpan.Zero)
            {
                DoTeleport(m);
            }
            else
            {
                Timer.DelayCall(m_Delay, new TimerStateCallback(DoTeleport_Callback), m);
            }
        }

        private void DoTeleport_Callback(object state)
        {
            DoTeleport((Mobile)state);
        }

        public virtual void DoTeleport(Mobile m)
        {
            Map map = m_MapDest;

            if (map == null || map == Map.Internal)
                map = m.Map;

            Point3D p = m_PointDest;

            if (p == Point3D.Zero)
                p = m.Location;

            Server.Mobiles.BaseCreature.TeleportPets(m, p, map);

            bool sendEffect = (!m.Hidden || m.AccessLevel <= AccessLevel.Counselor);

            if (m_SourceEffect && sendEffect)
                Effects.SendLocationEffect(m.Location, m.Map, 0x3728, 10, 10);

            m.MoveToWorld(p, map);

            if (m_DestEffect && sendEffect)
                Effects.SendLocationEffect(m.Location, m.Map, 0x3728, 10, 10);

            if (m_SoundID > 0 && sendEffect)
                Effects.PlaySound(m.Location, m.Map, m_SoundID);
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (this.Movable)
                m.SendMessage("This must be locked down in a house to use!");
            else if (m_Active)
            {
                int thisX = m_PointDest.X;
                int thisY = m_PointDest.Y;

                if (!m_Creatures && !m.Player)
                    return true;
                else if (m_Owner == null)
                {
                    m.SendMessage("This teleporter does not lead anywhere.");
                    return true;
                }
                else if (m_CombatCheck && SpellHelper.CheckCombat(m))
                {
                    m.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
                    return true;
                }
                //else if ( m.Map != m_MapDest ) // CANNOT TRAVEL BETWEEN WORLDS
                //{
                //	m_Owner = null;
                //	m_MapDest = null;
                //	m_PointDest = new Point3D( 0, 0, 0 );
                //	m.SendMessage("You cannot travel to another world this way.");
                //	return true;
                //}
                else if (m.Skills[SkillName.Magery].Base < 80.0 && m.Skills[SkillName.Elementalism].Base < 80.0 && m.Skills[SkillName.Necromancy].Base < 80.0 && m_MapDest == Map.Sosaria && thisX > 5798 && thisY > 2727 && thisX < 6105 && thisY < 3023) // TO STOP TRAVEL TO THE MOON
                {
                    m.SendMessage("You lack the magical essence to travel to the moon.");
                    return true;
                }
                else if (!SpellHelper.CheckMulti(m_PointDest, m_MapDest))
                {
                    m_Owner = null;
                    m_MapDest = null;
                    m_PointDest = new Point3D(0, 0, 0);
                    m.SendMessage("The home at the other end must have been demolished!");
                    return true;
                }
                else if (CheckAccess(m))
                {
                    StartTeleport(m);
                    return false;
                }
            }

            return true;
        }

        public bool Dye(Mobile from, DyeTub sender)
        {
            if (Deleted) return false;
            Hue = sender.DyedHue;
            return true;
        }

        public PlayersHouseTeleporter(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)3); // version
            writer.Write((bool)m_CombatCheck);
            writer.Write((bool)m_SourceEffect);
            writer.Write((bool)m_DestEffect);
            writer.Write((TimeSpan)m_Delay);
            writer.WriteEncodedInt((int)m_SoundID);
            writer.Write(m_Creatures);
            writer.Write(m_Active);
            writer.Write(m_PointDest);
            writer.Write(m_MapDest);
            writer.Write(m_Owner);
            writer.Write((int)m_Level);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 3:
                    {
                        m_CombatCheck = reader.ReadBool();
                        goto case 2;
                    }
                case 2:
                    {
                        m_SourceEffect = reader.ReadBool();
                        m_DestEffect = reader.ReadBool();
                        m_Delay = reader.ReadTimeSpan();
                        m_SoundID = reader.ReadEncodedInt();

                        goto case 1;
                    }
                case 1:
                    {
                        m_Creatures = reader.ReadBool();

                        goto case 0;
                    }
                case 0:
                    {
                        m_Active = reader.ReadBool();
                        m_PointDest = reader.ReadPoint3D();
                        m_MapDest = reader.ReadMap();
                        m_Owner = reader.ReadMobile();
                        m_Level = (SecureLevel)reader.ReadInt();

                        break;
                    }
            }
        }
    }
}