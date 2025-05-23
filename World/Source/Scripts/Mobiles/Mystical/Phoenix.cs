using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("smoldering ashes")]
    [Server.Engines.Craft.Forge]
    public class Phoenix : BaseMount
    {
        [Constructable]
        public Phoenix() : base("a phoenix", 243, 0x3E94, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Hue = 0xB73;
            BaseSoundID = 0x8F;

            SetStr(504, 700);
            SetDex(202, 300);
            SetInt(504, 700);

            SetHits(340, 383);

            SetDamage(25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Psychology, 90.2, 100.0);
            SetSkill(SkillName.Magery, 90.2, 100.0);
            SetSkill(SkillName.Meditation, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 86.0, 135.0);
            SetSkill(SkillName.Tactics, 80.1, 90.0);
            SetSkill(SkillName.FistFighting, 90.1, 100.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 95.1;

            AddItem(new LighterSource());
        }

        public override void GenerateLoot()
        {
            PackItem(new SulfurousAsh(Utility.RandomMinMax(100, 200)));
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
        }

        public override bool OnBeforeDeath()
        {
            this.Body = 13;
            Server.Misc.IntelligentAction.BurnAway(this);
            return base.OnBeforeDeath();
        }

        public override int Meat { get { return 1; } }
        public override MeatType MeatType { get { return MeatType.Bird; } }
        public override int Feathers { get { return 36; } }
        public override FoodType FavoriteFood { get { return FoodType.Gold; } }
        public override bool CanAngerOnTame { get { return true; } }
        public override int Skeletal { get { return Utility.Random(5); } }
        public override SkeletalType SkeletalType { get { return SkeletalType.Mystical; } }

        public Phoenix(Serial serial) : base(serial)
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