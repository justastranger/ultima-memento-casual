using System;
using Server;

namespace Server.Items
{
    public class LevelBronzeShield : BaseLevelShield
    {
        public override int BasePhysicalResistance { get { return 0; } }
        public override int BaseFireResistance { get { return 0; } }
        public override int BaseColdResistance { get { return 1; } }
        public override int BasePoisonResistance { get { return 0; } }
        public override int BaseEnergyResistance { get { return 0; } }

        public override int InitMinHits { get { return 25; } }
        public override int InitMaxHits { get { return 30; } }

        public override int AosStrReq { get { return 35; } }

        public override int ArmorBase { get { return 10; } }

        [Constructable]
        public LevelBronzeShield() : base(0x1B72)
        {
            Name = "large shield";
            Weight = 6.0;
        }

        public LevelBronzeShield(Serial serial) : base(serial)
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
