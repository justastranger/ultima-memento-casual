using System;
using Server;

namespace Server.Items
{
    public class WarriorStatueSouthAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new WarriorStatueSouthDeed(); } }

        [Constructable]
        public WarriorStatueSouthAddon()
        {
            AddComponent(new AddonComponent(0x2D13), 0, 0, 0);
        }

        public WarriorStatueSouthAddon(Serial serial) : base(serial)
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

    public class WarriorStatueSouthDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new WarriorStatueSouthAddon(); } }
        public override int LabelNumber { get { return 1072887; } } // warrior statue (south)

        [Constructable]
        public WarriorStatueSouthDeed()
        {
        }

        public WarriorStatueSouthDeed(Serial serial) : base(serial)
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