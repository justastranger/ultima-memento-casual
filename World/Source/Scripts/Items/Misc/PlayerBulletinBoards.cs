using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Multis;
using Server.Prompts;
using Server.Mobiles;
using Server.Network;
using Server.ContextMenus;

namespace Server.Items
{
    public class PlayerBBSouth : BasePlayerBB
    {
        public override int LabelNumber { get { return 1062421; } } // bulletin board (south)

        [Constructable]
        public PlayerBBSouth() : base(0x2311)
        {
            Weight = 15.0;
        }

        public PlayerBBSouth(Serial serial) : base(serial)
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

    public class PlayerBBEast : BasePlayerBB
    {
        public override int LabelNumber { get { return 1062420; } } // bulletin board (east)

        [Constructable]
        public PlayerBBEast() : base(0x2312)
        {
            Weight = 15.0;
        }

        public PlayerBBEast(Serial serial) : base(serial)
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

    public abstract class BasePlayerBB : Item, ISecurable
    {
        private PlayerBBMessage m_Greeting;
        private List<PlayerBBMessage> m_Messages;
        private string m_Title;
        private SecureLevel m_Level;

        public List<PlayerBBMessage> Messages
        {
            get { return m_Messages; }
        }

        public PlayerBBMessage Greeting
        {
            get { return m_Greeting; }
            set { m_Greeting = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SecureLevel Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        public BasePlayerBB(int itemID) : base(itemID)
        {
            m_Messages = new List<PlayerBBMessage>();
            m_Level = SecureLevel.Anyone;
        }

        public BasePlayerBB(Serial serial) : base(serial)
        {
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            SetSecureLevelEntry.AddTo(from, this, list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1);

            writer.Write((int)m_Level);

            writer.Write(m_Title);

            if (m_Greeting != null)
            {
                writer.Write(true);
                m_Greeting.Serialize(writer);
            }
            else
            {
                writer.Write(false);
            }

            writer.WriteEncodedInt(m_Messages.Count);

            for (int i = 0; i < m_Messages.Count; ++i)
                m_Messages[i].Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_Level = (SecureLevel)reader.ReadInt();
                        goto case 0;
                    }
                case 0:
                    {
                        if (version < 1)
                            m_Level = SecureLevel.Anyone;

                        m_Title = reader.ReadString();

                        if (reader.ReadBool())
                            m_Greeting = new PlayerBBMessage(reader);

                        int count = reader.ReadEncodedInt();

                        m_Messages = new List<PlayerBBMessage>(count);

                        for (int i = 0; i < count; ++i)
                            m_Messages.Add(new PlayerBBMessage(reader));

                        break;
                    }
            }
        }

        public static bool CheckAccess(BaseHouse house, Mobile from)
        {
            if (house.Public || !house.IsAosRules)
                return !house.IsBanned(from);

            return house.HasAccess(from);
        }

        public override void OnDoubleClick(Mobile from)
        {
            BaseHouse house = BaseHouse.FindHouseAt(this);

            if (house == null || !house.IsLockedDown(this))
                from.SendLocalizedMessage(1062396); // This bulletin board must be locked down in a house to be usable.
            else if (!from.InRange(this.GetWorldLocation(), 2) || !from.InLOS(this))
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            else if (CheckAccess(house, from))
                from.SendGump(new PlayerBBGump(from, house, this, 0));
        }

        public class PostPrompt : Prompt
        {
            private int m_Page;
            private BaseHouse m_House;
            private BasePlayerBB m_Board;
            private bool m_Greeting;

            public PostPrompt(int page, BaseHouse house, BasePlayerBB board, bool greeting)
            {
                m_Page = page;
                m_House = house;
                m_Board = board;
                m_Greeting = greeting;
            }

            public override void OnCancel(Mobile from)
            {
                OnResponse(from, "");
            }

