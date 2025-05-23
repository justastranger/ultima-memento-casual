using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class EnergyThrenodyScroll : SpellScroll
    {
        public override string DefaultDescription { get { return SongBook.SpellDescription(354); } }

        [Constructable]
        public EnergyThrenodyScroll() : this(1)
        {
        }

        [Constructable]
        public EnergyThrenodyScroll(int amount) : base(354, 0x1F46, amount)
        {
            Name = "energy threnody sheet music";
            Hue = 0x96;
        }

        public EnergyThrenodyScroll(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("The sheet music must be in your music book.");
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
    }
}