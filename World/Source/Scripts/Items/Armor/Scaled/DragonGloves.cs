using System;
using Server.Items;

namespace Server.Items
{
    [FlipableAttribute(0x2643, 0x2644)]
    public class DragonGloves : BaseArmor
    {
        public override int BasePhysicalResistance { get { return 3; } }
        public override int BaseFireResistance { get { return 3; } }
        public override int BaseColdResistance { get { return 3; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 3; } }

        public override int InitMinHits { get { return 50; } }
        public override int InitMaxHits { get { return 65; } }

        public override int AosStrReq { get { return 75; } }
        public override int OldStrReq { get { return 30; } }

        public override int OldDexBonus { get { return -2; } }

        public override int ArmorBase { get { return 40; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Scaled; } }
        public override CraftResource DefaultResource { get { return CraftResource.RedScales; } }

        [Constructable]
        public DragonGloves() : base(0x2643)
        {
            Name = "scalemail gloves";
            Weight = 3.0;
        }

        public DragonGloves(Serial serial) : base(serial)
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

            if (Weight == 1.0)
                Weight = 2.0;
        }
    }
}