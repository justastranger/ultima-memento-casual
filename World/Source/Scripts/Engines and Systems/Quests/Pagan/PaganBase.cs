using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Misc;
using Server.Network;
using Server.Accounting;
using Server.Mobiles;

namespace Server.Items
{
    public class PaganBase : BaseAddon
    {
        public int ItemType;

        [CommandProperty(AccessLevel.Owner)]
        public int Item_Type { get { return ItemType; } set { ItemType = value; InvalidateProperties(); } }

        [Constructable]
        public PaganBase(string pagan)
        {
            Light = LightType.Circle150;
            string sPed = "an ornately ";
            switch (Utility.RandomMinMax(0, 10))
            {
                case 0: sPed = "an ornately "; break;
                case 1: sPed = "a beautifully "; break;
                case 2: sPed = "an expertly "; break;
                case 3: sPed = "an artistically "; break;
                case 4: sPed = "an exquisitely "; break;
                case 5: sPed = "a decoratively "; break;
                case 6: sPed = "an ancient "; break;
                case 7: sPed = "an old "; break;
                case 8: sPed = "an unusually "; break;
                case 9: sPed = "a curiously "; break;
                case 10: sPed = "an oddly "; break;
            }
            sPed = sPed + "carved pedestal";

            int iThing = 0x1860;
            string sThing = "Breath of Air";
            int iColor = 0;
            int z = 2;
            ItemType = 1;
            if (pagan == "fire") { iThing = 0x1861; sThing = "Tongue of Flame"; ItemType = 2; }
            else if (pagan == "earth") { iThing = 0x1862; sThing = "Heart of Earth"; ItemType = 3; }
            else if (pagan == "water") { iThing = 0x1863; sThing = "Tear of the Seas"; ItemType = 4; }

            AddComplexComponent((BaseAddon)this, iThing, 0, 0, z, iColor, 29, sThing, 1);
            AddComplexComponent((BaseAddon)this, 5703, 0, 0, 0, 0, 29, sPed, 1);
            AddComplexComponent((BaseAddon)this, 13042, 0, 0, 0, 0, -1, "", 1);
        }

        public PaganBase(Serial serial) : base(serial)
        {
        }

        public override bool HandlesOnMovement { get { return true; } }
        private DateTime m_NextTalk;
        public DateTime NextTalk { get { return m_NextTalk; } set { m_NextTalk = value; } }
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m is PlayerMobile)
            {
                if (DateTime.Now >= m_NextTalk && Utility.InRange(m.Location, this.Location, 2))
                {
                    m.PrivateOverheadMessage(MessageType.Regular, 1150, false, "I could use that item on the pedestal to take it.", m.NetState);
                    m_NextTalk = (DateTime.Now + TimeSpan.FromSeconds(30));
                }
            }
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource)
        {
            AddComplexComponent(addon, item, xoffset, yoffset, zoffset, hue, lightsource, null, 1);
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource, string name, int amount)
        {
            AddonComponent ac;
            ac = new AddonComponent(item);
            if (name != null && name.Length > 0)
                ac.Name = name;
            if (hue != 0)
                ac.Hue = hue;
            if (amount > 1)
            {
                ac.Stackable = true;
                ac.Amount = amount;
            }
            if (lightsource != -1)
                ac.Light = (LightType)lightsource;
            addon.AddComponent(ac, xoffset, yoffset, zoffset);
        }

        public override void OnComponentUsed(AddonComponent ac, Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.SendMessage("You will have to get closer to take that.");
            }
            else if (from.Backpack.FindItemByType(typeof(ObeliskTip)) != null)
            {
                Item tips = from.Backpack.FindItemByType(typeof(ObeliskTip));
                ObeliskTip tip = (ObeliskTip)tips;
                bool clearPed = false;

                if (tip.ObeliskOwner == from)
                {
                    string paganType = "";

                    if (ItemType == 1)
                    {
                        paganType = "Breath of Air";
                        if (tip.HasAir > 0)
                        {
                            from.SendMessage("You already have the " + paganType + ".");
                        }
                        else
                        {
                            tip.HasAir = 1;
                            from.LocalOverheadMessage(MessageType.Emote, 1150, true, "You found the " + paganType + "!");
                            LoggingFunctions.LogGeneric(from, "has found the " + paganType + ".");
                            clearPed = true;
                        }
                    }
                    else if (ItemType == 2)
                    {
                        paganType = "Tongue of Flame";
                        if (tip.HasFire > 0)
                        {
                            from.SendMessage("You already have the " + paganType + ".");
                        }
                        else
                        {
                            tip.HasFire = 1;
                            from.LocalOverheadMessage(MessageType.Emote, 1150, true, "You found the " + paganType + "!");
                            LoggingFunctions.LogGeneric(from, "has found the " + paganType + ".");
                            clearPed = true;
                        }
                    }
                    else if (ItemType == 3)
                    {
                        paganType = "Heart of Earth";
                        if (tip.HasEarth > 0)
                        {
                            from.SendMessage("You already have the " + paganType + ".");
                        }
                        else
                        {
                            tip.HasEarth = 1;
                            from.LocalOverheadMessage(MessageType.Emote, 1150, true, "You found the " + paganType + "!");
                            LoggingFunctions.LogGeneric(from, "has found the " + paganType + ".");
                            clearPed = true;
                        }
                    }
                    else
                    {
                        paganType = "Tear of the Seas";
                        if (tip.HasWater > 0)
                        {
                            from.SendMessage("You already have the " + paganType + ".");
                        }
                        else
                        {
                            tip.HasWater = 1;
                            from.LocalOverheadMessage(MessageType.Emote, 1150, true, "You found the " + paganType + "!");
                            LoggingFunctions.LogGeneric(from, "has found the " + paganType + ".");
                            clearPed = true;
                        }
                    }

                    if (clearPed)
                    {
                        PaganBaseEmpty Pedul = new PaganBaseEmpty();
                        Pedul.ItemType = ItemType;
                        Pedul.MoveToWorld(new Point3D(this.X, this.Y, this.Z), this.Map);
                        from.SendSound(0x3D);
                        this.Delete();
                    }
                }
                else
                {
                    from.SendMessage("This piece of blackrock does not belong to you so it vanishes!");
                    bool remove = true;
                    foreach (Account a in Accounts.GetAccounts())
                    {
                        if (a == null)
                            break;

                        int index = 0;

                        for (int i = 0; i < a.Length; ++i)
                        {
                            Mobile m = a[i];

                            if (m == null)
                                continue;

                            if (m == tip.ObeliskOwner)
                            {
                                m.AddToBackpack(this);
                                remove = false;
                            }

                            ++index;
                        }
                    }
                    if (remove)
                    {
                        this.Delete();
                    }
                }
            }
            else
            {
                from.SendMessage("You need a similar piece of blackrock to move this stone!");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
            writer.Write(ItemType);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            ItemType = reader.ReadInt();
        }
    }
}