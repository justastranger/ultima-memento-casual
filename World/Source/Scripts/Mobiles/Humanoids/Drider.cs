using System;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
    [CorpseName("a drider corpse")]
    public class Drider : BaseCreature
    {
        public override int BreathPhysicalDamage { get { return 50; } }
        public override int BreathFireDamage { get { return 0; } }
        public override int BreathColdDamage { get { return 0; } }
        public override int BreathPoisonDamage { get { return 50; } }
        public override int BreathEnergyDamage { get { return 0; } }
        public override int BreathEffectHue { get { return 0; } }
        public override int BreathEffectSound { get { return 0x62A; } }
        public override int BreathEffectItemID { get { return 0x10D4; } }
        public override bool HasBreath { get { return true; } }
        public override double BreathEffectDelay { get { return 0.1; } }
        public override void BreathDealDamage(Mobile target, int form) { base.BreathDealDamage(target, 6); }

        [Constructable]
        public Drider() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a drider";
            Body = 693;
            BaseSoundID = 0x24D;

            SetStr(286, 310);
            SetDex(196, 220);
            SetInt(126, 145);

            SetHits(118, 132);

            SetDamage(5, 17);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 80);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Psychology, 65.1, 80.0);
            SetSkill(SkillName.Magery, 65.1, 80.0);
            SetSkill(SkillName.Meditation, 65.1, 80.0);
            SetSkill(SkillName.MagicResist, 45.1, 60.0);
            SetSkill(SkillName.Tactics, 55.1, 70.0);
            SetSkill(SkillName.FistFighting, 60.1, 75.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 36;

            PackItem(new SpidersSilk(8));

            Item Venom = new VenomSack();
            Venom.Name = "lethal venom sack";
            AddItem(Venom);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override Poison HitPoison { get { return Poison.Deadly; } }
        public override int Skeletal { get { return Utility.Random(2); } }
        public override SkeletalType SkeletalType { get { return SkeletalType.Drow; } }

        public Drider(Serial serial) : base(serial)
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