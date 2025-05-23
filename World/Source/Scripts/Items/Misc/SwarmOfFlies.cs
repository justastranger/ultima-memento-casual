using System;

namespace Server.Items
{
    public class SwarmOfFlies : Item
    {
        public override string DefaultName
        {
            get { return "a swarm of flies"; }
        }

        [Constructable]
        public SwarmOfFlies() : base(0x91B)
        {
            Hue = 1;
            Movable = false;
        }

        public SwarmOfFlies(Serial serial) : base(serial)
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
        }
    }
}