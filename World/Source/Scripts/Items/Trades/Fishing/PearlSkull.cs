using System;using Server;using Server.Network;using System.Text;using Server.Items;using Server.Mobiles;namespace Server.Items{
    public class PearlSkull : Item
    {
        [Constructable]
        public PearlSkull() : base(0x1AE0)
        {
            ItemID = Utility.RandomList(0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3, 0x1AE4);
            string sLiquid = "a strange";
            switch (Utility.RandomMinMax(0, 6))
            {
                case 0: sLiquid = "an odd"; break;
                case 1: sLiquid = "an unusual"; break;
                case 2: sLiquid = "a bizarre"; break;
                case 3: sLiquid = "a curious"; break;
                case 4: sLiquid = "a peculiar"; break;
                case 5: sLiquid = "a strange"; break;
                case 6: sLiquid = "a weird"; break;
            }
            Name = sLiquid + " skull";
            Weight = 1.0;
        }

        public PearlSkull(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("This must be in your backpack to use.");
                return;
            }
            else
            {
                from.AddToBackpack(new Oyster());
                from.SendMessage("You open the mouth of the skull and find a pearl.");
                this.Delete();
            }
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
    }}