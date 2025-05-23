using System;
using Server;

namespace Server.Items
{
    public class HalloweenBonePileAddon : BaseAddon
    {
        public override BaseAddonDeed Deed { get { return new HalloweenBonePileDeed(); } }

        [Constructable]
        public HalloweenBonePileAddon()
        {
            Name = "Bone Pile";
            AddComponent(new AddonComponent(6872), 1, 1, 0);
            AddComponent(new AddonComponent(6873), 0, 1, 0);
            AddComponent(new AddonComponent(6874), -1, 1, 0);
            AddComponent(new AddonComponent(6875), 0, 0, 0);
            AddComponent(new AddonComponent(6876), 1, 0, 0);
            AddComponent(new AddonComponent(6877), 1, -1, 0);
            AddComponent(new AddonComponent(6878), 2, -1, 0);
            AddComponent(new AddonComponent(6879), 2, 0, 0);
        }

        public HalloweenBonePileAddon(Serial serial) : base(serial)
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

    public class HalloweenBonePileDeed : BaseAddonDeed
    {
        public override BaseAddon Addon { get { return new HalloweenBonePileAddon(); } }

        [Constructable]
        public HalloweenBonePileDeed()
        {
            Name = "Bone Pile Deed";
            Hue = 0x96C;
        }

        public HalloweenBonePileDeed(Serial serial) : base(serial)
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