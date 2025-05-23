using System;using Server;using Server.Network;using System.Text;using Server.Misc;using Server.Regions;using Server.Mobiles;using System.Collections;using System.Collections.Generic;using Server.Accounting;namespace Server.Items{
    public class HenchmanItem : Item
    {
        public int HenchSerial;
        public int HenchTimer;
        public int HenchWeaponID;
        public int HenchShieldID;
        public int HenchHelmID;
        public int HenchArmorType;
        public int HenchWeaponType;
        public int HenchCloakColor;
        public int HenchCloak;
        public int HenchRobe;
        public int HenchHatColor;
        public int HenchGloves;
        public int HenchSpeech;
        public int HenchDead;
        public int HenchBody;
        public int HenchHue;
        public int HenchHair;
        public int HenchHairHue;
        public int HenchGearColor;
        public string HenchName;
        public string HenchTitle;
        public int HenchBandages;

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_Serial { get { return HenchSerial; } set { HenchSerial = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_Timer { get { return HenchTimer; } set { HenchTimer = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_WeaponID { get { return HenchWeaponID; } set { HenchWeaponID = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_ShieldID { get { return HenchShieldID; } set { HenchShieldID = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_HelmID { get { return HenchHelmID; } set { HenchHelmID = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_ArmorType { get { return HenchArmorType; } set { HenchArmorType = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_WeaponType { get { return HenchWeaponType; } set { HenchWeaponType = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_CloakColor { get { return HenchCloakColor; } set { HenchCloakColor = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_Cloak { get { return HenchCloak; } set { HenchCloak = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_Robe { get { return HenchRobe; } set { HenchRobe = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_HatColor { get { return HenchHatColor; } set { HenchHatColor = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_Gloves { get { return HenchGloves; } set { HenchGloves = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_Speech { get { return HenchSpeech; } set { HenchSpeech = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_Dead { get { return HenchDead; } set { HenchDead = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_Body { get { return HenchBody; } set { HenchBody = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_Hue { get { return HenchHue; } set { HenchHue = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_Hair { get { return HenchHair; } set { HenchHair = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_HairHue { get { return HenchHairHue; } set { HenchHairHue = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_GearColor { get { return HenchGearColor; } set { HenchGearColor = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public string Hench_Name { get { return HenchName; } set { HenchName = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public string Hench_Title { get { return HenchTitle; } set { HenchTitle = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public int Hench_Bandages { get { return HenchBandages; } set { HenchBandages = value; InvalidateProperties(); } }

        [Constructable]
        public HenchmanItem() : base(0xE2D)
        {
            if (HenchBody > 0) { } else { HenchTimer = 300; }
            if (HenchBody > 0) { } else { HenchBody = Utility.RandomList(400, 400, 401); }
            if (HenchSpeech > 0) { } else { HenchSpeech = Utility.RandomDyedHue(); }
            if (HenchTitle == null) { HenchTitle = TavernPatrons.GetTitle(); }
            if (HenchHue > 0) { } else { HenchHue = Utility.RandomSkinColor(); }
            if (HenchHairHue > 0) { } else { HenchHairHue = Utility.RandomHairHue(); }
            if (HenchHair > 0) { } else { HenchHair = Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x2047, 0x2049, 0x204A); }

            if (HenchName == null)
            {
                if (HenchBody == 400) { HenchName = NameList.RandomName("male"); }
                else { HenchName = NameList.RandomName("female"); }
            }

            Name = "henchman";
            Weight = 1.0;
        }

        public HenchmanItem(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            ArrayList pets = new ArrayList();
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m is HenchmanMonster || m is HenchmanArcher || m is HenchmanFighter || m is HenchmanWizard)
                {
                    BaseCreature bc = (BaseCreature)m;
                    if (bc.Controlled && bc.ControlMaster == from)
                        pets.Add(bc);
                }
            }

            int nFollowers = from.FollowersMax - from.Followers;

            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001);
            }
            else if (HenchDead > 0)
            {
                from.SendMessage("They are dead and you must hire a healer to resurrect them.");
            }
            else if (pets.Count > 1)
            {
                from.SendMessage("You already have enough henchman in your group.");
            }
            else if (nFollowers < 1)
            {
                from.SendMessage("You already have enough henchman in your group.");
            }
            else if (HenchmanFunctions.IsInRestRegion(from) == true)
            {
                Map map = from.Map;

                int nMounted = 0;
                if (from.Mounted == true) { nMounted = 1; }

                int nCap = (int)(from.Skills.Cap / 100);
                if (nCap > 100) { nCap = 100; }
                int nTotal = from.SkillsTotal;
                if (nTotal > (nCap * 100)) { nTotal = nCap * 100; }

                int nSkills = nTotal / nCap;
                int nStats = from.RawStr + from.RawDex + from.RawInt;

                BaseCreature friend = new HenchmanWizard(HenchBody, nMounted, nSkills, nStats);
                friend.Delete();

                if (this is HenchmanMonsterItem) { friend = new HenchmanMonster(HenchBody, nMounted, nSkills, nStats, HenchWeaponID, HenchShieldID); }
                else if (this is HenchmanWizardItem) { friend = new HenchmanWizard(HenchBody, nMounted, nSkills, nStats); }
                else if (this is HenchmanFighterItem) { friend = new HenchmanFighter(HenchBody, nMounted, nSkills, nStats); }
                else if (this is HenchmanArcherItem) { friend = new HenchmanArcher(HenchBody, nMounted, nSkills, nStats); }

                bool validLocation = false;
                Point3D loc = from.Location;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = from.X + Utility.Random(3) - 1;
                    int y = from.Y + Utility.Random(3) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                        loc = new Point3D(x, y, Z);
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }

                friend.ControlMaster = from;
                friend.Controlled = true;
                friend.ControlOrder = OrderType.Guard;
                friend.ControlSlots = 1;
                friend.Loyalty = 100;
                friend.Title = HenchTitle;
                friend.SpeechHue = HenchSpeech;
                friend.Name = HenchName;
                friend.Fame = HenchTimer;
                friend.Hunger = HenchBandages;

                if (!(this is HenchmanMonsterItem))
                {
                    friend.Hue = HenchHue;
                    friend.HairItemID = HenchHair;
                    friend.HairHue = HenchHairHue;
                    HenchmanFunctions.DressUp(this, friend, from);
                    HenchmanFunctions.NormalizeArmor(friend);
                }

                int nTime = (int)(friend.Fame / 5);
                from.SendMessage("" + friend.Name + " will probably adventure with you for another " + nTime.ToString() + " minutes.");

                friend.NameColor();
                friend.MoveToWorld(loc, map);
                this.LootType = LootType.Blessed;
                this.Visible = false;
                this.HenchSerial = friend.Serial;

                if (!(this is HenchmanMonsterItem))
                {
                    switch (Utility.Random(8))
                    {
                        case 0: friend.Say("I am ready for adventure."); break;
                        case 1: friend.Say("Where are we off to?"); break;
                        case 2: friend.Say("" + from.Name + ", I am here to serve."); break;
                        case 3: friend.Say("May monsters fear our hardy band."); break;
                        case 4: friend.Say("What dungeon do we dare explore?"); break;
                        case 5: friend.Say("I have been waiting here too long."); break;
                        case 6: friend.Say("Hello, " + from.Name + "."); break;
                        case 7: friend.Say("Fame and fortune await!"); break;
                    }
                }
            }
            else
            {
                from.SendMessage("You can only call your henchman from a home, inn, or tavern.");
            }
        }        public override void AddNameProperties(ObjectPropertyList list)
        {            base.AddNameProperties(list);
            string sInfo = HenchName + " " + HenchTitle;
            list.Add(1070722, sInfo);
            if (HenchDead > 0)
            {
                string sLive = HenchDead.ToString();
                list.Add(1049644, "Resurrect For " + sLive + " Gold"); // PARENTHESIS
            }        }

        public static void ResurrectHenchman(Mobile from)
        {
            if (from is PlayerMobile && from.Alive)
            {
                bool giveMessage = false;
                foreach (Item i in from.Backpack.Items)
                {
                    if (i is HenchmanFighterItem)
                    {
                        HenchmanFighterItem friend = (HenchmanFighterItem)i;
                        if (friend.HenchDead > 0)
                        {
                            friend.Name = "fighter henchman";
                            friend.HenchDead = 0;
                            friend.InvalidateProperties();
                            giveMessage = true;
                        }
                    }
                    else if (i is HenchmanWizardItem)
                    {
                        HenchmanWizardItem friend = (HenchmanWizardItem)i;
                        if (friend.HenchDead > 0)
                        {
                            friend.Name = "wizard henchman";
                            friend.HenchDead = 0;
                            friend.InvalidateProperties();
                            giveMessage = true;
                        }
                    }
                    else if (i is HenchmanArcherItem)
                    {
                        HenchmanArcherItem friend = (HenchmanArcherItem)i;
                        if (friend.HenchDead > 0)
                        {
                            friend.Name = "archer henchman";
                            friend.HenchDead = 0;
                            friend.InvalidateProperties();
                            giveMessage = true;
                        }
                    }
                    else if (i is HenchmanMonsterItem)
                    {
                        HenchmanMonsterItem friend = (HenchmanMonsterItem)i;
                        if (friend.HenchDead > 0)
                        {
                            friend.Name = "creature henchman";
                            friend.HenchDead = 0;
                            friend.InvalidateProperties();
                            giveMessage = true;
                        }
                    }
                }

                if (giveMessage)
                {
                    from.SendMessage("Your henchmen have been resurrected.");
                    from.PlaySound(0x214);
                    from.FixedEffect(0x376A, 10, 16);
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(HenchSerial);
            writer.Write(HenchTimer);
            writer.Write(HenchWeaponID);
            writer.Write(HenchShieldID);
            writer.Write(HenchHelmID);
            writer.Write(HenchArmorType);
            writer.Write(HenchWeaponType);
            writer.Write(HenchCloakColor);
            writer.Write(HenchCloak);            writer.Write(HenchSpeech);            writer.Write(HenchDead);            writer.Write(HenchBody);            writer.Write(HenchHue);            writer.Write(HenchHair);            writer.Write(HenchHairHue);            writer.Write(HenchGearColor);            writer.Write(HenchName);            writer.Write(HenchTitle);            writer.Write(HenchBandages);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            HenchSerial = reader.ReadInt();
            HenchTimer = reader.ReadInt();
            HenchWeaponID = reader.ReadInt();
            HenchShieldID = reader.ReadInt();
            HenchHelmID = reader.ReadInt();
            HenchArmorType = reader.ReadInt();
            HenchWeaponType = reader.ReadInt();
            HenchCloakColor = reader.ReadInt();
            HenchCloak = reader.ReadInt();
            HenchSpeech = reader.ReadInt();
            HenchDead = reader.ReadInt();
            HenchBody = reader.ReadInt();
            HenchHue = reader.ReadInt();
            HenchHair = reader.ReadInt();
            HenchHairHue = reader.ReadInt();
            HenchGearColor = reader.ReadInt();
            HenchName = reader.ReadString();
            HenchTitle = reader.ReadString();
            HenchBandages = reader.ReadInt();
            LootType = LootType.Regular;
            Visible = true;
        }
    }}