using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Ninjitsu
{
    public class FocusAttack : NinjaMove
    {
        public FocusAttack()
        {
        }

        public override int BaseMana { get { return 20; } }
        public override double RequiredSkill { get { return 60; } }

        public override TextDefinition AbilityMessage { get { return new TextDefinition(1063095); } } // You prepare to focus all of your abilities into your next strike.

        public override bool Validate(Mobile from)
        {
            if (from.FindItemOnLayer(Layer.TwoHanded) as BaseShield != null)
            {
                from.SendLocalizedMessage(1063096); // You cannot use this ability while holding a shield.
                return false;
            }

            Item handOne = from.FindItemOnLayer(Layer.OneHanded) as BaseWeapon;

            if (handOne != null && !(handOne is BaseRanged))
                return base.Validate(from);

            Item handTwo = from.FindItemOnLayer(Layer.TwoHanded) as BaseWeapon;

            if (handTwo != null && !(handTwo is BaseRanged))
                return base.Validate(from);

            from.SendLocalizedMessage(1063097); // You must be wielding a melee weapon without a shield to use this ability.
            return false;
        }

        public override double GetDamageScalar(Mobile attacker, Mobile defender)
        {
            double ninjitsu = attacker.Skills[SkillName.Ninjitsu].Value;

            return 1.0 + (ninjitsu * ninjitsu) / 43636;
        }

        public override double GetPropertyBonus(Mobile attacker)
        {
            double ninjitsu = attacker.Skills[SkillName.Ninjitsu].Value;

            double bonus = (ninjitsu * ninjitsu) / 43636;

            return 1.0 + (bonus * 3 + 0.01);
        }

        public override bool OnBeforeDamage(Mobile attacker, Mobile defender)
        {
            return Validate(attacker) && CheckMana(attacker, true);
        }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            ClearCurrentMove(attacker);

            attacker.SendLocalizedMessage(1063098); // You focus all of your abilities and strike with deadly force!
            attacker.PlaySound(0x510);

            CheckGain(attacker);
        }
    }
}
