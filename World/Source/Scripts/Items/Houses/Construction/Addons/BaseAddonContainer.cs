using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Multis;
using Server.Regions;

namespace Server.Items
{
    public abstract class BaseAddonContainer : BaseContainer, IChopable, IAddon
    {
        public override bool DisplayWeight { get { return false; } }

        [Hue, CommandProperty(AccessLevel.GameMaster)]
        public override int Hue
        {
            get
            {
                return base.Hue;
            }
            set
            {
                if (base.Hue != value)
                {
                    base.Hue = value;

                    if (!Deleted && this.ShareHue && m_Components != null)
                    {
                        Hue = value;

                        foreach (AddonContainerComponent c in m_Components)
                            c.Hue = value;
                    }
                }
            }
        }

        private CraftResource m_Resourced;

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resourced
        {
            get { return m_Resourced; }
            set
            {
                if (m_Resourced != value)
                {
                    m_Resourced = value;
                    Hue = CraftResources.GetHue(m_Resourced);

                    InvalidateProperties();
                }
            }
        }

        Item IAddon.Deed
        {
            get { return this.Deed; }
        }

        public virtual bool RetainDeedHue { get { return false; } }
        public virtual bool NeedsWall { get { return false; } }
        public virtual bool ShareHue { get { return true; } }
        public virtual Point3D WallPosition { get { return Point3D.Zero; } }
        public virtual BaseAddonContainerDeed Deed { get { return null; } }

        private List<AddonContainerComponent> m_Components;

        public List<AddonContainerComponent> Components
        {
            get { return m_Components; }
        }

        public BaseAddonContainer(int itemID) : base(itemID)
        {
            AddonComponent.ApplyLightTo(this);

            m_Components = new List<AddonContainerComponent>();
        }

        public BaseAddonContainer(Serial serial) : base(serial)
        {
        }

        public override void OnLocationChange(Point3D oldLoc)
        {
            base.OnLocationChange(oldLoc);

            if (Deleted)
                return;

            foreach (AddonContainerComponent c in m_Components)
                c.Location = new Point3D(X + c.Offset.X, Y + c.Offset.Y, Z + c.Offset.Z);
        }

        public override void OnAfterSpawn()
        {
            foreach (AddonContainerComponent c in m_Components)
                c.Name = null;
        }

        public override void OnMapChange()
        {
            base.OnMapChange();

            if (Deleted)
                return;

            foreach (AddonContainerComponent c in m_Components)
                c.Map = Map;
        }

        public override void OnDelete()
        {
            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (house != null)
                house.Addons.Remove(this);

            base.OnDelete();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (CraftResources.GetClilocLowerCaseName(m_Resourced) > 0 && m_SubResource == CraftResource.None)
                list.Add(CraftResources.GetClilocLowerCaseName(m_Resourced));
        }

        public override void OnAfterDelete()
        {
            base.OnAfterDelete();

            foreach (AddonContainerComponent c in m_Components)
                c.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.WriteItemList<AddonContainerComponent>(m_Components);
            writer.Write((int)m_Resourced);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Components = reader.ReadStrongItemList<AddonContainerComponent>();
            m_Resourced = (CraftResource)reader.ReadInt();

            AddonComponent.ApplyLightTo(this);
        }

        public void DropItemsToGround()
        {
            for (int i = Items.Count - 1; i >= 0; i--)
                Items[i].MoveToWorld(Location);
        }

        public void AddComponent(AddonContainerComponent c, int x, int y, int z)
        {
            if (Deleted)
                return;

            m_Components.Add(c);

            c.Addon = this;
            c.Offset = new Point3D(x, y, z);
            c.MoveToWorld(new Point3D(X + x, Y + y, Z + z), Map);
        }

