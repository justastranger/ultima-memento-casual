using System;using Server;using Server.Network;using System.Text;using Server.Items;using Server.Mobiles;namespace Server.Items{
    public class RottedReagents : Item
    {
        public int RottedCount;

        [CommandProperty(AccessLevel.Owner)]
        public int Rotted_Count { get { return RottedCount; } set { RottedCount = value; InvalidateProperties(); } }

        [Constructable]
        public RottedReagents() : base(0xE76)
        {
            Name = "bag of rotted reagents";
            Hue = 0xB97;
            Weight = 0.1;
        }

        public RottedReagents(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("These reagents are useless.");
        }        public override void AddNameProperties(ObjectPropertyList list)
        {
            string plural = "";
            if (RottedCount > 1) { plural = "s"; }            base.AddNameProperties(list);
            list.Add(1070722, "Contains " + RottedCount + " Ruined Reagent" + plural + "");        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(RottedCount);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            RottedCount = reader.ReadInt();
        }
    }}