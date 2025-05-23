/***************************************************************************
 *                               GumpButton.cs
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
using Server.Network;

namespace Server.Gumps
{
    public enum GumpButtonType
    {
        Page = 0,
        Reply = 1
    }

    public class GumpButton : GumpEntry
    {
        private int m_X, m_Y;
        private int m_ID1, m_ID2;
        private int m_ButtonID;
        private GumpButtonType m_Type;
        private int m_Param;

        public GumpButton(int x, int y, int normalID, int pressedID, int buttonID, GumpButtonType type, int param)
        {
            m_X = x;
            m_Y = y;
            m_ID1 = normalID;
            m_ID2 = pressedID;
            m_ButtonID = buttonID;
            m_Type = type;
            m_Param = param;
        }

        public int X
        {
            get
            {
                return m_X;
            }
            set
            {
                Delta(ref m_X, value);
            }
        }

        public int Y
        {
            get
            {
                return m_Y;
            }
            set
            {
                Delta(ref m_Y, value);
            }
        }

        public int NormalID
        {
            get
            {
                return m_ID1;
            }
            set
            {
                Delta(ref m_ID1, value);
            }
        }

        public int PressedID
        {
            get
            {
                return m_ID2;
            }
            set
            {
                Delta(ref m_ID2, value);
            }
        }

        public int ButtonID
        {
            get
            {
                return m_ButtonID;
            }
            set
            {
                Delta(ref m_ButtonID, value);
            }
        }

        public GumpButtonType Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                if (m_Type != value)
                {
                    m_Type = value;

                    Gump parent = Parent;

                    if (parent != null)
                    {
                        parent.Invalidate();
                    }
                }
            }
        }

        public int Param
        {
            get
            {
                return m_Param;
            }
            set
            {
                Delta(ref m_Param, value);
            }
        }

        public override string Compile()
        {
            return String.Format("{{ button {0} {1} {2} {3} {4} {5} {6} }}", m_X, m_Y, m_ID1, m_ID2, (int)m_Type, m_Param, m_ButtonID);
        }

        private static byte[] m_LayoutName = Gump.StringToBuffer("button");

        public override void AppendTo(IGumpWriter disp)
        {
            disp.AppendLayout(m_LayoutName);
            disp.AppendLayout(m_X);
            disp.AppendLayout(m_Y);
            disp.AppendLayout(m_ID1);
            disp.AppendLayout(m_ID2);
            disp.AppendLayout((int)m_Type);
            disp.AppendLayout(m_Param);
            disp.AppendLayout(m_ButtonID);
        }
    }
}