using System;
using Server;

namespace Server.Items
{
    public class SavageHelm : BaseArmor
    {
        public override int BasePhysicalResistance { get { return 5; } }
        public override int BaseFireResistance { get { return 5; } }
        public override int BaseColdResistance { get { return 6; } }
        public override int BasePoisonResistance { get { return 4; } }
        public override int BaseEnergyResistance { get { return 6; } }

        public override int InitMinHits { get { return 35; } }
        public override int InitMaxHits { get { return 50; } }

        public override int AosStrReq { get { return 20; } }
        public override int OldStrReq { get { return 40; } }

        public override int ArmorBase { get { return 40; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Bone; } }
        public override CraftResource DefaultResource { get { return CraftResource.BrittleSkeletal; } }

        [Constructable]
        public SavageHelm() : base(0x49C1)
        {
            Name = "skeletal helm";
            Weight = 3.0;
        }

        public SavageHelm(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version < 1)
                Resource = CraftResource.BrittleSkeletal;
        }
    }
}