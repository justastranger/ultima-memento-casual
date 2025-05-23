using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cat corpse")]
    public class WhiteCat : BaseCreature
    {
        [Constructable]
        public WhiteCat() : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a cat";
            Body = 0xC9;
            BaseSoundID = 0x69;
            Hue = 0x47E;

            SetStr(96, 125);
            SetDex(86, 105);
            SetInt(141, 165);

            SetHits(58, 75);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 35, 40);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Psychology, 50.1, 65.0);
            SetSkill(SkillName.Magery, 50.1, 65.0);
            SetSkill(SkillName.MagicResist, 60.1, 75.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.FistFighting, 50.1, 70.0);

            Fame = 0;
            Karma = 3500;

            VirtualArmor = 36;

            PackReg(Utility.RandomMinMax(5, 15));
            PackReg(Utility.RandomMinMax(5, 15));
            PackReg(Utility.RandomMinMax(5, 15));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.LowPotions);
        }

        public override int Meat { get { return 1; } }

        public WhiteCat(Serial serial) : base(serial)
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