using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a wolf corpse")]
    public class WinterWolf : BaseMount
    {
        public override int BreathPhysicalDamage { get { return 0; } }
        public override int BreathFireDamage { get { return 0; } }
        public override int BreathColdDamage { get { return 100; } }
        public override int BreathPoisonDamage { get { return 0; } }
        public override int BreathEnergyDamage { get { return 0; } }
        public override int BreathEffectHue { get { return 0x481; } }
        public override int BreathEffectSound { get { return 0x64F; } }
        public override bool HasBreath { get { return true; } }
        public override bool ReacquireOnMovement { get { return !Controlled; } }
        public override double BreathEffectDelay { get { return 0.1; } }
        public override void BreathDealDamage(Mobile target, int form) { base.BreathDealDamage(target, 19); }

        [Constructable]
        public WinterWolf() : this("a winter wolf")
        {
        }

        [Constructable]
        public WinterWolf(string name) : base(name, 277, 16017, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a winter wolf";
            BaseSoundID = 0xE5;
            Hue = 0xB5D;

            SetStr(102, 150);
            SetDex(81, 105);
            SetInt(36, 60);

            SetHits(66, 125);

            SetDamage(11, 17);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 80);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 57.6, 75.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.FistFighting, 60.1, 80.0);

            Fame = 3400;
            Karma = -3400;

            VirtualArmor = 30;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 85.5;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
        }

        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Canine; } }
        public override int Hides { get { return 5; } }
        public override HideType HideType { get { return HideType.Frozen; } }

        public WinterWolf(Serial serial) : base(serial)
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
            Hue = 0xB5D;
        }
    }
}