/***************************************************************************
 *                                 Poison.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id$
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Server
{
    [Parsable]
    public abstract class Poison
    {
        public abstract string Name { get; }
        public abstract int Level { get; }
        public abstract Timer ConstructTimer(Mobile m);

        public override string ToString()
        {
            return this.Name;
        }


        private static List<Poison> m_Poisons = new List<Poison>();

        public static void Register(Poison reg)
        {
            string regName = reg.Name.ToLower();

            for (int i = 0; i < m_Poisons.Count; i++)
            {
                if (reg.Level == m_Poisons[i].Level)
                    throw new Exception("A poison with that level already exists.");
                else if (regName == m_Poisons[i].Name.ToLower())
                    throw new Exception("A poison with that name already exists.");
            }

            m_Poisons.Add(reg);
        }

        public static Poison Lesser { get { return GetPoison("Lesser"); } }
        public static Poison Regular { get { return GetPoison("Regular"); } }
        public static Poison Greater { get { return GetPoison("Greater"); } }
        public static Poison Deadly { get { return GetPoison("Deadly"); } }
        public static Poison Lethal { get { return GetPoison("Lethal"); } }

        public static List<Poison> Poisons
        {
            get
            {
                return m_Poisons;
            }
        }

        public static Poison Parse(string value)
        {
            Poison p = null;

            int plevel;

            if (int.TryParse(value, out plevel))
                p = GetPoison(plevel);

            if (p == null)
                p = GetPoison(value);

            return p;
        }

        public static Poison GetPoison(int level)
        {
            for (int i = 0; i < m_Poisons.Count; ++i)
            {
                Poison p = m_Poisons[i];

                if (p.Level == level)
                    return p;
            }

            return null;
        }

        public static Poison GetPoison(string name)
        {
            for (int i = 0; i < m_Poisons.Count; ++i)
            {
                Poison p = m_Poisons[i];

                if (Utility.InsensitiveCompare(p.Name, name) == 0)
                    return p;
            }

            return null;
        }

        public static void Serialize(Poison p, GenericWriter writer)
        {
            if (p == null)
            {
                writer.Write((byte)0);
            }
            else
            {
                writer.Write((byte)1);
                writer.Write((byte)p.Level);
            }
        }

        public static Poison Deserialize(GenericReader reader)
        {
            switch (reader.ReadByte())
            {
                case 1: return GetPoison(reader.ReadByte());
                case 2:
                    //no longer used, safe to remove?
                    reader.ReadInt();
                    reader.ReadDouble();
                    reader.ReadInt();
                    reader.ReadTimeSpan();
                    break;
            }
            return null;
        }
    }
}
