using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using System.Linq;
using System.Collections.Generic;

namespace Server.Items
{
    [Flipable(0x577B, 0x577C)]
    public class GuildBoard : Item
    {
        [Constructable]
        public GuildBoard() : base(0x577B)
        {
            Weight = 1.0;
            Name = "Local Guilds";
            Hue = 0xB79;
        }

        public override void OnDoubleClick(Mobile e)
        {
            if (e.InRange(this.GetWorldLocation(), 4))
            {
                e.CloseGump(typeof(GuildBoardGump));
                e.SendGump(new GuildBoardGump(e));
            }
            else
            {
                e.SendLocalizedMessage(502138); // That is too far away for you to use
            }
        }

        public class GuildBoardGump : Gump
        {
            private class InternalSort : IComparer<BaseGuildmaster>
            {
                private readonly static List<NpcGuild> m_SortedGuilds = new List<NpcGuild>
                {
                    NpcGuild.AlchemistsGuild,
                    NpcGuild.ArchersGuild,
                    NpcGuild.AssassinsGuild,
                    NpcGuild.BardsGuild,
                    NpcGuild.NecromancersGuild, // Black magic
					NpcGuild.BlacksmithsGuild,
                    NpcGuild.CarpentersGuild,
                    NpcGuild.CartographersGuild,
                    NpcGuild.CulinariansGuild,
                    NpcGuild.DruidsGuild,
                    NpcGuild.ElementalGuild,
                    NpcGuild.HealersGuild,
                    NpcGuild.LibrariansGuild,
                    NpcGuild.FishermensGuild, // Mariner
					NpcGuild.MerchantsGuild,
                    NpcGuild.MinersGuild,
                    NpcGuild.RangersGuild,
                    NpcGuild.TailorsGuild,
                    NpcGuild.ThievesGuild,
                    NpcGuild.TinkersGuild,
                    NpcGuild.WarriorsGuild,
                    NpcGuild.MagesGuild, // Wizard
				};
                private readonly static List<Land> m_SortedLands = new List<Land>
                {
                    Land.Sosaria, // No prefix
					Land.Kuldar, // No prefix
					Land.Lodoria, // Elf
					Land.Savaged, // Ork

					// Unsorted
					Land.None,
                    Land.Ambrosia,
                    Land.Atlantis,
                    Land.IslesDread,
                    Land.Luna,
                    Land.Serpent,
                    Land.SkaraBrae,
                    Land.UmberVeil,
                    Land.Underworld
                };

                public InternalSort()
                {
                }

                public int Compare(BaseGuildmaster a, BaseGuildmaster b)
                {
                    if (a == null && b == null) return 0;
                    if (a.NpcGuild != b.NpcGuild) return m_SortedGuilds.IndexOf(a.NpcGuild) - m_SortedGuilds.IndexOf(b.NpcGuild);
                    if (a.Land != b.Land) return m_SortedLands.IndexOf(a.Land) - m_SortedLands.IndexOf(b.Land);
                    if (a.Title != b.Title) return Insensitive.Compare(a.Title, b.Title);

                    return Insensitive.Compare(Server.Misc.Worlds.GetRegionName(a.Map, a.Location), Server.Misc.Worlds.GetRegionName(b.Map, b.Location));
                }
            }

            public GuildBoardGump(Mobile from) : base(100, 100)
            {
                from.SendSound(0x59);

                var sortedGuildmasters = World.Mobiles.Values
                    .Where(x => x is BaseGuildmaster)
                    .Cast<BaseGuildmaster>()
                    .ToList();
                sortedGuildmasters.Sort(new InternalSort());

                string guildMasters = "<br><br>";
                foreach (Mobile target in sortedGuildmasters)
                    guildMasters = guildMasters + target.Name + "<br>" + target.Title + "<br>" + Server.Misc.Worlds.GetRegionName(target.Map, target.Location) + "<br><br>";

                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;

                AddPage(0);
                AddImage(0, 0, 9541, Server.Misc.PlayerSettings.GetGumpHue(from));

                PlayerMobile pm = (PlayerMobile)from;
                if (pm.NpcGuild != NpcGuild.None)
                {
                    AddHtml(55, 402, 285, 20, @"<BODY><BASEFONT Color=#e97f76>Resign From My Local Guild</BASEFONT></BODY>", (bool)false, (bool)false);
                    AddButton(16, 401, 4005, 4005, 10, GumpButtonType.Reply, 0);
                }

                string warn = "Be warned, each guild you join will have an increased fee to join. This is based on the number of guilds you were previously a member of. So when you join a guild for " + MyServerSettings.JoiningFee(from).ToString() + " gold, the next guild you join will require " + (MyServerSettings.JoiningFee(from) * 2).ToString() + " gold. The guild joined after that will be " + (MyServerSettings.JoiningFee(from) * 3).ToString() + " gold. ";
                if (!MySettings.S_GuildIncrease)
                    warn = "";

                string benefit = "One of the benefits of joining a local guild is the receiving of more gold for goods sold to other guild members. You will also receive";
                if (!MySettings.S_VendorsBuyStuff)
                    benefit = "";

                AddHtml(11, 12, 562, 20, @"<BODY><BASEFONT Color=#b6d593>LOCAL GUILDS</BASEFONT></BODY>", (bool)false, (bool)false);
                AddHtml(12, 44, 623, 349, @"<BODY><BASEFONT Color=#b6d593>There are many groups in the land that have established guild houses and are often looking for members. These guilds are separate from the various adventurer guilds that may be established on their own, as they focus on a group of people with a certain skillset and trade. Below is a listing of guild houses looking for members.<br><br>- Alchemists Guild<br>- Archers Guild<br>- Assassins Guild<br>- Bard Guild<br>- Black Magic Guild<br>- Blacksmith Guild<br>- Carpenters Guild<br>- Cartographers Guild<br>- Culinary Guild<br>- Druids Guild<br>- Elemental Guild<br>- Healer Guild<br>- Librarians Guild<br>- Mage Guild<br>- Mariners Guild<br>- Merchant Guild<br>- Miner Guild<br>- Ranger Guild<br>- Tailor Guild<br>- Thief Guild<br>- Tinker Guild<br>- Warrior Guild<br><br>The requirement for entry to any of these guilds (in addition to not being a member of another local guild) is " + MyServerSettings.JoiningFee(from).ToString() + " gold paid to the guildmaster. To join a guild, find the appropriate guildmaster and single click them to select 'Join'. They will then ask you for an amount of gold if you meet the qualifications. Just drop the exact amount of gold on them to join. You may resign from a guild by going back to your guildmaster, single clicking them, and selecting 'Resign' (or you can use this board to resign). Then you could join another guild. " + warn + "" + benefit + " a guild membership ring that will help you with skills that pertain to the guild, which would be yours and yours alone. If you lose your ring for any reason, give a guildmaster 400 gold to replace it. The skills aided by the ring are also the skills that you will gain quicker, being a member of the guild. You will also be able to purchase items from guildmasters, as they sell extra items to members of the guild.<br><br>In order to steal from other players, you must be a member of the Thieves Guild." + guildMasters + "</BASEFONT></BODY>", (bool)false, (bool)true);
                AddButton(609, 8, 4017, 4017, 0, GumpButtonType.Reply, 0);
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                Mobile from = state.Mobile;
                PlayerMobile pm = (PlayerMobile)from;
                from.SendSound(0x59);

                if (info.ButtonID > 0)
                    BaseGuildmaster.ResignGuild(from, null);
            }
        }

        public GuildBoard(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}