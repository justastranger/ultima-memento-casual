using System;
using System.Collections.Generic;
using Server;
using Server.Multis;

namespace Server.Mobiles
{
    public class VendorInventory
    {
        public static readonly TimeSpan GracePeriod = TimeSpan.FromDays(7.0);

        private BaseHouse m_House;
        private string m_VendorName;
        private string m_ShopName;
        private Mobile m_Owner;

        private List<Item> m_Items;
        private int m_Gold;

        private DateTime m_ExpireTime;
        private Timer m_ExpireTimer;

        public VendorInventory(BaseHouse house, Mobile owner, string vendorName, string shopName)
        {
            m_House = house;
            m_Owner = owner;
            m_VendorName = vendorName;
            m_ShopName = shopName;

            m_Items = new List<Item>();

            m_ExpireTime = DateTime.Now + GracePeriod;
            m_ExpireTimer = new ExpireTimer(this, GracePeriod);
            m_ExpireTimer.Start();
        }

        public BaseHouse House
        {
            get { return m_House; }
            set { m_House = value; }
        }

        public string VendorName
        {
            get { return m_VendorName; }
            set { m_VendorName = value; }
        }

        public string ShopName
        {
            get { return m_ShopName; }
            set { m_ShopName = value; }
        }

        public Mobile Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }

        public List<Item> Items
        {
            get { return m_Items; }
        }

        public int Gold
        {
            get { return m_Gold; }
            set { m_Gold = value; }
        }

        public DateTime ExpireTime
        {
            get { return m_ExpireTime; }
        }

        public void AddItem(Item item)
        {
            item.Internalize();
            m_Items.Add(item);
        }

        public void Delete()
        {
            foreach (Item item in Items)
            {
                item.Delete();
            }

            Items.Clear();
            Gold = 0;

            if (House != null)
                House.VendorInventories.Remove(this);

            m_ExpireTimer.Stop();
        }

        public void Serialize(GenericWriter writer)
        {
            writer.WriteEncodedInt(0); // version

            writer.Write((Mobile)m_Owner);
            writer.Write((string)m_VendorName);
            writer.Write((string)m_ShopName);

            writer.Write(m_Items, true);
            writer.Write((int)m_Gold);

            writer.WriteDeltaTime(m_ExpireTime);
        }

        public VendorInventory(BaseHouse house, GenericReader reader)
        {
            m_House = house;

            int version = reader.ReadEncodedInt();

            m_Owner = reader.ReadMobile();
            m_VendorName = reader.ReadString();
            m_ShopName = reader.ReadString();

            m_Items = reader.ReadStrongItemList();
            m_Gold = reader.ReadInt();

            m_ExpireTime = reader.ReadDeltaTime();

            if (m_Items.Count == 0 && m_Gold == 0)
            {
                Timer.DelayCall(TimeSpan.Zero, new TimerCallback(Delete));
            }
            else
            {
                TimeSpan delay = m_ExpireTime - DateTime.Now;
                m_ExpireTimer = new ExpireTimer(this, delay > TimeSpan.Zero ? delay : TimeSpan.Zero);
                m_ExpireTimer.Start();
            }
        }

        private class ExpireTimer : Timer
        {
            private VendorInventory m_Inventory;

            public ExpireTimer(VendorInventory inventory, TimeSpan delay) : base(delay)
            {
                m_Inventory = inventory;

                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                BaseHouse house = m_Inventory.House;

                if (house != null)
                {
                    if (m_Inventory.Gold > 0)
                    {
                        if (house.MovingCrate == null)
                            house.MovingCrate = new MovingCrate(house);

                        Banker.Deposit(house.MovingCrate, m_Inventory.Gold);
                    }

                    foreach (Item item in m_Inventory.Items)
                    {
                        if (!item.Deleted)
                            house.DropToMovingCrate(item);
                    }

                    m_Inventory.Gold = 0;
                    m_Inventory.Items.Clear();
                }

                m_Inventory.Delete();
            }
        }
    }
}