using System;

namespace Server.Items
{
    public class DecoGinsengRoot2 : Item
    {

        [Constructable]
        public DecoGinsengRoot2() : base(0x18EC)
        {
            Movable = true;
            Stackable = false;
        }

        public DecoGinsengRoot2(Serial serial) : base(serial)
        {
        }

        public override bool OnDragLift(Mobile from)
        {
            from.SendMessage("This cannot be used in alchemy, but it is rare and collectible.");
            return base.OnDragLift(from);
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
