using System;

namespace Server.Items
{
    public class TambourineTassel : BaseInstrument
    {
        [Constructable]
        public TambourineTassel() : base(0x66F6, 0x52, 0x53)
        {
            Name = "tambourine";
            Weight = 1.0;
        }

        public TambourineTassel(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            ItemID = 0x66F6;
        }
    }
}