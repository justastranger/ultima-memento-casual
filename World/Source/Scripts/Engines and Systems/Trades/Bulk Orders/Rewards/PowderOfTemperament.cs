using System;
using Server;
using Server.Targeting;
using Server.Engines.Craft;
using Server.Network;

namespace Server.Items
{
    public class PowderOfTemperament : Item, IUsesRemaining
    {
        private int m_UsesRemaining;

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get { return m_UsesRemaining; }
            set { m_UsesRemaining = value; InvalidateProperties(); }
        }

        public bool ShowUsesRemaining { get { return true; } set { } }

        public override int LabelNumber { get { return 1049082; } } // powder of temperament

        [Constructable]
        public PowderOfTemperament() : this(10)
        {
        }

        [Constructable]
        public PowderOfTemperament(int charges) : base(4102)
        {
            Weight = 1.0;
            Hue = 2419;
            UsesRemaining = charges;
        }

        public PowderOfTemperament(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.Write((int)m_UsesRemaining);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_UsesRemaining = reader.ReadInt();
                        break;
                    }
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060584, "{0}\t{1}", m_UsesRemaining.ToString(), "Uses");
        }

        public virtual void DisplayDurabilityTo(Mobile m)
        {
            LabelToAffix(m, 1017323, AffixType.Append, ": " + m_UsesRemaining.ToString()); // Durability
        }

        public override void OnSingleClick(Mobile from)
        {
            DisplayDurabilityTo(from);

            base.OnSingleClick(from);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
                from.Target = new InternalTarget(this);
            else
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
        }

        private class InternalTarget : Target
        {
            private PowderOfTemperament m_Powder;

            public InternalTarget(PowderOfTemperament powder) : base(2, false, TargetFlags.None)
            {
                m_Powder = powder;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Powder.Deleted || m_Powder.UsesRemaining <= 0)
                {
                    from.SendLocalizedMessage(1049086); // You have used up your powder of temperament.
                    return;
                }

                if (targeted is IDurability && targeted is Item)
                {
                    IDurability wearable = (IDurability)targeted;
                    Item item = (Item)targeted;

                    if (!wearable.CanFortify)
                    {
                        from.SendLocalizedMessage(1049083); // You cannot use the powder on that item.
                        return;
                    }

                    if (item.IsChildOf(from.Backpack) && m_Powder.IsChildOf(from.Backpack))
                    {
                        int origMaxHP = wearable.MaxHitPoints;
                        int origCurHP = wearable.HitPoints;

                        if (origMaxHP > 0)
                        {
                            int initMaxHP = Core.AOS ? 255 : wearable.InitMaxHits;

                            wearable.UnscaleDurability();

                            if (wearable.MaxHitPoints < initMaxHP)
                            {
                                int bonus = initMaxHP - wearable.MaxHitPoints;

                                if (bonus > 10)
                                    bonus = 10;

                                wearable.MaxHitPoints += bonus;
                                wearable.HitPoints += bonus;

                                wearable.ScaleDurability();

                                if (wearable.MaxHitPoints > 255) wearable.MaxHitPoints = 255;
                                if (wearable.HitPoints > 255) wearable.HitPoints = 255;

                                if (wearable.MaxHitPoints > origMaxHP)
                                {
                                    from.SendLocalizedMessage(1049084); // You successfully use the powder on the item.

                                    --m_Powder.UsesRemaining;

                                    if (m_Powder.UsesRemaining <= 0)
                                    {
                                        from.SendLocalizedMessage(1049086); // You have used up your powder of temperament.
                                        m_Powder.Delete();
                                    }
                                }
                                else
                                {
                                    wearable.MaxHitPoints = origMaxHP;
                                    wearable.HitPoints = origCurHP;
                                    from.SendLocalizedMessage(1049085); // The item cannot be improved any further.
                                }
                            }
                            else
                            {
                                from.SendLocalizedMessage(1049085); // The item cannot be improved any further.
                                wearable.ScaleDurability();
                            }
                        }
                        else
                        {
                            from.SendLocalizedMessage(1049083); // You cannot use the powder on that item.
                        }
                    }
                    else
                    {
                        from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1049083); // You cannot use the powder on that item.
                }
            }
        }
    }
}