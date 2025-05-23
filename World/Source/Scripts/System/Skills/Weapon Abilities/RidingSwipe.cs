
using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    /// <summary>
    /// If you are on foot, dismounts your opponent and damage the ethereal's rider or the
    /// living mount(which must be healed before ridden again). If you are mounted, damages
    /// and stuns the mounted opponent.
    /// </summary>
    public class RidingSwipe : WeaponAbility
    {
        public RidingSwipe()
        {
        }

        public override int BaseMana { get { return 30; } }

        public override bool RequiresSE { get { return true; } }

        public override bool CheckSkills(Mobile from)
        {
            return base.CheckSkills(from);
        }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            if (!defender.Mounted)
            {
                attacker.SendLocalizedMessage(1060848); // This attack only works on mounted targets
                ClearCurrentAbility(attacker);
                return;
            }

            if (!Validate(attacker) || !CheckMana(attacker, true))
                return;

            ClearCurrentAbility(attacker);

            if (!attacker.Mounted)
            {
                Mobile mount = defender.Mount as Mobile;
                BaseMount.Dismount(defender);

                if (mount != null)  //Ethy mounts don't take damage
                {
                    int amount = 10 + (int)(10.0 * (attacker.Skills[SkillName.Tactics].Value - 50.0) / 70.0 + 5);

                    AOS.Damage(mount, null, amount, 100, 0, 0, 0, 0);   //The mount just takes damage, there's no flagging as if it was attacking the mount directly

                    //TODO: Mount prevention until mount healed
                }
            }
            else
            {
                int amount = 10 + (int)(10.0 * (attacker.Skills[SkillName.Tactics].Value - 50.0) / 70.0 + 5);

                AOS.Damage(defender, attacker, amount, 100, 0, 0, 0, 0);

                if (Server.Items.ParalyzingBlow.IsImmune(defender)) //Does it still do damage?
                {
                    attacker.SendLocalizedMessage(1070804); // Your target resists paralysis.
                    defender.SendLocalizedMessage(1070813); // You resist paralysis.
                }
                else
                {
                    defender.Paralyze(TimeSpan.FromSeconds(3.0));
                    Server.Items.ParalyzingBlow.BeginImmunity(defender, Server.Items.ParalyzingBlow.FreezeDelayDuration);
                }
            }
        }
    }
}