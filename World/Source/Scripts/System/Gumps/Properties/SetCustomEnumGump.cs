using System;
using System.Reflection;
using System.Collections;
using Server;
using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
    public class SetCustomEnumGump : SetListOptionGump
    {
        private string[] m_Names;

        public SetCustomEnumGump(PropertyInfo prop, Mobile mobile, object o, Stack stack, int propspage, ArrayList list, string[] names) : base(prop, mobile, o, stack, propspage, list, names, null)
        {
            m_Names = names;
        }

        public override void OnResponse(NetState sender, RelayInfo relayInfo)
        {
            int index = relayInfo.ButtonID - 1;

            if (index >= 0 && index < m_Names.Length)
            {
                try
                {
                    MethodInfo info = m_Property.PropertyType.GetMethod("Parse", new Type[] { typeof(string) });

                    string result = "";

                    if (info != null)
                        result = Properties.SetDirect(m_Mobile, m_Object, m_Object, m_Property, m_Property.Name, info.Invoke(null, new object[] { m_Names[index] }), true);
                    else if (m_Property.PropertyType == typeof(Enum) || m_Property.PropertyType.IsSubclassOf(typeof(Enum)))
                        result = Properties.SetDirect(m_Mobile, m_Object, m_Object, m_Property, m_Property.Name, Enum.Parse(m_Property.PropertyType, m_Names[index], false), true);

                    m_Mobile.SendMessage(result);

                    if (result == "Property has been set.")
                        PropertiesGump.OnValueChanged(m_Object, m_Property, m_Stack);
                }
                catch
                {
                    m_Mobile.SendMessage("An exception was caught. The property may not have changed.");
                }
            }

            m_Mobile.SendGump(new PropertiesGump(m_Mobile, m_Object, m_Stack, m_List, m_Page));
        }
    }
}