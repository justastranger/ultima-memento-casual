using System;
using Server;

namespace Server.Items
{
    /// <summary>
    /// Strike your opponent with great force, partially bypassing their armor and inflicting greater damage.
    /// </summary>
    public class ArmorPierce : WeaponAbility
    {
        public ArmorPierce()
        {
        }

        public override bool CheckSkills(Mobile from)
        {
            return base.CheckSkills(from);
        }

        public override int BaseMana { get { return 20; } }
        public override double DamageScalar { get { return 1.25; } }

        public override bool RequiresSE { get { return true; } }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            if (!Validate(attacker) || !CheckMana(attacker, true))
                return;

            ClearCurrentAbility(attacker);

            attacker.SendLocalizedMessage(1063350); // You pierce your opponent's armor!
            defender.SendLocalizedMessage(1063351); // Your attacker pierced your armor!

            defender.FixedParticles(0x3728, 1, 26, 0x26D6, 0, 0, EffectLayer.Waist);
        }
    }
}