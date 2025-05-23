using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a wurm corpse")]
    public class SoulWorm : BaseCreature
    {
        [Constructable]
        public SoulWorm() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a grave wurm";
            Body = 955;
            BaseSoundID = 0x482;

            SetStr(122, 134);
            SetDex(56, 71);
            SetInt(116, 120);

            SetHits(109, 145);

            SetDamage(10, 19);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);
            SetResistance(ResistanceType.Poison, 10, 20);

            SetSkill(SkillName.MagicResist, 85.1, 90.0);
            SetSkill(SkillName.Tactics, 89.3, 94.0);
            SetSkill(SkillName.FistFighting, 89.3, 94.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 38;
        }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Regular; } }

        public void DrainLife()
        {
            ArrayList list = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(2))
            {
                if (m == this || !CanBeHarmful(m))
                    continue;

                if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team))
                    list.Add(m);
                else if (m.Player)
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                if (!m.CheckSkill(SkillName.MagicResist, 0, 70) && !Server.Items.HiddenTrap.IAmAWeaponSlayer(m, this))
                {
                    DoHarmful(m);

                    m.PlaySound(0x204);
                    m.FixedEffect(0x376A, 6, 1);
                    m.Paralyze(TimeSpan.FromSeconds(Math.Min(MySettings.S_paralyzeDuration, Utility.RandomMinMax(4, 8))));
                    m.SendMessage("You are hypnotized by the worm's gaze!");
                }
            }
        }

        public override void OnGaveMeleeAttack(Mobile m)
        {
            base.OnGaveMeleeAttack(m);

            if (0.1 >= Utility.RandomDouble())
                DrainLife();
        }

        public override void OnGotMeleeAttack(Mobile m)
        {
            base.OnGotMeleeAttack(m);

            if (0.1 >= Utility.RandomDouble())
                DrainLife();
        }

        public SoulWorm(Serial serial) : base(serial)
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