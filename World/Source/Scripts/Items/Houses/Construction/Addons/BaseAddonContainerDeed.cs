using System;

using Server;
using Server.Multis;
using Server.Targeting;
using Server.Engines.Craft;

namespace Server.Items
{
    [Flipable(0x14F0, 0x14EF)]
    public abstract class BaseAddonContainerDeed : Item, ICraftable
    {
        public abstract BaseAddonContainer Addon { get; }

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

        public BaseAddonContainerDeed() : base(0x14F0)
        {
            Weight = 1.0;

            if (!Core.AOS)
                LootType = LootType.Newbied;
        }

        public BaseAddonContainerDeed(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            // version 1
            writer.Write((int)m_Resourced);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    m_Resourced = (CraftResource)reader.ReadInt();
                    break;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
                from.Target = new InternalTarget(this);
            else
                from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (CraftResources.GetClilocLowerCaseName(m_Resourced) > 0 && m_SubResource == CraftResource.None)
                list.Add(CraftResources.GetClilocLowerCaseName(m_Resourced));
        }

        #region ICraftable
        public virtual int OnCraft(int quality, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue)
        {
            Type resourceType = typeRes;

            if (resourceType == null)
                resourceType = craftItem.Resources.GetAt(0).ItemType;

            Resourced = CraftResources.GetFromType(resourceType);

            CraftContext context = craftSystem.GetContext(from);

            if (context != null && context.DoNotColor)
                Hue = 0;

            return quality;
        }
        #endregion

        private class InternalTarget : Target
        {
            private BaseAddonContainerDeed m_Deed;

            public InternalTarget(BaseAddonContainerDeed deed) : base(-1, true, TargetFlags.None)
            {
                m_Deed = deed;

                CheckLOS = false;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                IPoint3D p = targeted as IPoint3D;
                Map map = from.Map;

                if (p == null || map == null || m_Deed.Deleted)
                    return;

                if (m_Deed.IsChildOf(from.Backpack))
                {
                    BaseAddonContainer addon = m_Deed.Addon;
                    addon.Resourced = m_Deed.Resourced;

                    Server.Spells.SpellHelper.GetSurfaceTop(ref p);

                    BaseHouse house = null;

                    AddonFitResult res = addon.CouldFit(p, map, from, ref house);

                    if (res == AddonFitResult.Valid)
                        addon.MoveToWorld(new Point3D(p), map);
                    else if (res == AddonFitResult.Blocked)
                        from.SendLocalizedMessage(500269); // You cannot build that there.
                    else if (res == AddonFitResult.NotInHouse)
                        from.SendLocalizedMessage(500274); // You can only place this in a house that you own!
                    else if (res == AddonFitResult.DoorsNotClosed)
                        from.SendMessage("You must close all house doors before placing this.");
                    else if (res == AddonFitResult.DoorTooClose)
                        from.SendLocalizedMessage(500271); // You cannot build near the door.
                    else if (res == AddonFitResult.NoWall)
                        from.SendLocalizedMessage(500268); // This object needs to be mounted on something.

                    if (res == AddonFitResult.Valid)
                    {
                        m_Deed.Delete();
                        house.Addons.Add(addon);
                        house.AddSecure(from, addon);
                    }
                    else
                    {
                        addon.Delete();
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                }
            }
        }
    }
}
