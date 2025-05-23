using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Network;
using Server.Gumps;
using Server.Multis;
using Server.ContextMenus;

namespace Server.Items
{
    [FlipableAttribute(0x234C, 0x234D)]
    public class RoseOfMoon : Item, ISecurable
    {
        private static readonly TimeSpan m_SpawnTime = TimeSpan.FromHours(4.0);

        private int m_Petals;
        private DateTime m_NextSpawnTime;
        private SpawnTimer m_SpawnTimer;

        private SecureLevel m_Level;

        public override int LabelNumber { get { return 1062913; } } // Rose of Moon

        [CommandProperty(AccessLevel.GameMaster)]
        public SecureLevel Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Petals
        {
            get { return m_Petals; }
            set
            {
                if (value >= 10)
                {
                    m_Petals = 10;

                    StopSpawnTimer();
                }
                else
                {
                    if (value <= 0)
                        m_Petals = 0;
                    else
                        m_Petals = value;

                    StartSpawnTimer(m_SpawnTime);
                }

                InvalidateProperties();
            }
        }

        [Constructable]
        public RoseOfMoon() : base(0x234D)
        {
            Weight = 1.0;
            LootType = LootType.Blessed;

            m_Petals = 0;
            StartSpawnTimer(TimeSpan.FromMinutes(1.0));
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1062925, Petals.ToString()); // Petals:  ~1_COUNT~
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            SetSecureLevelEntry.AddTo(from, this, list);
        }

        private void StartSpawnTimer(TimeSpan delay)
        {
            StopSpawnTimer();

            m_SpawnTimer = new SpawnTimer(this, delay);
            m_SpawnTimer.Start();

            m_NextSpawnTime = DateTime.Now + delay;
        }

        private void StopSpawnTimer()
        {
            if (m_SpawnTimer != null)
            {
                m_SpawnTimer.Stop();
                m_SpawnTimer = null;
            }
        }

        private class SpawnTimer : Timer
        {
            private RoseOfMoon m_Rose;

            public SpawnTimer(RoseOfMoon rose, TimeSpan delay) : base(delay)
            {
                m_Rose = rose;

                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                if (m_Rose.Deleted)
                    return;

                m_Rose.m_SpawnTimer = null;
                m_Rose.Petals++;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            }
            else if (Petals > 0)
            {
                from.AddToBackpack(new RoseOfMoonPetal(Petals));
                Petals = 0;
            }
        }

        public RoseOfMoon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)0); // version

            writer.WriteEncodedInt((int)m_Petals);
            writer.WriteDeltaTime((DateTime)m_NextSpawnTime);
            writer.WriteEncodedInt((int)m_Level);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            m_Petals = reader.ReadEncodedInt();
            m_NextSpawnTime = reader.ReadDeltaTime();
            m_Level = (SecureLevel)reader.ReadEncodedInt();

            if (m_Petals < 10)
                StartSpawnTimer(m_NextSpawnTime - DateTime.Now);
        }
    }

    public class RoseOfMoonPetal : Item
    {
        public override int LabelNumber { get { return 1062926; } } // Petal of the Rose of Moon

        [Constructable]
        public RoseOfMoonPetal() : this(1)
        {
        }

        [Constructable]
        public RoseOfMoonPetal(int amount) : base(0x1021)
        {
            Stackable = true;
            Amount = amount;

            Weight = 1.0;
            Hue = 0xE;
        }



        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042038); // You must have the object in your backpack to use it.
            }
            else if (from.GetStatMod("RoseOfMoonPetal") != null)
            {
                from.SendLocalizedMessage(1062927); // You have eaten one of these recently and eating another would provide no benefit.
            }
            else
            {
                from.PlaySound(0x1EE);
                from.AddStatMod(new StatMod(StatType.Str, "RoseOfMoonPetal", 5, TimeSpan.FromMinutes(5.0)));

                Consume();
            }
        }

        public RoseOfMoonPetal(Serial serial) : base(serial)
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