using System;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a feline corpse")]
    public class Panther : BaseCreature
    {
        [Constructable]
        public Panther() : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            int cat = Utility.RandomMinMax(1, 3);

            if (cat > 2)
            {
                Name = "a panther";
                Body = 885;
                BaseSoundID = 0x3EE;
                Hue = 0x96C;

                SetStr(112, 160);
                SetDex(120, 190);
                SetInt(50, 76);

                SetHits(64, 88);
                SetMana(0);

                SetDamage(8, 16);

                SetDamageType(ResistanceType.Physical, 100);

                SetResistance(ResistanceType.Physical, 30, 35);
                SetResistance(ResistanceType.Fire, 5, 10);
                SetResistance(ResistanceType.Cold, 10, 15);
                SetResistance(ResistanceType.Poison, 5, 10);

                SetSkill(SkillName.MagicResist, 15.1, 30.0);
                SetSkill(SkillName.Tactics, 45.1, 60.0);
                SetSkill(SkillName.FistFighting, 45.1, 60.0);

                Fame = 750;
                Karma = 0;

                VirtualArmor = 22;

                Tamable = true;
                ControlSlots = 1;
                MinTameSkill = 61.1;
            }
            else
            {
                Name = "a panther";
                Body = 0xD6;
                Hue = 0x901;
                BaseSoundID = 0x462;

                SetStr(61, 85);
                SetDex(86, 105);
                SetInt(26, 50);

                SetHits(37, 51);
                SetMana(0);

                SetDamage(4, 12);

                SetDamageType(ResistanceType.Physical, 100);

                SetResistance(ResistanceType.Physical, 20, 25);
                SetResistance(ResistanceType.Fire, 5, 10);
                SetResistance(ResistanceType.Cold, 10, 15);
                SetResistance(ResistanceType.Poison, 5, 10);

                SetSkill(SkillName.MagicResist, 15.1, 30.0);
                SetSkill(SkillName.Tactics, 50.1, 65.0);
                SetSkill(SkillName.FistFighting, 50.1, 65.0);

                Fame = 450;
                Karma = 0;

                VirtualArmor = 16;

                Tamable = true;
                ControlSlots = 1;
                MinTameSkill = 53.1;
            }
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 10; } }
        public override int Cloths { get { return 5; } }
        public override ClothType ClothType { get { return ClothType.Furry; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

        public Panther(Serial serial) : base(serial)
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