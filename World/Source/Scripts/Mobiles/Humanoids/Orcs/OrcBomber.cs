using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("an orcish corpse")]
    public class OrcBomber : BaseCreature
    {
        public override InhumanSpeech SpeechType { get { return InhumanSpeech.Orc; } }

        [Constructable]
        public OrcBomber() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 182;

            Name = NameList.RandomName("orc");
            Title = "the orcish alchemist";
            BaseSoundID = 0x45A;

            SetStr(147, 215);
            SetDex(91, 115);
            SetInt(61, 85);

            SetHits(95, 123);

            SetDamage(1, 8);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Fire, 25);

            SetResistance(ResistanceType.Physical, 25, 35);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Poison, 15, 20);
            SetResistance(ResistanceType.Energy, 25, 30);

            SetSkill(SkillName.MagicResist, 70.1, 85.0);
            SetSkill(SkillName.Swords, 60.1, 85.0);
            SetSkill(SkillName.Tactics, 75.1, 90.0);
            SetSkill(SkillName.FistFighting, 60.1, 85.0);

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 30;

            PackItem(new SulfurousAsh(Utility.RandomMinMax(6, 10)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(6, 10)));
            PackItem(new BlackPearl(Utility.RandomMinMax(6, 10)));
            PackItem(new MortarPestle());
            PackItem(new LesserExplosionPotion());
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 1; } }
        public override int Skeletal { get { return Utility.Random(2); } }
        public override SkeletalType SkeletalType { get { return SkeletalType.Orc; } }

        private DateTime m_NextBomb;
        private int m_Thrown;

        public override void OnActionCombat()
        {
            Mobile combatant = Combatant;

            if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            if (DateTime.Now >= m_NextBomb)
            {
                ThrowBomb(combatant);

                m_Thrown++;

                if (0.75 >= Utility.RandomDouble() && (m_Thrown % 2) == 1) // 75% chance to quickly throw another bomb
                    m_NextBomb = DateTime.Now + TimeSpan.FromSeconds(3.0);
                else
                    m_NextBomb = DateTime.Now + TimeSpan.FromSeconds(5.0 + (10.0 * Utility.RandomDouble())); // 5-15 seconds
            }
        }

        public void ThrowBomb(Mobile m)
        {
            DoHarmful(m);

            this.MovingParticles(m, 0x1C19, 1, 0, false, true, 0, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0);

            new InternalTimer(m, this).Start();
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Mobile, m_From;

            public InternalTimer(Mobile m, Mobile from) : base(TimeSpan.FromSeconds(1.0))
            {
                m_Mobile = m;
                m_From = from;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                m_Mobile.PlaySound(0x11D);
                AOS.Damage(m_Mobile, m_From, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
            }
        }

        public OrcBomber(Serial serial) : base(serial)
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
