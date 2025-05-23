using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a zuluu corpse")]
    public class ZuluuNative : BaseCreature
    {
        [Constructable]
        public ZuluuNative() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a zuluu";
            Hue = 0x89D;

            if (this.Female = Utility.RandomBool())
            {
                Body = 0x191;
                Item cloth9 = new FemaleLeatherChest();
                cloth9.Hue = 0xB9A;
                cloth9.Name = "Zuluu Tunic";
                AddItem(cloth9);
            }
            else
            {
                Body = 0x190;
            }

            HairHue = 0x96C;

            SetStr(216, 235);
            SetDex(206, 225);
            SetInt(71, 85);

            SetDamage(23, 27);

            SetDamageType(ResistanceType.Physical, 100);

            SetSkill(SkillName.Fencing, 70.0, 102.5);
            SetSkill(SkillName.Bludgeoning, 70.0, 102.5);
            SetSkill(SkillName.Poisoning, 70.0, 102.5);
            SetSkill(SkillName.MagicResist, 67.5, 100.0);
            SetSkill(SkillName.Swords, 70.0, 102.5);
            SetSkill(SkillName.Tactics, 70.0, 102.5);

            Fame = 3100;
            Karma = -3100;
            VirtualArmor = 20;

            PackItem(new Bandage(Utility.RandomMinMax(1, 15)));

            AddItem(new Spear());

            Item cloth1 = new SavageArms();
            cloth1.Hue = 0xB9A;
            cloth1.Name = "Zuluu Gauntlets";
            AddItem(cloth1);
            Item cloth2 = new SavageLegs();
            cloth2.Hue = 0xB9A;
            cloth2.Name = "Zuluu Leggings";
            AddItem(cloth2);
            Item cloth3 = new TribalMask();
            cloth3.Hue = 0xB9A;
            cloth3.Name = "Zuluu Tribal Mask";
            AddItem(cloth3);
            Item cloth4 = new LeatherSkirt();
            cloth4.Hue = 0xB9A;
            cloth4.Name = "Zuluu Skirt";
            cloth4.Layer = Layer.Waist;
            AddItem(cloth4);

            if (Utility.RandomMinMax(1, 10) == 1)
            {
                SwampDragon pet = new SwampDragon();
                pet.Hue = 2006;
                pet.Name = "a forest dragyn";
                pet.Rider = this;
                ActiveSpeed = 0.1;
                PassiveSpeed = 0.2;
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public override bool ClickTitle { get { return false; } }
        public override bool ShowFameTitle { get { return false; } }
        public override bool AlwaysAttackable { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override int Skeletal { get { return Utility.Random(3); } }
        public override SkeletalType SkeletalType { get { return SkeletalType.Brittle; } }

        public ZuluuNative(Serial serial) : base(serial)
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