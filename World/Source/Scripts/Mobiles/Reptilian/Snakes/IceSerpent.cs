using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("an ice serpent corpse")]
    [TypeAlias("Server.Mobiles.Iceserpant")]
    public class IceSerpent : BaseCreature
    {
        [Constructable]
        public IceSerpent() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ice serpent";
            Body = 21;
            Hue = 0xB57;
            BaseSoundID = 219;

            SetStr(216, 245);
            SetDex(26, 50);
            SetInt(66, 85);

            SetHits(130, 147);
            SetMana(0);

            SetDamage(7, 17);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Cold, 90);

            SetResistance(ResistanceType.Physical, 30, 35);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Anatomy, 27.5, 50.0);
            SetSkill(SkillName.MagicResist, 25.1, 40.0);
            SetSkill(SkillName.Tactics, 65.1, 70.0);
            SetSkill(SkillName.FistFighting, 60.1, 80.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 32;

            switch (Utility.Random(10))
            {
                case 0: PackItem(new LeftArm()); break;
                case 1: PackItem(new RightArm()); break;
                case 2: PackItem(new Torso()); break;
                case 3: PackItem(new Bone()); break;
                case 4: PackItem(new RibCage()); break;
                case 5: PackItem(new RibCage()); break;
                case 6: PackItem(new BonePile()); break;
                case 7: PackItem(new BonePile()); break;
                case 8: PackItem(new BonePile()); break;
                case 9: PackItem(new BonePile()); break;
            }

            if (0.025 > Utility.RandomDouble())
                PackItem(new GlacialStaff());

            AddItem(new LightSource());
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }

        public override bool DeathAdderCharmable { get { return true; } }

        public override int Meat { get { return 4; } }
        public override int Hides { get { return 15; } }
        public override HideType HideType { get { return HideType.Frozen; } }
        public override int Skin { get { return Utility.Random(3); } }
        public override SkinType SkinType { get { return SkinType.Snake; } }

        public IceSerpent(Serial serial) : base(serial)
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

            if (BaseSoundID == -1)
                BaseSoundID = 219;
        }
    }
}