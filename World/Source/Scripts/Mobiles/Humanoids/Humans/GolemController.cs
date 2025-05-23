using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a golem controller corpse")]
    public class GolemController : BaseCreature
    {
        [Constructable]
        public GolemController() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = NameList.RandomName("golem controller");
            Title = "the technomancer";

            Body = 400;
            Hue = 0x455;

            Utility.AssignRandomHair(this);
            int HairColor = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0, 8254, 8255, 8256, 8257, 8267, 8268, 8269);
            HairHue = HairColor;
            FacialHairHue = HairColor;

            AddArcane(new Robe());
            AddArcane(new ThighBoots());
            AddArcane(new LeatherGloves());
            AddArcane(new Cloak());

            SetStr(126, 150);
            SetDex(96, 120);
            SetInt(151, 175);

            SetHits(76, 90);

            SetDamage(6, 12);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 25, 35);
            SetResistance(ResistanceType.Cold, 35, 45);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.Psychology, 95.1, 100.0);
            SetSkill(SkillName.Magery, 95.1, 100.0);
            SetSkill(SkillName.Meditation, 95.1, 100.0);
            SetSkill(SkillName.MagicResist, 102.5, 125.0);
            SetSkill(SkillName.Tactics, 65.0, 87.5);
            SetSkill(SkillName.FistFighting, 65.0, 87.5);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 16;

            if (0.7 > Utility.RandomDouble())
                PackItem(new ArcaneGem());

            if (0.1 > Utility.RandomDouble())
                PackItem(new PowerCrystal());

            if (0.15 > Utility.RandomDouble())
                PackItem(new ClockworkAssembly());

            if (0.25 > Utility.RandomDouble())
                PackItem(new Gears(Utility.RandomMinMax(1, 5)));

            if (0.25 > Utility.RandomDouble())
                PackItem(new BottleOil(Utility.RandomMinMax(1, 5)));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        public void AddArcane(Item item)
        {
            if (item is IArcaneEquip)
            {
                IArcaneEquip eq = (IArcaneEquip)item;
                eq.CurArcaneCharges = eq.MaxArcaneCharges = 20;
            }

            item.Hue = ArcaneGem.DefaultArcaneHue;
            item.LootType = LootType.Newbied;

            AddItem(item);
        }

        public override bool ClickTitle { get { return false; } }
        public override bool ShowFameTitle { get { return false; } }
        public override int Skeletal { get { return Utility.Random(3); } }
        public override SkeletalType SkeletalType { get { return SkeletalType.Brittle; } }

        public GolemController(Serial serial) : base(serial)
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