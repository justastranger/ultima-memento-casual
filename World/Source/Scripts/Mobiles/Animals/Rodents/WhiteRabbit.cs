using System;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a rabbit corpse")]
    public class WhiteRabbit : BaseCreature
    {
        [Constructable]
        public WhiteRabbit() : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a rabbit";
            Body = 205;

            Hue = 1150;

            switch (Utility.RandomMinMax(0, 1))
            {
                case 0: Name = "a rabbit"; break;
                case 1: Name = "a hare"; break;
            }

            SetStr(6, 10);
            SetDex(26, 38);
            SetInt(6, 14);

            SetHits(4, 6);
            SetMana(0);

            SetDamage(1);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);

            SetSkill(SkillName.MagicResist, 5.0);
            SetSkill(SkillName.Tactics, 5.0);
            SetSkill(SkillName.FistFighting, 5.0);

            Fame = 150;
            Karma = 0;

            VirtualArmor = 6;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 1; } }
        public override int Cloths { get { return 1; } }
        public override ClothType ClothType { get { return ClothType.Wooly; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }

        public WhiteRabbit(Serial serial) : base(serial)
        {
        }

        public override int GetAttackSound()
        {
            return 0xC9;
        }

        public override int GetHurtSound()
        {
            return 0xCA;
        }

        public override int GetDeathSound()
        {
            return 0xCB;
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