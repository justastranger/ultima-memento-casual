using System;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Server.Spells.Sixth
{
    public class InvisibilitySpell : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Invisibility", "An Lor Xen",
                206,
                9002,
                Reagent.Bloodmoss,
                Reagent.Nightshade
            );

        public override SpellCircle Circle { get { return SpellCircle.Sixth; } }

        public InvisibilitySpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (m is Mobiles.BaseVendor || m is Mobiles.PlayerVendor || m is Mobiles.PlayerBarkeeper || m.AccessLevel > Caster.AccessLevel)
            {
                Caster.SendLocalizedMessage(501857); // This spell won't work on that!
            }
            else if (CheckBSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                Effects.SendLocationParticles(EffectItem.Create(new Point3D(m.X, m.Y, m.Z + 16), Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, PlayerSettings.GetMySpellHue(true, Caster, 0), 0, 5045, 0);
                m.PlaySound(0x3C4);

                m.Hidden = true;
                m.Combatant = null;
                m.Warmode = false;

                RemoveTimer(m);

                int nBenefit = 0;
                if (Caster is PlayerMobile)
                {
                    nBenefit = (int)(Caster.Skills[SkillName.Magery].Value);

                    foreach (Mobile pet in World.Mobiles.Values)
                    {
                        if (pet is BaseCreature)
                        {
                            BaseCreature bc = (BaseCreature)pet;
                            if (bc.Controlled && bc.ControlMaster == m)
                                pet.Hidden = true;
                        }
                    }
                }

                TimeSpan duration = TimeSpan.FromSeconds(((1.2 * Spell.ItemSkillValue(Caster, SkillName.Magery, true)) / 10) + nBenefit);

                Timer t = new InternalTimer(m, duration);

                BuffInfo.RemoveBuff(m, BuffIcon.HidingAndOrStealth);
                BuffInfo.RemoveBuff(m, BuffIcon.Invisibility);
                BuffInfo.RemoveBuff(m, BuffIcon.SpectralShadow);
                BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Invisibility, 1075825, duration, m)); //Invisibility/Invisible

                m_Table[m] = t;

                t.Start();
            }

            FinishSequence();
        }

        private static Hashtable m_Table = new Hashtable();

        public static bool HasTimer(Mobile m)
        {
            return m_Table[m] != null;
        }

        public static void RemoveTimer(Mobile m)
        {
            Timer t = (Timer)m_Table[m];

            if (t != null)
            {
                t.Stop();
                m_Table.Remove(m);
            }
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Mobile;

            public InternalTimer(Mobile m, TimeSpan duration) : base(duration)
            {
                Priority = TimerPriority.OneSecond;
                m_Mobile = m;
            }

            protected override void OnTick()
            {
                m_Mobile.RevealingAction();
                RemoveTimer(m_Mobile);
            }
        }

        public class InternalTarget : Target
        {
            private InvisibilitySpell m_Owner;

            public InternalTarget(InvisibilitySpell owner) : base(Core.ML ? 10 : 12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}