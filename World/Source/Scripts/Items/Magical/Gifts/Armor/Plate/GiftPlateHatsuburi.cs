using System;
using Server.Items;

namespace Server.Items
{
    public class GiftPlateHatsuburi : BaseGiftArmor
    {
        public override int BasePhysicalResistance { get { return 5; } }
        public override int BaseFireResistance { get { return 3; } }
        public override int BaseColdResistance { get { return 2; } }
        public override int BasePoisonResistance { get { return 2; } }
        public override int BaseEnergyResistance { get { return 3; } }

        public override int InitMinHits { get { return 55; } }
        public override int InitMaxHits { get { return 75; } }

        public override int AosStrReq { get { return 65; } }
        public override int OldStrReq { get { return 65; } }

        public override int ArmorBase { get { return 4; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

        [Constructable]
        public GiftPlateHatsuburi() : base(0x2775)
        {
            Weight = 5.0;
        }

        public GiftPlateHatsuburi(Serial serial) : base(serial)
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