using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a pirate corpse")]

    public class PirateCaptain : BaseCreature
    {
        [Constructable]
        public PirateCaptain() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomTalkHue();
            Hue = Utility.RandomSkinColor();

            if (this.Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                Utility.AssignRandomHair(this);
                HairHue = Utility.RandomHairHue();
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                Utility.AssignRandomHair(this);
                int HairColor = Utility.RandomHairHue();
                FacialHairItemID = Utility.RandomList(0, 8254, 8255, 8256, 8257, 8267, 8268, 8269);
                HairHue = HairColor;
                FacialHairHue = HairColor;
            }

            Title = "the pirate captain";

            AddItem(new Scimitar());

            PirateChest MyChest = new PirateChest(8, null);
            MyChest.ContainerOwner = "Treasure Chest of " + Name;
            PackItem(MyChest);

            AddItem(new ElvenBoots(0x83A));
            Item armor = new LeatherChest(); armor.Hue = 0x83A; AddItem(armor);
            AddItem(new FancyShirt(0));
            AddItem(new PirateHat(0));

            switch (Utility.Random(2))
            {
                case 0: AddItem(new LongPants(0xBB4)); break;
                case 1: AddItem(new ShortPants(0xBB4)); break;
            }

            SetStr(386, 400);
            SetDex(151, 165);
            SetInt(161, 175);

            SetHits(300, 400);

            SetDamage(8, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Fire, 25, 30);
            SetResistance(ResistanceType.Cold, 25, 30);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Searching, 80.0);
            SetSkill(SkillName.Anatomy, 125.0);
            SetSkill(SkillName.Poisoning, 60.0, 82.5);
            SetSkill(SkillName.MagicResist, 83.5, 92.5);
            SetSkill(SkillName.Swords, 125.0);
            SetSkill(SkillName.Tactics, 125.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 30;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Rich);
        }

        public override bool ClickTitle { get { return false; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool AlwaysAttackable { get { return true; } }
        public override int TreasureMapLevel { get { return Utility.RandomMinMax(1, 6); } }
        public override int Skeletal { get { return Utility.Random(3); } }
        public override SkeletalType SkeletalType { get { return SkeletalType.Brittle; } }

        public PirateCaptain(Serial serial) : base(serial)
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
    }
}