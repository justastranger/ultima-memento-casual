using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a feline corpse")]
    public class SnowLion : BaseMount
    {
        public override WeaponAbility GetWeaponAbility()
        {
            return WeaponAbility.BleedAttack;
        }

        [Constructable]
        public SnowLion() : this("a lion")
        {
        }

        [Constructable]
        public SnowLion(string name) : base(name, 187, 0x3EBA, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = 2496;
            BaseSoundID = 0x3EE;

            SetStr(112, 160);
            SetDex(120, 190);
            SetInt(50, 76);

            SetHits(64, 88);
            SetMana(0);

            SetDamage(8, 16);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 35);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 25, 35);
            SetResistance(ResistanceType.Poison, 5, 10);

            SetSkill(SkillName.MagicResist, 15.1, 30.0);
            SetSkill(SkillName.Tactics, 45.1, 60.0);
            SetSkill(SkillName.FistFighting, 45.1, 60.0);

            Fame = 750;
            Karma = 0;

            VirtualArmor = 22;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 40.1;
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 10; } }
        public override HideType HideType { get { return HideType.Frozen; } }
        public override int Cloths { get { return 5; } }
        public override ClothType ClothType { get { return ClothType.Wooly; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish | FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

        public SnowLion(Serial serial) : base(serial)
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