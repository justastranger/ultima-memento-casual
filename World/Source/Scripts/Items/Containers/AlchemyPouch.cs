using System;
using System.Linq;
using Server;

namespace Server.Items
{
	[Flipable( 0x1C10, 0x1CC6 )]
    public class AlchemyPouch : LargeSack
    {
		public override CraftResource DefaultResource{ get{ return CraftResource.RegularLeather; } }

		[Constructable]
		public AlchemyPouch() : base()
		{
			Weight = 1.0;
			MaxItems = 50;
			Name = "alchemy rucksack";
			Hue = 0x89F;
		}

		public override bool OnDragDropInto( Mobile from, Item dropped, Point3D p )
        {
			if ( dropped is Container && !(dropped is AlchemyPouch) )
			{
                from.SendMessage("You can only use another alchemy rucksack within this sack.");
                return false;
			}
            else if ( dropped.Catalog == Catalogs.Reagent || 
						dropped is GodBrewing || 
						dropped is Bottle || 
						dropped is Jar || 
						dropped is MortarPestle || 
						dropped is DruidCauldron || 
						dropped is WitchCauldron || 
						dropped is AlchemyPouch )
			{
				return base.OnDragDropInto(from, dropped, p);
			}
			else if (dropped is NotIdentified && dropped.Items.First().Catalog == Catalogs.Reagent)
			{
                return base.OnDragDropInto(from, dropped, p);
            }

			from.SendMessage("This rucksack is for small alchemical crafting items.");
			return false;
        }

		public override bool OnDragDrop( Mobile from, Item dropped )
        {
			if ( dropped is Container && !(dropped is AlchemyPouch) )
			{
                from.SendMessage("You can only use another alchemy rucksack within this sack.");
                return false;
			}
            else if ( dropped.Catalog == Catalogs.Reagent || 
						dropped is GodBrewing || 
						dropped is Bottle || 
						dropped is Jar || 
						dropped is MortarPestle || 
						dropped is WitchCauldron || 
						dropped is DruidCauldron || 
						dropped is AlchemyPouch )
			{
				return base.OnDragDrop(from, dropped);
			}

			from.SendMessage("This rucksack is for small alchemical crafting items.");
			return false;
        }

		public AlchemyPouch( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			Weight = 1.0;
			MaxItems = 50;
			Name = "alchemy rucksack";
		}

		public override int GetTotal(TotalType type)
        {
			// 1/20 weight ratio
			if (type == TotalType.Weight)
            {
                return (int)(TotalItemWeights() * (0.05));
            }
			// presenting as a single item so the variety of ingredients doesn't max out your item stack limit
			else if (type == TotalType.Items)
			{
				return 1;
			}
            else
            {
                return base.GetTotal(type);
            }
        }

        public override void UpdateTotal(Item sender, TotalType type, int delta)
        {
            if (type == TotalType.Weight)
            {
                base.UpdateTotal(sender, type, (int)(delta * (0.05)));
            }
			else if (type == TotalType.Gold)
			{
				base.UpdateTotal(sender, type, delta);
			}
			// don't update items count, it should stay at 1
        }

		private double TotalItemWeights()
        {
			double weight = 0.0;

			foreach (Item item in Items)
				weight += (item.Weight * (double)(item.Amount));

			return weight;
        }
	}
}