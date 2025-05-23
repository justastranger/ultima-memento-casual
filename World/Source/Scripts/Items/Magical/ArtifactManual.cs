using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using System.Collections.Generic;
using Server.Misc;
using System.Collections;
using Server.Targeting;

namespace Server.Items
{
    public enum ArtyBookEffect
    {
        Charges
    }

    public class ArtifactManual : Item
    {
        private ArtyBookEffect m_ArtyBookEffect;
        private int m_Charges;

        [CommandProperty(AccessLevel.GameMaster)]
        public ArtyBookEffect Effect
        {
            get { return m_ArtyBookEffect; }
            set { m_ArtyBookEffect = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Charges
        {
            get { return m_Charges; }
            set { m_Charges = value; InvalidateProperties(); }
        }

        [Constructable]
        public ArtifactManual() : base(0xF05)
        {
            Name = "Encyclopedia of Rarities";
            Hue = 0x961;
            ItemID = Utility.RandomList(0xEA9, 0xF05);
            Charges = Utility.RandomMinMax(5, 25);
            Weight = 1.0;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "This Identifies Items");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Charges > 0)
            {
                Target t;

                if (!IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1060640); // The item must be in your backpack to use it.
                }
                else
                {
                    from.SendMessage("What do you want to research with this?");
                    t = new BookTarget(this);
                    from.Target = t;
                }
            }
            else
            {
                from.SendMessage("Finding nothing about it, you throw it away.");
                this.Delete();
            }
        }

        private class BookTarget : Target
        {
            private ArtifactManual m_Book;

            public BookTarget(ArtifactManual researched) : base(1, false, TargetFlags.None)
            {
                m_Book = researched;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (Server.Items.ArtifactManual.LookupTheItem(from, targeted)) { m_Book.Charges = m_Book.Charges - 1; }
            }
        }

        public static bool LookupTheItem(Mobile from, object targeted)
        {
            bool useCharges = false;

            if (targeted is Item)
            {
                Item iBook = targeted as Item;

                if (!iBook.IsChildOf(from.Backpack))
                {
                    from.SendMessage("You can only examine an item in your pack.");
                }
                else if ((iBook.IsChildOf(from.Backpack)) && (iBook is NotIdentified)) //////////////////////////////////////////////////////////////////////////
                {
                    useCharges = true;
                    Container pack = (Container)iBook;
                    List<Item> items = new List<Item>();
                    foreach (Item item in pack.Items)
                    {
                        items.Add(item);
                    }
                    foreach (Item item in items)
                    {
                        from.AddToBackpack(item);
                    }

                    from.SendMessage("You successfully identify the item.");
                    iBook.Delete();
                }
                else
                {
                    from.SendMessage("You cannot find any information on that.");
                }
            }
            else
            {
                from.SendMessage("You cannot find any information on that.");
            }

            return useCharges;
        }

        public ArtifactManual(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write((int)m_ArtyBookEffect);
            writer.Write((int)m_Charges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 0:
                    {
                        m_ArtyBookEffect = (ArtyBookEffect)reader.ReadInt();
                        m_Charges = (int)reader.ReadInt();

                        break;
                    }
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(1060741, m_Charges.ToString());
        }
    }
}