        public AddonFitResult CouldFit(IPoint3D p, Map map, Mobile from, ref BaseHouse house)
        {
            if (Deleted)
                return AddonFitResult.Blocked;

            foreach (AddonContainerComponent c in m_Components)
            {
                Point3D p3D = new Point3D(p.X + c.Offset.X, p.Y + c.Offset.Y, p.Z + c.Offset.Z);

                if (!map.CanFit(p3D.X, p3D.Y, p3D.Z, c.ItemData.Height, false, true, (c.Z == 0)))
                    return AddonFitResult.Blocked;
                else if (!BaseAddon.CheckHouse(from, p3D, map, c.ItemData.Height, ref house))
                    return AddonFitResult.NotInHouse;

                if (c.NeedsWall)
                {
                    Point3D wall = c.WallPosition;

                    if (!BaseAddon.IsWall(p3D.X + wall.X, p3D.Y + wall.Y, p3D.Z + wall.Z, map))
                        return AddonFitResult.NoWall;
                }
            }

            Point3D p3 = new Point3D(p.X, p.Y, p.Z);

            if (!map.CanFit(p3.X, p3.Y, p3.Z, ItemData.Height, false, true, (Z == 0)))
                return AddonFitResult.Blocked;
            else if (!BaseAddon.CheckHouse(from, p3, map, ItemData.Height, ref house))
                return AddonFitResult.NotInHouse;

            if (NeedsWall)
            {
                Point3D wall = WallPosition;

                if (!BaseAddon.IsWall(p3.X + wall.X, p3.Y + wall.Y, p3.Z + wall.Z, map))
                    return AddonFitResult.NoWall;
            }

            if (house != null)
            {
                ArrayList doors = house.Doors;

                for (int i = 0; i < doors.Count; ++i)
                {
                    BaseDoor door = doors[i] as BaseDoor;

                    if (door != null && door.Open)
                        return AddonFitResult.DoorsNotClosed;

                    Point3D doorLoc = door.GetWorldLocation();
                    int doorHeight = door.ItemData.CalcHeight;

                    foreach (AddonContainerComponent c in m_Components)
                    {
                        Point3D addonLoc = new Point3D(p.X + c.Offset.X, p.Y + c.Offset.Y, p.Z + c.Offset.Z);
                        int addonHeight = c.ItemData.CalcHeight;

                        if (Utility.InRange(doorLoc, addonLoc, 1) && (addonLoc.Z == doorLoc.Z || ((addonLoc.Z + addonHeight) > doorLoc.Z && (doorLoc.Z + doorHeight) > addonLoc.Z)))
                            return AddonFitResult.DoorTooClose;
                    }

                    Point3D addonLo = new Point3D(p.X, p.Y, p.Z);
                    int addonHeigh = ItemData.CalcHeight;

                    if (Utility.InRange(doorLoc, addonLo, 1) && (addonLo.Z == doorLoc.Z || ((addonLo.Z + addonHeigh) > doorLoc.Z && (doorLoc.Z + doorHeight) > addonLo.Z)))
                        return AddonFitResult.DoorTooClose;
                }
            }

            return AddonFitResult.Valid;
        }

        public bool CouldFit(IPoint3D p, Map map)
        {
            BaseHouse house = null;

            return (CouldFit(p, map, null, ref house) == AddonFitResult.Valid);
        }

        public virtual void OnChop(Mobile from)
        {
            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (house != null && (house.IsOwner(from) || house.IsCoOwner(from) || house.IsFriend(from) || house.IsGuildMember(from)))
            {
                if (!IsSecure)
                {
                    Effects.PlaySound(GetWorldLocation(), Map, 0x3B3);
                    from.SendLocalizedMessage(500461); // You destroy the item.

                    int hue = 0;

                    if (RetainDeedHue)
                    {
                        for (int i = 0; hue == 0 && i < m_Components.Count; ++i)
                        {
                            AddonContainerComponent c = m_Components[i];

                            if (c.Hue != 0)
                                hue = c.Hue;
                        }
                    }

                    DropItemsToGround();

                    Delete();

                    house.Addons.Remove(this);

                    BaseAddonContainerDeed deed = Deed;

                    if (deed != null)
                    {
                        deed.Resourced = Resourced;

                        if (RetainDeedHue)
                            deed.Hue = hue;

                        from.AddToBackpack(deed);
                    }
                }
                else
                    from.SendLocalizedMessage(1074870); // This item must be unlocked/unsecured before re-deeding it.
            }
        }

        public virtual void OnComponentLoaded(AddonContainerComponent c)
        {
        }

        public virtual void OnComponentUsed(AddonContainerComponent c, Mobile from)
        {
        }
    }
}
