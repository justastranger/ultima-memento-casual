using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a pirate corpse")]
    public class ElfPirateCrew : BaseCreature
    {
        [Constructable]
        public ElfPirateCrew() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomTalkHue();
            Hue = 1316;
            Race = Race.Elf;

            if (this.Female = Utility.RandomBool())
            {
                Body = 606;
                Name = NameList.RandomName("elf_female");
            }
            else
            {
                Body = 605;
                Name = NameList.RandomName("elf_male");
            }

            Title = "the drow pirate";

            Utility.AssignRandomHair(this);
            HairHue = 1150;

            AddItem(new ElvenBoots(0x6F8));
            Item armor = new LeatherChest(); armor.Hue = 0x6F8; AddItem(armor);
            AddItem(new FancyShirt(0));

            switch (Utility.Random(2))
            {
                case 0: AddItem(new LongPants(0xBB4)); break;
                case 1: AddItem(new ShortPants(0xBB4)); break;
            }

            switch (Utility.Random(2))
            {
                case 0: AddItem(new Bandana(0x846)); break;
                case 1: AddItem(new SkullCap(0x846)); break;
            }

            AddItem(new Scimitar());

            SetStr(86, 100);
            SetDex(81, 95);
            SetInt(61, 75);

            SetDamage(10, 23);

            SetSkill(SkillName.Fencing, 66.0, 97.5);
            SetSkill(SkillName.Bludgeoning, 65.0, 87.5);
            SetSkill(SkillName.MagicResist, 25.0, 47.5);
            SetSkill(SkillName.Swords, 65.0, 87.5);
            SetSkill(SkillName.Tactics, 65.0, 87.5);
            SetSkill(SkillName.FistFighting, 15.0, 37.5);

            Fame = 1000;
            Karma = -1000;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public override bool ClickTitle { get { return false; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool AlwaysAttackable { get { return true; } }
        public override int Skeletal { get { return Utility.Random(2); } }
        public override SkeletalType SkeletalType { get { return SkeletalType.Drow; } }

        public ElfPirateCrew(Serial serial) : base(serial)
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