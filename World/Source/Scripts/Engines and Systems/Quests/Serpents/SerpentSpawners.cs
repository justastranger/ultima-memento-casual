using Server.Mobiles;

namespace Server.Items
{
    public class SerpentSpawnerOrder : Item
    {
        [Constructable]
        public SerpentSpawnerOrder() : base(0x25C0)
        {
            Name = "Serpent of Order";
            Weight = 1.0;
            Hue = 0x4AB;
        }

        public override void OnDoubleClick(Mobile from)
        {
            Item snake = from.Backpack.FindItemByType(typeof(BlackrockSerpentOrder));
            if (snake != null)
            {
                BaseCreature monster = new SerpentOfOrder();
                monster.MoveToWorld(this.Location, this.Map);
                monster.PlaySound(0x217);

                from.SendMessage("The Serpent of Order comes forth to challenge you!");
                snake.Delete();
                from.AddToBackpack(new BlackrockSerpentOrderDecoration());
            }
            else
            {
                from.SendMessage("The statue glows with an eerie blue color.");
            }
        }

        public SerpentSpawnerOrder(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SerpentSpawnerChaos : Item
    {
        [Constructable]
        public SerpentSpawnerChaos() : base(0x25C0)
        {
            Name = "Serpent of Chaos";
            Weight = 1.0;
            Hue = 0x4AA;
        }

        public override void OnDoubleClick(Mobile from)
        {
            Item snake = from.Backpack.FindItemByType(typeof(BlackrockSerpentChaos));
            if (snake != null)
            {
                BaseCreature monster = new SerpentOfChaos();
                monster.MoveToWorld(this.Location, this.Map);
                monster.PlaySound(0x217);

                from.SendMessage("The Serpent of Chaos comes forth to challenge you!");
                snake.Delete();
                from.AddToBackpack(new BlackrockSerpentOrderDecoration());
            }
            else
            {
                from.SendMessage("The statue glows with an eerie red color.");
            }
        }

        public SerpentSpawnerChaos(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class BlackrockSerpentOrderDecoration : Item
    {
        [Constructable]
        public BlackrockSerpentOrderDecoration() : base(0x25C0)
        {
            Name = "Inert Blackrock Serpent";
            Weight = 1.0;
            Hue = 0x96C;
        }

        public BlackrockSerpentOrderDecoration(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class BlackrockSerpentChaosDecoration : Item
    {
        [Constructable]
        public BlackrockSerpentChaosDecoration() : base(0x25C0)
        {
            Name = "Inert Blackrock Serpent";
            Weight = 1.0;
            Hue = 0x96C;
        }

        public BlackrockSerpentChaosDecoration(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}