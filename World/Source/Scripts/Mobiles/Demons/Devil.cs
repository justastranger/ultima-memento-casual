using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a devil corpse")]
    public class Devil : BaseCreature
    {
        public override double DispelDifficulty { get { return 150.0; } }
        public override double DispelFocus { get { return 25.0; } }

        [Constructable]
        public Devil() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = NameList.RandomName("devil");
            Body = Utility.RandomList(765, 804);
            Title = NameList.RandomName("devil_title");
            BaseSoundID = 357;

            if (Utility.RandomMinMax(1, 4) == 1) // FEMALE
            {
                Name = NameList.RandomName("goddess");
                Title = NameList.RandomName("deviless_title");
                Body = Utility.RandomList(174, 689);
                BaseSoundID = 0x4B0;
            }

            SetStr(986, 1185);
            SetDex(177, 255);
            SetInt(151, 250);

            SetHits(592, 711);

            SetDamage(22, 29);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.Psychology, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.FistFighting, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, 2);
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override int Meat { get { return 1; } }
        public override int Hides { get { return 18; } }
        public override HideType HideType { get { return HideType.Hellish; } }
        public override int Skin { get { return Utility.Random(6); } }
        public override SkinType SkinType { get { return SkinType.Demon; } }
        public override int Skeletal { get { return Utility.Random(8); } }
        public override SkeletalType SkeletalType { get { return SkeletalType.Devil; } }

        public Devil(Serial serial) : base(serial)
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