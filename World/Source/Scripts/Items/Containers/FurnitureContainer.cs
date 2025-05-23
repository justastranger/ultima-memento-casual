using System;
using System.Collections.Generic;
using Server;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
    [Furniture]
    [Flipable(0x2815, 0x2816)]
    public class TallCabinet : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public TallCabinet() : base(0x2815)
        {
            Weight = 1.0;
        }

        public TallCabinet(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x2817, 0x2818)]
    public class ShortCabinet : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public ShortCabinet() : base(0x2817)
        {
            Weight = 1.0;
        }

        public ShortCabinet(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x2857, 0x2858)]
    public class RedArmoire : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public RedArmoire() : base(0x2857)
        {
            Weight = 1.0;
        }

        public RedArmoire(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x285D, 0x285E)]
    public class CherryArmoire : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public CherryArmoire() : base(0x285D)
        {
            Weight = 1.0;
        }

        public CherryArmoire(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x285B, 0x285C)]
    public class MapleArmoire : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public MapleArmoire() : base(0x285B)
        {
            Weight = 1.0;
        }

        public MapleArmoire(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x2859, 0x285A)]
    public class ElegantArmoire : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public ElegantArmoire() : base(0x2859)
        {
            Weight = 1.0;
        }

        public ElegantArmoire(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0xa97, 0xa99, 0xa98, 0xa9a, 0xa9b, 0xa9c)]
    public class FullBookcase : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public FullBookcase() : base(0xA97)
        {
            Weight = 1.0;
        }

        public FullBookcase(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0xa9d, 0xa9e)]
    public class EmptyBookcase : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public EmptyBookcase() : base(0xA9D)
        {
        }

        public EmptyBookcase(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version == 0 && Weight == 1.0)
                Weight = -1;
        }
    }

    [Furniture]
    [Flipable(0x544F, 0x5450)]
    public class CounterFancy : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public CounterFancy() : base(0x544F)
        {
            Name = "counter";
            Weight = 1.0;
            GumpID = 0x48;
        }

        public CounterFancy(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x5451, 0x5452)]
    public class CounterWood : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public CounterWood() : base(0x5451)
        {
            Name = "counter";
            Weight = 1.0;
            GumpID = 0x48;
        }

        public CounterWood(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x5453, 0x5454)]
    public class CounterWooden : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public CounterWooden() : base(0x5453)
        {
            Name = "counter";
            Weight = 1.0;
            GumpID = 0x48;
        }

        public CounterWooden(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x5455, 0x5456)]
    public class CounterStained : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public CounterStained() : base(0x5455)
        {
            Name = "counter";
            Weight = 1.0;
            GumpID = 0x48;
        }

        public CounterStained(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x5457, 0x5458)]
    public class CounterPolished : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public CounterPolished() : base(0x5457)
        {
            Name = "counter";
            Weight = 1.0;
            GumpID = 0x48;
        }

        public CounterPolished(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x5459, 0x545A)]
    public class CounterRustic : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public CounterRustic() : base(0x5459)
        {
            Name = "counter";
            Weight = 1.0;
            GumpID = 0x48;
        }

        public CounterRustic(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x545B, 0x545C)]
    public class CounterDark : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public CounterDark() : base(0x545B)
        {
            Name = "counter";
            Weight = 1.0;
            GumpID = 0x48;
        }

        public CounterDark(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0x545D, 0x545E)]
    public class CounterLight : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public CounterLight() : base(0x545D)
        {
            Name = "counter";
            Weight = 1.0;
            GumpID = 0x48;
        }

        public CounterLight(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0xa2c, 0xa34)]
    public class Drawer : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public Drawer() : base(0xA2C)
        {
            Weight = 1.0;
        }

        public Drawer(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0xa30, 0xa38)]
    public class FancyDrawer : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public FancyDrawer() : base(0xA30)
        {
            Weight = 1.0;
        }

        public FancyDrawer(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    [Furniture]
    [Flipable(0xa4f, 0xa53)]
    public class Armoire : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public Armoire() : base(0xA4F)
        {
            Weight = 1.0;
        }

        public override void DisplayTo(Mobile m)
        {
            if (DynamicFurniture.Open(this, m))
                base.DisplayTo(m);
        }

        public Armoire(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            DynamicFurniture.Close(this);
        }
    }

    [Furniture]
    [Flipable(0xa4d, 0xa51)]
    public class FancyArmoire : BaseContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public FancyArmoire() : base(0xA4D)
        {
            Weight = 1.0;
        }

        public override void DisplayTo(Mobile m)
        {
            if (DynamicFurniture.Open(this, m))
                base.DisplayTo(m);
        }

        public FancyArmoire(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            DynamicFurniture.Close(this);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////
    [Furniture]
    [Flipable(0x4102, 0x4106)]
    public class SkullChest : LockableContainer
    {
        public override CraftResource DefaultResource { get { return CraftResource.RegularWood; } }

        [Constructable]
        public SkullChest() : base(0x4102)
        {
            Weight = 10.0;
            Name = "Skull Chest";
            GumpID = 0x49;
        }

        public override void DisplayTo(Mobile m)
        {
            if (DynamicFurniture.Open(this, m))
                base.DisplayTo(m);
        }

        public SkullChest(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            DynamicFurniture.Close(this);
        }
    }
    ////////////////////////////////////////////////////////////////////////////////////////

    public class DynamicFurniture
    {
        private static Dictionary<Container, Timer> m_Table = new Dictionary<Container, Timer>();

        public static bool Open(Container c, Mobile m)
        {
            if (m_Table.ContainsKey(c))
            {
                c.SendRemovePacket();
                Close(c);
                c.Delta(ItemDelta.Update);
                c.ProcessDelta();
                return false;
            }

            if ((c is Armoire || c is FancyArmoire || c is SkullChest) && !(c.RootParent is Mobile) && !(c.RootParent is Corpse) && !(c.RootParent is Container))
            {
                Timer t = new FurnitureTimer(c, m);
                t.Start();
                m_Table[c] = t;

                switch (c.ItemID)
                {
                    case 0xA4D: c.ItemID = 0xA4C; break;
                    case 0xA4F: c.ItemID = 0xA4E; break;
                    case 0xA51: c.ItemID = 0xA50; break;
                    case 0xA53: c.ItemID = 0xA52; break;
                    case 0x4102: c.ItemID = 0x4104; break;
                    case 0x4106: c.ItemID = 0x4109; break;
                }
            }

            return true;
        }

        public static void Close(Container c)
        {
            Timer t = null;

            m_Table.TryGetValue(c, out t);

            if (t != null)
            {
                t.Stop();
                m_Table.Remove(c);
            }

            if (c is Armoire || c is FancyArmoire || c is SkullChest)
            {
                switch (c.ItemID)
                {
                    case 0xA4C: c.ItemID = 0xA4D; break;
                    case 0xA4E: c.ItemID = 0xA4F; break;
                    case 0xA50: c.ItemID = 0xA51; break;
                    case 0xA52: c.ItemID = 0xA53; break;
                    case 0x4104: c.ItemID = 0x4102; break;
                    case 0x4109: c.ItemID = 0x4106; break;
                }
            }
        }
    }

    public class FurnitureTimer : Timer
    {
        private Container m_Container;
        private Mobile m_Mobile;

        public FurnitureTimer(Container c, Mobile m) : base(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.5))
        {
            Priority = TimerPriority.TwoFiftyMS;

            m_Container = c;
            m_Mobile = m;
        }

        protected override void OnTick()
        {
            if (m_Mobile.Map != m_Container.Map || !m_Mobile.InRange(m_Container.GetWorldLocation(), 3))
                DynamicFurniture.Close(m_Container);
        }
    }
}