using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Magical
{
    public class SummonDragonSpell : MagicalSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "", "",
                266,
                9040,
                false
            );

        public override SpellCircle Circle { get { return SpellCircle.Eighth; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 30; } }
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(2.0); } }

        public SummonDragonSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            if (!base.CheckCast())
                return false;

            if ((Caster.Followers + 3) > Caster.FollowersMax)
            {
                Caster.SendLocalizedMessage(1049645); // You have too many followers to summon that creature.
                return false;
            }

            return true;
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            Map map = Caster.Map;
            SpellHelper.GetSurfaceTop(ref p);

            if (map == null || !map.CanSpawnMobile(p.X, p.Y, p.Z))
            {
                Caster.SendLocalizedMessage(501942); // That location is blocked.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                TimeSpan duration;
                duration = TimeSpan.FromSeconds(120);
                BaseCreature.Summon(new SummonDragon(), false, Caster, new Point3D(p), 0x212, duration);

                Caster.SendMessage("You can double click the summoned to dispel them.");
            }
            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private SummonDragonSpell m_Owner;

            public InternalTarget(SummonDragonSpell owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetOutOfLOS(Mobile from, object o)
            {
                from.SendLocalizedMessage(501943); // Target cannot be seen. Try again.
                from.Target = new InternalTarget(m_Owner);
                from.Target.BeginTimeout(from, TimeoutTime - DateTime.Now);
                m_Owner = null;
            }

            protected override void OnTargetFinish(Mobile from)
            {
                if (m_Owner != null)
                    m_Owner.FinishSequence();
            }
        }
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Mobiles
{
    [CorpseName("a dragon corpse")]
    public class SummonDragon : BaseCreature
    {
        public override bool DeleteCorpseOnDeath { get { return Summoned; } }
        public override double DispelDifficulty { get { return 900.0; } }
        public override double DispelFocus { get { return 900.0; } }

        public override double GetFightModeRanking(Mobile m, FightMode acqType, bool bPlayerOnly)
        {
            return 200 / Math.Max(GetDistanceToSqrt(m), 1.0);
        }

        [Constructable]
        public SummonDragon() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a dragon";

            Body = 0x3D;
            Hue = 0xB85;
            BaseSoundID = 362;

            SetStr(401, 430);
            SetDex(133, 152);
            SetInt(101, 140);

            SetHits(241, 258);

            SetDamage(11, 17);

            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Fire, 20);

            SetResistance(ResistanceType.Physical, 45, 50);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 65.1, 80.0);
            SetSkill(SkillName.Tactics, 65.1, 90.0);
            SetSkill(SkillName.FistFighting, 65.1, 80.0);

            Fame = 0;
            Karma = 0;

            VirtualArmor = 50;
            ControlSlots = 3;
        }

        public override bool ReacquireOnMovement { get { return true; } }
        public override bool HasBreath { get { return true; } } // fire breath enabled

        public override void OnThink()
        {
            if (Core.SE && Summoned)
            {
                ArrayList spirtsOrVortexes = new ArrayList();

                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (BaseCreature.isVortex(m))
                    {
                        if (((BaseCreature)m).Summoned)
                            spirtsOrVortexes.Add(m);
                    }
                }

                while (spirtsOrVortexes.Count > 6)
                {
                    int index = Utility.Random(spirtsOrVortexes.Count);
                    Dispel(((Mobile)spirtsOrVortexes[index]));
                    spirtsOrVortexes.RemoveAt(index);
                }
            }

            base.OnThink();
        }

        public SummonDragon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}