using System;
using Server.Items;

namespace Server.Items
{
    public class LeatherNinjaMitts : BaseArmor
    {
        public override int BasePhysicalResistance { get { return 2; } }
        public override int BaseFireResistance { get { return 4; } }
        public override int BaseColdResistance { get { return 3; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 3; } }

        public override int InitMinHits { get { return 25; } }
        public override int InitMaxHits { get { return 25; } }

        public override int AosStrReq { get { return 10; } }
        public override int OldStrReq { get { return 10; } }

        public override int ArmorBase { get { return 3; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Leather; } }
        public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

        [Constructable]
        public LeatherNinjaMitts() : base(0x2792)
        {
            Weight = 2.0;
        }

        public LeatherNinjaMitts(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        if (reader.ReadBool())
                        {
                            reader.ReadInt();
                            reader.ReadInt();
                        }

                        Weight = 2.0;
                        ItemID = 0x2792;

                        break;
                    }
            }
        }
    }
}