using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a gargoyle corpse")]
    public class GargoyleMarble : BaseCreature
    {
        [Constructable]
        public GargoyleMarble() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a marble gargoyle";
            Body = 4;
            BaseSoundID = 372;
            Resource = CraftResource.MarbleBlock;
            Hue = CraftResources.GetClr(Resource);

            SetStr(146, 175);
            SetDex(76, 95);
            SetInt(81, 105);

            SetHits(88, 105);

            SetDamage(7, 14);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 35);
            SetResistance(ResistanceType.Fire, 25, 35);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 15, 25);

            SetSkill(SkillName.Psychology, 70.1, 85.0);
            SetSkill(SkillName.Magery, 70.1, 85.0);
            SetSkill(SkillName.MagicResist, 70.1, 85.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.FistFighting, 40.1, 80.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 32;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 4));
        }

        public override int TreasureMapLevel { get { return 1; } }
        public override int Meat { get { return 1; } }
        public override int Hides { get { return 3; } }
        public override HideType HideType { get { return HideType.Hellish; } }
        public override int Rocks { get { return Utility.RandomMinMax(5, 10); } }
        public override RockType RockType { get { return ResourceRocks(); } }
        public override int Skeletal { get { return Utility.Random(2); } }
        public override SkeletalType SkeletalType { get { return SkeletalType.Gargoyle; } }

        public override int GetAttackSound() { return 0x5F8; }  // A
        public override int GetDeathSound() { return 0x5F9; }   // D
        public override int GetHurtSound() { return 0x5FA; }        // H

        public GargoyleMarble(Serial serial) : base(serial)
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