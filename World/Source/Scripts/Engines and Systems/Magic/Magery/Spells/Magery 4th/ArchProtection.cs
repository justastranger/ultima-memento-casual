using System;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Engines.PartySystem;
using Server.Misc;

namespace Server.Spells.Fourth
{
    public class ArchProtectionSpell : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Arch Protection", "Vas Uus Sanct",
                Core.AOS ? 239 : 215,
                9011,
                Reagent.Garlic,
                Reagent.Ginseng,
                Reagent.MandrakeRoot,
                Reagent.SulfurousAsh
            );

        public override SpellCircle Circle { get { return SpellCircle.Fourth; } }

        public ArchProtectionSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                List<Mobile> targets = new List<Mobile>();

                Map map = Caster.Map;

                if (map != null)
                {
                    IPooledEnumerable eable = map.GetMobilesInRange(new Point3D(p), Core.AOS ? 2 : 3);

                    foreach (Mobile m in eable)
                    {
                        if (isFriendly(Caster, m))
                            targets.Add(m);
                    }

                    eable.Free();
                }

                Party party = Party.Get(Caster);

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = targets[i];

                    if (m == Caster || (party != null && party.Contains(m)))
                    {
                        Caster.DoBeneficial(m);
                        Spells.Second.ProtectionSpell.Toggle(Caster, m);
                    }
                }
            }

            FinishSequence();
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Owner;
            private int m_Val;

            public InternalTimer(Mobile target, Mobile caster, int val) : base(TimeSpan.FromSeconds(0))
            {
                double time = Spell.ItemSkillValue(caster, SkillName.Magery, false) * 1.2;
                if (time > 144)
                    time = 144;
                Delay = TimeSpan.FromSeconds(time);
                Priority = TimerPriority.OneSecond;

                m_Owner = target;
                m_Val = val;
            }

            protected override void OnTick()
            {
                m_Owner.EndAction(typeof(ArchProtectionSpell));
                m_Owner.VirtualArmorMod -= m_Val;
                if (m_Owner.VirtualArmorMod < 0)
                    m_Owner.VirtualArmorMod = 0;
            }
        }

        private class InternalTarget : Target
        {
            private ArchProtectionSpell m_Owner;

            public InternalTarget(ArchProtectionSpell owner) : base(Core.ML ? 10 : 12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                IPoint3D p = o as IPoint3D;

                if (p != null)
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
