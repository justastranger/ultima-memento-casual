using System;
using Server.Items;

namespace Server.Items
{
    public class DrakboneGreaves : BaseArmor
    {
        public override int BasePhysicalResistance { get { return 5; } }
        public override int BaseFireResistance { get { return 3; } }
        public override int BaseColdResistance { get { return 3; } }
        public override int BasePoisonResistance { get { return 2; } }
        public override int BaseEnergyResistance { get { return 2; } }

        public override int InitMinHits { get { return 55; } }
        public override int InitMaxHits { get { return 75; } }

        public override int AosStrReq { get { return 55; } }
        public override int OldStrReq { get { return 40; } }

        public override int OldDexBonus { get { return -4; } }

        public override int ArmorBase { get { return 30; } }
        public override int RevertArmorBase { get { return 7; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Scaled; } }
        public override CraftResource DefaultResource { get { return CraftResource.RedScales; } }

        [Constructable]
        public DrakboneGreaves() : base(0x49C2)
        {
            Name = "drakbone greaves";
            Weight = 3.0;
        }

        public DrakboneGreaves(Serial serial) : base(serial)
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