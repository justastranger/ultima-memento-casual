using System;using Server;using Server.Network;using System.Text;using Server.Items;using Server.Mobiles;namespace Server.Items{
    public class WrappedCandy : Item
    {
        [Constructable]
        public WrappedCandy() : base(0x469E)
        {
            Name = "wrapped candy";
            Weight = 0.1;
        }

        public WrappedCandy(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("This must be in your backpack to open.");
                return;
            }
            else
            {
                from.AddToBackpack(new ChocolateMonster());
                from.PrivateOverheadMessage(MessageType.Regular, 0x14C, false, "You unwrap candy", from.NetState);
                this.Delete();
            }
        }        public override void AddNameProperties(ObjectPropertyList list)
        {            base.AddNameProperties(list);
            list.Add(1049644, "Double-Click To Unwrap");        }

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