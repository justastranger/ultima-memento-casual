using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a broken statue")]
    public class LivingBronzeStatue : BaseCreature
    {
        private bool m_Stunning;

        [Constructable]
        public LivingBronzeStatue() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a bronze statue";
            Body = 876;
            BaseSoundID = 268;
            Hue = 2968;
            Resource = CraftResource.Bronze;

            SetStr(156, 185);
            SetDex(76, 95);
            SetInt(31, 62);

            SetHits(106, 123);

            SetDamage(15, 22);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 35);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.MagicResist, 50.1, 95.0);
            SetSkill(SkillName.Tactics, 60.1, 100.0);
            SetSkill(SkillName.FistFighting, 60.1, 100.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 42;
        }

        public override int GetAttackSound() { return 0x626; }  // A
        public override int GetDeathSound() { return 0x627; }   // D
        public override int GetHurtSound() { return 0x628; }        // H

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems);
        }

        public override bool BleedImmune { get { return true; } }
        public override bool BardImmune { get { return !Core.SE; } }
        public override bool Unprovokable { get { return Core.SE; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override bool IsScaredOfScaryThings { get { return false; } }
        public override bool IsScaryToPets { get { return true; } }
        public override int Granite { get { return Utility.RandomMinMax(5, 10); } }
        public override GraniteType GraniteType { get { return ResourceGranite(); } }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (!Server.Items.HiddenTrap.IAmAWeaponSlayer(defender, this))
            {
                if (!m_Stunning && 0.3 > Utility.RandomDouble())
                {
                    m_Stunning = true;

                    defender.Animate(21, 6, 1, true, false, 0);
                    this.PlaySound(0xEE);
                    defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "You have been stunned by a colossal blow!");

                    BaseWeapon weapon = this.Weapon as BaseWeapon;
                    if (weapon != null)
                        weapon.OnHit(this, defender);

                    if (defender.Alive)
                    {
                        defender.Frozen = true;
                        Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerStateCallback(Recover_Callback), defender);
                    }
                }
            }
        }

        private void Recover_Callback(object state)
        {
            Mobile defender = state as Mobile;

            if (defender != null)
            {
                defender.Frozen = false;
                defender.Combatant = null;
                defender.LocalOverheadMessage(MessageType.Regular, 0x3B2, false, "You recover your senses.");
            }

            m_Stunning = false;
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (Controlled || Summoned)
            {
                Mobile master = (this.ControlMaster);

                if (master == null)
                    master = this.SummonMaster;

                if (!Server.Items.HiddenTrap.IAmAWeaponSlayer(from, this) && master != null && master.Player && master.Map == this.Map && master.InRange(Location, 20))
                {
                    if (master.Mana >= amount)
                    {
                        master.Mana -= amount;
                    }
                    else
                    {
                        amount -= master.Mana;
                        master.Mana = 0;
                        master.Damage(amount);
                    }
                }
            }

            base.OnDamage(amount, from, willKill);
        }

        public override void CheckReflect(Mobile caster, ref bool reflect)
        {
            if (Utility.RandomMinMax(1, 2) == 1) { reflect = true; } // 50% spells are reflected back to the caster
            else { reflect = false; }
        }

        public LivingBronzeStatue(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}