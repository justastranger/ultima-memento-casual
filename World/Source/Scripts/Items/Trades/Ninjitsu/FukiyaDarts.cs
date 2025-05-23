using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
    public class FukiyaDarts : Item, ICraftable, INinjaAmmo
    {
        private int m_UsesRemaining;

        private Poison m_Poison;
        private int m_PoisonCharges;

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get { return m_UsesRemaining; }
            set { m_UsesRemaining = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Poison Poison
        {
            get { return m_Poison; }
            set { m_Poison = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PoisonCharges
        {
            get { return m_PoisonCharges; }
            set { m_PoisonCharges = value; InvalidateProperties(); }
        }

        public bool ShowUsesRemaining { get { return true; } set { } }

        [Constructable]
        public FukiyaDarts() : this(1)
        {
        }

        [Constructable]
        public FukiyaDarts(int amount) : base(0x2806)
        {
            Weight = 1.0;

            m_UsesRemaining = amount;
        }

        public FukiyaDarts(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060584, "{0}\t{1}", m_UsesRemaining.ToString(), "Uses");

            if (m_Poison != null && m_PoisonCharges > 0)
                list.Add(1062412 + m_Poison.Level, m_PoisonCharges.ToString());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)m_UsesRemaining);

            Poison.Serialize(m_Poison, writer);
            writer.Write((int)m_PoisonCharges);
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

                        m_Poison = Poison.Deserialize(reader);
                        m_PoisonCharges = reader.ReadInt();

                        break;
                    }
            }
        }

        public int OnCraft(int quality, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue)
        {
            if (quality == 2)
                UsesRemaining *= 2;

            return quality;
        }
    }
}