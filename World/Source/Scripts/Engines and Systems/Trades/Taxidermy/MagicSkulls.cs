using System;
using Server;
using Server.Mobiles;
using System.Collections.Generic;
using System.Collections;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Targeting;

namespace Server.Misc
{
    class Skulls
    {
        public static void MakeSkull(Mobile m, Container c, Mobile killer, string where)
        {
            int bone = 0;
            int color = m.Hue;

            if (m is DemonOfTheSea) { color = 490; bone = 2; }
            else if (m is BloodDemon) { bone = 2; }
            else if (m is Devil) { bone = 2; }
            else if (m is TitanPyros) { color = 0x846; bone = 2; }
            else if (m is Balron) { color = m.Hue; bone = 2; }
            else if (m is Archfiend) { color = 0x846; bone = 2; }
            else if (m is Satan) { color = 0x4AA; bone = 2; }
            else if (m is LesserDemon) { bone = 2; }
            else if (m is Xurtzar) { bone = 2; }
            else if (m is FireDemon) { bone = 2; }
            else if (m is Demon) { bone = 2; }
            else if (m is IceDevil) { color = 0x47F; bone = 2; }
            else if (m is DeepSeaDevil) { color = 490; bone = 2; }
            else if (m is Daemon) { color = m.Hue; bone = 2; }
            else if (m is Fiend) { color = 0x846; bone = 2; }
            else if (m is DaemonTemplate) { color = 0x846; bone = 2; }
            else if (m is Daemonic) { bone = 2; }
            else if (m is BlackGateDemon) { color = 0x497; bone = 2; }

            else if (m is DeepSeaGiant) { color = 1365; bone = 3; }
            else if (m is Titan) { bone = 3; }
            else if (m is ElderTitan) { bone = 3; }
            else if (m is Ettin) { bone = 3; }
            else if (m is Giant) { bone = 3; }
            else if (m is HillGiant) { bone = 3; }
            else if (m is HillGiantShaman) { bone = 3; }
            else if (m is AncientEttin) { bone = 3; }
            else if (m is AncientCyclops) { bone = 3; }
            else if (m is IceGiant) { bone = 3; }
            else if (m is LavaGiant) { bone = 3; }
            else if (m is MountainGiant) { bone = 3; }
            else if (m is ArcticEttin) { color = 0x47E; bone = 3; }
            else if (m is EttinShaman) { bone = 3; }
            else if (m is StoneGiant) { bone = 3; }
            else if (m is FireGiant) { color = 0x54F; bone = 3; }
            else if (m is ForestGiant) { color = 0x7D5; bone = 3; }
            else if (m is FrostGiant) { color = 0x482; bone = 3; }
            else if (m is JungleGiant) { color = 0x7D1; bone = 3; }
            else if (m is OrkDemigod) { bone = 3; }
            else if (m is UndeadGiant) { bone = 3; }
            else if (m is ZombieGiant) { bone = 3; }
            else if (m is GiantSkeleton) { bone = 3; }
            else if (m is SeaGiant) { color = 490; bone = 3; }
            else if (m is ShamanicCyclops) { bone = 3; }
            else if (m is Cyclops) { bone = 3; }
            else if (m is StormGiant) { color = 0x9C2; bone = 3; }
            else if (m is CloudGiant) { color = 0xBB4; bone = 3; }
            else if (m is StarGiant) { color = 0xB73; bone = 3; }
            else if (m is ZornTheBlacksmith) { bone = 3; }

            else if (m is GrayDragon) { bone = 1; }
            else if (m is Dragons) { bone = 1; }
            else if (m is RidingDragon) { bone = 1; }
            else if (m is Dragoon) { bone = 1; }
            else if (m is AsianDragon) { bone = 1; }
            else if (m is SeaDragon) { bone = 1; }
            else if (m is DragonGolem) { bone = 1; }
            else if (m is GemDragon) { bone = 1; }
            else if (m is Wyverns) { bone = 1; }

            else if (m is CaddelliteDragon) { bone = 4; }
            else if (m is AncientWyrm) { bone = 4; }
            else if (m is AncientWyvern) { bone = 4; }
            else if (m is SkeletalDragon) { bone = 4; }
            else if (m is Dracolich) { bone = 4; }
            else if (m is BottleDragon) { bone = 4; }
            else if (m is RadiationDragon) { bone = 4; }
            else if (m is CrystalDragon) { bone = 4; }
            else if (m is VoidDragon) { bone = 4; }
            else if (m is ShadowWyrm) { color = 0x497; bone = 4; }
            else if (m is ZombieDragon) { bone = 4; }
            else if (m is VolcanicDragon) { bone = 4; }
            else if (m is PrimevalFireDragon) { bone = 4; }
            else if (m is PrimevalGreenDragon) { bone = 4; }
            else if (m is PrimevalNightDragon) { bone = 4; }
            else if (m is PrimevalRedDragon) { bone = 4; }
            else if (m is PrimevalRoyalDragon) { bone = 4; }
            else if (m is PrimevalRunicDragon) { bone = 4; }
            else if (m is PrimevalSeaDragon) { bone = 4; }
            else if (m is ReanimatedDragon) { bone = 4; }
            else if (m is VampiricDragon) { bone = 4; }
            else if (m is PrimevalAbysmalDragon) { bone = 4; }
            else if (m is PrimevalAmberDragon) { bone = 4; }
            else if (m is PrimevalBlackDragon) { bone = 4; }
            else if (m is PrimevalDragon) { bone = 4; }
            else if (m is PrimevalSilverDragon) { bone = 4; }
            else if (m is PrimevalVolcanicDragon) { bone = 4; }
            else if (m is PrimevalStygianDragon) { bone = 4; }
            else if (m is AshDragon) { bone = 4; }
            else if (m is DragonKing) { bone = 4; }
            else if (m is ElderDragon) { bone = 4; }
            else if (m is SlasherOfVoid) { bone = 4; }

            else if (m is RottingMinotaur) { bone = 5; }
            else if (m is MinotaurCaptain) { bone = 5; }
            else if (m is MinotaurScout) { bone = 5; }
            else if (m is Minotaur) { bone = 5; }

            else if (m is Dracolich) { bone = 6; }
            else if (m is SkeletalDragon) { bone = 6; }
            else if (m is Wyrms) { color = 1150; }

            else if (m is Tyranasaur) { bone = 7; }
            else if (m is Stegosaurus) { bone = 7; }
            else if (m is PackStegosaurus) { bone = 7; }
            else if (m is Brontosaur) { bone = 7; }

            else if (m is VampirePrince) { bone = 8; }
            else if (m is VampireLord) { bone = 8; }
            else if (m is Dracula) { bone = 8; }

            if (bone == 1)
            {
                SkullDragon head = new SkullDragon();
                head.Name = "skull of " + m.Name;
                if (m.Title != "") { head.Name = head.Name + " " + m.Title; }

                head.Hue = color;
                head.SkullWhere = where;
                head.SkullKiller = killer.Name + " the " + Server.Misc.GetPlayerInfo.GetSkillTitle(killer);
                c.DropItem(head);
            }
            else if (bone == 2)
            {
                if (Utility.RandomMinMax(1, 10) == 1)
                {
                    int headz = Utility.RandomMinMax(1, 3);

                    if (headz == 1)
                    {
                        DeamonHeadA head = new DeamonHeadA();
                        head.Name = "head of " + m.Name;
                        if (m.Title != "") { head.Name = head.Name + " " + m.Title; }

                        head.Hue = color;
                        head.SkullWhere = where;
                        head.SkullKiller = killer.Name + " the " + Server.Misc.GetPlayerInfo.GetSkillTitle(killer);
                        c.DropItem(head);
                    }
                    else if (headz == 2)
                    {
                        DeamonHeadB head = new DeamonHeadB();
                        head.Name = "head of " + m.Name;
                        if (m.Title != "") { head.Name = head.Name + " " + m.Title; }

                        head.Hue = color;
                        head.SkullWhere = where;
                        head.SkullKiller = killer.Name + " the " + Server.Misc.GetPlayerInfo.GetSkillTitle(killer);
                        c.DropItem(head);
                    }
                    else
                    {
                        DeamonHeadC head = new DeamonHeadC();
                        head.Name = "head of " + m.Name;
                        if (m.Title != "") { head.Name = head.Name + " " + m.Title; }

                        head.Hue = color;
                        head.SkullWhere = where;
                        head.SkullKiller = killer.Name + " the " + Server.Misc.GetPlayerInfo.GetSkillTitle(killer);
                        c.DropItem(head);
                    }
                }
                else
                {
                    SkullDemon head = new SkullDemon();
                    head.Name = "skull of " + m.Name;
                    if (m.Title != "") { head.Name = head.Name + " " + m.Title; }

                    head.Hue = color;
                    head.SkullWhere = where;
                    head.SkullKiller = killer.Name + " the " + Server.Misc.GetPlayerInfo.GetSkillTitle(killer);
                    c.DropItem(head);
                }
            }
            else if (bone == 3)
            {
                SkullGiant head = new SkullGiant();
                head.Name = "skull of " + m.Name;
                if (m.Title != "") { head.Name = head.Name + " " + m.Title; }

                head.Hue = color;
                head.SkullWhere = where;
                head.SkullKiller = killer.Name + " the " + Server.Misc.GetPlayerInfo.GetSkillTitle(killer);
                c.DropItem(head);
            }
            else if (bone == 4)
            {
                SkullGreatDragon head = new SkullGreatDragon();
                head.Name = "skull of " + m.Name;
                if (m.Title != "") { head.Name = head.Name + " " + m.Title; }

                head.Hue = color;
                head.SkullWhere = where;
                head.SkullKiller = killer.Name + " the " + Server.Misc.GetPlayerInfo.GetSkillTitle(killer);
                c.DropItem(head);
            }
            else if (bone == 5)
            {
                SkullMinotaur head = new SkullMinotaur();
                head.Name = "skull of " + m.Name;
                if (m.Title != "") { head.Name = head.Name + " " + m.Title; }

                head.Hue = color;
                head.SkullWhere = where;
                head.SkullKiller = killer.Name + " the " + Server.Misc.GetPlayerInfo.GetSkillTitle(killer);
                c.DropItem(head);
            }
            else if (bone == 6)
            {
                SkullWyrm head = new SkullWyrm();
                head.Name = "skull of " + m.Name;
                if (m.Title != "") { head.Name = head.Name + " " + m.Title; }

                head.Hue = color;
                head.SkullWhere = where;
                head.SkullKiller = killer.Name + " the " + Server.Misc.GetPlayerInfo.GetSkillTitle(killer);
                c.DropItem(head);
            }
            else if (bone == 7)
            {
                SkullDinosaur head = new SkullDinosaur();
                head.Name = "skull of " + m.Name;
                if (m.Title != "") { head.Name = head.Name + " " + m.Title; }

                head.SkullWhere = where;
                head.SkullKiller = killer.Name + " the " + Server.Misc.GetPlayerInfo.GetSkillTitle(killer);
                c.DropItem(head);
            }
            else if (bone == 8)
            {
                VampireHead head = new VampireHead();
                head.Name = "head of " + m.Name;
                if (m.Title != "") { head.Name = head.Name + " " + m.Title; }

                head.SkullWhere = where;
                head.SkullKiller = killer.Name + " the " + Server.Misc.GetPlayerInfo.GetSkillTitle(killer);
                c.DropItem(head);
            }

            if (m is LavaGiant && Utility.RandomMinMax(1, 2) == 1)
            {
                HeartOfFire heart = new HeartOfFire();
                heart.Name = "heart of " + m.Name;
                if (m.Title != "") { heart.Name = heart.Name + " " + m.Title; }

                heart.HeartWhere = where;
                heart.HeartKiller = killer.Name + " the " + Server.Misc.GetPlayerInfo.GetSkillTitle(killer);
                c.DropItem(heart);
            }
            else if (m is IceGiant && Utility.RandomMinMax(1, 2) == 1)
            {
                HeartOfIce heart = new HeartOfIce();
                heart.Name = "heart of " + m.Name;
                if (m.Title != "") { heart.Name = heart.Name + " " + m.Title; }

                heart.HeartWhere = where;
                heart.HeartKiller = killer.Name + " the " + Server.Misc.GetPlayerInfo.GetSkillTitle(killer);
                c.DropItem(heart);
            }
        }
    }
}

