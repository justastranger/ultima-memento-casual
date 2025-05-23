using System;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a goat corpse")]
    public class Goat : BaseCreature
    {
        [Constructable]
        public Goat() : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a goat";
            Body = 209;
            BaseSoundID = 0x99;

            SetStr(19);
            SetDex(15);
            SetInt(5);

            SetHits(12);
            SetMana(0);

            SetDamage(3, 4);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 15);

            SetSkill(SkillName.MagicResist, 5.0);
            SetSkill(SkillName.Tactics, 5.0);
            SetSkill(SkillName.FistFighting, 5.0);

            Fame = 150;
            Karma = 0;

            VirtualArmor = 10;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 11.1;
        }

        public override int Meat { get { return 2; } }
        public override int Hides { get { return 8; } }
        public override int Cloths { get { return 4; } }
        public override ClothType ClothType { get { return ClothType.Wooly; } }
        public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay | FoodType.FruitsAndVegies; } }

        public Goat(Serial serial) : base(serial)
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