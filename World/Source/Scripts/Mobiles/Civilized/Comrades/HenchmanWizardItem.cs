using System;using Server;using Server.Network;using System.Text;using Server.Misc;using Server.Regions;using Server.Mobiles;using System.Collections;using System.Collections.Generic;using Server.Accounting;namespace Server.Items{
    public class HenchmanWizardItem : HenchmanItem
    {
        [Constructable]
        public HenchmanWizardItem()
        {
            ItemID = 0xE2D;

            if (HenchGearColor > 0) { Hue = HenchGearColor; }
            else
            {
                int color = Utility.Random(19);
                HenchGearColor = HenchmanFunctions.GetHue(color);
                Hue = HenchGearColor;
                HenchGloves = Utility.RandomMinMax(1, 2);
                HenchCloakColor = HenchmanFunctions.GetHue(color);
                HenchCloak = Utility.RandomMinMax(1, 2);
                HenchRobe = Utility.RandomMinMax(1, 2);
                if (Utility.Random(2) == 1) { HenchHatColor = HenchGearColor; } else { HenchHatColor = HenchCloakColor; }
            }

            if (HenchWeaponID > 0) { }
            else
            {
                switch (Utility.Random(3))
                {
                    case 0: HenchWeaponID = 0xE89; break;
                    case 1: HenchWeaponID = 0x13F8; break;
                    case 2: HenchWeaponID = 0xDF0; break;
                }
            }
            if (HenchHelmID > 0) { }
            else
            {
                switch (Utility.Random(10))
                {
                    case 0: HenchHelmID = 5914; break;
                    case 1: HenchHelmID = 5911; break;
                    case 2: HenchHelmID = 5910; break;
                    case 3: HenchHelmID = 5908; break;
                    case 4: HenchHelmID = 5912; break;
                    case 5: HenchHelmID = 5907; break;
                    case 6: HenchHelmID = 5444; break;
                    case 7: HenchHelmID = 0x1540; break;
                    case 8: HenchHelmID = 0; break;
                    case 9: HenchHelmID = 0x2B6E; break;
                }
            }

            Name = "wizard henchman";
        }

        public HenchmanWizardItem(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }}