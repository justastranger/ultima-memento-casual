using System;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a bull frog corpse")]
    [TypeAlias("Server.Mobiles.Bullfrog")]
    public class BullFrog : BaseCreature
    {
        private Timer m_Timer;

        [Constructable]
        public BullFrog() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a bull frog";
            Body = 81;
            Hue = Utility.RandomList(0x5AC, 0x5A3, 0x59A, 0x591, 0x588, 0x57F);
            BaseSoundID = 0x266;

            SetStr(46, 70);
            SetDex(6, 25);
            SetInt(11, 20);

            SetHits(28, 42);
            SetMana(0);

            SetDamage(1, 2);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);

            SetSkill(SkillName.MagicResist, 25.1, 40.0);
            SetSkill(SkillName.Tactics, 40.1, 60.0);
            SetSkill(SkillName.FistFighting, 40.1, 60.0);

            Fame = 350;
            Karma = -350;

            VirtualArmor = 6;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 23.1;

            m_Timer = new GiantToad.TeleportTimer(this, 0x1CC);
            m_Timer.Start();
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 4; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish | FoodType.Meat; } }

        public BullFrog(Serial serial) : base(serial)
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
            m_Timer = new GiantToad.TeleportTimer(this, 0x1CC);
            m_Timer.Start();
        }
    }
}