            public override void OnResponse(Mobile from, string text)
            {
                int page = m_Page;
                BaseHouse house = m_House;
                BasePlayerBB board = m_Board;

                if (house == null || !house.IsLockedDown(board))
                {
                    from.SendLocalizedMessage(1062396); // This bulletin board must be locked down in a house to be usable.
                    return;
                }
                else if (!from.InRange(board.GetWorldLocation(), 2) || !from.InLOS(board))
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                    return;
                }
                else if (!CheckAccess(house, from))
                {
                    from.SendLocalizedMessage(1062398); // You are not allowed to post to this bulletin board.
                    return;
                }
                else if (m_Greeting && !house.IsOwner(from))
                {
                    return;
                }

                text = text.Trim();

                if (text.Length > 255)
                    text = text.Substring(0, 255);

                if (text.Length > 0)
                {
                    PlayerBBMessage message = new PlayerBBMessage(DateTime.Now, from, text);

                    if (m_Greeting)
                    {
                        board.Greeting = message;
                    }
                    else
                    {
                        board.Messages.Add(message);

                        if (board.Messages.Count > 50)
                        {
                            board.Messages.RemoveAt(0);

                            if (page > 0)
                                --page;
                        }
                    }
                }

                from.SendGump(new PlayerBBGump(from, house, board, page));
            }
        }

        public class SetTitlePrompt : Prompt
        {
            private int m_Page;
            private BaseHouse m_House;
            private BasePlayerBB m_Board;

            public SetTitlePrompt(int page, BaseHouse house, BasePlayerBB board)
            {
                m_Page = page;
                m_House = house;
                m_Board = board;
            }

            public override void OnCancel(Mobile from)
            {
                OnResponse(from, "");
            }

            public override void OnResponse(Mobile from, string text)
            {
                int page = m_Page;
                BaseHouse house = m_House;
                BasePlayerBB board = m_Board;

                if (house == null || !house.IsLockedDown(board))
                {
                    from.SendLocalizedMessage(1062396); // This bulletin board must be locked down in a house to be usable.
                    return;
                }
                else if (!from.InRange(board.GetWorldLocation(), 2) || !from.InLOS(board))
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                    return;
                }
                else if (!CheckAccess(house, from))
                {
                    from.SendLocalizedMessage(1062398); // You are not allowed to post to this bulletin board.
                    return;
                }

                text = text.Trim();

                if (text.Length > 255)
                    text = text.Substring(0, 255);

                if (text.Length > 0)
                    board.Title = text;

                from.SendGump(new PlayerBBGump(from, house, board, page));
            }
        }
    }

