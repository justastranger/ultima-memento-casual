using System;
using Server;

namespace Server.Items
{
    public class ScaledShield : BaseShield
    {
        public override int BasePhysicalResistance { get { return 1; } }
        public override int BaseFireResistance { get { return 1; } }
        public override int BaseColdResistance { get { return 0; } }
        public override int BasePoisonResistance { get { return 0; } }
        public override int BaseEnergyResistance { get { return 1; } }

        public override int InitMinHits { get { return 55; } }
        public override int InitMaxHits { get { return 75; } }

        public override int AosStrReq { get { return 90; } }

        public override int ArmorBase { get { return 23; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Scaled; } }
        public override CraftResource DefaultResource { get { return CraftResource.RedScales; } }

        [Constructable]
        public ScaledShield() : base(0x6619)
        {
            Weight = 8.0;
            Name = "scaled shield";
        }

        public ScaledShield(Serial serial) : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);//version
        }
    }
}
