using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a skeletal corpse")]
    public class SkeletonArcher : BaseCreature
    {
        [Constructable]
        public SkeletonArcher() : base(AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a skeletal archer";
            Body = 699;
            BaseSoundID = 0x48D;

            SetStr(56, 80);
            SetDex(101, 130);
            SetInt(16, 40);

            SetHits(34, 48);

            SetDamage(3, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 25, 40);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 45.1, 60.0);
            SetSkill(SkillName.Tactics, 45.1, 60.0);
            SetSkill(SkillName.FistFighting, 45.1, 55.0);

            Fame = 450;
            Karma = -450;

            VirtualArmor = 16;

            switch (Utility.Random(6))
            {
                case 0: PackItem(new BoneArms()); break;
                case 1: PackItem(new BoneChest()); break;
                case 2: PackItem(new BoneGloves()); break;
                case 3: PackItem(new BoneLegs()); break;
                case 4: PackItem(new BoneHelm()); break;
                case 5: PackItem(new BoneSkirt()); break;
            }

            AddItem(new Bow());
            PackItem(new Arrow(Utility.RandomMinMax(5, 15)));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
        }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lesser; } }
        public override int Skeletal { get { return Utility.Random(3); } }
        public override SkeletalType SkeletalType { get { return SkeletalType.Brittle; } }

        public SkeletonArcher(Serial serial) : base(serial)
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
