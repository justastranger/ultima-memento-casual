using System;

namespace Server.Items
{
    public class DecoBloodspawn : Item
    {

        [Constructable]
        public DecoBloodspawn() : base(0xF7C)
        {
            Movable = true;
            Stackable = false;
        }

        public DecoBloodspawn(Serial serial) : base(serial)
        {
        }

        public override bool OnDragLift(Mobile from)
        {
            from.SendMessage("This pagan reagent cannot be used in alchemy, but it is rare and collectible.");
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
