using System;
using Server.Items;
using Server.Engines.Plants;

namespace Server.Mobiles
{
    [CorpseName("a dead plant")]
    public class StrangleVine : BaseCreature
    {
        [Constructable]
        public StrangleVine() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a strangle vine";
            Body = 8;
            Hue = 0xB88;
            BaseSoundID = 352;

            SetStr(151, 200);
            SetDex(76, 100);
            SetInt(26, 40);

            SetMana(0);

            SetDamage(7, 25);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.MagicResist, 70.0);
            SetSkill(SkillName.Tactics, 70.0);
            SetSkill(SkillName.FistFighting, 70.0);

            Fame = 700;
            Karma = -700;

            VirtualArmor = 35;

            PackReg(3);
            PackItem(new FertileDirt(Utility.RandomMinMax(1, 10)));

            PackItem(new Vines());

            if (Utility.Random(100) > 60)
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

        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override bool BleedImmune { get { return true; } }

        public StrangleVine(Serial serial) : base(serial)
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