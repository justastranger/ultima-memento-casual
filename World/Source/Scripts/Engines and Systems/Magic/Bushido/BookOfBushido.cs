using System;
using Server.Network;
using Server.Spells;

namespace Server.Items
{
    public class BookOfBushido : Spellbook
    {
        public override string DefaultDescription { get { return "This book is used by samurai, in order for them to use various combat abilities in battle. Some books have enhanced properties, that are only effective when the book is held."; } }

        public override SpellbookType SpellbookType { get { return SpellbookType.Samurai; } }
        public override int BookOffset { get { return 400; } }
        public override int BookCount { get { return 6; } }

        [Constructable]
        public BookOfBushido() : this((ulong)0x3F)
        {
        }

        [Constructable]
        public BookOfBushido(ulong content) : base(content, 0x238C)
        {
            Name = "bushido book";
            Layer = Layer.Trinket;
        }

        public BookOfBushido(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}