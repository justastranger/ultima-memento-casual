using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an ophidian corpse")]
    public class OphidianMatriarch : BaseCreature
    {
        [Constructable]
        public OphidianMatriarch() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ophidian matriarch";
            Body = 87;
            BaseSoundID = 644;

            SetStr(416, 505);
            SetDex(96, 115);
            SetInt(366, 455);

            SetHits(250, 303);

            SetDamage(11, 13);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 35, 45);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.Psychology, 90.1, 100.0);
            SetSkill(SkillName.Magery, 90.1, 100.0);
            SetSkill(SkillName.Meditation, 5.4, 25.0);
            SetSkill(SkillName.MagicResist, 90.1, 100.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.FistFighting, 60.1, 80.0);

            Fame = 16000;
            Karma = -16000;

            VirtualArmor = 50;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average, 2);
            AddLoot(LootPack.MedScrolls, 2);
        }

        public override Poison PoisonImmune { get { return Poison.Greater; } }
        public override int TreasureMapLevel { get { return 4; } }
        public override int Meat { get { return 1; } }
        public override int Hides { get { return 5; } }
        public override HideType HideType { get { return HideType.Barbed; } }
        public override int Skin { get { return Utility.Random(4); } }
        public override SkinType SkinType { get { return SkinType.Snake; } }

        public OphidianMatriarch(Serial serial) : base(serial)
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