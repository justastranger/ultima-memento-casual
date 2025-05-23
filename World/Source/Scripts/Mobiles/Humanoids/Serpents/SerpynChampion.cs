using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a serpyn corpse")]
    public class SerpynChampion : BaseCreature
    {
        [Constructable]
        public SerpynChampion() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a serpyn champion";
            Body = 143;
            BaseSoundID = 634;
            CanSwim = true;

            SetStr(417, 595);
            SetDex(166, 175);
            SetInt(46, 70);

            SetHits(266, 342);
            SetMana(0);

            SetDamage(16, 19);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 35, 40);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 35, 45);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.Poisoning, 60.1, 80.0);
            SetSkill(SkillName.MagicResist, 65.1, 80.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.FistFighting, 90.1, 100.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 40;

            Item Venom = new VenomSack();
            Venom.Name = "greater venom sack";
            AddItem(Venom);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
        }

        public override int Meat { get { return 2; } }
        public override int Hides { get { return 7; } }
        public override HideType HideType { get { return HideType.Spined; } }
        public override Poison PoisonImmune { get { return Poison.Greater; } }
        public override Poison HitPoison { get { return Poison.Greater; } }
        public override int Skin { get { return Utility.Random(4); } }
        public override SkinType SkinType { get { return SkinType.Snake; } }

        public SerpynChampion(Serial serial) : base(serial)
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