using System;using Server;using Server.Network;using System.Text;using Server.Items;using Server.Mobiles;namespace Server.Items{
    public class MagicalRope : Item
    {
        [Constructable]
        public MagicalRope() : base(0x14F8)
        {
            Name = "magical rope";
            Weight = 10;
        }

        public MagicalRope(Serial serial) : base(serial)
        {
        }        public override void AddNameProperties(ObjectPropertyList list)
        {            base.AddNameProperties(list);
            list.Add(1070722, "Say 'climb' to Use the Rope");        }

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