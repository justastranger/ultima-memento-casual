using System;
using Server;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
    public class Hold : Container
    {
        private int m_MaxWeightDefault = 800;

        private BaseBoat m_Boat;

        public Hold(BaseBoat boat) : base(0x3EAE)
        {
            m_Boat = boat;
            Movable = false;
            GumpID = 76;

            if (BaseBoat.isCarpet(m_Boat))
            {
                ItemID = 0x541E;
                Name = "magic bag";
                GumpID = 0x3D;
                DropSound = 0x48;
            }

            m_MaxWeightDefault = 1000;

            if (boat is LargeDragonBoat) m_MaxWeightDefault = 3200;
            else if (boat is MediumDragonBoat) m_MaxWeightDefault = 2200;
            else if (boat is SmallDragonBoat) m_MaxWeightDefault = 1400;
            else if (boat is LargeBoat) m_MaxWeightDefault = 2600;
            else if (boat is MediumBoat) m_MaxWeightDefault = 1800;
        }

        public override int DefaultMaxWeight { get { return m_MaxWeightDefault; } }

        public Hold(Serial serial) : base(serial)
        {
        }

        public void SetFacing(Direction dir)
        {
            if (BaseBoat.isCarpet(m_Boat))
            {
                switch (dir)
                {
                    case Direction.East: ItemID = 0x541F; break;
                    case Direction.West: ItemID = 0x541E; break;
                    case Direction.North: ItemID = 0x541E; break;
                    case Direction.South: ItemID = 0x541F; break;
                }
            }
            else
            {
                switch (dir)
                {
                    case Direction.East: ItemID = 0x3E65; break;
                    case Direction.West: ItemID = 0x3E93; break;
                    case Direction.North: ItemID = 0x3EAE; break;
                    case Direction.South: ItemID = 0x3EB9; break;
                }
            }
        }

        public override bool OnDragDrop(Mobile from, Item item)
        {
            if (m_Boat == null || !m_Boat.Contains(from) || m_Boat.IsMoving)
                return false;

            return base.OnDragDrop(from, item);
        }

        public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
        {
            if (m_Boat == null || !m_Boat.Contains(from) || m_Boat.IsMoving)
                return false;

            return base.OnDragDropInto(from, item, p);
        }

        public override bool CheckItemUse(Mobile from, Item item)
        {
            if (item != this && (m_Boat == null || !m_Boat.Contains(from) || m_Boat.IsMoving))
                return false;

            return base.CheckItemUse(from, item);
        }

        public override bool CheckLift(Mobile from, Item item, ref LRReason reject)
        {
            if (m_Boat == null || !m_Boat.Contains(from) || m_Boat.IsMoving)
                return false;

            return base.CheckLift(from, item, ref reject);
        }

        public override void OnAfterDelete()
        {
            if (m_Boat != null)
                m_Boat.Delete();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (m_Boat == null || !m_Boat.Contains(from))
            {
                if (m_Boat.TillerMan != null)
                    m_Boat.TillerMan.Say(BaseBoat.translateText(m_Boat, 502490)); // You must be on the ship to open the hold.
            }
            else if (m_Boat.IsMoving)
            {
                if (m_Boat.TillerMan != null)
                    m_Boat.TillerMan.Say(BaseBoat.translateText(m_Boat, 502491)); // I can not open the hold while the ship is moving.
            }
            else
            {
                base.OnDoubleClick(from);
            }
        }

        public override bool IsDecoContainer
        {
            get { return false; }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write(m_Boat);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        m_Boat = reader.ReadItem() as BaseBoat;

                        if (m_Boat == null || Parent != null)
                        {
                            Delete();
                        }
                        else
                        {
                            if (m_Boat is LargeDragonBoat) m_MaxWeightDefault = 3200;
                            else if (m_Boat is MediumDragonBoat) m_MaxWeightDefault = 2200;
                            else if (m_Boat is SmallDragonBoat) m_MaxWeightDefault = 1400;
                            else if (m_Boat is LargeBoat) m_MaxWeightDefault = 2600;
                            else if (m_Boat is MediumBoat) m_MaxWeightDefault = 1800;
                            else { m_MaxWeightDefault = 1000; }
                        }

                        Movable = false;

                        break;
                    }
            }
        }
    }
}