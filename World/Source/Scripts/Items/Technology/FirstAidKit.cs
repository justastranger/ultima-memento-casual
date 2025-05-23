using System;using Server;using Server.Network;using System.Text;using Server.Items;using Server.Mobiles;namespace Server.Items{
    public class FirstAidKit : Item
    {
        [Constructable]
        public FirstAidKit() : base(0x27FD)
        {
            Name = "first aid kit";
            Weight = 10;
            Technology = true;
        }

        public FirstAidKit(Serial serial) : base(serial)
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
                Item item = new Bandage(); item.Amount = Utility.RandomMinMax(5, 30); from.AddToBackpack(item);

                if (Utility.RandomMinMax(1, 2) == 1) { item = new LesserHealPotion(); Server.Items.BasePotion.MakePillBottle(item); item.Amount = Utility.RandomMinMax(1, 10); from.AddToBackpack(item); }
                if (Utility.RandomMinMax(1, 2) == 1) { item = new LesserCurePotion(); Server.Items.BasePotion.MakePillBottle(item); item.Amount = Utility.RandomMinMax(1, 10); from.AddToBackpack(item); }
                if (Utility.RandomMinMax(1, 2) == 1) { item = new LesserRejuvenatePotion(); Server.Items.BasePotion.MakePillBottle(item); item.Amount = Utility.RandomMinMax(1, 10); from.AddToBackpack(item); }

                if (Utility.RandomMinMax(1, 4) == 1) { item = new HealPotion(); Server.Items.BasePotion.MakePillBottle(item); item.Amount = Utility.RandomMinMax(1, 5); from.AddToBackpack(item); }
                if (Utility.RandomMinMax(1, 4) == 1) { item = new CurePotion(); Server.Items.BasePotion.MakePillBottle(item); item.Amount = Utility.RandomMinMax(1, 5); from.AddToBackpack(item); }
                if (Utility.RandomMinMax(1, 4) == 1) { item = new RefreshPotion(); Server.Items.BasePotion.MakePillBottle(item); item.Amount = Utility.RandomMinMax(1, 5); from.AddToBackpack(item); }
                if (Utility.RandomMinMax(1, 4) == 1) { item = new RejuvenatePotion(); Server.Items.BasePotion.MakePillBottle(item); item.Amount = Utility.RandomMinMax(1, 5); from.AddToBackpack(item); }

                if (Utility.RandomMinMax(1, 10) == 1) { item = new TotalRefreshPotion(); Server.Items.BasePotion.MakePillBottle(item); item.Amount = Utility.RandomMinMax(1, 3); from.AddToBackpack(item); }
                if (Utility.RandomMinMax(1, 10) == 1) { item = new GreaterCurePotion(); Server.Items.BasePotion.MakePillBottle(item); item.Amount = Utility.RandomMinMax(1, 3); from.AddToBackpack(item); }
                if (Utility.RandomMinMax(1, 10) == 1) { item = new GreaterHealPotion(); Server.Items.BasePotion.MakePillBottle(item); item.Amount = Utility.RandomMinMax(1, 3); from.AddToBackpack(item); }
                if (Utility.RandomMinMax(1, 10) == 1) { item = new GreaterRejuvenatePotion(); Server.Items.BasePotion.MakePillBottle(item); item.Amount = Utility.RandomMinMax(1, 3); from.AddToBackpack(item); }

                from.PrivateOverheadMessage(MessageType.Regular, 0x14C, false, "You dump the contents out into your pack.", from.NetState);
                this.Delete();
            }
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
    }}