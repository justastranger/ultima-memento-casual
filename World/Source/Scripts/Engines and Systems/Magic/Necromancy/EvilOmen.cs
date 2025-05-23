using System;
using System.Collections;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Necromancy
{
    public class EvilOmenSpell : NecromancerSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Evil Omen", "Pas Tym An Sanct",
                203,
                9031,
                Reagent.BatWing,
                Reagent.NoxCrystal
            );

        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(0.75); } }

        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 11; } }

        public EvilOmenSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (!(m is BaseCreature || m is PlayerMobile))
            {
                Caster.SendLocalizedMessage(1060508); // You can't curse that.
            }
            else if (CheckHSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                /* Curses the target so that the next harmful event that affects them is magnified.
				 * Damage to the target's hit points is increased 25%,
				 * the poison level of the attack will be 1 higher
				 * and the Resist Magic skill of the target will be fixed on 50.
				 *
				 * The effect lasts for one harmful event only.
				 */

                if (m.Spell != null)
                    m.Spell.OnCasterHurt();

                m.PlaySound(0xFC);
                m.FixedParticles(0x3728, 1, 13, 9912, 1150, 7, EffectLayer.Head);
                m.FixedParticles(0x3779, 1, 15, 9502, 67, 7, EffectLayer.Head);

                if (!m_Table.Contains(m))
                {
                    SkillMod mod = new DefaultSkillMod(SkillName.MagicResist, false, 50.0);

                    if (m.Skills[SkillName.MagicResist].Base > 50.0)
                        m.AddSkillMod(mod);

                    m_Table[m] = mod;
                }

                TimeSpan duration = TimeSpan.FromSeconds((Spell.ItemSkillValue(Caster, SkillName.Spiritualism, false) / 12) + 1.0);

                Timer.DelayCall(duration, new TimerStateCallback(EffectExpire_Callback), m);

                HarmfulSpell(m);

                BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.EvilOmen, 1075647, 1075648, duration, m));
            }

            FinishSequence();
        }

        private static Hashtable m_Table = new Hashtable();

        private static void EffectExpire_Callback(object state)
        {
            TryEndEffect((Mobile)state);
        }

        /*
		 * The naming here was confusing. Its a 1-off effect spell.
		 * So, we dont actually "checkeffect"; we endeffect with bool
		 * return to determine external behaviors.
		 *
		 * -refactored.
		 */

        public static bool TryEndEffect(Mobile m)
        {
            SkillMod mod = (SkillMod)m_Table[m];

            if (mod == null)
                return false;

            m_Table.Remove(m);
            mod.Remove();

            return true;
        }

        private class InternalTarget : Target
        {
            private EvilOmenSpell m_Owner;

            public InternalTarget(EvilOmenSpell owner)
                : base(Core.ML ? 10 : 12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
                else
                    from.SendLocalizedMessage(1060508); // You can't curse that.
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}