using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a scorpion corpse")]
    public class DeadlyScorpion : BaseCreature
    {
        [Constructable]
        public DeadlyScorpion() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a deadly scorpion";
            Body = Utility.RandomList(315, 873);
            BaseSoundID = 397;

            SetStr(73, 115);
            SetDex(76, 95);
            SetInt(16, 30);

            SetHits(50, 63);
            SetMana(0);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 20, 25);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.Poisoning, 80.1, 100.0);
            SetSkill(SkillName.MagicResist, 30.1, 35.0);
            SetSkill(SkillName.Tactics, 60.3, 75.0);
            SetSkill(SkillName.FistFighting, 50.3, 65.0);

            Fame = 2000;
            Karma = -2000;

            VirtualArmor = 28;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 67.1;

            Item Venom = new VenomSack();
            Venom.Name = "deadly venom sack";
            AddItem(Venom);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }

        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Arachnid; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }

        public DeadlyScorpion(Serial serial) : base(serial)
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