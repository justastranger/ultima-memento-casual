using System;using System.Collections;using Server;using Server.Items;using Server.Network;using Server.Prompts;using System.Net;using Server.Accounting;using Server.Mobiles;using Server.Misc;using Server.Commands;using Server.Regions;using Server.Spells;using Server.Gumps;using Server.Targeting;namespace Server.Items{
    [Flipable(0x577B, 0x577C)]
    public class StealingBoard : Item
    {
        [Constructable]
        public StealingBoard() : base(0x577B)
        {
            Weight = 1.0;
            Name = "Stealing the Past";
            Hue = 0xAEA;
        }

        public override void OnDoubleClick(Mobile e)
        {
            PlayerMobile pm = (PlayerMobile)e;

            if (pm.NpcGuild != NpcGuild.ThievesGuild)
            {
                e.SendMessage("This board seems to be written in some sort of thieves' cant.");
            }
            else if (e.InRange(this.GetWorldLocation(), 4))
            {
                e.CloseGump(typeof(BoardGump));

                Region reg = Region.Find(e.Location, e.Map);

                string sText = "There are those with great wealth that seek items that have been rumored to lie within the dungeons of the land. These nobles seek a crafty thief to sneak into these places and find these items. Find them, steal them, and sell them to the Thief Guildmaster to collect your fee.<br><br>";

                sText = sText + "- Rock<br>   "; reg = Region.Find((new Point3D(5603, 1231, 0)), Map.Sosaria); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Sosaria, (new Point3D(5603, 1231, 0)), 5603, 1231)) + "<br><br>";
                sText = sText + "- Skull Candle<br>   "; reg = Region.Find((new Point3D(3831, 3300, 46)), Map.Sosaria); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Sosaria, (new Point3D(3831, 3300, 46)), 3831, 3300)) + "<br><br>";
                sText = sText + "- Bottle<br>   "; reg = Region.Find((new Point3D(2196, 842, 6)), Map.SerpentIsland); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.SerpentIsland, (new Point3D(2196, 842, 6)), 2196, 842)) + "<br><br>";
                sText = sText + "- Damaged Books<br>   "; reg = Region.Find((new Point3D(231, 3496, 20)), Map.Sosaria); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Sosaria, (new Point3D(231, 3496, 20)), 231, 3496)) + "<br><br>";
                sText = sText + "- Stretched Hide<br>   "; reg = Region.Find((new Point3D(5698, 525, 0)), Map.Lodor); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Lodor, (new Point3D(5698, 525, 0)), 5698, 525)) + "<br><br>";
                sText = sText + "- Brazier<br>   "; reg = Region.Find((new Point3D(2457, 491, 0)), Map.SerpentIsland); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.SerpentIsland, (new Point3D(2457, 491, 0)), 2457, 491)) + "<br><br>";
                sText = sText + "- Lamp Post<br>   "; reg = Region.Find((new Point3D(5661, 3281, 0)), Map.Sosaria); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Sosaria, (new Point3D(5661, 3281, 0)), 5661, 3281)) + "<br><br>";
                sText = sText + "- Books, North<br>   "; reg = Region.Find((new Point3D(4021, 3423, 26)), Map.Sosaria); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Sosaria, (new Point3D(4021, 3423, 26)), 4021, 3423)) + "<br><br>";
                sText = sText + "- Books, West<br>   "; reg = Region.Find((new Point3D(2051, 60, 0)), Map.SerpentIsland); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.SerpentIsland, (new Point3D(2051, 60, 0)), 2051, 60)) + "<br><br>";
                sText = sText + "- Books, Face Down<br>   "; reg = Region.Find((new Point3D(5937, 1431, 6)), Map.Sosaria); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Sosaria, (new Point3D(5937, 1431, 6)), 5937, 1431)) + "<br><br>";
                sText = sText + "- Studded Leggings<br>   "; reg = Region.Find((new Point3D(5234, 230, 5)), Map.Sosaria); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Sosaria, (new Point3D(5234, 230, 5)), 5234, 230)) + "<br><br>";
                sText = sText + "- Egg Case<br>   "; reg = Region.Find((new Point3D(5479, 900, 0)), Map.Sosaria); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Sosaria, (new Point3D(5479, 900, 0)), 5479, 900)) + "<br><br>";
                sText = sText + "- Skinned Goat<br>   "; reg = Region.Find((new Point3D(5840, 361, 0)), Map.Lodor); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Lodor, (new Point3D(5840, 361, 0)), 5840, 361)) + "<br><br>";
                sText = sText + "- Gruesome Standard<br>   "; reg = Region.Find((new Point3D(4246, 3771, 0)), Map.Sosaria); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Sosaria, (new Point3D(4246, 3771, 0)), 4246, 3771)) + "<br><br>";
                sText = sText + "- Bloody Water<br>   "; reg = Region.Find((new Point3D(5374, 763, 0)), Map.Sosaria); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Sosaria, (new Point3D(5374, 763, 0)), 5374, 763)) + "<br><br>";
                sText = sText + "- Tarot Cards<br>   "; reg = Region.Find((new Point3D(5438, 187, 6)), Map.Lodor); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Lodor, (new Point3D(5438, 187, 6)), 5438, 187)) + "<br><br>";
                sText = sText + "- Ancient Backpack<br>   "; reg = Region.Find((new Point3D(5584, 412, 10)), Map.Sosaria); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Sosaria, (new Point3D(5584, 412, 10)), 5584, 412)) + "<br><br>";
                sText = sText + "- Studded Tunic<br>   "; reg = Region.Find((new Point3D(6118, 208, 27)), Map.Lodor); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Lodor, (new Point3D(6118, 208, 27)), 6118, 208)) + "<br><br>";
                sText = sText + "- Cocoon<br>   "; reg = Region.Find((new Point3D(5142, 1669, 0)), Map.Lodor); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Lodor, (new Point3D(5142, 1669, 0)), 5142, 1669)) + "<br><br>";
                sText = sText + "- Skinned Deer<br>   "; reg = Region.Find((new Point3D(4337, 3452, 25)), Map.Sosaria); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Sosaria, (new Point3D(4337, 3452, 25)), 4337, 3452)) + "<br><br>";
                sText = sText + "- Saddle<br>   "; reg = Region.Find((new Point3D(5608, 1839, 0)), Map.Lodor); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Lodor, (new Point3D(5608, 1839, 0)), 5608, 1839)) + "<br><br>";
                sText = sText + "- Leather Tunic<br>   "; reg = Region.Find((new Point3D(5627, 2193, 5)), Map.Sosaria); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.Sosaria, (new Point3D(5627, 2193, 5)), 5627, 2193)) + "<br><br>";
                sText = sText + "- Ruined Painting<br>   "; reg = Region.Find((new Point3D(2207, 425, 0)), Map.SerpentIsland); sText = sText + reg.Name + "<br>   ";
                sText = sText + Server.Lands.LandName(Server.Lands.GetLand(Map.SerpentIsland, (new Point3D(2207, 425, 0)), 2207, 425)) + "<br><br>";

                sText = sText + "These rare items might not always be there. A better thief may have beaten you to it, and you may be forced to wait for it to reappear again. So time is of the essence if you want to profit from the eagerness of royalty.";

                e.SendGump(new BoardGump(e, "STEALING THE PAST", "" + sText + "", "#deb7e2", true));
            }
            else
            {
                e.SendLocalizedMessage(502138); // That is too far away for you to use
            }
        }

        public StealingBoard(Serial serial) : base(serial)
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
    }}