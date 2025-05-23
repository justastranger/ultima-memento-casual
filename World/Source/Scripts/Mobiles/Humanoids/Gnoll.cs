using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Regions;
using Server.Mobiles;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("a gnoll corpse")]
    public class Gnoll : BaseCreature
    {
        [Constructable]
        public Gnoll() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a gnoll";
            Body = 510;
            BaseSoundID = 0x4F5;

            SetStr(100, 140);
            SetDex(90, 110);
            SetInt(72, 100);

            SetHits(80, 120);
            SetMana(0);

            SetDamage(5, 12);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 35);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 57.6, 75.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.FistFighting, 60.1, 80.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 16;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.Meager);
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 7; } }
        public override int Cloths { get { return 4; } }
        public override ClothType ClothType { get { return ClothType.Furry; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Canine; } }

        public Gnoll(Serial serial) : base(serial)
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