using Server.Commands.Generic;
using Server.Commands;
using Server.Guilds;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Prompts;
using Server.Regions;
using Server.Targeting;
using Server;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;

namespace Server.Guilds
{
    #region Ranks
    [Flags]
    public enum RankFlags
    {
        None = 0x00000000,
        CanInvitePlayer = 0x00000001,
        AccessGuildItems = 0x00000002,
        RemoveLowestRank = 0x00000004,
        RemovePlayers = 0x00000008,
        CanPromoteDemote = 0x00000010,
        ControlWarStatus = 0x00000020,
        AllianceControl = 0x00000040,
        CanSetGuildTitle = 0x00000080,
        CanVote = 0x00000100,

        All = Member | CanInvitePlayer | RemovePlayers | CanPromoteDemote | ControlWarStatus | AllianceControl | CanSetGuildTitle,
        Member = RemoveLowestRank | AccessGuildItems | CanVote
    }

    public class RankDefinition
    {
        public static RankDefinition[] Ranks = new RankDefinition[]
            {
                new RankDefinition( 1062963, 0, RankFlags.None ),	//Ronin
				new RankDefinition( 1062962, 1, RankFlags.Member ),	//Member
				new RankDefinition( 1062961, 2, RankFlags.Member | RankFlags.RemovePlayers | RankFlags.CanInvitePlayer | RankFlags.CanSetGuildTitle | RankFlags.CanPromoteDemote ),	//Emmissary
				new RankDefinition( 1062960, 3, RankFlags.Member | RankFlags.ControlWarStatus ),	//Warlord
				new RankDefinition( 1062959, 4, RankFlags.All )	//Leader
			};
        public static RankDefinition Leader { get { return Ranks[4]; } }
        public static RankDefinition Member { get { return Ranks[1]; } }
        public static RankDefinition Lowest { get { return Ranks[0]; } }

        private TextDefinition m_Name;
        private int m_Rank;
        private RankFlags m_Flags;

        public TextDefinition Name { get { return m_Name; } }
        public int Rank { get { return m_Rank; } }
        public RankFlags Flags { get { return m_Flags; } }

        public RankDefinition(TextDefinition name, int rank, RankFlags flags)
        {
            m_Name = name;
            m_Rank = rank;
            m_Flags = flags;
        }

        public bool GetFlag(RankFlags flag)
        {
            return ((m_Flags & flag) != 0);
        }

        public void SetFlag(RankFlags flag, bool value)
        {
            if (value)
                m_Flags |= flag;
            else
                m_Flags &= ~flag;
        }
    }

    #endregion

    #region Alliances
    public class AllianceInfo
    {
        private static Dictionary<string, AllianceInfo> m_Alliances = new Dictionary<string, AllianceInfo>();

        public static Dictionary<string, AllianceInfo> Alliances
        {
            get { return m_Alliances; }
        }

        private string m_Name;
        private Guild m_Leader;
        private List<Guild> m_Members;
        private List<Guild> m_PendingMembers;

        public string Name
        {
            get { return m_Name; }
        }

        public void CalculateAllianceLeader()
        {
            m_Leader = ((m_Members.Count >= 2) ? m_Members[Utility.Random(m_Members.Count)] : null);
        }

        public void CheckLeader()
        {
            if (m_Leader == null || m_Leader.Disbanded)
            {
                CalculateAllianceLeader();

                if (m_Leader == null)
                    Disband();
            }
        }

        public Guild Leader
        {
            get
            {
                CheckLeader();
                return m_Leader;
            }
            set
            {
                if (m_Leader != value && value != null)
                    AllianceMessage(1070765, value.Name); // Your Alliance is now led by ~1_GUILDNAME~

                m_Leader = value;

                if (m_Leader == null)
                    CalculateAllianceLeader();
            }
        }

        public bool IsPendingMember(Guild g)
        {
            if (g.Alliance != this)
                return false;

            return m_PendingMembers.Contains(g);
        }

        public bool IsMember(Guild g)
        {
            if (g.Alliance != this)
                return false;

            return m_Members.Contains(g);
        }

        public AllianceInfo(Guild leader, string name, Guild partner)
        {
            m_Leader = leader;
            m_Name = name;

            m_Members = new List<Guild>();
            m_PendingMembers = new List<Guild>();

            leader.Alliance = this;
            partner.Alliance = this;

            if (!m_Alliances.ContainsKey(m_Name.ToLower()))
                m_Alliances.Add(m_Name.ToLower(), this);
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write((int)0);   //Version

            writer.Write(m_Name);
            writer.Write(m_Leader);

            writer.WriteGuildList(m_Members, true);
            writer.WriteGuildList(m_PendingMembers, true);

            if (!m_Alliances.ContainsKey(m_Name.ToLower()))
                m_Alliances.Add(m_Name.ToLower(), this);
        }

