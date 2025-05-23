using System;using Server;using Server.Network;using System.Text;using Server.Items;using Server.Mobiles;namespace Server.Items{
    public class ManyArrows100 : Item
    {
        [Constructable]
        public ManyArrows100() : base(0xF41)
        {
            Name = "Bundle of 100 Arrows";
            Weight = 10;
        }

        public ManyArrows100(Serial serial) : base(serial)
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
                from.AddToBackpack(new Arrow(100));
                from.PrivateOverheadMessage(MessageType.Regular, 0x14C, false, "You separate the arrows into your backpack", from.NetState);
                this.Delete();
            }
        }        public override void AddNameProperties(ObjectPropertyList list)
        {            base.AddNameProperties(list);

            list.Add(1070722, "This Bundle Contains 100 Arrows");
            list.Add(1049644, "Double-Click To Separate Them Into Your Pack");        }

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
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ManyArrows1000 : Item
    {
        [Constructable]
        public ManyArrows1000() : base(0xF41)
        {
            Name = "Bundle of 1,000 Arrows";
            Weight = 100;
        }

        public ManyArrows1000(Serial serial) : base(serial)
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
                from.AddToBackpack(new Arrow(1000));
                from.PrivateOverheadMessage(MessageType.Regular, 0x14C, false, "You separate the arrows into your backpack", from.NetState);
                this.Delete();
            }
        }        public override void AddNameProperties(ObjectPropertyList list)
        {            base.AddNameProperties(list);

            list.Add(1070722, "This Bundle Contains 1,000 Arrows");
            list.Add(1049644, "Double-Click To Separate Them Into Your Pack");        }

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
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ManyBolts100 : Item
    {
        [Constructable]
        public ManyBolts100() : base(0x1BFD)
        {
            Name = "Bundle of 100 Bolts";
            Weight = 10;
        }

        public ManyBolts100(Serial serial) : base(serial)
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
                from.AddToBackpack(new Bolt(100));
                from.PrivateOverheadMessage(MessageType.Regular, 0x14C, false, "You separate the bolts into your backpack", from.NetState);
                this.Delete();
            }
        }        public override void AddNameProperties(ObjectPropertyList list)
        {            base.AddNameProperties(list);

            list.Add(1070722, "This Bundle Contains 100 Bolts");
            list.Add(1049644, "Double-Click To Separate Them Into Your Pack");        }

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
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class ManyBolts1000 : Item
    {
        [Constructable]
        public ManyBolts1000() : base(0x1BFD)
        {
            Name = "Bundle of 1,000 Bolts";
            Weight = 100;
        }

        public ManyBolts1000(Serial serial) : base(serial)
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
                from.AddToBackpack(new Bolt(1000));
                from.PrivateOverheadMessage(MessageType.Regular, 0x14C, false, "You separate the bolts into your backpack", from.NetState);
                this.Delete();
            }
        }        public override void AddNameProperties(ObjectPropertyList list)
        {            base.AddNameProperties(list);

            list.Add(1070722, "This Bundle Contains 1,000 Bolts");
            list.Add(1049644, "Double-Click To Separate Them Into Your Pack");        }

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