using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a hell cat corpse")]
    [TypeAlias("Server.Mobiles.Hellcat")]
    public class HellCat : BaseCreature
    {
        public override bool HasBreath { get { return true; } }
        public override bool ReacquireOnMovement { get { return !Controlled; } }
        public override double BreathEffectDelay { get { return 0.1; } }
        public override void BreathDealDamage(Mobile target, int form) { base.BreathDealDamage(target, 17); }

        [Constructable]
        public HellCat() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a hell cat";
            Body = 214;
            Hue = 1652;
            BaseSoundID = 0xBA;

            SetStr(51, 100);
            SetDex(52, 150);
            SetInt(13, 85);

            SetHits(48, 67);

            SetDamage(6, 12);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire, 60);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Energy, 15, 20);

            SetSkill(SkillName.MagicResist, 45.1, 60.0);
            SetSkill(SkillName.Tactics, 40.1, 55.0);
            SetSkill(SkillName.FistFighting, 30.1, 40.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 30;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 71.1;

            AddItem(new LightSource());
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }

        public override int Hides { get { return 10; } }
        public override HideType HideType { get { return HideType.Volcanic; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

        public HellCat(Serial serial) : base(serial)
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