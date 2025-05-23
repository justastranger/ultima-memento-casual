using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class TreasurePile05Addon : BaseAddon
    {
        public override string AddonName { get { return "treasure pile"; } }

        public override BaseAddonDeed Deed
        {
            get
            {
                return new TreasurePile05AddonDeed();
            }
        }

        [Constructable]
        public TreasurePile05Addon()
        {
            AddonComponent ac = null;
            ac = new AddonComponent(7017);
            AddComponent(ac, 0, -1, 0);
            ac = new AddonComponent(7016);
            AddComponent(ac, -1, -1, 0);
            ac = new AddonComponent(7015);
            AddComponent(ac, -1, 0, 0);
            ac = new AddonComponent(7014);
            AddComponent(ac, -2, 0, 0);
            ac = new AddonComponent(7013);
            AddComponent(ac, -2, -1, 0);
            ac = new AddonComponent(7012);
            AddComponent(ac, -2, 1, 0);
            ac = new AddonComponent(7011);
            AddComponent(ac, -1, 1, 0);
            ac = new AddonComponent(7010);
            AddComponent(ac, 0, 1, 0);
            ac = new AddonComponent(7009);
            AddComponent(ac, 0, 0, 0);
            ac = new AddonComponent(7018);
            AddComponent(ac, 1, -1, 0);
            ac = new AddonComponent(7019);
            AddComponent(ac, 2, -1, 0);
            ac = new AddonComponent(7008);
            AddComponent(ac, 1, 0, 0);
            ac = new AddonComponent(7007);
            AddComponent(ac, 2, 0, 0);

        }

        public TreasurePile05Addon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class TreasurePile05AddonDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get
            {
                return new TreasurePile05Addon();
            }
        }

        [Constructable]
        public TreasurePile05AddonDeed()
        {
            ItemID = 0x0E41;
            Weight = 50.0;
            Name = "Chest of Decorative Treasure";
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1049644, "Double Click To Dump In Your Home");
        }

        public TreasurePile05AddonDeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}