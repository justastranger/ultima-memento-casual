using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;

namespace Server.Spells.Research
{
    public class ResearchGasCloud : ResearchSpell
    {
        public override int spellIndex { get { return 32; } }
        public int CirclePower = 5;
        public static int spellID = 32;
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(2.25); } }
        public override double RequiredSkill { get { return (double)(Int32.Parse(Server.Misc.Research.SpellInformation(spellIndex, 8))); } }
        public override int RequiredMana { get { return Int32.Parse(Server.Misc.Research.SpellInformation(spellIndex, 7)); } }

        private static SpellInfo m_Info = new SpellInfo(
                Server.Misc.Research.SpellInformation(spellID, 2),
                Server.Misc.Research.CapsCast(Server.Misc.Research.SpellInformation(spellID, 4)),
                260,
                9032,
                Reagent.Nightshade, Reagent.SilverWidow, Reagent.SwampBerries, Reagent.SilverSerpentVenom
            );

        public ResearchGasCloud(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            if (!base.CheckCast())
                return false;

            if ((Caster.Followers + 4) > Caster.FollowersMax)
            {
                Caster.SendLocalizedMessage(1049645); // You have too many followers to summon that creature.
                return false;
            }

            return true;
        }

        public override void OnCast()
        {
            Caster.SendMessage("Choose where you will unleash the gas cloud.");
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
                double time = DamagingSkill(Caster) / 1.2;
                if (time > 200) { time = 200.0; }
                if (time < 60) { time = 60.0; }

                TimeSpan duration = TimeSpan.FromSeconds(time);

                BaseCreature.Summon(new GasCloud(), false, Caster, new Point3D(p), 0x231, duration);
                Server.Misc.Research.ConsumeScroll(Caster, true, spellIndex, alwaysConsume, Scroll);

                Caster.SendMessage("You can double click the summoned to dispel them.");
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private ResearchGasCloud m_Owner;

            public InternalTarget(ResearchGasCloud owner) : base(Core.ML ? 10 : 12, true, TargetFlags.None)
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