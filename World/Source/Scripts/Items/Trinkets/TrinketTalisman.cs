using System;
using Server;
using Server.Misc;

namespace Server.Items
{
	public class TrinketTalisman : BaseTrinket
	{
		private int m_Subtype;
		public int Subtype { get { return m_Subtype; } }
		public override Catalogs DefaultCatalog{ get{ return Catalogs.Trinket; } }
		public override CraftResource DefaultResource{ get{ return CraftResource.None; } }

		// fake constructor for NotIdentified
		public static string GetSubtypeName(int type)
        {
            switch (type)
            {
                case 0: return "talisman";
                case 1: return "idol";
                case 2: return "totem";
                case 3: return "symbol";
                case 4:
                    switch (Utility.RandomMinMax(0, 2))
                    {
                        case 0: return "bag";
                        case 1: return "pouch";
                        case 2: return "sack";
                    }
                    break;
                case 5: return "ankh";  // Ankh
                case 6: return "censer";    // Censer
                case 7: return "cube";  // Cube
                case 8: return "lamp";  // Lamp
                case 9:
                    switch (Utility.RandomMinMax(0, 3))
                    {
                        case 0: return "box";
                        case 1: return "chest";
                        case 2: return "casket";
                        case 3: return "coffer";
                    }
                    break;
                case 10:
                    switch (Utility.RandomMinMax(0, 2))
                    {
                        case 0: return "ball";
                        case 1: return "orb";
                        case 2: return "sphere";
                    }
                    break;
                case 11: return "dice"; // Dice
                case 12: return "eye";  // Eye
                case 13:
                    switch (Utility.RandomMinMax(0, 2))
                    {
                        case 0: return "gem";
                        case 1: return "crystal";
                        case 2: return "jewel";
                    }
                    break;
                case 14: return "unicorn horn"; // Unicorn Horn
                case 15:
                    switch (Utility.RandomMinMax(0, 1))
                    {
                        case 0: return "rose";
                        case 1: return "flower";
                    }
                    break;
                case 16:
                    switch (Utility.RandomMinMax(0, 2))
                    {
                        case 0: return "medal";
                        case 1: return "badge";
                        case 2: return "medallion";
                    }
                    break;
                case 17:
                    switch (Utility.RandomMinMax(0, 2))
                    {
                        case 0: return "mug";
                        case 1: return "tankard";
                        case 2: return "stein";
                    }
                    break;
                case 18: return "mushroom";
                case 19:
                    switch (Utility.RandomMinMax(1, 3))
                    {
                        case 1: return "orb";
                        case 2: return "stone";
                        case 3: return "sphere";
                    }
                    break;
                case 20:
                    switch (Utility.RandomMinMax(0, 4))
                    {
                        case 0: return "plant";
                        case 1: return "flower";
                        case 2: return "weed";
                        case 3: return "vine";
                        case 4: return "herb";
                    }
                    break;
                case 21: return "tablet";
                case 22: return "flask"; // Bottle
                case 23: return "rune"; // Rune
                case 24: return "rune"; // Rune
                case 25:
                    switch (Utility.RandomMinMax(0, 1))
                    {
                        case 0: return "scroll";
                        case 1: return "parchment";
                    }
                    break;
                case 26:
                    switch (Utility.RandomMinMax(0, 1))
                    {
                        case 0: return "scroll";
                        case 1: return "parchment";
                    }
                    break;
                case 27: return "skull";    // Skull
                case 28:
                    switch (Utility.RandomMinMax(0, 1))
                    {
                        case 0: return "rock";
                        case 1: return "stone";
                    }
                    break;
                case 29:
                    switch (Utility.RandomMinMax(0, 4))
                    {
                        case 0: return "urn";
                        case 1: return "vase";
                        case 2: return "jar";
                        case 3: return "pot";
                        case 4: return "ewer";
                    }
                    break;
                case 30: return "vial"; // Vial
                case 31: return "bone"; // Bone
                case 32: return "eye";  // Evil Eye
                case 33:
                    switch (Utility.RandomMinMax(0, 1))
                    {
                        case 0: return "frog";
                        case 1: return "toad";
                    }
                    break;
                case 34:
                    switch (Utility.RandomMinMax(0, 3))
                    {
                        case 0: return "chalice";
                        case 1: return "goblet";
                        case 2: return "cup";
                        case 3: return "grail";
                    }
                    break;
                case 35:
                    switch (Utility.RandomMinMax(0, 2))
                    {
                        case 0: return "gem";
                        case 1: return "crystal";
                        case 2: return "jewel";
                    }
                    break;
                case 36: return "skull";    // Monster Skull
                case 37: return Server.Misc.RandomThings.GetRandomBookType(false);
                case 38: return "doll"; // Doll
                case 39:
                    switch (Utility.RandomMinMax(0, 1))
                    {
                        case 0: return "hand";
                        case 1: return "claw";
                    }
                    break;
                case 40: return "heart";    // Heart
                case 41: return "rabbit foot";  // Rabbit's Foot
                case 42:
                    switch (Utility.RandomMinMax(0, 1))
                    {
                        case 0: return "gems";
                        case 1: return "jewels";
                    }
                    break;
                case 43: return "teeth";
                case 44:
                    switch (Utility.RandomMinMax(0, 1))
                    {
                        case 0: return "chains";
                        case 1: return "shackles";
                    }
                    break;
                case 45: return "map";
                case 46: return "skull";    // Creature Skull
                case 47: return "fishing hook";    // Fishing Hook
                case 48: return "coin"; // Coin
                case 49: return "head"; // Monster Head
                case 50: return "brazier"; // Brazier
            }
			return "talisman";
        }

