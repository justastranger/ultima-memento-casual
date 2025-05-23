using System;
using System.Collections;
using Server;

namespace Server.Items
{
    public class HitLower
    {
        public static readonly TimeSpan AttackEffectDuration = TimeSpan.FromSeconds(10.0);
        public static readonly TimeSpan DefenseEffectDuration = TimeSpan.FromSeconds(8.0);

        private static Hashtable m_AttackTable = new Hashtable();

        public static bool IsUnderAttackEffect(Mobile m)
        {
            return m_AttackTable.Contains(m);
        }

        public static bool ApplyAttack(Mobile m)
        {
            if (IsUnderAttackEffect(m))
                return false;

            m_AttackTable[m] = new AttackTimer(m);
            m.SendLocalizedMessage(1062319); // Your attack chance has been reduced!
            return true;
        }

        private static void RemoveAttack(Mobile m)
        {
            m_AttackTable.Remove(m);
            m.SendLocalizedMessage(1062320); // Your attack chance has returned to normal.
        }

        private class AttackTimer : Timer
        {
            private Mobile m_Player;

            public AttackTimer(Mobile player) : base(AttackEffectDuration)
            {
                m_Player = player;

                Priority = TimerPriority.TwoFiftyMS;

                Start();
            }

            protected override void OnTick()
            {
                RemoveAttack(m_Player);
            }
        }

        private static Hashtable m_DefenseTable = new Hashtable();

        public static bool IsUnderDefenseEffect(Mobile m)
        {
            return m_DefenseTable.Contains(m);
        }

        public static bool ApplyDefense(Mobile m)
        {
            if (IsUnderDefenseEffect(m))
                return false;

            m_DefenseTable[m] = new DefenseTimer(m);
            m.SendLocalizedMessage(1062318); // Your defense chance has been reduced!
            return true;
        }

        private static void RemoveDefense(Mobile m)
        {
            m_DefenseTable.Remove(m);
            m.SendLocalizedMessage(1062321); // Your defense chance has returned to normal.
        }

        private class DefenseTimer : Timer
        {
            private Mobile m_Player;

            public DefenseTimer(Mobile player) : base(DefenseEffectDuration)
            {
                m_Player = player;

                Priority = TimerPriority.TwoFiftyMS;

                Start();
            }

            protected override void OnTick()
            {
                RemoveDefense(m_Player);
            }
        }
    }
}