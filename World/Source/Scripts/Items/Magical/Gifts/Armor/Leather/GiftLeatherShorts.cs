using System;
using Server.Items;

namespace Server.Items
{
    [FlipableAttribute(0x1c00, 0x1c01)]
    public class GiftLeatherShorts : BaseGiftArmor
    {
        public override int BasePhysicalResistance { get { return 2; } }
        public override int BaseFireResistance { get { return 4; } }
        public override int BaseColdResistance { get { return 3; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 3; } }

        public override int InitMinHits { get { return 30; } }
        public override int InitMaxHits { get { return 40; } }

        public override int AosStrReq { get { return 20; } }
        public override int OldStrReq { get { return 10; } }

        public override int ArmorBase { get { return 13; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Leather; } }
        public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

        public override bool AllowMaleWearer { get { return false; } }

        [Constructable]
        public GiftLeatherShorts() : base(0x1C00)
        {
            Weight = 3.0;
        }

        public GiftLeatherShorts(Serial serial) : base(serial)
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