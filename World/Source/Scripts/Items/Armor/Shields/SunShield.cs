using System;
using Server;

namespace Server.Items
{
    public class SunShield : BaseShield
    {
        public override int BasePhysicalResistance { get { return 1; } }
        public override int BaseFireResistance { get { return 1; } }
        public override int BaseColdResistance { get { return 0; } }
        public override int BasePoisonResistance { get { return 0; } }
        public override int BaseEnergyResistance { get { return 0; } }

        public override int InitMinHits { get { return 100; } }
        public override int InitMaxHits { get { return 125; } }

        public override int AosStrReq { get { return 95; } }

        public override int ArmorBase { get { return 32; } }

        [Constructable]
        public SunShield() : base(0x65ED)
        {
            Name = "sun shield";
            Weight = 7.0;
        }

        public SunShield(Serial serial) : base(serial)
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
