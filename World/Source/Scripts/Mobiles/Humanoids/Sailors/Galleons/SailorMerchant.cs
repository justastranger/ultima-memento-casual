using System;
using Server;
using Server.Misc;
using Server.Items;
using System.Collections;
using System.Collections.Generic;
using Server.Targeting;
using Server.Multis;

namespace Server.Mobiles
{
    public class SailorMerchant : BasePirate
    {
        [Constructable]
        public SailorMerchant()
        {
            Title = "the ship captain";
            Hue = Utility.RandomSkinColor();

            if (this.Female = Utility.RandomBool())
            {
                this.Body = 0x191;
                this.Name = NameList.RandomName("female");
                Utility.AssignRandomHair(this);
                HairHue = Utility.RandomHairHue();
            }
            else
            {
                this.Body = 0x190;
                this.Name = NameList.RandomName("male");
                Utility.AssignRandomHair(this);
                int HairColor = Utility.RandomHairHue();
                FacialHairItemID = Utility.RandomList(0, 8254, 8255, 8256, 8257, 8267, 8268, 8269);
                HairHue = HairColor;
                FacialHairHue = HairColor;
            }

            switch (Utility.RandomMinMax(1, 6))
            {
                case 1: AddItem(new Scimitar()); break;
                case 2: AddItem(new Cutlass()); break;
                case 3: AddItem(new Katana()); break;
                case 4: AddItem(new ShortSpear()); break;
                case 5: AddItem(new Longsword()); break;
                case 6: AddItem(new Kryss()); break;
            }

            DressSailor(this);

            AI = AIType.AI_Melee;
            FightMode = FightMode.Evil;
            ship = new GalleonLarge();
            ship.Hue = ShipColor("");

            SetStr(536, 585);
            SetDex(126, 145);
            SetInt(281, 305);

            SetHits(322, 351);
            SetMana(0);

            SetDamage(16, 23);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 45, 50);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 25, 35);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 60.3, 105.0);
            SetSkill(SkillName.Tactics, 80.1, 100.0);
            SetSkill(SkillName.FistFighting, 80.1, 90.0);
            SetSkill(SkillName.Swords, 80.1, 90.0);
            SetSkill(SkillName.Fencing, 80.1, 90.0);

            Fame = 11000;
            Karma = 11000;

            VirtualArmor = 48;
            healme = "Heal me mateys!";
        }

        public SailorMerchant(Serial serial) : base(serial)
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

        public override int BreathPhysicalDamage { get { return 0; } }
        public override int BreathFireDamage { get { if (YellHue < 2) { return 100; } else { return 0; } } }
        public override int BreathColdDamage { get { if (YellHue == 3) { return 100; } else { return 0; } } }
        public override int BreathPoisonDamage { get { if (YellHue == 2) { return 100; } else { return 0; } } }
        public override int BreathEnergyDamage { get { return 0; } }
        public override int BreathEffectHue { get { if (YellHue == 1) { return 0x488; } else if (YellHue == 2) { return 0xB92; } else if (YellHue == 3) { return 0x5B5; } else { return 0x4FD; } } }
        public override int BreathEffectSound { get { return 0x238; } }
        public override int BreathEffectItemID { get { return 0x1005; } } // EXPLOSION POTION
        public override bool HasBreath { get { return true; } }
        public override double BreathEffectDelay { get { return 0.1; } }
        public override void BreathDealDamage(Mobile target, int form) { base.BreathDealDamage(target, 2); }
        public override double BreathDamageScalar { get { return 0.4; } }
    }
}