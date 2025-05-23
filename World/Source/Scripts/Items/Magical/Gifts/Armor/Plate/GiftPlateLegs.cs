using System;
using Server.Items;

namespace Server.Items
{
    public class GiftPlateLegs : BaseGiftArmor
    {
        public override int BasePhysicalResistance { get { return 5; } }
        public override int BaseFireResistance { get { return 3; } }
        public override int BaseColdResistance { get { return 2; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 2; } }

        public override int InitMinHits { get { return 50; } }
        public override int InitMaxHits { get { return 65; } }

        public override int AosStrReq { get { return 90; } }

        public override int OldStrReq { get { return 60; } }
        public override int OldDexBonus { get { return -6; } }

        public override int ArmorBase { get { return 40; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

        [Constructable]
        public GiftPlateLegs() : base(0x46AA)
        {
            Weight = 7.0;
            Name = "platemail legs";
            ItemID = Utility.RandomList(0x46AA, 0x46AB, 0x6396, 0x6397, 0x6398, 0x264D);
        }

        public GiftPlateLegs(Serial serial) : base(serial)
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