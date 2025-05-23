using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a hawk corpse")]
    public class Hawk : BaseCreature
    {
        [Constructable]
        public Hawk() : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a hawk";
            Body = 5;
            Hue = Utility.RandomNeutralHue();
            BaseSoundID = 0x2EE;

            SetStr(31, 47);
            SetDex(36, 60);
            SetInt(8, 20);

            SetHits(20, 27);
            SetMana(0);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 20, 25);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 15.3, 30.0);
            SetSkill(SkillName.Tactics, 18.1, 37.0);
            SetSkill(SkillName.FistFighting, 20.1, 30.0);

            Fame = 300;
            Karma = 0;

            VirtualArmor = 22;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 17.1;
        }

        public override void OnCarve(Mobile from, Corpse corpse, Item with)
        {
            base.OnCarve(from, corpse, with);

            if (Utility.RandomMinMax(1, 5) == 1)
            {
                Item egg = new Eggs(Utility.RandomMinMax(1, 2));
                corpse.DropItem(egg);
            }
        }

        public override int Meat { get { return 1; } }
        public override MeatType MeatType { get { return MeatType.Bird; } }
        public override int Feathers { get { return 36; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }

        public Hawk(Serial serial) : base(serial)
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