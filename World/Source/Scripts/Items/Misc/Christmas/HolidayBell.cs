﻿using System;

namespace Server.Items
{
    public class HolidayBell : Item
    {
        private static string[] m_StaffNames = new string[]
        {
            "Adrick",
            "Alai",
            "Bulldoz",
            "Evocare",
            "FierY-iCe",
            "Greyburn",
            "Hanse",
            "Ignatz",
            "Jalek",
            "LadyMOI",
            "Lord Krum",
            "Malantus",
            "Nimrond",
            "Oaks",
            "Prophet",
            "Runesabre",
            "Sage",
            "Stellerex",
            "T-Bone",
            "Tajima",
            "Tyrant",
            "Vex"
        };
        private static int[] m_Hues = new int[]
        {
            0xA, 0x24, 0x42, 0x56, 0x1A, 0x4C, 0x3C, 0x60, 0x2E, 0x55, 0x23, 0x38, 0x482, 0x6, 0x10
        };

        [CommandProperty(AccessLevel.GameMaster)]
        public int SoundID
        {
            get { return m_SoundID; }
            set { m_SoundID = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Giver { get { return m_Maker; } set { m_Maker = value; } }

        public override string DefaultName
        {
            get { return String.Format("A Holiday Bell From {0}", Giver); }
        }

        private string m_Maker;
        private int m_SoundID;

        [Constructable]
        public HolidayBell()
            : this(m_StaffNames[Utility.Random(m_StaffNames.Length)])
        {
        }

        [Constructable]
        public HolidayBell(string maker)
            : base(0x1C12)
        {
            m_Maker = maker;

            LootType = LootType.Blessed;
            Hue = m_Hues[Utility.Random(m_Hues.Length)];
            SoundID = 0x0F5 + Utility.Random(14);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
            }
            else from.PlaySound(m_SoundID);
        }

        public HolidayBell(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((string)m_Maker);

            writer.WriteEncodedInt((int)m_SoundID);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Maker = reader.ReadString();
            m_SoundID = reader.ReadEncodedInt();

            Utility.Intern(ref m_Maker);
        }
    }
}