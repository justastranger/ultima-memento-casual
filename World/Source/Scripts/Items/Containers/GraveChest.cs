using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Multis;
using Server.Network;
using Server.ContextMenus;
using Server.Engines.PartySystem;
using Server.Misc;

namespace Server.Items
{
    public class GraveChest : SkullChest
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        public string ContainerOwner;

        [CommandProperty(AccessLevel.Owner)]
        public string Container_Owner { get { return ContainerOwner; } set { ContainerOwner = value; InvalidateProperties(); } }

        public string ContainerDigger;

        [CommandProperty(AccessLevel.Owner)]
        public string Container_Digger { get { return ContainerDigger; } set { ContainerDigger = value; InvalidateProperties(); } }

        [Constructable]
        public GraveChest() : this(0, null)
        {
        }

        [Constructable]
        public GraveChest(int level, Mobile digger) : base()
        {
            Catalog = Catalogs.TreasureChest;

            if (level > 0 && digger != null)
            {
                Name = "graveyard chest";
                ContainerFunctions.FillTheContainer(level, this, digger);
                if (GetPlayerInfo.LuckyPlayer(digger.Luck)) { ContainerFunctions.FillTheContainer(level, this, digger); }

                ContainerFunctions.LockTheContainer(level, this, 1);

                Hue = Utility.RandomList(0x961, 0x962, 0x963, 0x964, 0x965, 0x966, 0x967, 0x968, 0x969, 0x96A, 0x96B, 0x96C, 0x973, 0x974, 0x975, 0x976, 0x977, 0x978, 0x497, 0x47E);

                string sBox = "coffin";
                switch (Utility.Random(4))
                {
                    case 0: sBox = "casket"; ItemID = Utility.RandomList(0x27E9, 0x27EA); GumpID = 0x41D; Weight = 25.0; break;
                    case 1: sBox = "sarcophagus"; ItemID = Utility.RandomList(0x27E0, 0x280A, 0x2802, 0x2803); GumpID = 0x1D; Weight = 100.0; break;
                    case 2: sBox = "coffin"; ItemID = Utility.RandomList(0x2800, 0x2801); GumpID = 0x41D; Weight = 25.0; break;
                    case 3: sBox = "chest"; break;
                }

                ContainerOwner = ContainerFunctions.GetOwner(sBox);
                ContainerDigger = digger.Name;

                Name = sBox;
                ColorText1 = sBox;
                ColorHue1 = "c866ec";
                ColorText2 = ContainerOwner;
                ColorHue2 = "c866ec";
                ColorText3 = "Dug Up By " + ContainerDigger + "";
                ColorHue3 = "c895db";
            }
        }

        public GraveChest(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(ContainerOwner);
            writer.Write(ContainerDigger);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            ContainerOwner = reader.ReadString();
            ContainerDigger = reader.ReadString();
        }
    }
}