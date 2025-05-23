/***************************************************************************
 *                                EventLog.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The Zaggawrath Software Team
 *   email                : info@Zaggawrath.com
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
using System.Diagnostics;
using DiagELog = System.Diagnostics.EventLog;

namespace Server
{
    public static class EventLog
    {
        static EventLog()
        {
            if (!DiagELog.SourceExists("AdventureGame"))
            {
                DiagELog.CreateEventSource("AdventureGame", "Application");
            }
        }

        public static void Error(int eventID, string text)
        {
            DiagELog.WriteEntry("AdventureGame", text, EventLogEntryType.Error, eventID);
        }

        public static void Error(int eventID, string format, params object[] args)
        {
            Error(eventID, String.Format(format, args));
        }

        public static void Warning(int eventID, string text)
        {
            DiagELog.WriteEntry("AdventureGame", text, EventLogEntryType.Warning, eventID);
        }

        public static void Warning(int eventID, string format, params object[] args)
        {
            Warning(eventID, String.Format(format, args));
        }

        public static void Inform(int eventID, string text)
        {
            DiagELog.WriteEntry("AdventureGame", text, EventLogEntryType.Information, eventID);
        }

        public static void Inform(int eventID, string format, params object[] args)
        {
            Inform(eventID, String.Format(format, args));
        }
    }
}