using System;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a bear corpse")]
    [TypeAlias("Server.Mobiles.Bear")]
    public class BlackBear : BaseCreature
    {
        [Constructable]
        public BlackBear() : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a black bear";
            Body = 211;
            Hue = 0xB3A;
            BaseSoundID = 0xA3;

            SetStr(76, 100);
            SetDex(56, 75);
            SetInt(11, 14);

            SetHits(46, 60);
            SetMana(0);

            SetDamage(4, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Cold, 10, 15);
            SetResistance(ResistanceType.Poison, 5, 10);

            SetSkill(SkillName.MagicResist, 20.1, 40.0);
            SetSkill(SkillName.Tactics, 40.1, 60.0);
            SetSkill(SkillName.FistFighting, 40.1, 60.0);

            Fame = 450;
            Karma = 0;

            VirtualArmor = 24;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 35.1;
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 12; } }
        public override int Cloths { get { return 6; } }
        public override ClothType ClothType { get { return ClothType.Furry; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish | FoodType.Meat | FoodType.FruitsAndVegies; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Bear; } }

        public BlackBear(Serial serial) : base(serial)
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
            Hue = 0xB3A;
        }
    }
}