using System;

namespace Server.Engines.Harvest
{
    public class HarvestBank
    {
        private int m_Current;
        private int m_Maximum;
        private DateTime m_NextRespawn;
        private HarvestVein m_Vein, m_DefaultVein;

        HarvestDefinition m_Definition;

        public HarvestDefinition Definition
        {
            get { return m_Definition; }
        }

        public bool IsEmpty { get { return m_Current < 1; } }

        public int Current
        {
            get
            {
                CheckRespawn();
                return m_Current;
            }
        }

        public HarvestVein Vein
        {
            get
            {
                CheckRespawn();
                return m_Vein;
            }
            set
            {
                m_Vein = value;
            }
        }

        public HarvestVein DefaultVein
        {
            get
            {
                CheckRespawn();
                return m_DefaultVein;
            }
        }

        public void CheckRespawn()
        {
            if (m_Current == m_Maximum || m_NextRespawn > DateTime.Now)
                return;

            m_Current = m_Maximum;

            if (m_Definition.RandomizeVeins)
            {
                m_DefaultVein = m_Definition.GetVeinFrom(Utility.RandomDouble());
            }

            m_Vein = m_DefaultVein;
        }

        public virtual void Consume(int amount, Mobile from)
        {
            CheckRespawn();

            if (m_Current == m_Maximum)
            {
                double min = m_Definition.MinRespawn.TotalMinutes;
                double max = m_Definition.MaxRespawn.TotalMinutes;
                double rnd = Utility.RandomDouble();

                m_Current = m_Maximum - amount;
                double minutes = min + (rnd * (max - min));
                m_NextRespawn = DateTime.Now + TimeSpan.FromMinutes(minutes);
            }
            else
            {
                m_Current -= amount;
            }

            if (m_Current < 0)
                m_Current = 0;
        }

        public void Deplete(Mobile from)
        {
            Consume(m_Current, from);
        }

        public HarvestBank(HarvestDefinition def, HarvestVein defaultVein)
        {
            m_Maximum = Utility.RandomMinMax(def.MinTotal, def.MaxTotal);
            m_Current = m_Maximum;
            m_DefaultVein = defaultVein;
            m_Vein = m_DefaultVein;

            m_Definition = def;
        }
    }
}