using System;
using Server;

namespace Server.Items
{
    public class DreadHelm : BaseArmor
    {
        public override int BasePhysicalResistance { get { return 5; } }
        public override int BaseFireResistance { get { return 3; } }
        public override int BaseColdResistance { get { return 2; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 2; } }

        public override int InitMinHits { get { return 50; } }
        public override int InitMaxHits { get { return 65; } }

        public override int AosStrReq { get { return 80; } }
        public override int OldStrReq { get { return 40; } }

        public override int OldDexBonus { get { return -1; } }

        public override int ArmorBase { get { return 40; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

        [Constructable]
        public DreadHelm() : base(0x2FBB)
        {
            Name = "dread helm";
            Weight = 5.0;
            ItemID = Utility.RandomList(0x2FBB, 0x267F);
        }

        public DreadHelm(Serial serial) : base(serial)
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
                Weight = 5.0;
        }
    }
}