using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Engines.Plants;

namespace Server.Mobiles
{
    [CorpseName("a fallen tree")]
    public class EvilEnt : BaseCreature
    {
        public override int BreathPhysicalDamage { get { return 100; } }
        public override int BreathFireDamage { get { return 0; } }
        public override int BreathColdDamage { get { return 0; } }
        public override int BreathPoisonDamage { get { return 0; } }
        public override int BreathEnergyDamage { get { return 0; } }
        public override int BreathEffectHue { get { return 0; } }
        public override int BreathEffectSound { get { return 0x65A; } }
        public override int BreathEffectItemID { get { return 0x707; } } // LARGE LOG
        public override bool ReacquireOnMovement { get { return !Controlled; } }
        public override bool HasBreath { get { return true; } }
        public override double BreathEffectDelay { get { return 0.1; } }
        public override void BreathDealDamage(Mobile target, int form) { base.BreathDealDamage(target, 7); }

        [Constructable]
        public EvilEnt() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = NameList.RandomName("trees");
            Title = "the rotted ent";
            Body = 313;
            BaseSoundID = 442;

            SetStr(736, 785);
            SetDex(126, 145);
            SetInt(481, 505);

            SetHits(622, 651);

            SetDamage(19, 25);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 25, 35);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Psychology, 85.1, 100.0);
            SetSkill(SkillName.Magery, 105.1, 120.0);
            SetSkill(SkillName.MagicResist, 90.2, 110.0);
            SetSkill(SkillName.Tactics, 60.1, 80.0);
            SetSkill(SkillName.FistFighting, 60.1, 80.0);

            Fame = 17000;
            Karma = -17000;

            VirtualArmor = 50;

            switch (Utility.Random(14))
            {
                case 0: Resource = CraftResource.None; break;
                case 1: Resource = CraftResource.AshTree; break;
                case 2: Resource = CraftResource.CherryTree; break;
                case 3: Resource = CraftResource.EbonyTree; break;
                case 4: Resource = CraftResource.GoldenOakTree; break;
                case 5: Resource = CraftResource.HickoryTree; break;
                case 6: Resource = CraftResource.MahoganyTree; break;
                case 7: Resource = CraftResource.OakTree; break;
                case 8: Resource = CraftResource.PineTree; break;
                case 9: Resource = CraftResource.RosewoodTree; break;
                case 10: Resource = CraftResource.WalnutTree; break;
                case 11: Resource = CraftResource.PetrifiedTree; break;
                case 12: Resource = CraftResource.DriftwoodTree; break;
                case 13: Resource = CraftResource.ElvenTree; break;
            }

            if (Utility.Random(100) > 40)
            {
                int seed_to_give = Utility.Random(100);

                if (seed_to_give > 90)
                {
                    PlantType type;
                    switch (Utility.Random(17))
                    {
                        case 0: type = PlantType.CampionFlowers; break;
                        case 1: type = PlantType.Poppies; break;
                        case 2: type = PlantType.Snowdrops; break;
                        case 3: type = PlantType.Bulrushes; break;
                        case 4: type = PlantType.Lilies; break;
                        case 5: type = PlantType.PampasGrass; break;
                        case 6: type = PlantType.Rushes; break;
                        case 7: type = PlantType.ElephantEarPlant; break;
                        case 8: type = PlantType.Fern; break;
                        case 9: type = PlantType.PonytailPalm; break;
                        case 10: type = PlantType.SmallPalm; break;
                        case 11: type = PlantType.CenturyPlant; break;
                        case 12: type = PlantType.WaterPlant; break;
                        case 13: type = PlantType.SnakePlant; break;
                        case 14: type = PlantType.PricklyPearCactus; break;
                        case 15: type = PlantType.BarrelCactus; break;
                        default: type = PlantType.TribarrelCactus; break;
                    }
                    PlantHue hue;
                    switch (Utility.Random(4))
                    {
                        case 0: hue = PlantHue.Pink; break;
                        case 1: hue = PlantHue.Magenta; break;
                        case 2: hue = PlantHue.FireRed; break;
                        default: hue = PlantHue.Aqua; break;
                    }

                    PackItem(new Seed(type, hue, false));
                }
                else if (seed_to_give > 70)
                {
                    PackItem(Engines.Plants.Seed.RandomPeculiarSeed(Utility.RandomMinMax(1, 4)));
                }
                else if (seed_to_give > 40)
                {
                    PackItem(Engines.Plants.Seed.RandomBonsaiSeed());
                }
                else
                {
                    PackItem(new Engines.Plants.Seed());
                }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Average);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            if (1 == Utility.RandomMinMax(1, 30) && Resource != CraftResource.None)
            {
                TransmutationPotion loot = new TransmutationPotion();
                loot.Resource = Resource;
                loot.Amount = 1;
                c.DropItem(loot);
            }
        }

        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override bool BleedImmune { get { return true; } }
        public override int Wood { get { return Utility.RandomMinMax(10, 20); } }
        public override WoodType WoodType { get { return ResourceWood(); } }

        public override int GetAngerSound()
        {
            return 0x61E;
        }

        public override int GetDeathSound()
        {
            return 0x61F;
        }

        public override int GetHurtSound()
        {
            return 0x620;
        }

        public override int GetIdleSound()
        {
            return 0x621;
        }

        public EvilEnt(Serial serial) : base(serial)
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