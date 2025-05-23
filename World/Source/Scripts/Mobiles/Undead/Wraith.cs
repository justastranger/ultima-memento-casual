using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ghostly corpse")]
    public class Wraith : BaseCreature
    {
        [Constructable]
        public Wraith() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a wraith";
            Body = 84;
            Hue = 0x9C2;
            BaseSoundID = 0x482;

            SetStr(76, 100);
            SetDex(76, 95);
            SetInt(36, 60);

            SetHits(46, 60);

            SetDamage(7, 11);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 25, 30);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Poison, 10, 20);

            SetSkill(SkillName.Psychology, 55.1, 70.0);
            SetSkill(SkillName.Magery, 55.1, 70.0);
            SetSkill(SkillName.MagicResist, 55.1, 70.0);
            SetSkill(SkillName.Tactics, 45.1, 60.0);
            SetSkill(SkillName.FistFighting, 45.1, 55.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 28;

            PackReg(10);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }

        public override bool OnBeforeDeath()
        {
            this.Body = 13;
            return base.OnBeforeDeath();
        }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override int Cloths { get { return 5; } }
        public override ClothType ClothType { get { return ClothType.Haunted; } }

        public Wraith(Serial serial) : base(serial)
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