        public AllianceInfo(GenericReader reader)
        {
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Name = reader.ReadString();
                        m_Leader = reader.ReadGuild() as Guild;

                        m_Members = reader.ReadStrongGuildList<Guild>();
                        m_PendingMembers = reader.ReadStrongGuildList<Guild>();

                        break;
                    }
            }
        }

        public void AddPendingGuild(Guild g)
        {
            if (g.Alliance != this || m_PendingMembers.Contains(g) || m_Members.Contains(g))
                return;

            m_PendingMembers.Add(g);
        }

        public void TurnToMember(Guild g)
        {
            if (g.Alliance != this || !m_PendingMembers.Contains(g) || m_Members.Contains(g))
                return;

            g.GuildMessage(1070760, this.Name); // Your Guild has joined the ~1_ALLIANCENAME~ Alliance.
            AllianceMessage(1070761, g.Name); // A new Guild has joined your Alliance: ~1_GUILDNAME~

            m_PendingMembers.Remove(g);
            m_Members.Add(g);
            g.Alliance.InvalidateMemberProperties();
        }

        public void RemoveGuild(Guild g)
        {
            if (m_PendingMembers.Contains(g))
            {
                m_PendingMembers.Remove(g);
            }

            if (m_Members.Contains(g))  //Sanity, just incase someone with a custom script adds a character to BOTH arrays
            {
                m_Members.Remove(g);
                g.InvalidateMemberProperties();

                g.GuildMessage(1070763, this.Name); // Your Guild has been removed from the ~1_ALLIANCENAME~ Alliance.
                AllianceMessage(1070764, g.Name); // A Guild has left your Alliance: ~1_GUILDNAME~
            }

            //g.Alliance = null;	//NO G.Alliance call here.  Set the Guild's Alliance to null, if you JUST use RemoveGuild, it removes it from the alliance, but doesn't remove the link from the guild to the alliance.  setting g.Alliance will call this method.
            //to check on OSI: have 3 guilds, make 2 of them a member, one pending.  remove one of the memebers.  alliance still exist?
            //ANSWER: NO

            if (g == m_Leader)
            {
                CalculateAllianceLeader();

                /*
				if( m_Leader == null ) //only when m_members.count < 2
					Disband();
				else
					AllianceMessage( 1070765, m_Leader.Name ); // Your Alliance is now led by ~1_GUILDNAME~
				*/
            }

            if (m_Members.Count < 2)
                Disband();
        }

        public void Disband()
        {
            AllianceMessage(1070762); // Your Alliance has dissolved.

            for (int i = 0; i < m_PendingMembers.Count; i++)
                m_PendingMembers[i].Alliance = null;

            for (int i = 0; i < m_Members.Count; i++)
                m_Members[i].Alliance = null;

            AllianceInfo aInfo = null;

            m_Alliances.TryGetValue(m_Name.ToLower(), out aInfo);

            if (aInfo == this)
                m_Alliances.Remove(m_Name.ToLower());
        }

        public void InvalidateMemberProperties()
        {
            InvalidateMemberProperties(false);
        }

        public void InvalidateMemberProperties(bool onlyOPL)
        {
            for (int i = 0; i < m_Members.Count; i++)
            {
                Guild g = m_Members[i];

                g.InvalidateMemberProperties(onlyOPL);
            }
        }

        public void InvalidateMemberNotoriety()
        {
            for (int i = 0; i < m_Members.Count; i++)
                m_Members[i].InvalidateMemberNotoriety();
        }

        #region Alliance[Text]Message(...)
        public void AllianceMessage(int num, bool append, string format, params object[] args)
        {
            AllianceMessage(num, append, String.Format(format, args));
        }
        public void AllianceMessage(int number)
        {
            for (int i = 0; i < m_Members.Count; ++i)
                m_Members[i].GuildMessage(number);
        }
        public void AllianceMessage(int number, string args)
        {
            AllianceMessage(number, args, 0x3B2);
        }
        public void AllianceMessage(int number, string args, int hue)
        {
            for (int i = 0; i < m_Members.Count; ++i)
                m_Members[i].GuildMessage(number, args, hue);
        }
        public void AllianceMessage(int number, bool append, string affix)
        {
            AllianceMessage(number, append, affix, "", 0x3B2);
        }
        public void AllianceMessage(int number, bool append, string affix, string args)
        {
            AllianceMessage(number, append, affix, args, 0x3B2);
        }
        public void AllianceMessage(int number, bool append, string affix, string args, int hue)
        {
            for (int i = 0; i < m_Members.Count; ++i)
                m_Members[i].GuildMessage(number, append, affix, args, hue);
        }

        public void AllianceTextMessage(string text)
        {
            AllianceTextMessage(0x3B2, text);
        }
        public void AllianceTextMessage(string format, params object[] args)
        {
            AllianceTextMessage(0x3B2, String.Format(format, args));
        }
        public void AllianceTextMessage(int hue, string text)
        {
            for (int i = 0; i < m_Members.Count; ++i)
                m_Members[i].GuildTextMessage(hue, text);
        }
        public void AllianceTextMessage(int hue, string format, params object[] args)
        {
            AllianceTextMessage(hue, String.Format(format, args));
        }

        public void AllianceChat(Mobile from, int hue, string text)
        {
            Packet p = null;
            for (int i = 0; i < m_Members.Count; i++)
            {
                Guild g = m_Members[i];

                for (int j = 0; j < g.Members.Count; j++)
                {
                    Mobile m = g.Members[j];

                    NetState state = m.NetState;

                    if (state != null)
                    {
                        if (p == null)
                            p = Packet.Acquire(new UnicodeMessage(from.Serial, from.Body, MessageType.Alliance, hue, 3, from.Language, from.Name, text));

                        state.Send(p);
                    }
                }
            }

            Packet.Release(p);
        }

        public void AllianceChat(Mobile from, string text)
        {
            PlayerMobile pm = from as PlayerMobile;

            AllianceChat(from, (pm == null) ? 0x3B2 : pm.AllianceMessageHue, text);
        }
        #endregion

        public class AllianceRosterGump : GuildDiplomacyGump
        {
            protected override bool AllowAdvancedSearch { get { return false; } }

            private AllianceInfo m_Alliance;

            public AllianceRosterGump(PlayerMobile pm, Guild g, AllianceInfo alliance) : base(pm, g, true, "", 0, alliance.m_Members, alliance.Name)
            {
                m_Alliance = alliance;
            }

            public AllianceRosterGump(PlayerMobile pm, Guild g, AllianceInfo alliance, IComparer<Guild> currentComparer, bool ascending, string filter, int startNumber) : base(pm, g, currentComparer, ascending, filter, startNumber, alliance.m_Members, alliance.Name)
            {
                m_Alliance = alliance;
            }

            public override Gump GetResentGump(PlayerMobile pm, Guild g, IComparer<Guild> comparer, bool ascending, string filter, int startNumber)
            {
                return new AllianceRosterGump(pm, g, m_Alliance, comparer, ascending, filter, startNumber);
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                if (info.ButtonID != 8) //So that they can't get to the AdvancedSearch button
                    base.OnResponse(sender, info);
            }
        }
    }
    #endregion

    #region Wars
    public enum WarStatus
    {
        InProgress = -1,
        Win,
        Lose,
        Draw,
        Pending
    }

    public class WarDeclaration
    {
        private int m_Kills;
        private int m_MaxKills;

        private TimeSpan m_WarLength;
        private DateTime m_WarBeginning;

        private Guild m_Guild;
        private Guild m_Opponent;

        private bool m_WarRequester;

        public int Kills
        {
            get { return m_Kills; }
            set { m_Kills = value; }
        }
        public int MaxKills
        {
            get { return m_MaxKills; }
            set { m_MaxKills = value; }
        }
        public TimeSpan WarLength
        {
            get { return m_WarLength; }
            set { m_WarLength = value; }
        }
        public Guild Opponent
        {
            get { return m_Opponent; }
        }
        public Guild Guild
        {
            get { return m_Guild; }
        }
        public DateTime WarBeginning
        {
            get { return m_WarBeginning; }
            set { m_WarBeginning = value; }
        }
        public bool WarRequester
        {
            get { return m_WarRequester; }
            set { m_WarRequester = value; }
        }

        public WarDeclaration(Guild g, Guild opponent, int maxKills, TimeSpan warLength, bool warRequester)
        {
            m_Guild = g;
            m_MaxKills = maxKills;
            m_Opponent = opponent;
            m_WarLength = warLength;
            m_WarRequester = warRequester;
        }

        public WarDeclaration(GenericReader reader)
        {
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Kills = reader.ReadInt();
                        m_MaxKills = reader.ReadInt();

                        m_WarLength = reader.ReadTimeSpan();
                        m_WarBeginning = reader.ReadDateTime();

                        m_Guild = reader.ReadGuild() as Guild;
                        m_Opponent = reader.ReadGuild() as Guild;

                        m_WarRequester = reader.ReadBool();

                        break;
                    }
            }
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write((int)0);   //version

            writer.Write(m_Kills);
            writer.Write(m_MaxKills);

            writer.Write(m_WarLength);
            writer.Write(m_WarBeginning);

            writer.Write(m_Guild);
            writer.Write(m_Opponent);

            writer.Write(m_WarRequester);
        }

        public WarStatus Status
        {
            get
            {
                if (m_Opponent == null || m_Opponent.Disbanded)
                    return WarStatus.Win;

                if (m_Guild == null || m_Guild.Disbanded)
                    return WarStatus.Lose;

                WarDeclaration w = m_Opponent.FindActiveWar(m_Guild);

                if (m_Opponent.FindPendingWar(m_Guild) != null && m_Guild.FindPendingWar(m_Opponent) != null)
                    return WarStatus.Pending;

                if (w == null)
                    return WarStatus.Win;

                if (m_WarLength != TimeSpan.Zero && (m_WarBeginning + m_WarLength) < DateTime.Now)
                {
                    if (m_Kills > w.m_Kills)
                        return WarStatus.Win;
                    else if (m_Kills < w.m_Kills)
                        return WarStatus.Lose;
                    else
                        return WarStatus.Draw;
                }
                else if (m_MaxKills > 0)
                {
                    if (m_Kills >= m_MaxKills)
                        return WarStatus.Win;
                    else if (w.m_Kills >= w.MaxKills)
                        return WarStatus.Lose;
                }

                return WarStatus.InProgress;
            }
        }
    }

    public class WarTimer : Timer
    {
        private static TimeSpan InternalDelay = TimeSpan.FromMinutes(1.0);

        public static void Initialize()
        {
            if (Guild.NewGuildSystem)
                new WarTimer().Start();
        }

        public WarTimer() : base(InternalDelay, InternalDelay)
        {
            Priority = TimerPriority.FiveSeconds;
        }

        protected override void OnTick()
        {
            foreach (Guild g in Guild.List.Values)
                g.CheckExpiredWars();
        }
    }

    #endregion

    public class Guild : BaseGuild
    {
        public static void Configure()
        {
            EventSink.CreateGuild += new CreateGuildHandler(EventSink_CreateGuild);
            EventSink.GuildGumpRequest += new GuildGumpRequestHandler(EventSink_GuildGumpRequest);

            CommandSystem.Register("GuildProps", AccessLevel.Counselor, new CommandEventHandler(GuildProps_OnCommand));
        }

        #region GuildProps
        [Usage("GuildProps")]
        [Description("Opens a menu where you can view and edit guild properties of a targeted player or guild stone.  If the new Guild system is active, also brings up the guild gump.")]
        private static void GuildProps_OnCommand(CommandEventArgs e)
        {
            string arg = e.ArgString.Trim();
            Mobile from = e.Mobile;

            if (arg.Length == 0)
            {
                e.Mobile.Target = new GuildPropsTarget();
            }
            else
            {
                Guild g = null;

                int id;

                if (int.TryParse(arg, out id))
                    g = Guild.Find(id) as Guild;

                if (g == null)
                {
                    g = Guild.FindByAbbrev(arg) as Guild;

                    if (g == null)
                        g = Guild.FindByName(arg) as Guild;
                }

                if (g != null)
                {
                    from.SendGump(new PropertiesGump(from, g));

                    if (NewGuildSystem && from.AccessLevel >= AccessLevel.GameMaster && from is PlayerMobile)
                        from.SendGump(new GuildInfoGump((PlayerMobile)from, g));
                }
            }

        }

        private class GuildPropsTarget : Target
        {
            public GuildPropsTarget() : base(-1, true, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (!BaseCommand.IsAccessible(from, o))
                {
                    from.SendMessage("That is not accessible.");
                    return;
                }

                Guild g = null;

                if (o is Guildstone)
                {
                    Guildstone stone = o as Guildstone;
                    if (stone.Guild == null || stone.Guild.Disbanded)
                    {
                        from.SendMessage("The guild associated with that Guildstone no longer exists");
                        return;
                    }
                    else
                        g = stone.Guild;
                }
                else if (o is Mobile)
                {
                    g = ((Mobile)o).Guild as Guild;
                }

                if (g != null)
                {
                    from.SendGump(new PropertiesGump(from, g));

                    if (NewGuildSystem && from.AccessLevel >= AccessLevel.GameMaster && from is PlayerMobile)
                        from.SendGump(new GuildInfoGump((PlayerMobile)from, g));
                }
                else
                {
                    from.SendMessage("That is not in a guild!");
                }
            }
        }
        #endregion

        #region EventSinks
        public static void EventSink_GuildGumpRequest(GuildGumpRequestArgs args)
        {
            PlayerMobile pm = args.Mobile as PlayerMobile;
            if (!NewGuildSystem || pm == null)
                return;

            if (pm.Guild == null)
                pm.SendGump(new CreateGuildGump(pm));
            else
                pm.SendGump(new GuildInfoGump(pm, pm.Guild as Guild));
        }

        public static BaseGuild EventSink_CreateGuild(CreateGuildEventArgs args)
        {
            return (BaseGuild)(new Guild(args.Id));
        }
        #endregion

        public static bool NewGuildSystem { get { return Core.SE; } }

        public static readonly int RegistrationFee = 25000;
        public static readonly int AbbrevLimit = 4;
        public static readonly int NameLimit = 40;
        public static readonly int MajorityPercentage = 66;
        public static readonly TimeSpan InactiveTime = TimeSpan.FromDays(30);

        #region New Alliances

        public AllianceInfo Alliance
        {
            get
            {
                if (m_AllianceInfo != null)
                    return m_AllianceInfo;
                else if (m_AllianceLeader != null)
                    return m_AllianceLeader.m_AllianceInfo;
                else
                    return null;
            }
            set
            {
                AllianceInfo current = this.Alliance;

                if (value == current)
                    return;

                if (current != null)
                {
                    current.RemoveGuild(this);
                }

                if (value != null)
                {

                    if (value.Leader == this)
                        m_AllianceInfo = value;
                    else
                        m_AllianceLeader = value.Leader;

                    value.AddPendingGuild(this);
                }
                else
                {
                    m_AllianceInfo = null;
                    m_AllianceLeader = null;
                }
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public string AllianceName
        {
            get
            {
                AllianceInfo al = this.Alliance;
                if (al != null)
                    return al.Name;

                return null;
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public Guild AllianceLeader
        {
            get
            {
                AllianceInfo al = this.Alliance;

                if (al != null)
                    return al.Leader;

                return null;
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public bool IsAllianceMember
        {
            get
            {
                AllianceInfo al = this.Alliance;

                if (al != null)
                    return al.IsMember(this);

                return false;
            }
        }

        [CommandProperty(AccessLevel.Counselor)]
        public bool IsAlliancePendingMember
        {
            get
            {
                AllianceInfo al = this.Alliance;

                if (al != null)
                    return al.IsPendingMember(this);

                return false;
            }
        }

        public static Guild GetAllianceLeader(Guild g)
        {
            AllianceInfo alliance = g.Alliance;

            if (alliance != null && alliance.Leader != null && alliance.IsMember(g))
                return alliance.Leader;

            return g;
        }

        #endregion

        #region New Wars

        public List<WarDeclaration> PendingWars
        {
            get { return m_PendingWars; }
        }
        public List<WarDeclaration> AcceptedWars
        {
            get { return m_AcceptedWars; }
        }

        public WarDeclaration FindPendingWar(Guild g)
        {
            for (int i = 0; i < PendingWars.Count; i++)
            {
                WarDeclaration w = PendingWars[i];

                if (w.Opponent == g)
                    return w;
            }

            return null;
        }

        public WarDeclaration FindActiveWar(Guild g)
        {
            for (int i = 0; i < AcceptedWars.Count; i++)
            {
                WarDeclaration w = AcceptedWars[i];

                if (w.Opponent == g)
                    return w;
            }

            return null;
        }

        public void CheckExpiredWars()
        {
            for (int i = 0; i < AcceptedWars.Count; i++)
            {
                WarDeclaration w = AcceptedWars[i];
                Guild g = w.Opponent;

                WarStatus status = w.Status;

                if (status != WarStatus.InProgress)
                {
                    AllianceInfo myAlliance = this.Alliance;
                    bool inAlliance = (myAlliance != null && myAlliance.IsMember(this));

                    AllianceInfo otherAlliance = ((g != null) ? g.Alliance : null);
                    bool otherInAlliance = (otherAlliance != null && otherAlliance.IsMember(this));

                    if (inAlliance)
                    {
                        myAlliance.AllianceMessage(1070739 + (int)status, (g == null) ? "a deleted opponent" : (otherInAlliance ? otherAlliance.Name : g.Name));
                        myAlliance.InvalidateMemberProperties();
                    }
                    else
                    {
                        GuildMessage(1070739 + (int)status, (g == null) ? "a deleted opponent" : (otherInAlliance ? otherAlliance.Name : g.Name));
                        InvalidateMemberProperties();
                    }

                    this.AcceptedWars.Remove(w);

                    if (g != null)
                    {
                        if (status != WarStatus.Draw)
                            status = (WarStatus)((int)status + 1 % 2);

                        if (otherInAlliance)
                        {
                            otherAlliance.AllianceMessage(1070739 + (int)status, (inAlliance ? this.Alliance.Name : this.Name));
                            otherAlliance.InvalidateMemberProperties();
                        }
                        else
                        {
                            g.GuildMessage(1070739 + (int)status, (inAlliance ? this.Alliance.Name : this.Name));
                            g.InvalidateMemberProperties();
                        }

                        g.AcceptedWars.Remove(g.FindActiveWar(this));
                    }
                }
            }

            for (int i = 0; i < PendingWars.Count; i++)
            {
                WarDeclaration w = PendingWars[i];
                Guild g = w.Opponent;

                if (w.Status != WarStatus.Pending)
                {
                    //All sanity in here
                    this.PendingWars.Remove(w);

                    if (g != null)
                    {
                        g.PendingWars.Remove(g.FindPendingWar(this));
                    }
                }
            }
        }

        public static void HandleDeath(Mobile victim)
        {
            HandleDeath(victim, null);
        }

        public static void HandleDeath(Mobile victim, Mobile killer)
        {
            if (!NewGuildSystem)
                return;

            if (killer == null)
                killer = victim.FindMostRecentDamager(false);

            if (killer == null || victim.Guild == null || killer.Guild == null)
                return;

            Guild victimGuild = GetAllianceLeader(victim.Guild as Guild);
            Guild killerGuild = GetAllianceLeader(killer.Guild as Guild);

            WarDeclaration war = killerGuild.FindActiveWar(victimGuild);

            if (war == null)
                return;

            war.Kills++;

            if (war.Opponent == victimGuild)
                killerGuild.CheckExpiredWars();
            else
                victimGuild.CheckExpiredWars();
        }
        #endregion

        #region Var declarations
        private Mobile m_Leader;

        private string m_Name;
        private string m_Abbreviation;

        private List<Guild> m_Allies;
        private List<Guild> m_Enemies;

        private List<Mobile> m_Members;

        private Item m_Guildstone;
        private Item m_Teleporter;

        private string m_Charter;
        private string m_Website;

        private DateTime m_LastFealty;

        private GuildType m_Type;
        private DateTime m_TypeLastChange;

        private List<Guild> m_AllyDeclarations, m_AllyInvitations;

        private List<Guild> m_WarDeclarations, m_WarInvitations;
        private List<Mobile> m_Candidates, m_Accepted;

        private List<WarDeclaration> m_PendingWars, m_AcceptedWars;

        private AllianceInfo m_AllianceInfo;
        private Guild m_AllianceLeader;
        #endregion

        public Guild(Mobile leader, string name, string abbreviation)
        {
            #region Ctor mumbo-jumbo
            m_Leader = leader;

            m_Members = new List<Mobile>();
            m_Allies = new List<Guild>();
            m_Enemies = new List<Guild>();
            m_WarDeclarations = new List<Guild>();
            m_WarInvitations = new List<Guild>();
            m_AllyDeclarations = new List<Guild>();
            m_AllyInvitations = new List<Guild>();
            m_Candidates = new List<Mobile>();
            m_Accepted = new List<Mobile>();

            m_LastFealty = DateTime.Now;

            m_Name = name;
            m_Abbreviation = abbreviation;

            m_TypeLastChange = DateTime.MinValue;

            AddMember(m_Leader);

            if (m_Leader is PlayerMobile)
                ((PlayerMobile)m_Leader).GuildRank = RankDefinition.Leader;

            m_AcceptedWars = new List<WarDeclaration>();
            m_PendingWars = new List<WarDeclaration>();
            #endregion
        }

        public Guild(int id) : base(id)//serialization ctor
        {
        }

        public void InvalidateMemberProperties()
        {
            InvalidateMemberProperties(false);
        }

        public void InvalidateMemberProperties(bool onlyOPL)
        {
            if (m_Members != null)
            {
                for (int i = 0; i < m_Members.Count; i++)
                {
                    Mobile m = m_Members[i];
                    m.InvalidateProperties();

                    if (!onlyOPL)
                        m.Delta(MobileDelta.Noto);
                }
            }
        }

        public void InvalidateMemberNotoriety()
        {
            if (m_Members != null)
            {
                for (int i = 0; i < m_Members.Count; i++)
                    m_Members[i].Delta(MobileDelta.Noto);
            }
        }

        public void InvalidateWarNotoriety()
        {
            Guild g = GetAllianceLeader(this);

            if (g.Alliance != null)
                g.Alliance.InvalidateMemberNotoriety();
            else
                g.InvalidateMemberNotoriety();

            if (g.AcceptedWars == null)
                return;

            foreach (WarDeclaration warDec in g.AcceptedWars)
            {
                Guild opponent = warDec.Opponent;

                if (opponent.Alliance != null)
                    opponent.Alliance.InvalidateMemberNotoriety();
                else
                    opponent.InvalidateMemberNotoriety();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Leader
        {
            get
            {
                if (m_Leader == null || m_Leader.Deleted || m_Leader.Guild != this)
                    CalculateGuildmaster();

                return m_Leader;
            }
            set
            {
                if (value != null)
                    this.AddMember(value); //Also removes from old guild.

                if (m_Leader is PlayerMobile && m_Leader.Guild == this)
                    ((PlayerMobile)m_Leader).GuildRank = RankDefinition.Member;

                m_Leader = value;

                if (m_Leader is PlayerMobile)
                    ((PlayerMobile)m_Leader).GuildRank = RankDefinition.Leader;
            }
        }

        public override bool Disbanded
        {
            get
            {
                return (m_Leader == null || m_Leader.Deleted);
            }
        }

        public override void OnDelete(Mobile mob)
        {
            RemoveMember(mob);
        }

        public void Disband()
        {
            m_Leader = null;

            BaseGuild.List.Remove(this.Id);

            foreach (Mobile m in m_Members)
            {
                m.SendLocalizedMessage(502131); // Your guild has disbanded.

                if (m is PlayerMobile)
                    ((PlayerMobile)m).GuildRank = RankDefinition.Lowest;

                m.Guild = null;
            }

            m_Members.Clear();

            for (int i = m_Allies.Count - 1; i >= 0; --i)
                if (i < m_Allies.Count)
                    RemoveAlly(m_Allies[i]);

            for (int i = m_Enemies.Count - 1; i >= 0; --i)
                if (i < m_Enemies.Count)
                    RemoveEnemy(m_Enemies[i]);

            if (!NewGuildSystem && m_Guildstone != null)
                m_Guildstone.Delete();

            m_Guildstone = null;

            CheckExpiredWars();

            Alliance = null;
        }

        #region Is<something>(...)
        public bool IsMember(Mobile m)
        {
            return m_Members.Contains(m);
        }

        public bool IsAlly(Guild g)
        {
            if (NewGuildSystem)
            {
                return (Alliance != null && Alliance.IsMember(this) && Alliance.IsMember(g));
            }

            return m_Allies.Contains(g);
        }

        public bool IsEnemy(Guild g)
        {
            if (NewGuildSystem)
                return IsWar(g);

            if (m_Type != GuildType.Regular && g.m_Type != GuildType.Regular && m_Type != g.m_Type)
                return true;

            return m_Enemies.Contains(g);
        }

        public bool IsWar(Guild g)
        {
            if (g == null)
                return false;

            if (NewGuildSystem)
            {
                Guild guild = GetAllianceLeader(this);
                Guild otherGuild = GetAllianceLeader(g);

                if (guild.FindActiveWar(otherGuild) != null)
                    return true;

                return false;
            }

            return m_Enemies.Contains(g);
        }
        #endregion

        #region Serialization
        public override void Serialize(GenericWriter writer)
        {
            if (this.LastFealty + TimeSpan.FromDays(1.0) < DateTime.Now)
                this.CalculateGuildmaster();

            CheckExpiredWars();

            if (Alliance != null)
                Alliance.CheckLeader();

            writer.Write((int)5);//version

            #region War Serialization
            writer.Write(m_PendingWars.Count);

            for (int i = 0; i < m_PendingWars.Count; i++)
            {
                m_PendingWars[i].Serialize(writer);
            }

            writer.Write(m_AcceptedWars.Count);

            for (int i = 0; i < m_AcceptedWars.Count; i++)
            {
                m_AcceptedWars[i].Serialize(writer);
            }
            #endregion

            #region Alliances

            bool isAllianceLeader = (m_AllianceLeader == null && m_AllianceInfo != null);
            writer.Write(isAllianceLeader);

            if (isAllianceLeader)
                m_AllianceInfo.Serialize(writer);
            else
                writer.Write(m_AllianceLeader);

            #endregion

            //

            writer.WriteGuildList(m_AllyDeclarations, true);
            writer.WriteGuildList(m_AllyInvitations, true);

            writer.Write(m_TypeLastChange);

            writer.Write((int)m_Type);

            writer.Write(m_LastFealty);

            writer.Write(m_Leader);
            writer.Write(m_Name);
            writer.Write(m_Abbreviation);

            writer.WriteGuildList<Guild>(m_Allies, true);
            writer.WriteGuildList<Guild>(m_Enemies, true);
            writer.WriteGuildList(m_WarDeclarations, true);
            writer.WriteGuildList(m_WarInvitations, true);

            writer.Write(m_Members, true);
            writer.Write(m_Candidates, true);
            writer.Write(m_Accepted, true);

            writer.Write(m_Guildstone);
            writer.Write(m_Teleporter);

            writer.Write(m_Charter);
            writer.Write(m_Website);
        }

        public override void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();

            switch (version)
            {
                case 5:
                    {
                        int count = reader.ReadInt();

                        m_PendingWars = new List<WarDeclaration>();
                        for (int i = 0; i < count; i++)
                        {
                            m_PendingWars.Add(new WarDeclaration(reader));
                        }

                        count = reader.ReadInt();
                        m_AcceptedWars = new List<WarDeclaration>();
                        for (int i = 0; i < count; i++)
                        {
                            m_AcceptedWars.Add(new WarDeclaration(reader));
                        }

                        bool isAllianceLeader = reader.ReadBool();

                        if (isAllianceLeader)
                            m_AllianceInfo = new AllianceInfo(reader);
                        else
                            m_AllianceLeader = reader.ReadGuild() as Guild;

                        goto case 4;
                    }
                case 4:
                    {
                        m_AllyDeclarations = reader.ReadStrongGuildList<Guild>();
                        m_AllyInvitations = reader.ReadStrongGuildList<Guild>();

                        goto case 3;
                    }
                case 3:
                    {
                        m_TypeLastChange = reader.ReadDateTime();

                        goto case 2;
                    }
                case 2:
                    {
                        m_Type = (GuildType)reader.ReadInt();

                        goto case 1;
                    }
                case 1:
                    {
                        m_LastFealty = reader.ReadDateTime();

                        goto case 0;
                    }
                case 0:
                    {
                        m_Leader = reader.ReadMobile();

                        if (m_Leader is PlayerMobile)
                            ((PlayerMobile)m_Leader).GuildRank = RankDefinition.Leader;

                        m_Name = reader.ReadString();
                        m_Abbreviation = reader.ReadString();

                        m_Allies = reader.ReadStrongGuildList<Guild>();
                        m_Enemies = reader.ReadStrongGuildList<Guild>();
                        m_WarDeclarations = reader.ReadStrongGuildList<Guild>();
                        m_WarInvitations = reader.ReadStrongGuildList<Guild>();

                        m_Members = reader.ReadStrongMobileList();
                        m_Candidates = reader.ReadStrongMobileList();
                        m_Accepted = reader.ReadStrongMobileList();

                        m_Guildstone = reader.ReadItem();
                        m_Teleporter = reader.ReadItem();

                        m_Charter = reader.ReadString();
                        m_Website = reader.ReadString();

                        break;
                    }
            }

            if (m_AllyDeclarations == null)
                m_AllyDeclarations = new List<Guild>();

            if (m_AllyInvitations == null)
                m_AllyInvitations = new List<Guild>();

            if (m_AcceptedWars == null)
                m_AcceptedWars = new List<WarDeclaration>();

            if (m_PendingWars == null)
                m_PendingWars = new List<WarDeclaration>();

            /*
			if ( ( !NewGuildSystem && m_Guildstone == null )|| m_Members.Count == 0 )
				Disband();
			*/

            Timer.DelayCall(TimeSpan.Zero, new TimerCallback(VerifyGuild_Callback));
        }

        private void VerifyGuild_Callback()
        {
            if ((!NewGuildSystem && m_Guildstone == null) || m_Members.Count == 0)
                Disband();

            CheckExpiredWars();

            AllianceInfo alliance = this.Alliance;

            if (alliance != null)
                alliance.CheckLeader();

            alliance = this.Alliance;   //CheckLeader could possibly change the value of this.Alliance

            if (alliance != null && !alliance.IsMember(this) && !alliance.IsPendingMember(this))    //This block is there to fix a bug in the code in an older version.  
                this.Alliance = null;   //Will call Alliance.RemoveGuild which will set it null & perform all the pertient checks as far as alliacne disbanding

        }

        #endregion

        #region Add/Remove Member/Old Ally/Old Enemy
        public void AddMember(Mobile m)
        {
            if (!m_Members.Contains(m))
            {
                if (m.Guild != null && m.Guild != this)
                    ((Guild)m.Guild).RemoveMember(m);

                m_Members.Add(m);
                m.Guild = this;

                if (!NewGuildSystem)
                    m.GuildFealty = m_Leader;
                else
                    m.GuildFealty = null;

                if (m is PlayerMobile)
                    ((PlayerMobile)m).GuildRank = RankDefinition.Lowest;

                Guild guild = m.Guild as Guild;

                if (guild != null)
                    guild.InvalidateWarNotoriety();
            }
        }

        public void RemoveMember(Mobile m)
        {
            RemoveMember(m, 1018028); // You have been dismissed from your guild.
        }
        public void RemoveMember(Mobile m, int message)
        {
            if (m_Members.Contains(m))
            {
                m_Members.Remove(m);

                Guild guild = m.Guild as Guild;

                m.Guild = null;

                if (m is PlayerMobile)
                    ((PlayerMobile)m).GuildRank = RankDefinition.Lowest;

                if (message > 0)
                    m.SendLocalizedMessage(message);

                if (m == m_Leader)
                {
                    CalculateGuildmaster();

                    if (m_Leader == null)
                        Disband();
                }

                if (m_Members.Count == 0)
                    Disband();

                if (guild != null)
                    guild.InvalidateWarNotoriety();

                m.Delta(MobileDelta.Noto);
            }
        }

        public void AddAlly(Guild g)
        {
            if (!m_Allies.Contains(g))
            {
                m_Allies.Add(g);

                g.AddAlly(this);
            }
        }

        public void RemoveAlly(Guild g)
        {
            if (m_Allies.Contains(g))
            {
                m_Allies.Remove(g);

                g.RemoveAlly(this);
            }
        }

        public void AddEnemy(Guild g)
        {
            if (!m_Enemies.Contains(g))
            {
                m_Enemies.Add(g);

                g.AddEnemy(this);
            }
        }

        public void RemoveEnemy(Guild g)
        {
            if (m_Enemies != null && m_Enemies.Contains(g))
            {
                m_Enemies.Remove(g);

                g.RemoveEnemy(this);
            }
        }

        #endregion

        #region Guild[Text]Message(...)
        public void GuildMessage(int num, bool append, string format, params object[] args)
        {
            GuildMessage(num, append, String.Format(format, args));
        }
        public void GuildMessage(int number)
        {
            for (int i = 0; i < m_Members.Count; ++i)
                m_Members[i].SendLocalizedMessage(number);
        }
        public void GuildMessage(int number, string args)
        {
            GuildMessage(number, args, 0x3B2);
        }
        public void GuildMessage(int number, string args, int hue)
        {
            for (int i = 0; i < m_Members.Count; ++i)
                m_Members[i].SendLocalizedMessage(number, args, hue);
        }
        public void GuildMessage(int number, bool append, string affix)
        {
            GuildMessage(number, append, affix, "", 0x3B2);
        }
        public void GuildMessage(int number, bool append, string affix, string args)
        {
            GuildMessage(number, append, affix, args, 0x3B2);
        }
        public void GuildMessage(int number, bool append, string affix, string args, int hue)
        {
            for (int i = 0; i < m_Members.Count; ++i)
                m_Members[i].SendLocalizedMessage(number, append, affix, args, hue);
        }

        public void GuildTextMessage(string text)
        {
            GuildTextMessage(0x3B2, text);
        }
        public void GuildTextMessage(string format, params object[] args)
        {
            GuildTextMessage(0x3B2, String.Format(format, args));
        }
        public void GuildTextMessage(int hue, string text)
        {
            for (int i = 0; i < m_Members.Count; ++i)
                m_Members[i].SendMessage(hue, text);
        }
        public void GuildTextMessage(int hue, string format, params object[] args)
        {
            GuildTextMessage(hue, String.Format(format, args));
        }

        public void GuildChat(Mobile from, int hue, string text)
        {
            Packet p = null;
            for (int i = 0; i < m_Members.Count; i++)
            {
                Mobile m = m_Members[i];

                NetState state = m.NetState;

                if (state != null)
                {
                    if (p == null)
                        p = Packet.Acquire(new UnicodeMessage(from.Serial, from.Body, MessageType.Guild, hue, 3, from.Language, from.Name, text));

                    state.Send(p);
                }
            }

            Packet.Release(p);
        }

        public void GuildChat(Mobile from, string text)
        {
            PlayerMobile pm = from as PlayerMobile;

            GuildChat(from, (pm == null) ? 0x3B2 : pm.GuildMessageHue, text);
        }
        #endregion

        #region Voting
        public bool CanVote(Mobile m)
        {
            if (NewGuildSystem)
            {
                PlayerMobile pm = m as PlayerMobile;
                if (pm == null || !pm.GuildRank.GetFlag(RankFlags.CanVote))
                    return false;
            }

            return (m != null && !m.Deleted && m.Guild == this);
        }
        public bool CanBeVotedFor(Mobile m)
        {
            if (NewGuildSystem)
            {
                PlayerMobile pm = m as PlayerMobile;
                if (pm == null || pm.LastOnline + InactiveTime < DateTime.Now)
                    return false;
            }

            return (m != null && !m.Deleted && m.Guild == this);
        }

        public void CalculateGuildmaster()
        {
            Dictionary<Mobile, int> votes = new Dictionary<Mobile, int>();

            int votingMembers = 0;

            for (int i = 0; m_Members != null && i < m_Members.Count; ++i)
            {
                Mobile memb = m_Members[i];

                if (!CanVote(memb))
                    continue;

                Mobile m = memb.GuildFealty;

                if (!CanBeVotedFor(m))
                {
                    if (m_Leader != null && !m_Leader.Deleted && m_Leader.Guild == this)
                        m = m_Leader;
                    else
                        m = memb;
                }

                if (m == null)
                    continue;

                int v;

                if (!votes.TryGetValue(m, out v))
                    votes[m] = 1;
                else
                    votes[m] = v + 1;

                votingMembers++;
            }

            Mobile winner = null;
            int highVotes = 0;

            foreach (KeyValuePair<Mobile, int> kvp in votes)
            {
                Mobile m = (Mobile)kvp.Key;
                int val = (int)kvp.Value;

                if (winner == null || val > highVotes)
                {
                    winner = m;
                    highVotes = val;
                }
            }

            if (NewGuildSystem && (highVotes * 100) / Math.Max(votingMembers, 1) < MajorityPercentage && m_Leader != null && winner != m_Leader && !m_Leader.Deleted && m_Leader.Guild == this)
                winner = m_Leader;

            if (m_Leader != winner && winner != null)
                GuildMessage(1018015, true, winner.Name); // Guild Message: Guildmaster changed to:

            Leader = winner;
            m_LastFealty = DateTime.Now;
        }

        #endregion

        #region Getters & Setters
        [CommandProperty(AccessLevel.GameMaster)]
        public Item Guildstone
        {
            get
            {
                return m_Guildstone;
            }
            set
            {
                m_Guildstone = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Item Teleporter
        {
            get
            {
                return m_Teleporter;
            }
            set
            {
                m_Teleporter = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;

                InvalidateMemberProperties(true);

                if (m_Guildstone != null)
                    m_Guildstone.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Website
        {
            get
            {
                return m_Website;
            }
            set
            {
                m_Website = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override string Abbreviation
        {
            get
            {
                return m_Abbreviation;
            }
            set
            {
                m_Abbreviation = value;

                InvalidateMemberProperties(true);

                if (m_Guildstone != null)
                    m_Guildstone.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Charter
        {
            get
            {
                return m_Charter;
            }
            set
            {
                m_Charter = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public override GuildType Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                if (m_Type != value)
                {
                    m_Type = value;
                    m_TypeLastChange = DateTime.Now;

                    InvalidateMemberProperties();
                }
            }
        }


        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastFealty
        {
            get
            {
                return m_LastFealty;
            }
            set
            {
                m_LastFealty = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime TypeLastChange
        {
            get
            {
                return m_TypeLastChange;
            }
        }


        public List<Guild> Allies
        {
            get
            {
                return m_Allies;
            }
        }

        public List<Guild> Enemies
        {
            get
            {
                return m_Enemies;
            }
        }

        public List<Guild> AllyDeclarations
        {
            get
            {
                return m_AllyDeclarations;
            }
        }

        public List<Guild> AllyInvitations
        {
            get
            {
                return m_AllyInvitations;
            }
        }

        public List<Guild> WarDeclarations
        {
            get
            {
                return m_WarDeclarations;
            }
        }

        public List<Guild> WarInvitations
        {
            get
            {
                return m_WarInvitations;
            }
        }

        public List<Mobile> Candidates
        {
            get
            {
                return m_Candidates;
            }
        }

        public List<Mobile> Accepted
        {
            get
            {
                return m_Accepted;
            }
        }

        public List<Mobile> Members
        {
            get
            {
                return m_Members;
            }
        }

        #endregion

    }
}

namespace Server.Items
{
    public class GuildTeleporter : Item
    {
        private Item m_Stone;

        public override int LabelNumber { get { return 1041054; } } // guildstone teleporter

        [Constructable]
        public GuildTeleporter() : this(null)
        {
        }

        public GuildTeleporter(Item stone) : base(0x1869)
        {
            Weight = 1.0;
            LootType = LootType.Blessed;

            m_Stone = stone;
        }

        public GuildTeleporter(Serial serial) : base(serial)
        {
        }

        public override bool DisplayLootType { get { return false; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_Stone);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            LootType = LootType.Blessed;

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Stone = reader.ReadItem();

                        break;
                    }
            }

            if (Weight == 0.0)
                Weight = 1.0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Guild.NewGuildSystem)
                return;

            Guildstone stone = m_Stone as Guildstone;

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else if (stone == null || stone.Deleted || stone.Guild == null || stone.Guild.Teleporter != this)
            {
                from.SendLocalizedMessage(501197); // This teleporting object can not determine what guildstone to teleport
            }
            else
            {
                BaseHouse house = BaseHouse.FindHouseAt(from);

                if (house == null)
                {
                    from.SendLocalizedMessage(501138); // You can only place a guildstone in a house.
                }
                else if (!house.IsOwner(from))
                {
                    from.SendLocalizedMessage(501141); // You can only place a guildstone in a house you own!
                }
                else if (house.FindGuildstone() != null)
                {
                    from.SendLocalizedMessage(501142);//Only one guildstone may reside in a given house.
                }
                else
                {
                    m_Stone.MoveToWorld(from.Location, from.Map);
                    Delete();
                    stone.Guild.Teleporter = null;
                }
            }
        }
    }
}

namespace Server.Items
{
    public class GuildDeed : Item
    {
        public override int LabelNumber { get { return 1041055; } } // a guild deed

        [Constructable]
        public GuildDeed() : base(0x14F0)
        {
            Weight = 1.0;
        }

        public GuildDeed(Serial serial) : base(serial)
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
                Weight = 1.0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Guild.NewGuildSystem)
                return;

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else if (from.Guild != null)
            {
                from.SendLocalizedMessage(501137); // You must resign from your current guild before founding another!
            }
            else
            {
                BaseHouse house = BaseHouse.FindHouseAt(from);

                if (house == null)
                {
                    from.SendLocalizedMessage(501138); // You can only place a guildstone in a house.
                }
                else if (house.FindGuildstone() != null)
                {
                    from.SendLocalizedMessage(501142);//Only one guildstone may reside in a given house.
                }
                else if (!house.IsOwner(from))
                {
                    from.SendLocalizedMessage(501141); // You can only place a guildstone in a house you own!
                }
                else
                {
                    from.SendLocalizedMessage(1013060); // Enter new guild name (40 characters max):
                    from.Prompt = new InternalPrompt(this);
                }
            }
        }

        private class InternalPrompt : Prompt
        {
            private GuildDeed m_Deed;

            public InternalPrompt(GuildDeed deed)
            {
                m_Deed = deed;
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (m_Deed.Deleted)
                    return;

                if (!m_Deed.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                }
                else if (from.Guild != null)
                {
                    from.SendLocalizedMessage(501137); // You must resign from your current guild before founding another!
                }
                else
                {
                    BaseHouse house = BaseHouse.FindHouseAt(from);

                    if (house == null)
                    {
                        from.SendLocalizedMessage(501138); // You can only place a guildstone in a house.
                    }
                    else if (house.FindGuildstone() != null)
                    {
                        from.SendLocalizedMessage(501142);//Only one guildstone may reside in a given house.
                    }
                    else if (!house.IsOwner(from))
                    {
                        from.SendLocalizedMessage(501141); // You can only place a guildstone in a house you own!
                    }
                    else
                    {
                        m_Deed.Delete();

                        if (text.Length > 40)
                            text = text.Substring(0, 40);

                        Guild guild = new Guild(from, text, "none");

                        from.Guild = guild;
                        from.GuildTitle = "Guildmaster";

                        Guildstone stone = new Guildstone(guild);

                        stone.MoveToWorld(from.Location, from.Map);

                        guild.Guildstone = stone;
                    }
                }
            }

            public override void OnCancel(Mobile from)
            {
                from.SendLocalizedMessage(501145); // Placement of guildstone cancelled.
            }
        }
    }
}

namespace Server.Items
{
    public class Guildstone : Item, IAddon, IChopable
    {
        private Guild m_Guild;
        private string m_GuildName;
        private string m_GuildAbbrev;

        public Guild Guild
        {
            get
            {
                return m_Guild;
            }
        }

        public override int LabelNumber { get { return 1041429; } } // a guildstone

        public Guildstone(Guild g) : this(g, g.Name, g.Abbreviation)
        {
        }

        public Guildstone(Guild g, string guildName, string abbrev) : base(Guild.NewGuildSystem ? 0xED6 : 0xED4)
        {
            m_Guild = g;
            m_GuildName = guildName;
            m_GuildAbbrev = abbrev;

            Movable = false;
        }

        public Guildstone(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            if (m_Guild != null && !m_Guild.Disbanded)
            {
                m_GuildName = m_Guild.Name;
                m_GuildAbbrev = m_Guild.Abbreviation;
            }

            writer.Write((int)3); // version

            writer.Write(m_BeforeChangeover);

            writer.Write(m_GuildName);
            writer.Write(m_GuildAbbrev);

            writer.Write(m_Guild);
        }

        private bool m_BeforeChangeover;
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 3:
                    {
                        m_BeforeChangeover = reader.ReadBool();
                        goto case 2;
                    }
                case 2:
                    {
                        m_GuildName = reader.ReadString();
                        m_GuildAbbrev = reader.ReadString();

                        goto case 1;
                    }
                case 1:
                    {
                        m_Guild = reader.ReadGuild() as Guild;

                        goto case 0;
                    }
                case 0:
                    {
                        break;
                    }
            }

            if (Guild.NewGuildSystem && ItemID == 0xED4)
                ItemID = 0xED6;

            if (m_Guild != null)
            {
                m_GuildName = m_Guild.Name;
                m_GuildAbbrev = m_Guild.Abbreviation;
            }

            if (version <= 2)
                m_BeforeChangeover = true;

            if (Guild.NewGuildSystem && m_BeforeChangeover)
                Timer.DelayCall(TimeSpan.Zero, new TimerCallback(AddToHouse));

            if (!Guild.NewGuildSystem && m_Guild == null)
                this.Delete();
        }

        private void AddToHouse()
        {
            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (Guild.NewGuildSystem && m_BeforeChangeover && house != null && !house.Addons.Contains(this))
            {
                house.Addons.Add(this);
                m_BeforeChangeover = false;
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_Guild != null && !m_Guild.Disbanded)
            {
                string name;
                string abbr;

                if ((name = m_Guild.Name) == null || (name = name.Trim()).Length <= 0)
                    name = "(unnamed)";

                if ((abbr = m_Guild.Abbreviation) == null || (abbr = abbr.Trim()).Length <= 0)
                    abbr = "";

                //list.Add( 1060802, Utility.FixHtml( name ) ); // Guild name: ~1_val~
                list.Add(1060802, String.Format("{0} [{1}]", Utility.FixHtml(name), Utility.FixHtml(abbr)));
            }
            else
            {
                list.Add(1060802, String.Format("{0} [{1}]", Utility.FixHtml(m_GuildName), Utility.FixHtml(m_GuildAbbrev)));
            }
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            string name;

            if (m_Guild == null)
                name = "(unfounded)";
            else if ((name = m_Guild.Name) == null || (name = name.Trim()).Length <= 0)
                name = "(unnamed)";

            this.LabelTo(from, name);
        }

        public override void OnAfterDelete()
        {
            if (!Guild.NewGuildSystem && m_Guild != null && !m_Guild.Disbanded)
                m_Guild.Disband();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Guild.NewGuildSystem)
                return;

            if (m_Guild == null || m_Guild.Disbanded)
            {
                Delete();
            }
            else if (!from.InRange(GetWorldLocation(), 2))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
            }
            else if (m_Guild.Accepted.Contains(from))
            {
                m_Guild.Accepted.Remove(from);
                m_Guild.AddMember(from);

                GuildGump.EnsureClosed(from);
                from.SendGump(new GuildGump(from, m_Guild));
            }
            else if (from.AccessLevel < AccessLevel.GameMaster && !m_Guild.IsMember(from))
            {
                from.Send(new MessageLocalized(Serial, ItemID, MessageType.Regular, 0x3B2, 3, 501158, "", "")); // You are not a member ...
            }
            else
            {
                GuildGump.EnsureClosed(from);
                from.SendGump(new GuildGump(from, m_Guild));
            }
        }

        #region IAddon Members
        public Item Deed
        {
            get { return new GuildstoneDeed(m_Guild, m_GuildName, m_GuildAbbrev); }
        }

        public bool CouldFit(IPoint3D p, Map map)
        {
            return map.CanFit(p.X, p.Y, p.Z, this.ItemData.Height);
        }

        #endregion

        #region IChopable Members

        public void OnChop(Mobile from)
        {
            if (!Guild.NewGuildSystem)
                return;

            BaseHouse house = BaseHouse.FindHouseAt(this);

            if ((house == null && m_BeforeChangeover) || (house != null && house.IsOwner(from) && house.Addons.Contains(this)))
            {
                Effects.PlaySound(GetWorldLocation(), Map, 0x3B3);
                from.SendLocalizedMessage(500461); // You destroy the item.

                Delete();

                if (house != null && house.Addons.Contains(this))
                    house.Addons.Remove(this);

                Item deed = Deed;

                if (deed != null)
                {
                    from.AddToBackpack(deed);
                }
            }
        }

        #endregion
    }

    [Flipable(0x14F0, 0x14EF)]
    public class GuildstoneDeed : Item
    {
        public override int LabelNumber { get { return 1041233; } } // deed to a guildstone

        private Guild m_Guild;
        private string m_GuildName;
        private string m_GuildAbbrev;

        [Constructable]
        public GuildstoneDeed(Guild g, string guildName, string abbrev) : base(0x14F0)
        {
            m_Guild = g;
            m_GuildName = guildName;
            m_GuildAbbrev = abbrev;

            Weight = 1.0;
        }

        public GuildstoneDeed(Serial serial) : base(serial)
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

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_Guild != null && !m_Guild.Disbanded)
            {
                string name;
                string abbr;

                if ((name = m_Guild.Name) == null || (name = name.Trim()).Length <= 0)
                    name = "(unnamed)";

                if ((abbr = m_Guild.Abbreviation) == null || (abbr = abbr.Trim()).Length <= 0)
                    abbr = "";

                //list.Add( 1060802, Utility.FixHtml( name ) ); // Guild name: ~1_val~
                list.Add(1060802, String.Format("{0} [{1}]", Utility.FixHtml(name), Utility.FixHtml(abbr)));
            }
            else
            {
                list.Add(1060802, String.Format("{0} [{1}]", Utility.FixHtml(m_GuildName), Utility.FixHtml(m_GuildAbbrev)));
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                BaseHouse house = BaseHouse.FindHouseAt(from);

                if (house != null && house.IsOwner(from))
                {
                    from.SendLocalizedMessage(1062838); // Where would you like to place this decoration?
                    from.BeginTarget(-1, true, Targeting.TargetFlags.None, new TargetStateCallback(Placement_OnTarget), null);
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

            if (p == null || Deleted)
                return;

            Point3D loc = new Point3D(p);

            BaseHouse house = BaseHouse.FindHouseAt(loc, from.Map, 16);

            if (IsChildOf(from.Backpack))
            {
                if (house != null && house.IsOwner(from))
                {
                    Item addon = new Guildstone(m_Guild, m_GuildName, m_GuildAbbrev);

                    addon.MoveToWorld(loc, from.Map);

                    house.Addons.Add(addon);
                    Delete();
                }
                else
                {
                    from.SendLocalizedMessage(1042036); // That location is not in your house.
                }
            }
            else
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
        }

    }
}