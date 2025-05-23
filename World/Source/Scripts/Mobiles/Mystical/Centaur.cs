using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a centaur corpse")]
    public class Centaur : BaseCreature
    {
        [Constructable]
        public Centaur() : base(AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4)
        {
            Name = NameList.RandomName("centaur");
            Title = "the centaur";
            Body = 101;
            BaseSoundID = 679;

            SetStr(202, 300);
            SetDex(104, 260);
            SetInt(91, 100);

            SetHits(130, 172);

            SetDamage(13, 24);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 35, 45);
            SetResistance(ResistanceType.Cold, 25, 35);
            SetResistance(ResistanceType.Poison, 45, 55);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.Anatomy, 95.1, 115.0);
            SetSkill(SkillName.Marksmanship, 95.1, 100.0);
            SetSkill(SkillName.MagicResist, 50.3, 80.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.FistFighting, 95.1, 100.0);

            Fame = 6500;
            Karma = 6500;

            VirtualArmor = 50;
            AddItem(new Bow());
            PackItem(new Arrow(Utility.RandomMinMax(80, 90))); // OSI it is different: in a sub backpack, this is probably just a limitation of their engine
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems);
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 8; } }
        public override int Skeletal { get { return Utility.Random(2); } }
        public override SkeletalType SkeletalType { get { return SkeletalType.Mystical; } }

        public Centaur(Serial serial) : base(serial)
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

            if (BaseSoundID == 678)
                BaseSoundID = 679;
        }
    }
}