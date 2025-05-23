using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Commands;
using Server.Network;
using Server.Targeting;

namespace Server.Gumps
{
    public class AddGump : Gump
    {
        private string m_SearchString;
        private Type[] m_SearchResults;
        private int m_Page;

        public static void Initialize()
        {
            CommandSystem.Register("AddMenu", AccessLevel.GameMaster, new CommandEventHandler(AddMenu_OnCommand));
        }

        [Usage("AddMenu [searchString]")]
        [Description("Opens an add menu, with an optional initial search string. This menu allows you to search for Items or Mobiles and add them interactively.")]
        private static void AddMenu_OnCommand(CommandEventArgs e)
        {
            string val = e.ArgString.Trim();
            Type[] types;
            bool explicitSearch = false;

            if (val.Length == 0)
            {
                types = Type.EmptyTypes;
            }
            else if (val.Length < 3)
            {
                e.Mobile.SendMessage("Invalid search string.");
                types = Type.EmptyTypes;
            }
            else
            {
                types = Match(val).ToArray();
                explicitSearch = true;
            }

            e.Mobile.SendGump(new AddGump(e.Mobile, val, 0, types, explicitSearch));
        }

        public AddGump(Mobile from, string searchString, int page, Type[] searchResults, bool explicitSearch) : base(50, 50)
        {
            m_SearchString = searchString;
            m_SearchResults = searchResults;
            m_Page = page;

            from.CloseGump(typeof(AddGump));

            AddPage(0);

            AddImage(0, 0, 5201, Server.Misc.PlayerSettings.GetGumpHue(from));
            AddImage(0, 0, 5199, 0);

            AddButton(10, 9, 4011, 4013, 1, GumpButtonType.Reply, 0);
            AddTextEntry(44, 10, 180, 20, 0x480, 0, searchString);

            AddHtmlLocalized(230, 10, 100, 20, 3010005, 0x7FFF, false, false);

            if (searchResults.Length > 0)
            {
                for (int i = (page * 10); i < ((page + 1) * 10) && i < searchResults.Length; ++i)
                {
                    int index = i % 10;

                    AddLabel(44, 39 + (index * 20), 0x480, searchResults[i].Name);
                    AddButton(10, 39 + (index * 20), 4023, 4025, 4 + i, GumpButtonType.Reply, 0);
                }
            }
            else
            {
                AddLabel(15, 44, 0x480, explicitSearch ? "Nothing matched your search terms." : "No results to display.");
            }

            if (m_Page > 0)
                AddButton(10, 249, 4014, 4016, 2, GumpButtonType.Reply, 0);
            else
                AddImage(10, 249, 4014);

            AddHtmlLocalized(44, 250, 170, 20, 1061028, m_Page > 0 ? 0x7FFF : 0x5EF7, false, false); // Previous page

            if (((m_Page + 1) * 10) < searchResults.Length)
                AddButton(210, 249, 4005, 4007, 3, GumpButtonType.Reply, 0);
            else
                AddImage(210, 249, 4005);

            AddHtmlLocalized(244, 250, 170, 20, 1061027, ((m_Page + 1) * 10) < searchResults.Length ? 0x7FFF : 0x5EF7, false, false); // Next page
        }

        private static Type typeofItem = typeof(Item), typeofMobile = typeof(Mobile);

        private static void Match(string match, Type[] types, List<Type> results)
        {
            if (match.Length == 0)
                return;

            match = match.ToLower();

            for (int i = 0; i < types.Length; ++i)
            {
                Type t = types[i];

                if ((typeofMobile.IsAssignableFrom(t) || typeofItem.IsAssignableFrom(t)) && t.Name.ToLower().IndexOf(match) >= 0 && !results.Contains(t))
                {
                    ConstructorInfo[] ctors = t.GetConstructors();

                    for (int j = 0; j < ctors.Length; ++j)
                    {
                        if (ctors[j].GetParameters().Length == 0 && ctors[j].IsDefined(typeof(ConstructableAttribute), false))
                        {
                            results.Add(t);
                            break;
                        }
                    }
                }
            }
        }

        public static List<Type> Match(string match)
        {
            List<Type> results = new List<Type>();
            Type[] types;

            Assembly[] asms = ScriptCompiler.Assemblies;

            for (int i = 0; i < asms.Length; ++i)
            {
                types = ScriptCompiler.GetTypeCache(asms[i]).Types;
                Match(match, types, results);
            }

            types = ScriptCompiler.GetTypeCache(Core.Assembly).Types;
            Match(match, types, results);

            results.Sort(new TypeNameComparer());

            return results;
        }

        private class TypeNameComparer : IComparer<Type>
        {
            public int Compare(Type x, Type y)
            {
                return x.Name.CompareTo(y.Name);
            }
        }

        public class InternalTarget : Target
        {
            private Type m_Type;
            private Type[] m_SearchResults;
            private string m_SearchString;
            private int m_Page;

            public InternalTarget(Type type, Type[] searchResults, string searchString, int page) : base(-1, true, TargetFlags.None)
            {
                m_Type = type;
                m_SearchResults = searchResults;
                m_SearchString = searchString;
                m_Page = page;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                IPoint3D p = o as IPoint3D;

                if (p != null)
                {
                    if (p is Item)
                        p = ((Item)p).GetWorldTop();
                    else if (p is Mobile)
                        p = ((Mobile)p).Location;

                    Server.Commands.Add.Invoke(from, new Point3D(p), new Point3D(p), new string[] { m_Type.Name });

                    from.Target = new InternalTarget(m_Type, m_SearchResults, m_SearchString, m_Page);
                }
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                if (cancelType == TargetCancelType.Canceled)
                    from.SendGump(new AddGump(from, m_SearchString, m_Page, m_SearchResults, true));
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 1: // Search
                    {
                        TextRelay te = info.GetTextEntry(0);
                        string match = (te == null ? "" : te.Text.Trim());

                        if (match.Length < 3)
                        {
                            from.SendMessage("Invalid search string.");
                            from.SendGump(new AddGump(from, match, m_Page, m_SearchResults, false));
                        }
                        else
                        {
                            from.SendGump(new AddGump(from, match, 0, Match(match).ToArray(), true));
                        }

                        break;
                    }
                case 2: // Previous page
                    {
                        if (m_Page > 0)
                            from.SendGump(new AddGump(from, m_SearchString, m_Page - 1, m_SearchResults, true));

                        break;
                    }
                case 3: // Next page
                    {
                        if ((m_Page + 1) * 10 < m_SearchResults.Length)
                            from.SendGump(new AddGump(from, m_SearchString, m_Page + 1, m_SearchResults, true));

                        break;
                    }
                default:
                    {
                        int index = info.ButtonID - 4;

                        if (index >= 0 && index < m_SearchResults.Length)
                        {
                            from.SendMessage("Where do you wish to place this object? <ESC> to cancel.");
                            from.Target = new InternalTarget(m_SearchResults[index], m_SearchResults, m_SearchString, m_Page);
                        }

                        break;
                    }
            }
        }
    }
}