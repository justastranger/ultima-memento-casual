using System;using Server;using Server.Network;using System.Text;using Server.Items;using Server.Mobiles;namespace Server.Items{
    public class BrokenGear : Item
    {
        [Constructable]
        public BrokenGear() : base(0x5739)
        {
            Name = "broken item";
            Hue = Utility.RandomList(0xB97, 0xB98, 0xB99, 0xB9A, 0xB88);
            Weight = 0.1;
        }

        public BrokenGear(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("This is totally useless.");
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