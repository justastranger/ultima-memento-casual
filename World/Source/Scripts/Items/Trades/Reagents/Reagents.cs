using System;using Server;using Server.Network;using System.Text;using Server.Items;using Server.Mobiles;namespace Server.Items{
    public class reagents_magic_jar1 : Item
    {
        public override Catalogs DefaultCatalog { get { return Catalogs.Reagent; } }

        [Constructable]
        public reagents_magic_jar1() : base(0x1007)
        {
            // SELL FOR 2000 GOLD //
            Name = "Jar of Wizard Reagents";
        }

        public reagents_magic_jar1(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("This must be in your backpack to use.");
                return;
            }
            else
            {
                from.AddToBackpack(new BlackPearl(50));
                from.AddToBackpack(new Bloodmoss(50));
                from.AddToBackpack(new Garlic(50));
                from.AddToBackpack(new Ginseng(50));
                from.AddToBackpack(new MandrakeRoot(50));
                from.AddToBackpack(new Nightshade(50));
                from.AddToBackpack(new SulfurousAsh(50));
                from.AddToBackpack(new SpidersSilk(50));
                from.PrivateOverheadMessage(MessageType.Regular, 0x14C, false, "You dump the contents into your backpack", from.NetState);
                this.Delete();
            }
        }        public override void AddNameProperties(ObjectPropertyList list)
        {            base.AddNameProperties(list);

            list.Add(1070722, "This Jar Contains 50 Of Each Wizard Reagent");
            list.Add(1049644, "Double-Click To Empty Into Your Pack");        }

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
    public class reagents_magic_jar2 : Item
    {
        public override Catalogs DefaultCatalog { get { return Catalogs.Reagent; } }

        [Constructable]
        public reagents_magic_jar2() : base(0x1007)
        {
            // SELL FOR 1500 GOLD //
            Name = "Jar of Necromancer Reagents";
            Hue = 0xB97;
        }

        public reagents_magic_jar2(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.AddToBackpack(new BatWing(50));
            from.AddToBackpack(new GraveDust(50));
            from.AddToBackpack(new DaemonBlood(50));
            from.AddToBackpack(new NoxCrystal(50));
            from.AddToBackpack(new PigIron(50));
            from.AddToBackpack(new BitterRoot(50));
            from.AddToBackpack(new BlackSand(50));
            from.AddToBackpack(new BloodRose(50));
            from.AddToBackpack(new DriedToad(50));
            from.AddToBackpack(new Maggot(50));
            from.AddToBackpack(new MummyWrap(50));
            from.AddToBackpack(new VioletFungus(50));
            from.AddToBackpack(new WerewolfClaw(50));
            from.AddToBackpack(new Wolfsbane(50));
            from.PrivateOverheadMessage(MessageType.Regular, 0x14C, false, "You dump the contents into your backpack", from.NetState);
            this.Delete();
        }        public override void AddNameProperties(ObjectPropertyList list)
        {            base.AddNameProperties(list);

            list.Add(1070722, "This Jar Contains 50 Of Each Necromancer Reagent");
            list.Add(1049644, "Double-Click To Empty Into Your Pack");        }

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
    public class reagents_magic_jar3 : Item
    {
        public override Catalogs DefaultCatalog { get { return Catalogs.Reagent; } }

        [Constructable]
        public reagents_magic_jar3() : base(0x1007)
        {
            // SELL FOR 5000 GOLD //
            Name = "Jar of Alchemical Reagents";
            Hue = 0x488;
        }

        public reagents_magic_jar3(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.AddToBackpack(new BlackPearl(50));
            from.AddToBackpack(new Bloodmoss(50));
            from.AddToBackpack(new Garlic(50));
            from.AddToBackpack(new Ginseng(50));
            from.AddToBackpack(new MandrakeRoot(50));
            from.AddToBackpack(new Nightshade(50));
            from.AddToBackpack(new SulfurousAsh(50));
            from.AddToBackpack(new SpidersSilk(50));
            from.AddToBackpack(new Brimstone(50));
            from.AddToBackpack(new ButterflyWings(50));
            from.AddToBackpack(new EyeOfToad(50));
            from.AddToBackpack(new FairyEgg(50));
            from.AddToBackpack(new GargoyleEar(50));
            from.AddToBackpack(new BeetleShell(50));
            from.AddToBackpack(new MoonCrystal(50));
            from.AddToBackpack(new PixieSkull(50));
            from.AddToBackpack(new RedLotus(50));
            from.AddToBackpack(new SeaSalt(50));
            from.AddToBackpack(new SilverWidow(50));
            from.AddToBackpack(new SwampBerries(50));

            from.PrivateOverheadMessage(MessageType.Regular, 0x14C, false, "You dump the contents into your backpack", from.NetState);
            this.Delete();
        }        public override void AddNameProperties(ObjectPropertyList list)
        {            base.AddNameProperties(list);

            list.Add(1070722, "This Jar Contains 50 Of Each Alchemical Reagent");
            list.Add(1049644, "Double-Click To Empty Into Your Pack");        }

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
    }}