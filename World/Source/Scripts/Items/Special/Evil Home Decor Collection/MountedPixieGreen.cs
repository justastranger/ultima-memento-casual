using System;
using Server;
using Server.Network;

namespace Server.Items
{
    [Flipable(0x2A71, 0x2A72)]
    public class MountedPixieGreenComponent : AddonComponent
    {
        public override int LabelNumber { get { return 1074482; } } // Mounted pixie

        public MountedPixieGreenComponent() : base(0x2A71)
        {
        }

        public MountedPixieGreenComponent(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Utility.InRange(Location, from.Location, 2))
                Effects.PlaySound(Location, Map, Utility.RandomMinMax(0x554, 0x557));
            else
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
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
        }
    }

    public class MountedPixieGreenAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new MountedPixieGreenDeed(); } }

        public MountedPixieGreenAddon() : base()
        {
            AddComponent(new MountedPixieGreenComponent(), 0, 0, 0);
        }

        public MountedPixieGreenAddon(Serial serial) : base(serial)
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
        }
    }

    public class MountedPixieGreenDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new MountedPixieGreenAddon(); } }
        public override int LabelNumber { get { return 1074482; } } // Mounted pixie

        [Constructable]
        public MountedPixieGreenDeed() : base()
        {

        }

        public MountedPixieGreenDeed(Serial serial) : base(serial)
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
        }
    }
}
