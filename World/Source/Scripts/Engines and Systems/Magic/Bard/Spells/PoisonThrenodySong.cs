using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Misc;

namespace Server.Spells.Song
{
    public class PoisonThrenodySong : Song
    {

        private static SpellInfo m_Info = new SpellInfo(
                "Poison Threnody", "*plays a poison threnody*",
                -1
            );

        public PoisonThrenodySong(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(5); } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 25; } }

        public override void OnCast()
        {
            base.OnCast();

            Caster.Target = new InternalTarget(this);
        }

        public virtual bool CheckSlayer(BaseInstrument instrument, Mobile defender)
        {
            SlayerEntry atkSlayer = SlayerGroup.GetEntryByName(instrument.Slayer);
            SlayerEntry atkSlayer2 = SlayerGroup.GetEntryByName(instrument.Slayer2);

            if (atkSlayer != null && atkSlayer.Slays(defender) || atkSlayer2 != null && atkSlayer2.Slays(defender))
                return true;

            return false;
        }

        public void Target(Mobile m)
        {
            Spellbook book = Spellbook.Find(Caster, -1, SpellbookType.Song);
            if (book == null)
                return;

            m_Book = (SongBook)book;

            bool sings = false;

            PlayerMobile p = m as PlayerMobile;

            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckHSequence(m))
            {
                sings = true;

                Mobile source = Caster;
                SpellHelper.Turn(source, m);

                m.FixedParticles(0x374A, 10, 30, 5013, 0x238, 2, EffectLayer.Waist);

                bool IsSlayer = false;
                if (m is BaseCreature) { IsSlayer = CheckSlayer(m_Book.Instrument, m); }

                int amount = (int)(MusicSkill(Caster) / 16);
                TimeSpan duration = TimeSpan.FromSeconds((double)(MusicSkill(Caster)));

                if (IsSlayer == true)
                {
                    amount = amount * 2;
                    duration = TimeSpan.FromSeconds((double)(MusicSkill(Caster) * 2));
                }

                m.SendMessage("Your resistance to poison has decreased.");
                ResistanceMod mod1 = new ResistanceMod(ResistanceType.Poison, -amount);

                m.AddResistanceMod(mod1);

                ExpireTimer timer1 = new ExpireTimer(m, mod1, duration);
                timer1.Start();

                string args = String.Format("{0}", amount);
                BuffInfo.RemoveBuff(m, BuffIcon.PoisonThrenody);
                BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.PoisonThrenody, 1063583, 1063584, duration, m, args.ToString(), true));
            }

            BardFunctions.UseBardInstrument(m_Book.Instrument, sings, Caster);
            FinishSequence();
        }

        private class ExpireTimer : Timer
        {
            private Mobile m_Mobile;
            private ResistanceMod m_Mods;

            public ExpireTimer(Mobile m, ResistanceMod mod, TimeSpan delay) : base(delay)
            {
                m_Mobile = m;
                m_Mods = mod;
            }

            public void DoExpire()
            {
                PlayerMobile p = m_Mobile as PlayerMobile;
                m_Mobile.RemoveResistanceMod(m_Mods);

                Stop();
            }

            protected override void OnTick()
            {
                if (m_Mobile != null)
                {
                    m_Mobile.SendMessage("The effect of the poison threnody wears off.");
                    DoExpire();
                }
            }
        }

        private class InternalTarget : Target
        {
            private PoisonThrenodySong m_Owner;

            public InternalTarget(PoisonThrenodySong owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
