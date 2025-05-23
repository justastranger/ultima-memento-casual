using System;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Spells.Chivalry
{
    public class HolyLightSpell : PaladinSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Holy Light", "Augus Luminos",
                -1,
                9002
            );

        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(1.75); } }

        public override double RequiredSkill { get { return 55.0; } }
        public override int RequiredMana { get { return 10; } }
        public override int RequiredTithing { get { return 10; } }
        public override int MantraNumber { get { return 1060724; } } // Augus Luminos
        public override bool BlocksMovement { get { return false; } }

        public HolyLightSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override bool DelayedDamage { get { return false; } }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(3))
                    if (Caster != m && SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false))
                        targets.Add(m);

                Caster.PlaySound(0x212);
                Caster.PlaySound(0x206);

                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 1, 29, 0x47D, 2, 9962, 0);
                Effects.SendLocationParticles(EffectItem.Create(new Point3D(Caster.X, Caster.Y, Caster.Z - 7), Caster.Map, EffectItem.DefaultDuration), 0x37C4, 1, 29, 0x47D, 2, 9502, 0);

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = targets[i];

                    int damage = ComputePowerValue(10) + Utility.RandomMinMax(0, 2);

                    // TODO: Should caps be applied?
                    if (damage < 8)
                        damage = 8;
                    else if (damage > 24)
                        damage = 24;

                    Caster.DoHarmful(m);
                    SpellHelper.Damage(this, m, damage, 0, 0, 0, 0, 100);
                }
            }

            FinishSequence();
        }
    }
}