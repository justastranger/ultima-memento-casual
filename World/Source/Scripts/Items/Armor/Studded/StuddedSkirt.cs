using System;
using Server.Items;

namespace Server.Items
{
    [FlipableAttribute(0x1C08, 0x1C09)]
    public class StuddedSkirt : BaseArmor
    {
        public override int BasePhysicalResistance { get { return 2; } }
        public override int BaseFireResistance { get { return 4; } }
        public override int BaseColdResistance { get { return 3; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 4; } }

        public override int InitMinHits { get { return 35; } }
        public override int InitMaxHits { get { return 45; } }

        public override int AosStrReq { get { return 30; } }
        public override int OldStrReq { get { return 35; } }

        public override int ArmorBase { get { return 16; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Studded; } }
        public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }

        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.Half; } }

        [Constructable]
        public StuddedSkirt() : base(0x1C08)
        {
            Weight = 5.0;
            Name = "studded skirt";
        }

        public StuddedSkirt(Serial serial) : base(serial)
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

            if (Weight == 3.0)
                Weight = 5.0;
        }
    }
}