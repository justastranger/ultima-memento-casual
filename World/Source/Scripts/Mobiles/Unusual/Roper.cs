using System;
using Server.Items;
using Server.Engines.Plants;

namespace Server.Mobiles
{
    [CorpseName("a roper corpse")]
    public class Roper : BaseCreature
    {
        private Timer m_Timer;

        [Constructable]
        public Roper() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a roper";
            Body = 8;
            Hue = 0x845;
            BaseSoundID = 0x4F5;

            SetStr(96, 120);
            SetDex(66, 85);
            SetInt(16, 30);

            SetHits(58, 72);
            SetMana(0);

            SetDamage(6, 12);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison, 60);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 15.1, 20.0);
            SetSkill(SkillName.Tactics, 65.1, 80.0);
            SetSkill(SkillName.FistFighting, 65.1, 80.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 28;

            m_Timer = new GiantToad.TeleportTimer(this, 0x1FE);
            m_Timer.Start();
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            Granite granite = new Granite();
            granite.Amount = 1;
            c.DropItem(granite);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
        }

        public Roper(Serial serial) : base(serial)
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
            m_Timer = new GiantToad.TeleportTimer(this, 0x1FE);
            m_Timer.Start();
        }
    }
}