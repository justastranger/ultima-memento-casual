using System;
using System.Collections.Generic;
using Server.Multis;
using Server.ContextMenus;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	[Furniture]
	[Flipable( 0xE3D, 0xE3C )]
	public class MerchantCrate : Container
	{
		public override string DefaultDescription
		{
			get
			{
				if ( !MySettings.S_MerchantCrates )
					return "These crates are commonly used by merchants to store wares.";

				return "Merchant crates are something that craftsmen can secure in their homes and sell the goods they make. Once a day, someone from the Merchants Guild will pick up anything left in the crate. They will leave gold in the crate for the owner's hard work. When you put something in the crate, you will be aware of the gold value of the item placed in it. A craftsmen may only acquire gold from armor, weapons, or clothing that were crafted. Any non-crafted armor, weapons, or clothing will be valued at 0 gold. Crafted armor and weapons will have varying values depending on the resources used to create the item. Also durability and quality may increase the value. Almost anything crafted will have a value to the Merchants Guild. Other items like potions, scrolls, tools, furniture, or food can be sold for a price. Any tools must have at least 50 uses to be of any value. So if an item cannot be crafted, then you probably will not get any gold for it. The exception to this are ingots and logs, as they are highly sought in the land. Different types of ingots are worth more depending on the type.<br><br>The crate will indicate how much gold it has available to transfer to yourself in the form of a bank check. Single click the crate and select the 'Transfer' option to withdraw all of the gold from the crate. Although there is a gold value indicated on the crate, the one withdrawing the amount may get more depending on whether they are in the Merchants Guild and/or they have a good Mercantile skill. These crates must be secured in a home to be of any use.";
			}
		}

		public override bool DisplaysContent{ get{ return false; } }
		public override bool DisplayWeight{ get{ return false; } }

		public override int DefaultMaxWeight{ get{ return 0; } } // A value of 0 signals unlimited weight

		public override bool IsDecoContainer{ get{ return false; } }

		public int CrateGold;

		[CommandProperty(AccessLevel.Owner)]
		public int Crate_Gold{ get { return CrateGold; } set { CrateGold = value; InvalidateProperties(); } }

		[Constructable]
		public MerchantCrate() : base( 0xE3D )
		{
			Name = "merchant crate";
			Hue = 0x83F;
			Weight = 1.0;
		}

		public MerchantCrate( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 2 ); // version
            writer.Write( CrateGold );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			CrateGold = reader.ReadInt();

			if (version < 2)
			{
				AddItem(new DungeoneerCrate());
			}
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list ) 
		{ 
			base.GetContextMenuEntries( from, list ); 
			if ( !this.Movable && BaseHouse.CheckAccessible( from, this ) && CrateGold > 0 ){ list.Add( new CashOutEntry( from, this ) ); }
		} 

		public class CashOutEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private MerchantCrate m_Crate;
	
			public CashOutEntry( Mobile from, MerchantCrate crate ) : base( 6113, 3 )
			{
				m_Mobile = from;
				m_Crate = crate;
			}

			public override void OnClick()
			{
			    if( !( m_Mobile is PlayerMobile ) )
				return;
				
				PlayerMobile mobile = (PlayerMobile) m_Mobile;
				{
					if ( m_Crate.CrateGold > 0 )
					{
						double barter = (int)( m_Mobile.Skills[SkillName.Mercantile].Value / 2 );

						if ( mobile.NpcGuild == NpcGuild.MerchantsGuild )
							barter = barter + 25.0; // FOR GUILD MEMBERS

						barter = barter / 100;

						int bonus = (int)( m_Crate.CrateGold * barter );

						int cash = m_Crate.CrateGold + bonus;

						m_Mobile.AddToBackpack( new BankCheck( cash ) );
						m_Mobile.SendMessage("You now have a check for " + cash.ToString() + " gold.");
						m_Crate.CrateGold = 0;
						m_Crate.InvalidateProperties();
					}
					else
					{
						m_Mobile.SendMessage("There is no gold in this crate!");
					}
				}
            }
        }

		public override void OnDoubleClick( Mobile from )
		{
			if ( !MySettings.S_MerchantCrates )
			{
				base.OnDoubleClick( from );
				return;
			}

			if ( this.Movable )
			{
                from.SendMessage("This must be locked down in a house to use!");
			}
			else if ( from.AccessLevel > AccessLevel.Counselor || from.InRange( this.GetWorldLocation(), 2 ) )
			{
				Open( from );
			}
			else
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
			}
		}

		public virtual void Open( Mobile from )
		{
			DisplayTo( from );
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			if ( MySettings.S_MerchantCrates )
			{
				list.Add( 1049644, "Contains: " + CrateGold.ToString() + " Gold");
			}
        }
	}
}