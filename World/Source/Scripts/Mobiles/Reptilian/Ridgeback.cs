using System;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a dinosaur corpse")]
    public class Ridgeback : BaseMount
    {
        [Constructable]
        public Ridgeback() : this("a stegladon")
        {
        }

        [Constructable]
        public Ridgeback(string name) : base(name, 0x11C, 0x3E92, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0x4F5;
            Hue = Utility.RandomList(0xB79, 0xB19, 0xAEF, 0xACE, 0xAB0);

            SetStr(58, 100);
            SetDex(56, 75);
            SetInt(16, 30);

            SetHits(41, 54);
            SetMana(0);

            SetDamage(3, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 25.3, 40.0);
            SetSkill(SkillName.Tactics, 29.3, 44.0);
            SetSkill(SkillName.FistFighting, 35.1, 45.0);

            Fame = 300;
            Karma = 0;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 23.1;
        }

        public override double GetControlChance(Mobile m, bool useBaseSkill)
        {
            return 1.0;
        }

        public override int Meat { get { return 5; } }
        public override int Hides { get { return 12; } }
        public override HideType HideType { get { return HideType.Dinosaur; } }
        public override int Scales { get { return 3; } }
        public override ScaleType ScaleType { get { return ScaleType.Dinosaur; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public Ridgeback(Serial serial) : base(serial)
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

            Body = 0x11C;
            ItemID = 0x3E92;
        }
    }
}