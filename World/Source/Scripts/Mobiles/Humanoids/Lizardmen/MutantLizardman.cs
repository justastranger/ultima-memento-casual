using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("a lizardman corpse")]
    public class MutantLizardman : BaseCreature
    {
        [Constructable]
        public MutantLizardman() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = NameList.RandomName("lizardman");
            Title = "the mutant lizardman";
            Body = 111;
            BaseSoundID = 417;

            SetStr(136, 165);
            SetDex(56, 75);
            SetInt(31, 55);

            SetHits(82, 99);

            SetDamage(7, 17);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 35, 40);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.MagicResist, 40.1, 55.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.FistFighting, 50.1, 60.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 38;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 1; } }
        public override int Hides { get { return 12; } }
        public override HideType HideType { get { return HideType.Horned; } }
        public override int Scales { get { return 1; } }
        public override ScaleType ScaleType { get { return ScaleType.SciFi; } }
        public override Poison PoisonImmune { get { return Poison.Regular; } }
        public override Poison HitPoison { get { return Poison.Lesser; } }
        public override int Skeletal { get { return Utility.Random(3); } }
        public override SkeletalType SkeletalType { get { return SkeletalType.Reptile; } }

        public MutantLizardman(Serial serial) : base(serial)
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