		[Constructable]
		public TrinketTalisman() : base( 0x2F58 ) 
		{
			Name = "talisman";
			Hue = 0;

			int trinket = Utility.RandomMinMax( 0, 50 );
			m_Subtype = trinket;
			switch ( trinket ) 
			{
				case 0 :	Name = "talisman"; 						ItemID = 0x2F58; 		break;
				case 1 :	Name = "idol"; 							ItemID = 0x2F59; 		break;
				case 2 :	Name = "totem"; 						ItemID = 0x2F5A; 		break;
				case 3 :	Name = "symbol"; 						ItemID = 0x2F5B; 		break;
				case 4 : 	Hue = 0xABE;							ItemID = 0x2C7E;				// Pouch
					switch ( Utility.RandomMinMax( 0, 2 ) ) 
					{
						case 0 : Name = "bag"; break;
						case 1 : Name = "pouch"; break;
						case 2 : Name = "sack"; break;
					}
					break;
				case 5 : 	Name = "ankh";							ItemID = 0x2C7F;		break;	// Ankh
				case 6 : 	Name = "censer";						ItemID = 0x2C80;		break;	// Censer
				case 7 : 	Name = "cube";							ItemID = 0x2C81;		break;	// Cube
				case 8 : 	Name = "lamp";							ItemID = 0x2C82;		break;	// Lamp
				case 9 : 											ItemID = 0x2C83;				// Chest
					switch ( Utility.RandomMinMax( 0, 3 ) ) 
					{
						case 0 : Name = "box"; break;
						case 1 : Name = "chest"; break;
						case 2 : Name = "casket"; break;
						case 3 : Name = "coffer"; break;
					}
					break;
				case 10 : 											ItemID = 0x2C84;				// Crystal Ball
					switch ( Utility.RandomMinMax( 0, 2 ) ) 
					{
						case 0 : Name = "ball"; break;
						case 1 : Name = "orb"; break;
						case 2 : Name = "sphere"; break;
					}
					break;
				case 11 : 	Name = "dice";							ItemID = 0x2C85;		break;	// Dice
				case 12 : 	Name = "eye";							ItemID = 0x2C86;		break;	// Eye
				case 13 : 											ItemID = 0x2C87;				// Emerald
					switch ( Utility.RandomMinMax( 0, 2 ) ) 
					{
						case 0 : Name = "gem"; break;
						case 1 : Name = "crystal"; break;
						case 2 : Name = "jewel"; break;
					}
					break;
				case 14 : 	Name = "unicorn horn";					ItemID = 0x2C88;		break;	// Unicorn Horn
				case 15 : 	Name = "rose";							ItemID = 0x2C89;				// Rose
					switch ( Utility.RandomMinMax( 0, 1 ) ) 
					{
						case 0 : Name = "rose"; break;
						case 1 : Name = "flower"; break;
					}
					break;
				case 16 :											ItemID = 0x2C8A;				// Medal
					switch ( Utility.RandomMinMax( 0, 2 ) ) 
					{
						case 0 : Name = "medal"; break;
						case 1 : Name = "badge"; break;
						case 2 : Name = "medallion"; break;
					}
					break;
				case 17 :											ItemID = 0x2C8B;				// Mug
					switch ( Utility.RandomMinMax( 0, 2 ) ) 
					{
						case 0 : Name = "mug"; break;
						case 1 : Name = "tankard"; break;
						case 2 : Name = "stein"; break;
					}
					break;
				case 18 : 	Name = "mushroom";						ItemID = 0x2C8C;		break;	// Mushroom
				case 19 :											ItemID = 0x2C8D;				// Pearl
					switch ( Utility.RandomMinMax( 1, 3 ) ) 
					{
						case 1 : Name = "orb"; break;
						case 2 : Name = "stone"; break;
						case 3 : Name = "sphere"; break;
					}
					break;
				case 20 :											ItemID = 0x2C8E;				// Plant
					switch ( Utility.RandomMinMax( 0, 4 ) ) 
					{
						case 0 : Name = "plant"; break;
						case 1 : Name = "flower"; break;
						case 2 : Name = "weed"; break;
						case 3 : Name = "vine"; break;
						case 4 : Name = "herb"; break;
					}
					break;
				case 21 : 	Name = "tablet";						ItemID = 0x2C8F;		break;	// Tablet
				case 22 : 	Name = "flask"; Hue = Utility.RandomColor(0); ItemID = 0x2C90; break; // Bottle
				case 23 : 	Name = "rune";							ItemID = 0x2C91;		break;	// Rune
				case 24 : 	Name = "rune";							ItemID = 0x2C92;		break;	// Rune
				case 25 :											ItemID = 0x2C93;				// Scroll
					switch ( Utility.RandomMinMax( 0, 1 ) ) 
					{
						case 0 : Name = "scroll"; break;
						case 1 : Name = "parchment"; break;
					}
					break;
				case 26 :											ItemID = 0x2C94;				// Scroll
					switch ( Utility.RandomMinMax( 0, 1 ) ) 
					{
						case 0 : Name = "scroll"; break;
						case 1 : Name = "parchment"; break;
					}
					break;
				case 27 : 	Name = "skull";							ItemID = 0x2C95;		break;	// Skull
				case 28 : 											ItemID = 0x2C96;				// Stone
					switch ( Utility.RandomMinMax( 0, 1 ) ) 
					{
						case 0 : Name = "rock"; break;
						case 1 : Name = "stone"; break;
					}
					break;
				case 29 :											ItemID = 0x2C97;				// Urn
					switch ( Utility.RandomMinMax( 0, 4 ) ) 
					{
						case 0 : Name = "urn"; break;
						case 1 : Name = "vase"; break;
						case 2 : Name = "jar"; break;
						case 3 : Name = "pot"; break;
						case 4 : Name = "ewer"; break;
					}
					break;
				case 30 : 	Name = "vial"; Hue = Utility.RandomColor(0); ItemID = 0x2C98; break; // Vial
				case 31 : 	Name = "bone";							ItemID = 0x2C99;		break;	// Bone
				case 32 : 	Name = "eye";							ItemID = 0x2C9A;		break;	// Evil Eye
				case 33 :											ItemID = 0x2C9B;				// Frog
					switch ( Utility.RandomMinMax( 0, 1 ) ) 
					{
						case 0 : Name = "frog"; break;
						case 1 : Name = "toad"; break;
					}
					break;
				case 34 :											ItemID = 0x2C9C;				// Goblet
					switch ( Utility.RandomMinMax( 0, 3 ) ) 
					{
						case 0 : Name = "chalice"; break;
						case 1 : Name = "goblet"; break;
						case 2 : Name = "cup"; break;
						case 3 : Name = "grail"; break;
					}
					break;
				case 35 : 											ItemID = 0x2C9D;				// Ruby
					switch ( Utility.RandomMinMax( 0, 2 ) ) 
					{
						case 0 : Name = "gem"; break;
						case 1 : Name = "crystal"; break;
						case 2 : Name = "jewel"; break;
					}
					break;
				case 36 : 	Name = "skull";							ItemID = 0x2C9E;		break;	// Monster Skull
				case 37 : 																			// Book
					ItemID = Utility.RandomList( 0x4FCF, 0x4FD0, 0x4FD1, 0x4FD2, 0x4FD3 );
					Name = Server.Misc.RandomThings.GetRandomBookType(false);
					break;
				case 38 : 	Name = "doll";							ItemID = 0x2D81;		break;	// Doll
				case 39 :											ItemID = 0x2D7F;				// Hand or Claw
					switch ( Utility.RandomMinMax( 0, 1 ) ) 
					{
						case 0 : Name = "hand"; break;
						case 1 : Name = "claw"; break;
					}
					break;
				case 40 : 	Name = "heart";							ItemID = 0x2D7E;		break;	// Heart
				case 41 : 	Name = "rabbit foot";					ItemID = 0x2D80;		break;	// Rabbit's Foot
				case 42 : 	Name = "gems"; Hue = 0xABE;				ItemID = 0x4D0E;				// Rabbit's Foot
					switch ( Utility.RandomMinMax( 0, 1 ) ) 
					{
						case 0 : Name = "gems"; break;
						case 1 : Name = "jewels"; break;
					}
					break;
				case 43 : 	Name = "teeth";	Hue = 0xABE;			ItemID = 0x4D0F;		break;	// Rabbit's Foot
				case 44 : 	Name = "chains";						ItemID = 0x4D10;				// Rabbit's Foot
					switch ( Utility.RandomMinMax( 0, 1 ) ) 
					{
						case 0 : Name = "chains"; break;
						case 1 : Name = "shackles"; break;
					}
					break;

				case 45 : 	Name = "map";							ItemID = 0x4D11;				// Rabbit's Foot
					switch ( Utility.RandomMinMax( 0, 1 ) ) 
					{
						case 0 : SkillBonuses.SetValues( 4, SkillName.Cartography, Utility.RandomMinMax( 5, 20 ) ); break;
						case 1 : SkillBonuses.SetValues( 4, SkillName.Tracking, Utility.RandomMinMax( 5, 20 ) ); break;
					}
					break;
				case 46 : 	Name = "skull";							ItemID = 0x4D12;		break;	// Creature Skull
				case 47 : 	Name = "fishing hook";					ItemID = 0x4D13;		SkillBonuses.SetValues( 4, SkillName.Seafaring, Utility.RandomMinMax( 5, 20 ) );		break;	// Fishing Hook
				case 48 : 	Name = "coin";							ItemID = 0x4D14;		break;	// Coin
				case 49 : 	Name = "head";							ItemID = 0x4D15;		break;	// Monster Head
				case 50 : 	Name = "brazier"; if ( Hue == 0 ){ Hue = 0xB17; } ItemID = 0x4D16; Attributes.NightSight = 1; break; // Brazier
			}

			Resource = CraftResource.None;
			Layer = Layer.Trinket;
			Weight = 1.0;
			if ( Utility.RandomMinMax( 1, 4 ) == 1 )
				Hue = Utility.RandomColor(0);

			switch ( Utility.Random( 15 ) )
			{
				case 0:		Name = "mystical " + Name;		break;
				case 1:		Name = "magical " + Name;		break;
				case 2:		Name = "enchanted " + Name;		break;
				case 3:		Name = "unusual " + Name;		break;
				case 4:		Name = "ancient " + Name;		break;
				case 5:		Name = "strange " + Name;		break;
				case 6:		Name = "charmed " + Name;		break;
				case 7:		Name = "extraordinary " + Name;	break;
				case 8:		Name = "weird " + Name;			break;
				case 9:		Name = "mythical " + Name;		break;
				case 10:	Name = "mysterious " + Name;	break;
				case 11:	Name = "wondrous " + Name;		break;
				case 12:	Name = "marvelous " + Name;		break;
				case 13:	Name = "marvelous " + Name;		break;
				case 14:	Name = "marvelous " + Name;		break;
			}
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Trinket");
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Trinkets are equipped on your hip." );
			return;
		}

		public TrinketTalisman( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}