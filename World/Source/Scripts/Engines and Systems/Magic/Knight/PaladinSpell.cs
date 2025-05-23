using System;
using Server;
using Server.Spells;
using Server.Network;

namespace Server.Spells.Chivalry
{
    public abstract class PaladinSpell : Spell
    {
        public abstract double RequiredSkill { get; }
        public abstract int RequiredMana { get; }
        public abstract int RequiredTithing { get; }
        public abstract int MantraNumber { get; }

        public override SkillName CastSkill { get { return SkillName.Knightship; } }
        public override SkillName DamageSkill { get { return SkillName.Knightship; } }

        public override bool ClearHandsOnCast { get { return false; } }

        //public override int CastDelayBase{ get{ return 1; } }

        public override int CastRecoveryBase { get { return 7; } }

        public PaladinSpell(Mobile caster, Item scroll, SpellInfo info) : base(caster, scroll, info)
        {
        }

        public override bool CheckCast()
        {
            int cost = ScaleMana(RequiredMana);

            if (!base.CheckCast())
                return false;

            if (Caster.Stam < cost)
            {
                Caster.SendMessage("You are too fatigued to do that now.");
                return false;
            }
            else if (Caster.Karma < 0)
            {
                Caster.SendMessage("You do not have enough Karma to use this ability.");
                return false;
            }
            else if (Caster.TithingPoints < RequiredTithing)
            {
                Caster.SendLocalizedMessage(1060173, RequiredTithing.ToString()); // You must have at least ~1_TITHE_REQUIREMENT~ Tithing Points to use this ability,
                return false;
            }
            else if (Caster.Mana < cost)
            {
                Caster.SendLocalizedMessage(1060174, cost.ToString()); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
                return false;
            }

            return true;
        }

        public override bool CheckFizzle()
        {
            int requiredTithing = this.RequiredTithing;

            if (AosAttributes.GetValue(Caster, AosAttribute.LowerRegCost) > Utility.Random(100))
                requiredTithing = 0;

            int cost = ScaleMana(RequiredMana);

            if (Caster.Stam < cost)
            {
                Caster.SendMessage("You are too fatigued to do that now.");
                return false;
            }
            else if (Caster.Karma < 0)
            {
                Caster.SendMessage("You do not have enough Karma to use this ability.");
                return false;
            }
            else if (Caster.TithingPoints < requiredTithing)
            {
                Caster.SendLocalizedMessage(1060173, RequiredTithing.ToString()); // You must have at least ~1_TITHE_REQUIREMENT~ Tithing Points to use this ability,
                return false;
            }
            else if (Caster.Mana < cost)
            {
                Caster.SendLocalizedMessage(1060174, cost.ToString()); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
                return false;
            }

            Caster.TithingPoints -= requiredTithing;

            if (!base.CheckFizzle())
                return false;

            return true;
        }

        public override void SayMantra()
        {
            Caster.PublicOverheadMessage(MessageType.Regular, 0x3B2, MantraNumber, "", false);
        }

        public override void DoFizzle()
        {
            Caster.PlaySound(0x1D6);
            Caster.NextSpellTime = DateTime.Now;
        }

        public override void DoHurtFizzle()
        {
            Caster.PlaySound(0x1D6);
        }

        public override void OnDisturb(DisturbType type, bool message)
        {
            base.OnDisturb(type, message);

            if (message)
                Caster.PlaySound(0x1D6);
        }

        public override void OnBeginCast()
        {
            base.OnBeginCast();

            SendCastEffect();
        }

        public virtual void SendCastEffect()
        {
            Caster.FixedEffect(0x37C4, 10, (int)(GetCastDelay().TotalSeconds * 28), 4, 3);
        }

        public override void GetCastSkills(out double min, out double max)
        {
            min = RequiredSkill;
            max = RequiredSkill + 50.0;
        }

        public override int GetMana()
        {
            return RequiredMana;
        }

        public int ComputePowerValue(int div)
        {
            return ComputePowerValue(Caster, div);
        }

        public static int ComputePowerValue(Mobile from, int div)
        {
            if (from == null)
                return 0;

            int v = (int)Math.Sqrt(from.Karma + 20000 + (from.Skills.Knightship.Fixed * 10));

            return v / div;
        }
    }
}
