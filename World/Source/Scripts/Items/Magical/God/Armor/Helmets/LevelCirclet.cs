using System;
using Server;

namespace Server.Items
{
    [FlipableAttribute(0x2B6F, 0x3166)]
    public class LevelCirclet : BaseLevelArmor
    {
        public override int BasePhysicalResistance { get { return 1; } }
        public override int BaseFireResistance { get { return 5; } }
        public override int BaseColdResistance { get { return 2; } }
        public override int BasePoisonResistance { get { return 2; } }
        public override int BaseEnergyResistance { get { return 5; } }

        public override int InitMinHits { get { return 20; } }
        public override int InitMaxHits { get { return 35; } }

        public override int AosStrReq { get { return 10; } }
        public override int OldStrReq { get { return 10; } }

        public override int ArmorBase { get { return 30; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

        [Constructable]
        public LevelCirclet() : base(0x672E)
        {
            Name = "circlet";
            Weight = 2.0;
        }

        public LevelCirclet(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
            ItemID = 0x672E;
        }
    }
}