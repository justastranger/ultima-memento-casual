using System;
using Server;
using Server.Network;

namespace Server.Items
{
    [Flipable(0x2A73, 0x2A74)]
    public class MountedPixieOrangeComponent : AddonComponent
    {
        public override int LabelNumber { get { return 1074482; } } // Mounted pixie

        public MountedPixieOrangeComponent() : base(0x2A73)
        {
        }

        public MountedPixieOrangeComponent(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Utility.InRange(Location, from.Location, 2))
                Effects.PlaySound(Location, Map, Utility.RandomMinMax(0x558, 0x55B));
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

    public class MountedPixieOrangeAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new MountedPixieOrangeDeed(); } }

        public MountedPixieOrangeAddon() : base()
        {
            AddComponent(new MountedPixieOrangeComponent(), 0, 0, 0);
        }

        public MountedPixieOrangeAddon(Serial serial) : base(serial)
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

    public class MountedPixieOrangeDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new MountedPixieOrangeAddon(); } }
        public override int LabelNumber { get { return 1074482; } } // Mounted pixie

        [Constructable]
        public MountedPixieOrangeDeed() : base()
        {

        }

        public MountedPixieOrangeDeed(Serial serial) : base(serial)
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
