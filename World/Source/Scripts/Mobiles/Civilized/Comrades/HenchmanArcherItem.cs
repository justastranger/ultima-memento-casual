using System;using Server;using Server.Network;using System.Text;using Server.Misc;using Server.Regions;using Server.Mobiles;using System.Collections;using System.Collections.Generic;using Server.Accounting;namespace Server.Items{
    public class HenchmanArcherItem : HenchmanItem
    {
        [Constructable]
        public HenchmanArcherItem() : base()
        {
            ItemID = 0xF50;

            if (HenchGearColor > 0) { Hue = HenchGearColor; }
            else { Hue = Utility.RandomList(0, 0x8AC, 0x845, 0x851, 0x47E, 0x4AA, 0xB85, 0x497, 0x89F, 0x483); HenchGearColor = Hue; }

            HenchCloak = Utility.RandomMinMax(1, 2);
            HenchCloakColor = HenchmanFunctions.GetHue(Utility.Random(19));

            if (HenchArmorType > 0) { } else { HenchArmorType = Utility.RandomMinMax(1, 2); }
            if (HenchWeaponType > 0) { } else { HenchWeaponType = Utility.RandomMinMax(1, 2); }

            if (HenchWeaponID > 0) { }
            else
            {
                if (HenchWeaponType != 1) // BOW
                {
                    switch (Utility.Random(4))
                    {
                        case 0: HenchWeaponID = 0x13B2; break;
                        case 1: HenchWeaponID = 0x2D2B; break;
                        case 2: HenchWeaponID = 0x26C2; break;
                        case 3: HenchWeaponID = 0x2D1E; break;
                    }
                }
                else // CROSSBOW
                {
                    switch (Utility.Random(3))
                    {
                        case 0: HenchWeaponID = 0x26C3; break;
                        case 1: HenchWeaponID = 0xF50; break;
                        case 2: HenchWeaponID = 0x13FD; break;
                    }
                }
            }
            if (HenchHelmID > 0) { }
            else
            {
                switch (Utility.Random(5))
                {
                    case 0: HenchHelmID = 0x2B6E; break;
                    case 1: HenchHelmID = 0x13BB; break;
                    case 2: HenchHelmID = 0x1DB9; break;
                    case 3: HenchHelmID = 0x1DB9; break;
                    case 4: HenchHelmID = 0; break;
                }
            }

            Name = "archer henchman";
        }

        public HenchmanArcherItem(Serial serial) : base(serial)
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