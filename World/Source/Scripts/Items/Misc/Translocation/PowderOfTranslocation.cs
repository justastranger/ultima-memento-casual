using System;
using Server;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public interface ITranslocationItem
    {
        int Charges { get; set; }
        int Recharges { get; set; }
        int MaxCharges { get; }
        int MaxRecharges { get; }
        string TranslocationItemName { get; }
    }

    public class PowderOfTranslocation : Item
    {
        [Constructable]
        public PowderOfTranslocation() : this(1)
        {
        }

        [Constructable]
        public PowderOfTranslocation(int amount) : base(0x26B8)
        {
            Stackable = true;
            Weight = 0.1;
            Amount = amount;
        }



        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 2))
            {
                from.Target = new InternalTarget(this);
            }
            else
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            }
        }

        private class InternalTarget : Target
        {
            private PowderOfTranslocation m_Powder;

            public InternalTarget(PowderOfTranslocation powder) : base(-1, false, TargetFlags.None)
            {
                m_Powder = powder;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Powder.Deleted)
                    return;

                if (!from.InRange(m_Powder.GetWorldLocation(), 2))
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                }
                else if (targeted is ITranslocationItem)
                {
                    ITranslocationItem transItem = (ITranslocationItem)targeted;

                    if (transItem.Charges >= transItem.MaxCharges)
                    {
                        MessageHelper.SendLocalizedMessageTo(m_Powder, from, 1054137, 0x59); // This item cannot absorb any more powder of translocation.
                    }
                    else if (transItem.Recharges >= transItem.MaxRecharges)
                    {
                        MessageHelper.SendLocalizedMessageTo(m_Powder, from, 1054138, 0x59); // This item has been oversaturated with powder of translocation and can no longer be recharged.
                    }
                    else
                    {
                        if (transItem.Charges + m_Powder.Amount > transItem.MaxCharges)
                        {
                            int delta = transItem.MaxCharges - transItem.Charges;

                            m_Powder.Amount -= delta;
                            transItem.Charges = transItem.MaxCharges;
                            transItem.Recharges += delta;
                        }
                        else
                        {
                            transItem.Charges += m_Powder.Amount;
                            transItem.Recharges += m_Powder.Amount;
                            m_Powder.Delete();
                        }

                        if (transItem is Item)
                        {
                            // The ~1_translocationItem~ glows with green energy and absorbs magical power from the powder.
                            MessageHelper.SendLocalizedMessageTo((Item)transItem, from, 1054139, transItem.TranslocationItemName, 0x43);
                        }
                    }
                }
                else
                {
                    MessageHelper.SendLocalizedMessageTo(m_Powder, from, 1054140, 0x59); // Powder of translocation has no effect on this item.
                }
            }
        }

        public PowderOfTranslocation(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}