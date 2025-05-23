using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ghostly corpse")]
    public class SpectralGargoyle : BaseCreature
    {
        [Constructable]
        public SpectralGargoyle() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a spectral gargoyle";
            Body = 4;
            Hue = 0x4001;
            BaseSoundID = 0x482;

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

            PackReg(17, 24);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls);
        }

        public override bool ShowFameTitle { get { return false; } }
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override int Cloths { get { return 6; } }
        public override ClothType ClothType { get { return ClothType.Haunted; } }

        public SpectralGargoyle(Serial serial) : base(serial)
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