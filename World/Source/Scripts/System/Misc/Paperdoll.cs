using System;
using System.Collections.Generic;
using Server;
using Server.Network;
using Server.Multis;
using Server.Mobiles;

namespace Server.Misc
{
    public class Paperdoll
    {
        public static void Initialize()
        {
            EventSink.PaperdollRequest += new PaperdollRequestEventHandler(EventSink_PaperdollRequest);
        }

        public static void EventSink_PaperdollRequest(PaperdollRequestEventArgs e)
        {
            Mobile beholder = e.Beholder;
            Mobile beheld = e.Beheld;

            beholder.Send(new DisplayPaperdoll(beheld, Titles.ComputeTitle(beholder, beheld), beheld.AllowEquipFrom(beholder)));

            if (ObjectPropertyList.Enabled)
            {
                List<Item> items = beheld.Items;

                for (int i = 0; i < items.Count; ++i)
                    beholder.Send(items[i].OPLPacket);
            }
        }
    }
}