using System;

namespace Server.Items
{
    public abstract class Hair : Item
    {
        /*
		
		public static Hair GetRandomHair( bool female )
		{
			return GetRandomHair( female, Utility.RandomHairHue() );
		}

		public static Hair GetRandomHair( bool female, int hairHue )
		{
			if( female )
			{
				switch ( Utility.Random( 9 ) )
				{
					case 0: return new Afro( hairHue );
					case 1: return new KrisnaHair( hairHue );
					case 2: return new PageboyHair( hairHue );
					case 3: return new PonyTail( hairHue );
					case 4: return new ReceedingHair( hairHue );
					case 5: return new TwoPigTails( hairHue );
					case 6: return new ShortHair( hairHue );
					case 7: return new LongHair( hairHue );
					default: return new BunsHair( hairHue );
				}
			}
			else
			{
				switch ( Utility.Random( 8 ) )
				{
					case 0: return new Afro( hairHue );
					case 1: return new KrisnaHair( hairHue );
					case 2: return new PageboyHair( hairHue );
					case 3: return new PonyTail( hairHue );
					case 4: return new ReceedingHair( hairHue );
					case 5: return new TwoPigTails( hairHue );
					case 6: return new ShortHair( hairHue );
					default: return new LongHair( hairHue );
				}
			}
		}

		public static Hair CreateByID( int id, int hue )
		{
			switch ( id )
			{
				case 0x203B: return new ShortHair( hue );
				case 0x203C: return new LongHair( hue );
				case 0x203D: return new PonyTail( hue );
				case 0x2044: return new Mohawk( hue );
				case 0x2045: return new PageboyHair( hue );
				case 0x2046: return new BunsHair( hue );
				case 0x2047: return new Afro( hue );
				case 0x2048: return new ReceedingHair( hue );
				case 0x2049: return new TwoPigTails( hue );
				case 0x204A: return new KrisnaHair( hue );
				default: return new GenericHair( id, hue );
			}
		}
		 * */

        protected Hair(int itemID)
            : this(itemID, 0)
        {
        }

        protected Hair(int itemID, int hue)
            : base(itemID)
        {
            LootType = LootType.Blessed;
            Layer = Layer.Hair;
            Hue = hue;
        }

        public Hair(Serial serial)
            : base(serial)
        {
        }

        public override bool DisplayLootType { get { return false; } }

        public override bool VerifyMove(Mobile from)
        {
            return (from.AccessLevel >= AccessLevel.GameMaster);
        }

        public override DeathMoveResult OnParentDeath(Mobile parent)
        {
            //			Dupe( Amount );

            parent.HairItemID = this.ItemID;
            parent.HairHue = this.Hue;

            return DeathMoveResult.MoveToCorpse;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            LootType = LootType.Blessed;

            int version = reader.ReadInt();
        }
    }

    public class GenericHair : Hair
    {

        private GenericHair(int itemID)
            : this(itemID, 0)
        {
        }

        private GenericHair(int itemID, int hue)
            : base(itemID, hue)
        {
        }

        public GenericHair(Serial serial)
            : base(serial)
        {
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

    public class Mohawk : Hair
    {

        private Mohawk()
            : this(0)
        {
        }

        private Mohawk(int hue)
            : base(0x2044, hue)
        {
        }

        public Mohawk(Serial serial)
            : base(serial)
        {
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

    public class PageboyHair : Hair
    {

        private PageboyHair()
            : this(0)
        {
        }

        private PageboyHair(int hue)
            : base(0x2045, hue)
        {
        }

        public PageboyHair(Serial serial)
            : base(serial)
        {
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

    public class BunsHair : Hair
    {

        private BunsHair()
            : this(0)
        {
        }

        private BunsHair(int hue)
            : base(0x2046, hue)
        {
        }

        public BunsHair(Serial serial)
            : base(serial)
        {
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

    public class LongHair : Hair
    {

        private LongHair()
            : this(0)
        {
        }

        private LongHair(int hue)
            : base(0x203C, hue)
        {
        }

        public LongHair(Serial serial)
            : base(serial)
        {
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

    public class ShortHair : Hair
    {

        private ShortHair()
            : this(0)
        {
        }

        private ShortHair(int hue)
            : base(0x203B, hue)
        {
        }

        public ShortHair(Serial serial)
            : base(serial)
        {
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

    public class PonyTail : Hair
    {

        private PonyTail()
            : this(0)
        {
        }

        private PonyTail(int hue)
            : base(0x203D, hue)
        {
        }

        public PonyTail(Serial serial)
            : base(serial)
        {
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

    public class Afro : Hair
    {

        private Afro()
            : this(0)
        {
        }

        private Afro(int hue)
            : base(0x2047, hue)
        {
        }

        public Afro(Serial serial)
            : base(serial)
        {
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

    public class ReceedingHair : Hair
    {

        private ReceedingHair()
            : this(0)
        {
        }

        private ReceedingHair(int hue)
            : base(0x2048, hue)
        {
        }

        public ReceedingHair(Serial serial)
            : base(serial)
        {
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

    public class TwoPigTails : Hair
    {

        private TwoPigTails()
            : this(0)
        {
        }

        private TwoPigTails(int hue)
            : base(0x2049, hue)
        {
        }

        public TwoPigTails(Serial serial)
            : base(serial)
        {
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

    public class KrisnaHair : Hair
    {

        private KrisnaHair()
            : this(0)
        {
        }

        private KrisnaHair(int hue)
            : base(0x204A, hue)
        {
        }

        public KrisnaHair(Serial serial)
            : base(serial)
        {
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