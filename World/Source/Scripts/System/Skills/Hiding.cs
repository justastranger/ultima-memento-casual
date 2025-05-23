using System;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Multis;
using Server.Mobiles;

namespace Server.SkillHandlers
{
    public class Hiding
    {
        private static bool m_CombatOverride;

        public static bool CombatOverride
        {
            get { return m_CombatOverride; }
            set { m_CombatOverride = value; }
        }

        public static void Initialize()
        {
            SkillInfo.Table[21].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse(Mobile m)
        {
            if (m.Spell != null)
            {
                m.SendLocalizedMessage(501238); // You are busy doing something else and cannot hide.
                return TimeSpan.FromSeconds(1.0);
            }

            if (Core.ML && m.Target != null)
            {
                Targeting.Target.Cancel(m);
            }

            double bonus = 0.0;

            BaseHouse house = BaseHouse.FindHouseAt(m);

            if (house != null && house.IsFriend(m))
            {
                bonus = 100.0;
            }
            else if (!Core.AOS)
            {
                if (house == null)
                    house = BaseHouse.FindHouseAt(new Point3D(m.X - 1, m.Y, 127), m.Map, 16);

                if (house == null)
                    house = BaseHouse.FindHouseAt(new Point3D(m.X + 1, m.Y, 127), m.Map, 16);

                if (house == null)
                    house = BaseHouse.FindHouseAt(new Point3D(m.X, m.Y - 1, 127), m.Map, 16);

                if (house == null)
                    house = BaseHouse.FindHouseAt(new Point3D(m.X, m.Y + 1, 127), m.Map, 16);

                if (house != null)
                    bonus = 50.0;
            }

            //int range = 18 - (int)(m.Skills[SkillName.Hiding].Value / 10);
            int range = Math.Min((int)((100 - m.Skills[SkillName.Hiding].Value) / 2) + 8, 18);  //Cap of 18 not OSI-exact, intentional difference

            bool badCombat = (!m_CombatOverride && m.Combatant != null && m.InRange(m.Combatant.Location, range) && m.Combatant.InLOS(m));
            if (m.CheckSkill(SkillName.Hiding, 0, 250)) { badCombat = false; } // ADDED A LITTLE EXTRA HIDING ABILITY FOR HIGH SKILL
            bool ok = (!badCombat /*&& m.CheckSkill( SkillName.Hiding, 0.0 - bonus, 100.0 - bonus )*/ );

            if (ok)
            {
                if (!m_CombatOverride)
                {
                    foreach (Mobile check in m.GetMobilesInRange(range))
                    {
                        if (check.InLOS(m) && check.Combatant == m)
                        {
                            badCombat = true;
                            ok = false;
                            break;
                        }
                    }
                }

                ok = (!badCombat && m.CheckSkill(SkillName.Hiding, 0.0 - bonus, 100.0 - bonus));
            }

            if (badCombat)
            {
                m.RevealingAction();

                m.LocalOverheadMessage(MessageType.Regular, 0x22, 501237); // You can't seem to hide right now.

                return TimeSpan.FromSeconds(4.0);
            }
            else
            {
                if (ok)
                {
                    m.Hidden = true;
                    m.Warmode = false;
                    m.LocalOverheadMessage(MessageType.Regular, 0x1F4, 501240); // You have hidden yourself well.

                    foreach (Mobile pet in World.Mobiles.Values)
                    {
                        if (pet is BaseCreature)
                        {
                            BaseCreature bc = (BaseCreature)pet;
                            if (bc.Controlled && bc.ControlMaster == m)
                                pet.Hidden = true;
                        }
                    }
                }
                else
                {
                    m.RevealingAction();

                    m.LocalOverheadMessage(MessageType.Regular, 0x22, 501241); // You can't seem to hide here.
                }

                return TimeSpan.FromSeconds(4.0);
            }
        }
    }
}