namespace Server.Items
{
    public class BaseSkull : Item
    {
        public string SkullKiller;
        public string SkullWhere;

        [CommandProperty(AccessLevel.Owner)]
        public string Skull_Killer { get { return SkullKiller; } set { SkullKiller = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public string Skull_Where { get { return SkullWhere; } set { SkullWhere = value; InvalidateProperties(); } }

        [Constructable]
        public BaseSkull(int itemId) : base(itemId)
        {
            Weight = 10.0;
        }

        public BaseSkull(Serial serial) : base(serial)
        {

        }
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "From " + SkullWhere);
            list.Add(1049644, "Slain by " + SkullKiller);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(SkullKiller);
            writer.Write(SkullWhere);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            SkullKiller = reader.ReadString();
            SkullWhere = reader.ReadString();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile)
            {
                PlayerMobile player = (PlayerMobile)from;
                player.Target = new InternalTarget(this, player);
            }
        }

        private class InternalTarget : Target
        {
            private BaseSkull i_skull;
            private PlayerMobile i_source;
            public InternalTarget(BaseSkull skull, PlayerMobile from) : base(8, true, TargetFlags.None)
            {
                i_skull = skull;
                i_source = from;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is DemonGate && (i_skull is SkullDemon || i_skull is DeamonHeadA || i_skull is DeamonHeadB || i_skull is DeamonHeadC))
                {
                    DemonGate gate = (DemonGate)targeted;
                    gate.Agitate(i_source, i_skull);
                }
                else
                {
                    from.SendMessage("Nothing happens.");
                }
            }
        }
    }
    public class BaseHeart : Item
    {
        public string HeartKiller;
        public string HeartWhere;

        [CommandProperty(AccessLevel.Owner)]
        public string Heart_Killer { get { return HeartKiller; } set { HeartKiller = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.Owner)]
        public string Heart_Where { get { return HeartWhere; } set { HeartWhere = value; InvalidateProperties(); } }

        [Constructable]
        public BaseHeart(int itemId) : base(itemId)
        {
            Weight = 10.0;
        }

        public BaseHeart(Serial serial) : base(serial)
        {

        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "From " + HeartWhere);
            list.Add(1049644, "Slain by " + HeartKiller);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(HeartKiller);
            writer.Write(HeartWhere);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            HeartKiller = reader.ReadString();
            HeartWhere = reader.ReadString();
        }
    }
    public class HeartOfIce : BaseHeart
    {

        [Constructable]
        public HeartOfIce() : base(0x1444)
        {
            Hue = 0x480;
            Movable = true;
            Light = LightType.Circle225;
            Name = "ice heart";
        }

        public HeartOfIce(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

    public class HeartOfFire : BaseHeart
    {
        [Constructable]
        public HeartOfFire() : base(0x81E)
        {
            Movable = true;
            Light = LightType.Circle225;
            Name = "fire heart";
        }

        public HeartOfFire(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

    [Furniture]
    [Flipable(0x3DE0, 0x3DE1)]
    public class SkullMinotaur : BaseSkull
    {
        [Constructable]
        public SkullMinotaur() : base(0x3DE0)
        {
            Name = "minotaur skull";
        }

        public SkullMinotaur(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }

    }

    [Furniture]
    [Flipable(0x65CA, 0x65CB)]
    public class SkullDinosaur : BaseSkull
    {
        [Constructable]
        public SkullDinosaur() : base(0x65CA)
        {
            Name = "dinosaur skull";
        }

        public SkullDinosaur(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

    [Furniture]
    [Flipable(0x3DCC, 0x3DCD)]
    public class SkullWyrm : BaseSkull
    {
        [Constructable]
        public SkullWyrm() : base(0x3DCC)
        {
            Name = "dragon skull";
        }

        public SkullWyrm(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

    [Furniture]
    [Flipable(0x1AEE, 0x1AEF)]
    public class SkullGreatDragon : BaseSkull
    {
        [Constructable]
        public SkullGreatDragon() : base(0x1AEE)
        {
            Weight = 20.0;
            Name = "dragon skull";
        }

        public SkullGreatDragon(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

    [Furniture]
    [Flipable(8782, 8783)]
    public class SkullDragon : BaseSkull
    {
        [Constructable]
        public SkullDragon() : base(8782)
        {
            Name = "dragon skull";
        }

        public SkullDragon(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

    [Furniture]
    [Flipable(8784, 8785)]
    public class SkullDemon : BaseSkull
    {
        [Constructable]
        public SkullDemon() : base(8784)
        {
            Name = "demon skull";
        }

        public SkullDemon(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

    [Furniture]
    [Flipable(0x224, 0x225)]
    public class SkullGiant : BaseSkull
    {
        [Constructable]
        public SkullGiant() : base(0x224)
        {
            Name = "giant skull";
        }

        public SkullGiant(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

    [Furniture]
    [Flipable(0x65C2, 0x65C3)]
    public class DeamonHeadA : BaseSkull
    {
        [Constructable]
        public DeamonHeadA() : base(0x65C2)
        {
            Name = "demon head";
        }

        public DeamonHeadA(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

    [Furniture]
    [Flipable(0x65C4, 0x65C5)]
    public class DeamonHeadB : BaseSkull
    {
        [Constructable]
        public DeamonHeadB() : base(0x65C4)
        {
            Name = "demon head";
        }

        public DeamonHeadB(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
    [Furniture]
    [Flipable(0x65C6, 0x65C7)]
    public class DeamonHeadC : BaseSkull
    {
        [Constructable]
        public DeamonHeadC() : base(0x65C6)
        {
            Name = "demon head";
        }

        public DeamonHeadC(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
    [Furniture]
    [Flipable(0x65C8, 0x65C9)]
    public class VampireHead : BaseSkull
    {
        [Constructable]
        public VampireHead() : base(0x65C8)
        {
            Name = "vampire head";
        }

        public VampireHead(Serial serial) : base(serial)
        {
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}