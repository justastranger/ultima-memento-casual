using System;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Spells.Research
{
    public class ResearchSummonDevil : ResearchSpell
    {
        public override int spellIndex { get { return 54; } }
        public int CirclePower = 8;
        public static int spellID = 54;
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(2.25); } }
        public override double RequiredSkill { get { return (double)(Int32.Parse(Server.Misc.Research.SpellInformation(spellIndex, 8))); } }
        public override int RequiredMana { get { return Int32.Parse(Server.Misc.Research.SpellInformation(spellIndex, 7)); } }

        private static SpellInfo m_Info = new SpellInfo(
                Server.Misc.Research.SpellInformation(spellID, 2),
                Server.Misc.Research.CapsCast(Server.Misc.Research.SpellInformation(spellID, 4)),
                269,
                9050,
                Reagent.DaemonBlood, Reagent.EyeOfToad, Reagent.Brimstone, Reagent.DemonClaw
            );

        public ResearchSummonDevil(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            if (!base.CheckCast())
                return false;

            if ((Caster.Followers + 5) > Caster.FollowersMax)
            {
                Caster.SendLocalizedMessage(1049645); // You have too many followers to summon that creature.
                return false;
            }

            return true;
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                double time = DamagingSkill(Caster) * 2;
                if (time > 480) { time = 480.0; }
                if (time < 120) { time = 120.0; }

                TimeSpan duration = TimeSpan.FromSeconds(time);

                BaseCreature m_Devil = new Devil();
                m_Devil.ControlSlots = 5;
                SpellHelper.Summon(m_Devil, Caster, 0x216, duration, false, false);
                m_Devil.FixedParticles(0x3728, 8, 20, 5042, Server.Misc.PlayerSettings.GetMySpellHue(true, Caster, 0), 0, EffectLayer.Head);
                Server.Misc.Research.ConsumeScroll(Caster, true, spellIndex, alwaysConsume, Scroll);

                KarmaMod(Caster, ((int)RequiredSkill + RequiredMana));
            }

            FinishSequence();
        }
    }
}