using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("an afreet corpse")]
    public class Afreet : BaseCreature
    {
        public override int BreathPhysicalDamage { get { return 50; } }
        public override int BreathFireDamage { get { return 50; } }
        public override int BreathColdDamage { get { return 0; } }
        public override int BreathPoisonDamage { get { return 0; } }
        public override int BreathEnergyDamage { get { return 0; } }
        public override int BreathEffectHue { get { return 0x96D; } }
        public override bool ReacquireOnMovement { get { return true; } }
        public override bool HasBreath { get { return true; } }
        public override int BreathEffectSound { get { return 0x654; } }
        public override double BreathEffectDelay { get { return 0.1; } }
        public override void BreathDealDamage(Mobile target, int form) { base.BreathDealDamage(target, 15); }

        [Constructable]
        public Afreet() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an afreet";

            Body = 248;
            BaseSoundID = 357;

            SetStr(476, 505);
            SetDex(266, 285);
            SetInt(171, 195);

            SetHits(286, 303);

            SetDamage(11, 13);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 85.1, 95.0);
            SetSkill(SkillName.Tactics, 70.1, 80.0);
            SetSkill(SkillName.FistFighting, 60.1, 80.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 58;
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (this.Body == 13 && willKill == false && Utility.Random(4) == 1)
            {
                this.Body = 248;
                this.BaseSoundID = 357;
            }
            else if (willKill == false && Utility.Random(4) == 1)
            {
                this.Body = 790;
                this.BaseSoundID = 768;
            }

            base.OnDamage(amount, from, willKill);
        }

        public override void OnDeath(Container c)
        {
            int color = this.Hue;

            base.OnDeath(c);

            if (0.02 > Utility.RandomDouble())
            {
                switch (Utility.RandomMinMax(1, 10))
                {
                    case 1:
                        Robe robe = new Robe();
                        robe.Name = "robe of the Afreet";
                        robe.ItemID = 0x1F03;
                        robe.Attributes.BonusMana = 5;
                        robe.Attributes.BonusInt = 4;
                        robe.Attributes.RegenMana = 1;
                        robe.Attributes.LowerRegCost = 10;
                        robe.SkillBonuses.SetValues(0, SkillName.MagicResist, 8);
                        robe.SkillBonuses.SetValues(1, SkillName.Magery, 8);
                        robe.Hue = color;
                        c.DropItem(robe);
                        break;
                    case 2:
                        Cap hat = new Cap();
                        hat.Name = "hat of the Afreet";
                        hat.ItemID = 0x1718;
                        hat.Attributes.BonusMana = 5;
                        hat.Attributes.BonusInt = 2;
                        hat.Attributes.RegenMana = 1;
                        hat.Attributes.LowerRegCost = 5;
                        hat.SkillBonuses.SetValues(0, SkillName.MagicResist, 8);
                        hat.SkillBonuses.SetValues(1, SkillName.Magery, 8);
                        hat.Hue = color;
                        c.DropItem(hat);
                        break;
                    case 3:
                        Boots boots = new Boots();
                        boots.Name = "boots of the Afreet";
                        boots.ItemID = 0x1711;
                        boots.Attributes.BonusMana = 5;
                        boots.Attributes.BonusInt = 2;
                        boots.Attributes.RegenMana = 1;
                        boots.Attributes.LowerRegCost = 5;
                        boots.SkillBonuses.SetValues(0, SkillName.MagicResist, 8);
                        boots.SkillBonuses.SetValues(1, SkillName.Magery, 8);
                        boots.Hue = color;
                        c.DropItem(boots);
                        break;
                    case 4:
                        Cloak cloak = new Cloak();
                        cloak.Name = "cloak of the Afreet";
                        cloak.ItemID = 0x1530;
                        cloak.Attributes.BonusMana = 5;
                        cloak.Attributes.BonusInt = 4;
                        cloak.Attributes.RegenMana = 1;
                        cloak.Attributes.LowerRegCost = 10;
                        cloak.SkillBonuses.SetValues(0, SkillName.MagicResist, 8);
                        cloak.SkillBonuses.SetValues(1, SkillName.Magery, 8);
                        cloak.Hue = color;
                        c.DropItem(cloak);
                        break;
                    case 5:
                        Belt belt = new Belt();
                        belt.Name = "belt of the Afreet";
                        belt.ItemID = 0x1530;
                        belt.Attributes.BonusMana = 5;
                        belt.Attributes.BonusInt = 2;
                        belt.Attributes.RegenMana = 1;
                        belt.Attributes.LowerRegCost = 5;
                        belt.SkillBonuses.SetValues(0, SkillName.MagicResist, 8);
                        belt.SkillBonuses.SetValues(1, SkillName.Magery, 8);
                        belt.Hue = color;
                        c.DropItem(belt);
                        break;
                    case 6:
                        TrinketCandle candle = new TrinketCandle();
                        candle.Name = "candle of the Afreet";
                        candle.ItemID = 0x1530;
                        candle.Attributes.BonusMana = 5;
                        candle.Attributes.BonusInt = 4;
                        candle.Attributes.RegenMana = 1;
                        candle.Attributes.LowerRegCost = 10;
                        candle.SkillBonuses.SetValues(0, SkillName.MagicResist, 8);
                        candle.SkillBonuses.SetValues(1, SkillName.Magery, 8);
                        candle.Hue = color;
                        c.DropItem(candle);
                        break;
                }
            }
        }

        public override bool OnBeforeDeath()
        {
            this.Body = 790;
            this.BaseSoundID = 768;
            return base.OnBeforeDeath();
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average, 2);
            AddLoot(LootPack.Gems);
        }

        public override int TreasureMapLevel { get { return 4; } }
        public override int Hides { get { return 18; } }
        public override HideType HideType { get { return HideType.Hellish; } }

        public override void CheckReflect(Mobile caster, ref bool reflect)
        {
            reflect = true; // Every spell is reflected back to the caster
        }

        public Afreet(Serial serial) : base(serial)
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
