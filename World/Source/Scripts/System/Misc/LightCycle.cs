using System;
using Server;
using Server.Network;
using Server.Commands;

namespace Server
{
    public class LightCycle
    {
        public const int DayLevel = 0;
        public const int NightLevel = 20;
        public const int DungeonLevel = 26;
        public const int CaveLevel = 26;
        public const int JailLevel = 13;

        private static int m_LevelOverride = int.MinValue;

        public static int LevelOverride
        {
            get { return m_LevelOverride; }
            set
            {
                m_LevelOverride = value;

                for (int i = 0; i < NetState.Instances.Count; ++i)
                {
                    NetState ns = NetState.Instances[i];
                    Mobile m = ns.Mobile;

                    if (m != null)
                        m.CheckLightLevels(false);
                }
            }
        }

        public static void Initialize()
        {
            new LightCycleTimer().Start();
            EventSink.Login += new LoginEventHandler(OnLogin);

            CommandSystem.Register("GlobalLight", AccessLevel.GameMaster, new CommandEventHandler(Light_OnCommand));
        }

        [Usage("GlobalLight <value>")]
        [Description("Sets the current global light level.")]
        private static void Light_OnCommand(CommandEventArgs e)
        {
            if (e.Length >= 1)
            {
                LevelOverride = e.GetInt32(0);
                e.Mobile.SendMessage("Global light level override has been changed to {0}.", m_LevelOverride);
            }
            else
            {
                LevelOverride = int.MinValue;
                e.Mobile.SendMessage("Global light level override has been cleared.");
            }
        }

        public static void OnLogin(LoginEventArgs args)
        {
            Mobile m = args.Mobile;

            m.CheckLightLevels(true);
        }

        public static int ComputeLevelFor(Mobile from)
        {
            if (m_LevelOverride > int.MinValue)
                return m_LevelOverride;

            if (Server.Misc.Worlds.GetRegionName(from.Map, from.Location) == "the Underworld")
                return DungeonLevel;

            int hours, minutes;

            //Server.Items.Clock.GetTime( from.Map, from.X, from.Y, out hours, out minutes );
            Server.Items.Clock.GetTime(Map.Sosaria, 100, 100, out hours, out minutes);

            /* OSI times:
			 * 
			 * Midnight ->  3:59 AM : Night
			 *  4:00 AM -> 11:59 PM : Day
			 * 
			 * RunUO times:
			 * 
			 * 10:00 PM -> 11:59 PM : Scale to night
			 * Midnight ->  3:59 AM : Night
			 *  4:00 AM ->  5:59 AM : Scale to day
			 *  6:00 AM ->  9:59 PM : Day

				if ( hours < 4 )
					return NightLevel;

				if ( hours < 6 )
					return NightLevel + (((((hours - 4) * 60) + minutes) * (DayLevel - NightLevel)) / 120);

				if ( hours < 22 )
					return DayLevel;

				if ( hours < 24 )
					return DayLevel + (((((hours - 22) * 60) + minutes) * (NightLevel - DayLevel)) / 120);
			 */

            if (hours < 6)
                return NightLevel;

            if (hours < 8)
                return NightLevel + (((((hours - 6) * 60) + minutes) * (DayLevel - NightLevel)) / 120);

            if (hours < 18)
                return DayLevel;

            if (hours < 20)
                return DayLevel + (((((hours - 18) * 60) + minutes) * (NightLevel - DayLevel)) / 120);

            return NightLevel; // should never be
        }

        private class LightCycleTimer : Timer
        {
            public LightCycleTimer() : base(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(5.0))
            {
                Priority = TimerPriority.FiveSeconds;
            }

            protected override void OnTick()
            {
                for (int i = 0; i < NetState.Instances.Count; ++i)
                {
                    NetState ns = NetState.Instances[i];
                    Mobile m = ns.Mobile;

                    if (m != null)
                        m.CheckLightLevels(false);
                }
            }
        }

        public class NightSightTimer : Timer
        {
            private Mobile m_Owner;

            public NightSightTimer(Mobile owner) : base(TimeSpan.FromMinutes(Utility.Random(15, 25)))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                m_Owner.EndAction(typeof(LightCycle));
                m_Owner.LightLevel = 0;
                BuffInfo.RemoveBuff(m_Owner, BuffIcon.NightSight);
                BuffInfo.RemoveBuff(m_Owner, BuffIcon.EyesOfTheDead);
                BuffInfo.RemoveBuff(m_Owner, BuffIcon.PotionNightSight);
            }
        }
    }
}
