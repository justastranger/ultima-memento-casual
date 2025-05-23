using System;using Server;using Server.Network;using System.Text;using Server.Items;using Server.Mobiles;namespace Server.Items{
    public class TaintedBandage : Item
    {
        [Constructable]
        public TaintedBandage() : base(0xE21)
        {
            Name = "tainted bandage";
            Hue = 0x972;
            Weight = 0.1;
        }

        public TaintedBandage(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("You cannot use tainted bandages.");
        }        public override void AddNameProperties(ObjectPropertyList list)
        {            base.AddNameProperties(list);
            list.Add(1070722, "Ruined");        }

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