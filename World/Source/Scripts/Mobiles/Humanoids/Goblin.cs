using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("a goblin corpse")]
    public class Goblin : BaseCreature
    {
        [Constructable]
        public Goblin() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a goblin";
            Body = 632;

            if (Utility.Random(20) == 0)
            {
                Body = 381;

                Item weapon = new Spear();
                weapon.ItemID = 0x2691;
                weapon.Name = "goblin spear";
                PackItem(weapon);

                Item helm = new OrcHelm();
                helm.Name = "goblin helmet";
                PackItem(helm);
            }

            BaseSoundID = 422;

            SetStr(25, 36);
            SetDex(25, 36);
            SetInt(10, 20);

            SetHits(25, 38);

            SetDamage(1, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 25, 30);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 50.1, 75.0);
            SetSkill(SkillName.Tactics, 55.1, 80.0);
            SetSkill(SkillName.FistFighting, 50.1, 70.0);

            Fame = 300;
            Karma = -300;

            VirtualArmor = 8;
        }

        public override bool OnBeforeDeath()
        {
            if (Body == 381)
            {
                Hits = HitsMax;
                Body = 632;

                BaseCreature wolf = new GreyWolf();
                wolf.Name = "a worg";
                wolf.IsTempEnemy = true;
                wolf.Karma = 0 - wolf.Fame;
                wolf.Tamable = false;
                wolf.AI = AIType.AI_Melee;
                wolf.FightMode = FightMode.Closest;
                wolf.MoveToWorld(Location, Map);
                wolf.Combatant = Combatant;
                return false;
            }

            return base.OnBeforeDeath();
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override int Meat { get { return 1; } }
        public override int GetAttackSound() { return 0x5FD; }  // A
        public override int GetDeathSound() { return 0x5FE; }   // D
        public override int GetHurtSound() { return 0x5FF; }        // H

        public Goblin(Serial serial) : base(serial)
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