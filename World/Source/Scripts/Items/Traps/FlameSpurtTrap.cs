using System;
using Server;
using Server.Network;
using Server.Items;
using System.Collections.Generic;
using Server.Misc;
using System.Collections;
using Server.Mobiles;
using System.Text;
using System.IO;

namespace Server.Items
{
    public class FlameSpurtTrap : BaseTrap
    {
        private Item m_Spurt;
        private Timer m_Timer;

        [Constructable]
        public FlameSpurtTrap() : base(0x1B71)
        {
            Visible = false;
        }

        public virtual void StartTimer()
        {
            if (m_Timer == null)
                m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), new TimerCallback(Refresh));
        }

        public virtual void StopTimer()
        {
            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = null;
        }

        public virtual void CheckTimer()
        {
            Map map = this.Map;

            if (map != null && map.GetSector(GetWorldLocation()).Active)
                StartTimer();
            else
                StopTimer();
        }

        public override void OnLocationChange(Point3D oldLocation)
        {
            base.OnLocationChange(oldLocation);

            CheckTimer();
        }

        public override void OnMapChange()
        {
            base.OnMapChange();

            CheckTimer();
        }

        public override void OnSectorActivate()
        {
            base.OnSectorActivate();

            StartTimer();
        }

        public override void OnSectorDeactivate()
        {
            base.OnSectorDeactivate();

            StopTimer();
        }

        public override void OnDelete()
        {
            base.OnDelete();

            if (m_Spurt != null)
                m_Spurt.Delete();
        }

        public virtual void Refresh()
        {
            if (Deleted)
                return;

            bool foundPlayer = false;

            foreach (Mobile mob in GetMobilesInRange(3))
            {
                if (!mob.Player || !mob.Alive || mob.AccessLevel > AccessLevel.Counselor)
                    continue;

                if (Server.Misc.SeeIfGemInBag.GemInPocket(mob) == true || Server.Misc.SeeIfJewelInBag.JewelInPocket(mob) == true)
                    return;

                if (((this.Z + 8) >= mob.Z && (mob.Z + 16) > this.Z))
                {
                    foundPlayer = true;
                    break;
                }
            }

            if (!foundPlayer)
            {
                if (m_Spurt != null)
                    m_Spurt.Delete();

                m_Spurt = null;
            }
            else if (m_Spurt == null || m_Spurt.Deleted)
            {
                m_Spurt = new Static(0x3709);
                m_Spurt.MoveToWorld(this.Location, this.Map);

                Effects.PlaySound(GetWorldLocation(), Map, 0x309);
            }
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (!m.Alive || !m.Player || m.AccessLevel > AccessLevel.Counselor)
                return true;

            if (m is PlayerMobile && Spells.Research.ResearchAirWalk.UnderEffect(m))
            {
                Point3D air = new Point3D((m.X + 1), (m.Y + 1), (m.Z + 5));
                Effects.SendLocationParticles(EffectItem.Create(air, m.Map, EffectItem.DefaultDuration), 0x2007, 9, 32, Server.Misc.PlayerSettings.GetMySpellHue(true, m, 0), 0, 5022, 0);
                m.PlaySound(0x014);
                return true;
            }

            if (m.Player && m.Alive && !HiddenTrap.CheckTrapAvoidance(m, this))
            {
                CheckTimer();

                int itHurts = (int)((Utility.RandomMinMax(50, 200) * (100 - m.FireResistance)) / 100);
                m.Damage(itHurts, m);
                Spells.SpellHelper.Damage(TimeSpan.FromTicks(1), m, m, itHurts);
                m.PlaySound(m.Female ? 0x327 : 0x437);

                LoggingFunctions.LogTraps(m, "a spouting flame trap");
            }

            return false;
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m.Location == oldLocation || !m.Player || !m.Alive || m.AccessLevel > AccessLevel.Counselor)
                return;

            if (CheckRange(m.Location, oldLocation, 1))
            {
                CheckTimer();

                Spells.SpellHelper.Damage(TimeSpan.FromTicks(1), m, m, Utility.RandomMinMax(1, 10));
                m.PlaySound(m.Female ? 0x327 : 0x437);

                if (m.Body.IsHuman)
                    m.Animate(20, 1, 1, true, false, 0);
            }
        }

        public FlameSpurtTrap(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((Item)m_Spurt);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        Item item = reader.ReadItem();

                        if (item != null)
                            item.Delete();

                        CheckTimer();

                        break;
                    }
            }
        }
    }
}