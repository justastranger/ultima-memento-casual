using System;
using System.Reflection;
using System.Collections;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Commands;

namespace Server.Gumps
{
    public class SetObjectTarget : Target
    {
        private PropertyInfo m_Property;
        private Mobile m_Mobile;
        private object m_Object;
        private Stack m_Stack;
        private Type m_Type;
        private int m_Page;
        private ArrayList m_List;

        public SetObjectTarget(PropertyInfo prop, Mobile mobile, object o, Stack stack, Type type, int page, ArrayList list) : base(-1, false, TargetFlags.None)
        {
            m_Property = prop;
            m_Mobile = mobile;
            m_Object = o;
            m_Stack = stack;
            m_Type = type;
            m_Page = page;
            m_List = list;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            try
            {
                if (m_Type == typeof(Type))
                    targeted = targeted.GetType();
                else if ((m_Type == typeof(BaseAddon) || m_Type.IsAssignableFrom(typeof(BaseAddon))) && targeted is AddonComponent)
                    targeted = ((AddonComponent)targeted).Addon;

                if (m_Type.IsAssignableFrom(targeted.GetType()))
                {
                    CommandLogging.LogChangeProperty(m_Mobile, m_Object, m_Property.Name, targeted.ToString());
                    m_Property.SetValue(m_Object, targeted, null);
                    PropertiesGump.OnValueChanged(m_Object, m_Property, m_Stack);
                }
                else
                {
                    m_Mobile.SendMessage("That cannot be assigned to a property of type : {0}", m_Type.Name);
                }
            }
            catch
            {
                m_Mobile.SendMessage("An exception was caught. The property may not have changed.");
            }
        }

        protected override void OnTargetFinish(Mobile from)
        {
            if (m_Type == typeof(Type))
                from.SendGump(new PropertiesGump(m_Mobile, m_Object, m_Stack, m_List, m_Page));
            else
                from.SendGump(new SetObjectGump(m_Property, m_Mobile, m_Object, m_Stack, m_Type, m_Page, m_List));
        }
    }
}