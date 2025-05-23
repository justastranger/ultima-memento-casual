using System;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Spells.Necromancy;

namespace Server.Spells.Chivalry
{
    public class DispelEvilSpell : PaladinSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Dispel Evil", "Dispiro Malum",
                -1,
                9002
            );

        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(0.25); } }

        public override double RequiredSkill { get { return 35.0; } }
        public override int RequiredMana { get { return 10; } }
        public override int RequiredTithing { get { return 10; } }
        public override int MantraNumber { get { return 1060721; } } // Dispiro Malum
        public override bool BlocksMovement { get { return false; } }

        public DispelEvilSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override bool DelayedDamage { get { return false; } }

        public override void SendCastEffect()
        {
            Caster.FixedEffect(0x37C4, 10, 7, 4, 3); // At player
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in Caster.GetMobilesInRange(8))
                {
                    if (m is BaseCreature)
                    {
                        BaseCreature mn = m as BaseCreature;
                        if (mn.IsTempEnemy)
                            targets.Add(m);
                        else if (Caster != m && SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false))
                            targets.Add(m);
                    }
                    else if (Caster != m && SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false))
                    {
                        targets.Add(m);
                    }
                }

                Caster.PlaySound(0xF5);
                Caster.PlaySound(0x299);
                Caster.FixedParticles(0x37C4, 1, 25, 9922, 14, 3, EffectLayer.Head);

                int dispelSkill = ComputePowerValue(2);

                double chiv = Caster.Skills.Knightship.Value;

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = targets[i];
                    BaseCreature bc = m as BaseCreature;

                    if (bc != null)
                    {
                        bool dispellable = bc.Summoned && !bc.IsAnimatedDead;

                        if (dispellable)
                        {
                            double dispelChance = (50.0 + ((100 * (chiv - bc.DispelDifficulty)) / (bc.DispelFocus * 2))) / 100;
                            dispelChance *= dispelSkill / 100.0;

                            if (dispelChance > Utility.RandomDouble())
                            {
                                Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x3728, 8, 20, 5042);
                                Effects.PlaySound(m, m.Map, 0x201);

                                m.Delete();
                                continue;
                            }
                        }
                        else if (bc.IsTempEnemy)
                        {
                            if (chiv > Utility.RandomMinMax(1, 100))
                            {
                                Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x3728, 8, 20, 5042);
                                Effects.PlaySound(m, m.Map, 0x201);
                                m.Delete();
                                continue;
                            }
                        }

                        bool evil = !bc.Controlled && bc.Karma < 0;

                        if (evil)
                        {
                            // TODO: Is this right?
                            double fleeChance = (100 - Math.Sqrt(m.Fame / 2)) * chiv * dispelSkill;
                            fleeChance /= 1000000;

                            if (fleeChance > Utility.RandomDouble())
                            {
                                // guide says 2 seconds, it's longer
                                bc.BeginFlee(TimeSpan.FromSeconds(30.0));
                            }
                        }
                    }

                    TransformContext context = TransformationSpellHelper.GetContext(m);
                    if (context != null && context.Spell is NecromancerSpell)   //Trees are not evil!	TODO: OSI confirm?
                    {
                        // transformed ..

                        double drainChance = 0.5 * (Caster.Skills.Knightship.Value / Math.Max(m.Skills.Necromancy.Value, 1));

                        if (drainChance > Utility.RandomDouble())
                        {
                            int drain = (5 * dispelSkill) / 100;

                            m.Stam -= drain;
                            m.Mana -= drain;
                        }
                    }
                }
            }

            FinishSequence();
        }
    }
}