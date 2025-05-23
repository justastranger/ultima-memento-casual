using System;using Server;using Server.Network;using System.Text;using Server.Misc;using Server.Regions;using Server.Mobiles;using System.Collections;using System.Collections.Generic;using Server.Accounting;namespace Server.Items{
    public class HenchmanFighterItem : HenchmanItem
    {
        [Constructable]
        public HenchmanFighterItem()
        {
            ItemID = 0x1419;

            if (HenchGearColor > 0) { Hue = HenchGearColor; }
            else { Hue = Utility.RandomList(0, 0x973, 0x966, 0x96D, 0x972, 0x8A5, 0x979, 0x89F, 0x8AB, 0x492, 0x5B4, 0x48F, 0xB93, 0xB92, 0x497, 0x4AC, 0x5B5, 0x5B6, 0x48B, 0x48E); HenchGearColor = Hue; }

            if (HenchArmorType > 0) { } else { HenchArmorType = Utility.RandomMinMax(1, 3); }
            if (HenchWeaponType > 0) { } else { HenchWeaponType = Utility.RandomMinMax(1, 3); }

            HenchCloak = Utility.RandomMinMax(1, 2);
            HenchCloakColor = HenchmanFunctions.GetHue(Utility.Random(19));

            if (HenchWeaponID > 0) { }
            else
            {
                if (HenchWeaponType != 1) // SWORDS
                {
                    switch (Utility.Random(8))
                    {
                        case 0: HenchWeaponID = 0x1441; break;
                        case 1: HenchWeaponID = 0x13FF; break;
                        case 2: HenchWeaponID = 0x1401; break;
                        case 3: HenchWeaponID = 0xF61; break;
                        case 4: HenchWeaponID = 0x13B6; break;
                        case 5: HenchWeaponID = 0x13B8; break;
                        case 6: HenchWeaponID = 0x13B9; break;
                        case 7: HenchWeaponID = 0xF5E; break;
                    }
                }
                else // MACES
                {
                    switch (Utility.Random(4))
                    {
                        case 0: HenchWeaponID = 0x1407; break;
                        case 1: HenchWeaponID = 0x143D; break;
                        case 2: HenchWeaponID = 0xF5C; break;
                        case 3: HenchWeaponID = 0x143B; break;
                    }
                }
            }
            if (HenchShieldID > 0) { }
            else
            {
                switch (Utility.Random(14))
                {
                    case 0: HenchShieldID = 0x1BC3; break;
                    case 1: HenchShieldID = 0x1B73; break;
                    case 2: HenchShieldID = 0x1B72; break;
                    case 3: HenchShieldID = 0x1B7A; break;
                    case 4: HenchShieldID = 0x1B79; break;
                    case 5: HenchShieldID = 0x1BC4; break;
                    case 6: HenchShieldID = 0x1B7B; break;
                    case 7: HenchShieldID = 0x1B74; break;
                    case 8: HenchShieldID = 0x1B76; break;
                    case 9: HenchShieldID = 0x2FCB; break;
                    case 10: HenchShieldID = 0x2FCA; break;
                    case 11: HenchShieldID = 0x2FC9; break;
                    case 12: HenchShieldID = 0x2B74; break;
                    case 13: HenchShieldID = 0x2B75; break;
                }
            }
            if (HenchHelmID > 0) { }
            else
            {
                switch (Utility.Random(5))
                {
                    case 0: HenchHelmID = 0x1412; break;
                    case 1: HenchHelmID = 0x140A; break;
                    case 2: HenchHelmID = 0x140C; break;
                    case 3: HenchHelmID = 0x1408; break;
                    case 4: HenchHelmID = 0; break;
                }
            }

            Name = "fighter henchman";
        }

        public HenchmanFighterItem(Serial serial) : base(serial)
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