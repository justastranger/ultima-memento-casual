using System;
using Server;

namespace Server.Items
{
    [Flipable]
    public class WallSconce : BaseLight
    {
        public override Catalogs DefaultCatalog { get { return Catalogs.Wax; } }

        public override int LitItemID
        {
            get
            {
                if (ItemID == 0x9FB)
                    return 0x9FD;
                else
                    return 0xA02;
            }
        }

        public override int UnlitItemID
        {
            get
            {
                if (ItemID == 0x9FD)
                    return 0x9FB;
                else
                    return 0xA00;
            }
        }

        [Constructable]
        public WallSconce() : base(0x9FB)
        {
            Movable = true;
            Duration = TimeSpan.Zero;
            BurntOut = false;
            Burning = false;
            Light = LightType.WestBig;
            Weight = 3.0;
        }

        public WallSconce(Serial serial) : base(serial)
        {
        }

        public void Flip()
        {
            if (Light == LightType.WestBig)
                Light = LightType.NorthBig;
            else if (Light == LightType.NorthBig)
                Light = LightType.WestBig;

            switch (ItemID)
            {
                case 0x9FB: ItemID = 0xA00; break;
                case 0x9FD: ItemID = 0xA02; break;

                case 0xA00: ItemID = 0x9FB; break;
                case 0xA02: ItemID = 0x9FD; break;
            }
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