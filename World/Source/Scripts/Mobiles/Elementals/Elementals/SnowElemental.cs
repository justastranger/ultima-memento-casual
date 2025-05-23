using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an elemental corpse")]
    public class SnowElemental : BaseCreature
    {
        public override int BreathPhysicalDamage { get { return 50; } }
        public override int BreathFireDamage { get { return 0; } }
        public override int BreathColdDamage { get { return 50; } }
        public override int BreathPoisonDamage { get { return 0; } }
        public override int BreathEnergyDamage { get { return 0; } }
        public override int BreathEffectHue { get { return 0x47E; } }
        public override int BreathEffectSound { get { return 0x658; } }
        public override int BreathEffectItemID { get { return 0; } }
        public override bool ReacquireOnMovement { get { return !Controlled; } }
        public override bool HasBreath { get { return true; } }
        public override double BreathEffectDelay { get { return 0.1; } }
        public override void BreathDealDamage(Mobile target, int form) { base.BreathDealDamage(target, 32); }

        [Constructable]
        public SnowElemental() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a snow elemental";
            Body = 142;
            Hue = 2819;
            BaseSoundID = 268;

            SetStr(326, 355);
            SetDex(166, 185);
            SetInt(71, 95);

            SetHits(196, 213);

            SetDamage(11, 17);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 80);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 50.1, 65.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.FistFighting, 80.1, 100.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 50;

            PackItem(new BlackPearl(3));
            Item ore = new IronOre(2);
            ore.ItemID = 0x19B9;
            PackItem(ore);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        public override bool BleedImmune { get { return true; } }

        public override int TreasureMapLevel { get { return Utility.RandomList(2, 3); } }

        public SnowElemental(Serial serial) : base(serial)
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