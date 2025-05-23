using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an orc corpse")]
    public class OrcishLord : BaseCreature
    {
        public override InhumanSpeech SpeechType { get { return InhumanSpeech.Orc; } }

        [Constructable]
        public OrcishLord() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = NameList.RandomName("orc");
            Title = "the orc warlord";
            Body = Utility.RandomList(7, 473);
            BaseSoundID = 0x45A;

            SetStr(147, 215);
            SetDex(91, 115);
            SetInt(61, 85);

            SetHits(95, 123);

            SetDamage(4, 14);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 70.1, 85.0);
            SetSkill(SkillName.Swords, 60.1, 85.0);
            SetSkill(SkillName.Tactics, 75.1, 90.0);
            SetSkill(SkillName.FistFighting, 60.1, 85.0);

            Fame = 2500;
            Karma = -2500;

            switch (Utility.Random(5))
            {
                case 0: PackItem(new Lockpick()); break;
                case 1: PackItem(new MortarPestle()); break;
                case 2: PackItem(new Bottle()); break;
                case 3: PackItem(new RawRibs()); break;
                case 4: PackItem(new Spade()); break;
            }

            PackItem(new RingmailChest());
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.Average);
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override int TreasureMapLevel { get { return 1; } }
        public override int Meat { get { return 1; } }
        public override int Skeletal { get { return Utility.Random(2); } }
        public override SkeletalType SkeletalType { get { return SkeletalType.Orc; } }

        public OrcishLord(Serial serial) : base(serial)
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