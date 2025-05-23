using System;
using Server.Items;

namespace Server.Items
{
    public class GiftChainLegs : BaseGiftArmor
    {
        public override int BasePhysicalResistance { get { return 4; } }
        public override int BaseFireResistance { get { return 4; } }
        public override int BaseColdResistance { get { return 4; } }
        public override int BasePoisonResistance { get { return 1; } }
        public override int BaseEnergyResistance { get { return 2; } }

        public override int InitMinHits { get { return 45; } }
        public override int InitMaxHits { get { return 60; } }

        public override int AosStrReq { get { return 60; } }
        public override int OldStrReq { get { return 20; } }

        public override int OldDexBonus { get { return -3; } }

        public override int ArmorBase { get { return 28; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Chainmail; } }

        [Constructable]
        public GiftChainLegs() : base(0x13BE)
        {
            Weight = 7.0;
            Name = "chainmail leggings";
            ItemID = Utility.RandomList(0x13be, 0x13c3, 0x2656, 0x2659);
        }

        public GiftChainLegs(Serial serial) : base(serial)
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