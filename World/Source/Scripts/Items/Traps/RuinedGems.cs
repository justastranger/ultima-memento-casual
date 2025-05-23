using System;using Server;using Server.Network;using System.Text;using Server.Items;using Server.Mobiles;namespace Server.Items{
    public class RuinedGems : Item
    {
        public int RuinedCount;

        [CommandProperty(AccessLevel.Owner)]
        public int Ruined_Count { get { return RuinedCount; } set { RuinedCount = value; InvalidateProperties(); } }

        [Constructable]
        public RuinedGems() : base(0x5739)
        {
            Name = "fused lode stone";
            Weight = 1.0;
        }

        public RuinedGems(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("This is a useless lump of rock.");
        }        public override void AddNameProperties(ObjectPropertyList list)
        {
            string plural = "";
            if (RuinedCount > 1) { plural = "s"; }            base.AddNameProperties(list);
            list.Add(1070722, "Rock With " + RuinedCount + " Fused Stone" + plural + "");        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(RuinedCount);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            RuinedCount = reader.ReadInt();
        }
    }}