    public class PlayerBBMessage
    {
        private DateTime m_Time;
        private Mobile m_Poster;
        private string m_Message;

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime Time
        {
            get { return m_Time; }
            set { m_Time = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Poster
        {
            get { return m_Poster; }
            set { m_Poster = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        public PlayerBBMessage(DateTime time, Mobile poster, string message)
        {
            m_Time = time;
            m_Poster = poster;
            m_Message = message;
        }

        public PlayerBBMessage(GenericReader reader)
        {
            int version = reader.ReadEncodedInt();

            switch (version)
            {
                case 0:
                    {
                        m_Time = reader.ReadDateTime();
                        m_Poster = reader.ReadMobile();
                        m_Message = reader.ReadString();
                        break;
                    }
            }
        }

        public void Serialize(GenericWriter writer)
        {
            writer.WriteEncodedInt(0); // version

            writer.Write(m_Time);
            writer.Write(m_Poster);
            writer.Write(m_Message);
        }
    }

    public class PlayerBBGump : Gump
    {
        private int m_Page;
        private Mobile m_From;
        private BaseHouse m_House;
        private BasePlayerBB m_Board;

        private const int LabelColor = 0x7FFF;
        private const int LabelHue = 1153;

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            int page = m_Page;
            Mobile from = m_From;
            BaseHouse house = m_House;
            BasePlayerBB board = m_Board;

            if (house == null || !house.IsLockedDown(board))
            {
                from.SendLocalizedMessage(1062396); // This bulletin board must be locked down in a house to be usable.
                return;
            }
            else if (!from.InRange(board.GetWorldLocation(), 2) || !from.InLOS(board))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return;
            }
            else if (!BasePlayerBB.CheckAccess(house, from))
            {
                from.SendLocalizedMessage(1062398); // You are not allowed to post to this bulletin board.
                return;
            }

            switch (info.ButtonID)
            {
                case 1: // Post message
                    {
                        from.Prompt = new BasePlayerBB.PostPrompt(page, house, board, false);
                        from.SendLocalizedMessage(1062397); // Please enter your message:

                        break;
                    }
                case 2: // Set title
                    {
                        if (house.IsOwner(from))
                        {
                            from.Prompt = new BasePlayerBB.SetTitlePrompt(page, house, board);
                            from.SendLocalizedMessage(1062402); // Enter new title:
                        }

                        break;
                    }
                case 3: // Post greeting
                    {
                        if (house.IsOwner(from))
                        {
                            from.Prompt = new BasePlayerBB.PostPrompt(page, house, board, true);
                            from.SendLocalizedMessage(1062404); // Enter new greeting (this will always be the first post):
                        }

                        break;
                    }
                case 4: // Scroll up
                    {
                        if (page == 0)
                            page = board.Messages.Count;
                        else
                            page -= 1;

                        from.SendGump(new PlayerBBGump(from, house, board, page));

                        break;
                    }
                case 5: // Scroll down
                    {
                        page += 1;
                        page %= board.Messages.Count + 1;

                        from.SendGump(new PlayerBBGump(from, house, board, page));

                        break;
                    }
                case 6: // Banish poster
                    {
                        if (house.IsOwner(from))
                        {
                            if (page >= 1 && page <= board.Messages.Count)
                            {
                                PlayerBBMessage message = (PlayerBBMessage)board.Messages[page - 1];
                                Mobile poster = message.Poster;

                                if (poster == null)
                                {
                                    from.SendGump(new PlayerBBGump(from, house, board, page));
                                    return;
                                }

                                if (poster.AccessLevel > AccessLevel.Counselor && from.AccessLevel <= poster.AccessLevel)
                                {
                                    from.SendLocalizedMessage(501354); // Uh oh...a bigger boot may be required.
                                }
                                else if (house.IsFriend(poster))
                                {
                                    from.SendLocalizedMessage(1060750); // That person is a friend, co-owner, or owner of this house, and therefore cannot be banished!
                                }
                                else if (poster is PlayerVendor)
                                {
                                    from.SendLocalizedMessage(501351); // You cannot eject a vendor.
                                }
                                else if (house.Bans.Count >= BaseHouse.MaxBans)
                                {
                                    from.SendLocalizedMessage(501355); // The ban limit for this house has been reached!
                                }
                                else if (house.IsBanned(poster))
                                {
                                    from.SendLocalizedMessage(501356); // This person is already banned!
                                }
                                else if (poster is BaseCreature && ((BaseCreature)poster).NoHouseRestrictions)
                                {
                                    from.SendLocalizedMessage(1062040); // You cannot ban that.
                                }
                                else
                                {
                                    if (!house.Bans.Contains(poster))
                                        house.Bans.Add(poster);

                                    from.SendLocalizedMessage(1062417); // That person has been banned from this house.

                                    if (house.IsInside(poster) && !BasePlayerBB.CheckAccess(house, poster))
                                        poster.MoveToWorld(house.BanLocation, house.Map);
                                }
                            }

                            from.SendGump(new PlayerBBGump(from, house, board, page));
                        }

                        break;
                    }
                case 7: // Delete message
                    {
                        if (house.IsOwner(from))
                        {
                            if (page >= 1 && page <= board.Messages.Count)
                                board.Messages.RemoveAt(page - 1);

                            from.SendGump(new PlayerBBGump(from, house, board, 0));
                        }

                        break;
                    }
                case 8: // Post props
                    {
                        if (from.AccessLevel >= AccessLevel.GameMaster)
                        {
                            PlayerBBMessage message = board.Greeting;

                            if (page >= 1 && page <= board.Messages.Count)
                                message = (PlayerBBMessage)board.Messages[page - 1];

                            from.SendGump(new PlayerBBGump(from, house, board, page));
                            from.SendGump(new PropertiesGump(from, message));
                        }

                        break;
                    }
            }
        }

        public PlayerBBGump(Mobile from, BaseHouse house, BasePlayerBB board, int page) : base(50, 10)
        {
            from.CloseGump(typeof(PlayerBBGump));

            m_Page = page;
            m_From = from;
            m_House = house;
            m_Board = board;

            AddPage(0);

            AddImage(30, 30, 5400);

            AddButton(393, 145, 2084, 2084, 4, GumpButtonType.Reply, 0); // Scroll up
            AddButton(390, 371, 2085, 2085, 5, GumpButtonType.Reply, 0); // Scroll down

            AddButton(32, 183, 5412, 5413, 1, GumpButtonType.Reply, 0); // Post message

            if (house.IsOwner(from))
            {
                AddButton(63, 90, 5601, 5605, 2, GumpButtonType.Reply, 0);
                AddHtmlLocalized(81, 89, 230, 20, 1062400, LabelColor, false, false); // Set title

                AddButton(63, 109, 5601, 5605, 3, GumpButtonType.Reply, 0);
                AddHtmlLocalized(81, 108, 230, 20, 1062401, LabelColor, false, false); // Post greeting
            }

            string title = board.Title;

            if (title != null)
                AddHtml(183, 68, 180, 23, title, false, false);

            AddHtmlLocalized(385, 89, 60, 20, 1062409, LabelColor, false, false); // Post

            AddLabel(440, 89, LabelHue, page.ToString());
            AddLabel(455, 89, LabelHue, "/");
            AddLabel(470, 89, LabelHue, board.Messages.Count.ToString());

            PlayerBBMessage message = board.Greeting;

            if (page >= 1 && page <= board.Messages.Count)
                message = (PlayerBBMessage)board.Messages[page - 1];

            AddImageTiled(150, 220, 240, 1, 2700); // Separator

            AddHtmlLocalized(150, 180, 100, 20, 1062405, 16715, false, false); // Posted On:
            AddHtmlLocalized(150, 200, 100, 20, 1062406, 16715, false, false); // Posted By:

            if (message != null)
            {
                AddHtml(255, 180, 150, 20, message.Time.ToString("yyyy-MM-dd HH:mm:ss"), false, false);

                Mobile poster = message.Poster;
                string name = (poster == null ? null : poster.Name);

                if (name == null || (name = name.Trim()).Length == 0)
                    name = "Someone";

                AddHtml(255, 200, 150, 20, name, false, false);

                string body = message.Message;

                if (body == null)
                    body = "";

                AddHtml(150, 240, 250, 100, body, false, false);

                if (message != board.Greeting && house.IsOwner(from))
                {
                    AddButton(130, 395, 1209, 1210, 6, GumpButtonType.Reply, 0);
                    AddHtmlLocalized(150, 393, 150, 20, 1062410, LabelColor, false, false); // Banish Poster

                    AddButton(310, 395, 1209, 1210, 7, GumpButtonType.Reply, 0);
                    AddHtmlLocalized(330, 393, 150, 20, 1062411, LabelColor, false, false); // Delete Message
                }

                if (from.AccessLevel >= AccessLevel.GameMaster)
                    AddButton(135, 242, 1209, 1210, 8, GumpButtonType.Reply, 0); // Post props
            }
        }
    }
}