using System;
using Server.Items;

namespace Server.Items
{
    public class ScalyChest : BaseArmor
    {
        public override int BasePhysicalResistance { get { return 2; } }
        public override int BaseFireResistance { get { return 2; } }
        public override int BaseColdResistance { get { return 2; } }
        public override int BasePoisonResistance { get { return 2; } }
        public override int BaseEnergyResistance { get { return 2; } }

        public override int InitMinHits { get { return 40; } }
        public override int InitMaxHits { get { return 55; } }

        public override int AosStrReq { get { return 40; } }
        public override int OldStrReq { get { return 20; } }

        public override int OldDexBonus { get { return -2; } }

        public override int ArmorBase { get { return 22; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Scaled; } }
        public override CraftResource DefaultResource { get { return CraftResource.RedScales; } }

        [Constructable]
        public ScalyChest() : base(0x6617)
        {
            Weight = 8.0;
            Name = "scaly tunic";
        }

        public ScalyChest(Serial serial) : base(serial)
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
                Weight = 15.0;
        }
    }
}