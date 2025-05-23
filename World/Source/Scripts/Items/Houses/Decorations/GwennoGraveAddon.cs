using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class GwennoGraveAddon : BaseAddon
    {
        public override string AddonName { get { return "monument of gwenno"; } }

        private static int[,] m_AddOnSimpleComponents = new int[,] {
              {4487, 1, 1, 0}, {4488, 2, 0, 0}, {4489, 2, -2, 0}// 1	2	3	
			, {4486, 0, 2, 0}, {4485, -2, 2, 0}// 4	5	
		};

        public override BaseAddonDeed Deed
        {
            get
            {
                return new GwennoGraveAddonDeed();
            }
        }

        [Constructable]
        public GwennoGraveAddon()
        {
            for (int i = 0; i < m_AddOnSimpleComponents.Length / 4; i++)
                AddComponent(new AddonComponent(m_AddOnSimpleComponents[i, 0]), m_AddOnSimpleComponents[i, 1], m_AddOnSimpleComponents[i, 2], m_AddOnSimpleComponents[i, 3]);
        }

        public GwennoGraveAddon(Serial serial) : base(serial)
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

    public class GwennoGraveAddonDeed : BaseAddonDeed
    {
        public override BaseAddon Addon
        {
            get
            {
                return new GwennoGraveAddon();
            }
        }

        [Constructable]
        public GwennoGraveAddonDeed()
        {
            Name = "Gwenno Monument Deed";
        }

        public GwennoGraveAddonDeed(Serial serial) : base(serial)
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