using System;
using Server.Items;

namespace Server.Items
{
    public class LevelPlateDo : BaseLevelArmor
    {
        public override int BasePhysicalResistance { get { return 5; } }
        public override int BaseFireResistance { get { return 3; } }
        public override int BaseColdResistance { get { return 2; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 2; } }

        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 70; } }

        public override int AosStrReq { get { return 85; } }
        public override int OldStrReq { get { return 85; } }

        public override int ArmorBase { get { return 3; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

        [Constructable]
        public LevelPlateDo() : base(0x277D)
        {
            Weight = 10.0;
        }

        public LevelPlateDo(Serial serial) : base(serial)
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