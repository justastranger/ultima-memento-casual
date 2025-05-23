/***************************************************************************
 *                             PacketHandlers.cs
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
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Server.Accounting;
using Server.Gumps;
using Server.Targeting;
using Server.Items;
using Server.Menus;
using Server.Mobiles;
using Server.Movement;
using Server.Prompts;
using Server.HuePickers;
using Server.ContextMenus;
using Server.Diagnostics;
using CV = Server.ClientVersion;

namespace Server.Network
{
    public enum MessageType
    {
        Regular = 0x00,
        System = 0x01,
        Emote = 0x02,
        Label = 0x06,
        Focus = 0x07,
        Whisper = 0x08,
        Yell = 0x09,
        Spell = 0x0A,

        Guild = 0x0D,
        Alliance = 0x0E,
        Command = 0x0F,

        Encoded = 0xC0
    }

    public static class PacketHandlers
    {
        private static PacketHandler[] m_Handlers;
        private static PacketHandler[] m_6017Handlers;

        private static PacketHandler[] m_ExtendedHandlersLow;
        private static Dictionary<int, PacketHandler> m_ExtendedHandlersHigh;

        private static EncodedPacketHandler[] m_EncodedHandlersLow;
        private static Dictionary<int, EncodedPacketHandler> m_EncodedHandlersHigh;

        public static PacketHandler[] Handlers
        {
            get { return m_Handlers; }
        }

        static PacketHandlers()
        {
            m_Handlers = new PacketHandler[0x100];
            m_6017Handlers = new PacketHandler[0x100];

            m_ExtendedHandlersLow = new PacketHandler[0x100];
            m_ExtendedHandlersHigh = new Dictionary<int, PacketHandler>();

            m_EncodedHandlersLow = new EncodedPacketHandler[0x100];
            m_EncodedHandlersHigh = new Dictionary<int, EncodedPacketHandler>();

            Register(0x00, 104, false, new OnPacketReceive(CreateCharacter));
            Register(0x01, 5, false, new OnPacketReceive(Disconnect));
            Register(0x02, 7, true, new OnPacketReceive(MovementReq));
            Register(0x03, 0, true, new OnPacketReceive(AsciiSpeech));
            Register(0x04, 2, true, new OnPacketReceive(GodModeRequest));
            Register(0x05, 5, true, new OnPacketReceive(AttackReq));
            Register(0x06, 5, true, new OnPacketReceive(UseReq));
            Register(0x07, 7, true, new OnPacketReceive(LiftReq));
            Register(0x08, 14, true, new OnPacketReceive(DropReq));
            Register(0x09, 5, true, new OnPacketReceive(LookReq));
            Register(0x0A, 11, true, new OnPacketReceive(Edit));
            Register(0x12, 0, true, new OnPacketReceive(TextCommand));
            Register(0x13, 10, true, new OnPacketReceive(EquipReq));
            Register(0x14, 6, true, new OnPacketReceive(ChangeZ));
            Register(0x22, 3, true, new OnPacketReceive(Resynchronize));
            Register(0x2C, 2, true, new OnPacketReceive(DeathStatusResponse));
            Register(0x34, 10, true, new OnPacketReceive(MobileQuery));
            Register(0x3A, 0, true, new OnPacketReceive(ChangeSkillLock));
            Register(0x3B, 0, true, new OnPacketReceive(VendorBuyReply));
            Register(0x47, 11, true, new OnPacketReceive(NewTerrain));
            Register(0x48, 73, true, new OnPacketReceive(NewAnimData));
            Register(0x58, 106, true, new OnPacketReceive(NewRegion));
            Register(0x5D, 73, false, new OnPacketReceive(PlayCharacter));
            Register(0x61, 9, true, new OnPacketReceive(DeleteStatic));
            Register(0x6C, 19, true, new OnPacketReceive(TargetResponse));
            Register(0x6F, 0, true, new OnPacketReceive(SecureTrade));
            Register(0x72, 5, true, new OnPacketReceive(SetWarMode));
            Register(0x73, 2, false, new OnPacketReceive(PingReq));
            Register(0x75, 35, true, new OnPacketReceive(RenameRequest));
            Register(0x79, 9, true, new OnPacketReceive(ResourceQuery));
            Register(0x7E, 2, true, new OnPacketReceive(GodviewQuery));
            Register(0x7D, 13, true, new OnPacketReceive(MenuResponse));
            Register(0x80, 62, false, new OnPacketReceive(AccountLogin));
            Register(0x83, 39, false, new OnPacketReceive(DeleteCharacter));
            Register(0x91, 65, false, new OnPacketReceive(GameLogin));
            Register(0x95, 9, true, new OnPacketReceive(HuePickerResponse));
            Register(0x96, 0, true, new OnPacketReceive(GameCentralMoniter));
            Register(0x98, 0, true, new OnPacketReceive(MobileNameRequest));
            Register(0x9A, 0, true, new OnPacketReceive(AsciiPromptResponse));
            Register(0x9B, 258, true, new OnPacketReceive(HelpRequest));
            Register(0x9D, 51, true, new OnPacketReceive(GMSingle));
            Register(0x9F, 0, true, new OnPacketReceive(VendorSellReply));
            Register(0xA0, 3, false, new OnPacketReceive(PlayServer));
            Register(0xA4, 149, false, new OnPacketReceive(SystemInfo));
            Register(0xA7, 4, true, new OnPacketReceive(RequestScrollWindow));
            Register(0xAD, 0, true, new OnPacketReceive(UnicodeSpeech));
            Register(0xB1, 0, true, new OnPacketReceive(DisplayGumpResponse));
            Register(0xB5, 64, true, new OnPacketReceive(ChatRequest));
            Register(0xB6, 9, true, new OnPacketReceive(ObjectHelpRequest));
            Register(0xB8, 0, true, new OnPacketReceive(ProfileReq));
            Register(0xBB, 9, false, new OnPacketReceive(AccountID));
            Register(0xBD, 0, false, new OnPacketReceive(ClientVersion));
            Register(0xBE, 0, true, new OnPacketReceive(AssistVersion));
            Register(0xBF, 0, true, new OnPacketReceive(ExtendedCommand));
            Register(0xC2, 0, true, new OnPacketReceive(UnicodePromptResponse));
            Register(0xC8, 2, true, new OnPacketReceive(SetUpdateRange));
            Register(0xC9, 6, true, new OnPacketReceive(TripTime));
            Register(0xCA, 6, true, new OnPacketReceive(UTripTime));
            Register(0xCF, 0, false, new OnPacketReceive(AccountLogin));
            Register(0xD0, 0, true, new OnPacketReceive(ConfigurationFile));
            Register(0xD1, 2, true, new OnPacketReceive(LogoutReq));
            Register(0xD6, 0, true, new OnPacketReceive(BatchQueryProperties));
            Register(0xD7, 0, true, new OnPacketReceive(EncodedCommand));
            Register(0xE1, 0, false, new OnPacketReceive(ClientType));
            Register(0xEF, 21, false, new OnPacketReceive(LoginServerSeed));
            Register(0xF8, 106, false, new OnPacketReceive(CreateCharacter70160));

            Register6017(0x08, 15, true, new OnPacketReceive(DropReq6017));

            RegisterExtended(0x05, false, new OnPacketReceive(ScreenSize));
            RegisterExtended(0x06, true, new OnPacketReceive(PartyMessage));
            RegisterExtended(0x07, true, new OnPacketReceive(QuestArrow));
            RegisterExtended(0x09, true, new OnPacketReceive(DisarmRequest));
            RegisterExtended(0x0A, true, new OnPacketReceive(StunRequest));
            RegisterExtended(0x0B, false, new OnPacketReceive(Language));
            RegisterExtended(0x0C, true, new OnPacketReceive(CloseStatus));
            RegisterExtended(0x0E, true, new OnPacketReceive(Animate));
            RegisterExtended(0x0F, false, new OnPacketReceive(Empty)); // What's this?
            RegisterExtended(0x10, true, new OnPacketReceive(QueryProperties));
            RegisterExtended(0x13, true, new OnPacketReceive(ContextMenuRequest));
            RegisterExtended(0x15, true, new OnPacketReceive(ContextMenuResponse));
            RegisterExtended(0x1A, true, new OnPacketReceive(StatLockChange));
            RegisterExtended(0x1C, true, new OnPacketReceive(CastSpell));
            RegisterExtended(0x24, false, new OnPacketReceive(UnhandledBF));

            RegisterEncoded(0x19, true, new OnEncodedPacketReceive(SetAbility));
            RegisterEncoded(0x28, true, new OnEncodedPacketReceive(GuildGumpRequest));

            RegisterEncoded(0x32, true, new OnEncodedPacketReceive(QuestGumpRequest));
        }

        public static void Register(int packetID, int length, bool ingame, OnPacketReceive onReceive)
        {
            m_Handlers[packetID] = new PacketHandler(packetID, length, ingame, onReceive);

            if (m_6017Handlers[packetID] == null)
                m_6017Handlers[packetID] = new PacketHandler(packetID, length, ingame, onReceive);
        }

        public static PacketHandler GetHandler(int packetID)
        {
            return m_Handlers[packetID];
        }

        public static void Register6017(int packetID, int length, bool ingame, OnPacketReceive onReceive)
        {
            m_6017Handlers[packetID] = new PacketHandler(packetID, length, ingame, onReceive);
        }

        public static PacketHandler Get6017Handler(int packetID)
        {
            return m_6017Handlers[packetID];
        }

        public static void RegisterExtended(int packetID, bool ingame, OnPacketReceive onReceive)
        {
            if (packetID >= 0 && packetID < 0x100)
                m_ExtendedHandlersLow[packetID] = new PacketHandler(packetID, 0, ingame, onReceive);
            else
                m_ExtendedHandlersHigh[packetID] = new PacketHandler(packetID, 0, ingame, onReceive);
        }

        public static PacketHandler GetExtendedHandler(int packetID)
        {
            if (packetID >= 0 && packetID < 0x100)
                return m_ExtendedHandlersLow[packetID];
            else
            {
                PacketHandler handler;
                m_ExtendedHandlersHigh.TryGetValue(packetID, out handler);
                return handler;
            }
        }

        public static void RemoveExtendedHandler(int packetID)
        {
            if (packetID >= 0 && packetID < 0x100)
                m_ExtendedHandlersLow[packetID] = null;
            else
                m_ExtendedHandlersHigh.Remove(packetID);
        }

        public static void RegisterEncoded(int packetID, bool ingame, OnEncodedPacketReceive onReceive)
        {
            if (packetID >= 0 && packetID < 0x100)
                m_EncodedHandlersLow[packetID] = new EncodedPacketHandler(packetID, ingame, onReceive);
            else
                m_EncodedHandlersHigh[packetID] = new EncodedPacketHandler(packetID, ingame, onReceive);
        }

        public static EncodedPacketHandler GetEncodedHandler(int packetID)
        {
            if (packetID >= 0 && packetID < 0x100)
                return m_EncodedHandlersLow[packetID];
            else
            {
                EncodedPacketHandler handler;
                m_EncodedHandlersHigh.TryGetValue(packetID, out handler);
                return handler;
            }
        }

        public static void RemoveEncodedHandler(int packetID)
        {
            if (packetID >= 0 && packetID < 0x100)
                m_EncodedHandlersLow[packetID] = null;
            else
                m_EncodedHandlersHigh.Remove(packetID);
        }

        public static void RegisterThrottler(int packetID, ThrottlePacketCallback t)
        {
            PacketHandler ph = GetHandler(packetID);

            if (ph != null)
                ph.ThrottleCallback = t;

            ph = Get6017Handler(packetID);

            if (ph != null)
                ph.ThrottleCallback = t;
        }

        private static void UnhandledBF(NetState state, PacketReader pvSrc)
        {
        }

        public static void Empty(NetState state, PacketReader pvSrc)
        {
        }

        public static void SetAbility(NetState state, IEntity e, EncodedReader reader)
        {
            EventSink.InvokeSetAbility(new SetAbilityEventArgs(state.Mobile, reader.ReadInt32()));
        }

        public static void GuildGumpRequest(NetState state, IEntity e, EncodedReader reader)
        {
            EventSink.InvokeGuildGumpRequest(new GuildGumpRequestArgs(state.Mobile));
        }

        public static void QuestGumpRequest(NetState state, IEntity e, EncodedReader reader)
        {
            EventSink.InvokeQuestGumpRequest(new QuestGumpRequestArgs(state.Mobile));
        }

        public static void EncodedCommand(NetState state, PacketReader pvSrc)
        {
            IEntity e = World.FindEntity(pvSrc.ReadInt32());
            int packetID = pvSrc.ReadUInt16();

            EncodedPacketHandler ph = GetEncodedHandler(packetID);

            if (ph != null)
            {
                if (ph.Ingame && state.Mobile == null)
                {
                    Console.WriteLine("Client: {0}: Sent ingame packet (0xD7x{1:X2}) before having been attached to a mobile", state, packetID);
                    state.Dispose();
                }
                else if (ph.Ingame && state.Mobile.Deleted)
                {
                    state.Dispose();
                }
                else
                {
                    ph.OnReceive(state, e, new EncodedReader(pvSrc));
                }
            }
            else
            {
                pvSrc.Trace(state);
            }
        }

        public static void RenameRequest(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;
            Mobile targ = World.FindMobile(pvSrc.ReadInt32());

            if (targ != null)
                EventSink.InvokeRenameRequest(new RenameRequestEventArgs(from, targ, pvSrc.ReadStringSafe()));
        }

        public static void ChatRequest(NetState state, PacketReader pvSrc)
        {
            EventSink.InvokeChatRequest(new ChatRequestEventArgs(state.Mobile));
        }

        public static void SecureTrade(NetState state, PacketReader pvSrc)
        {
            switch (pvSrc.ReadByte())
            {
                case 1: // Cancel
                    {
                        Serial serial = pvSrc.ReadInt32();

                        SecureTradeContainer cont = World.FindItem(serial) as SecureTradeContainer;

                        if (cont != null && cont.Trade != null && (cont.Trade.From.Mobile == state.Mobile || cont.Trade.To.Mobile == state.Mobile))
                            cont.Trade.Cancel();

                        break;
                    }
                case 2: // Check
                    {
                        Serial serial = pvSrc.ReadInt32();

                        SecureTradeContainer cont = World.FindItem(serial) as SecureTradeContainer;

                        if (cont != null)
                        {
                            SecureTrade trade = cont.Trade;

                            bool value = (pvSrc.ReadInt32() != 0);

                            if (trade != null && trade.From.Mobile == state.Mobile)
                            {
                                trade.From.Accepted = value;
                                trade.Update();
                            }
                            else if (trade != null && trade.To.Mobile == state.Mobile)
                            {
                                trade.To.Accepted = value;
                                trade.Update();
                            }
                        }

                        break;
                    }
            }
        }

        public static void VendorBuyReply(NetState state, PacketReader pvSrc)
        {
            pvSrc.Seek(1, SeekOrigin.Begin);

            int msgSize = pvSrc.ReadUInt16();
            Mobile vendor = World.FindMobile(pvSrc.ReadInt32());
            byte flag = pvSrc.ReadByte();

            if (vendor == null)
            {
                return;
            }
            else if (vendor.Deleted || !Utility.RangeCheck(vendor.Location, state.Mobile.Location, 10))
            {
                state.Send(new EndVendorBuy(vendor));
                return;
            }

            if (flag == 0x02)
            {
                msgSize -= 1 + 2 + 4 + 1;

                if ((msgSize / 7) > 100)
                    return;

                List<BuyItemResponse> buyList = new List<BuyItemResponse>(msgSize / 7);
                for (; msgSize > 0; msgSize -= 7)
                {
                    byte layer = pvSrc.ReadByte();
                    Serial serial = pvSrc.ReadInt32();
                    int amount = pvSrc.ReadInt16();

                    buyList.Add(new BuyItemResponse(serial, amount));
                }

                if (buyList.Count > 0)
                {
                    IVendor v = vendor as IVendor;

                    if (v != null && v.OnBuyItems(state.Mobile, buyList))
                        state.Send(new EndVendorBuy(vendor));
                }
            }
            else
            {
                state.Send(new EndVendorBuy(vendor));
            }
        }

        public static void VendorSellReply(NetState state, PacketReader pvSrc)
        {
            Serial serial = pvSrc.ReadInt32();
            Mobile vendor = World.FindMobile(serial);

            if (vendor == null)
            {
                return;
            }
            else if (vendor.Deleted || !Utility.RangeCheck(vendor.Location, state.Mobile.Location, 10))
            {
                state.Send(new EndVendorSell(vendor));
                return;
            }

            int count = pvSrc.ReadUInt16();
            if (count < 100 && pvSrc.Size == (1 + 2 + 4 + 2 + (count * 6)))
            {
                List<SellItemResponse> sellList = new List<SellItemResponse>(count);

                for (int i = 0; i < count; i++)
                {
                    Item item = World.FindItem(pvSrc.ReadInt32());
                    int Amount = pvSrc.ReadInt16();

                    if (item != null && Amount > 0)
                        sellList.Add(new SellItemResponse(item, Amount));
                }

                if (sellList.Count > 0)
                {
                    IVendor v = vendor as IVendor;

                    if (v != null && v.OnSellItems(state.Mobile, sellList))
                        state.Send(new EndVendorSell(vendor));
                }
            }
        }

        public static void DeleteCharacter(NetState state, PacketReader pvSrc)
        {
            pvSrc.Seek(30, SeekOrigin.Current);
            int index = pvSrc.ReadInt32();

            EventSink.InvokeDeleteRequest(new DeleteRequestEventArgs(state, index));
        }

        public static void ResourceQuery(NetState state, PacketReader pvSrc)
        {
            if (VerifyGC(state))
            {
            }
        }

        public static void GameCentralMoniter(NetState state, PacketReader pvSrc)
        {
            if (VerifyGC(state))
            {
                int type = pvSrc.ReadByte();
                int num1 = pvSrc.ReadInt32();

                Console.WriteLine("God Client: {0}: Game central moniter", state);
                Console.WriteLine(" - Type: {0}", type);
                Console.WriteLine(" - Number: {0}", num1);

                pvSrc.Trace(state);
            }
        }

        public static void GodviewQuery(NetState state, PacketReader pvSrc)
        {
            if (VerifyGC(state))
            {
                Console.WriteLine("God Client: {0}: Godview query 0x{1:X}", state, pvSrc.ReadByte());
            }
        }

        public static void GMSingle(NetState state, PacketReader pvSrc)
        {
            if (VerifyGC(state))
                pvSrc.Trace(state);
        }

        public static void DeathStatusResponse(NetState state, PacketReader pvSrc)
        {
            // Ignored
        }

        public static void ObjectHelpRequest(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;

            Serial serial = pvSrc.ReadInt32();
            int unk = pvSrc.ReadByte();
            string lang = pvSrc.ReadString(3);

            if (serial.IsItem)
            {
                Item item = World.FindItem(serial);

                if (item != null && from.Map == item.Map && Utility.InUpdateRange(item.GetWorldLocation(), from.Location) && from.CanSee(item))
                    item.OnHelpRequest(from);
            }
            else if (serial.IsMobile)
            {
                Mobile m = World.FindMobile(serial);

                if (m != null && from.Map == m.Map && Utility.InUpdateRange(m.Location, from.Location) && from.CanSee(m))
                    m.OnHelpRequest(m);
            }
        }

        public static void MobileNameRequest(NetState state, PacketReader pvSrc)
        {
            Mobile m = World.FindMobile(pvSrc.ReadInt32());

            if (m != null && Utility.InUpdateRange(state.Mobile, m) && state.Mobile.CanSee(m))
                state.Send(new MobileName(m));
        }

        public static void RequestScrollWindow(NetState state, PacketReader pvSrc)
        {
            int lastTip = pvSrc.ReadInt16();
            int type = pvSrc.ReadByte();
        }

        public static void AttackReq(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;
            Mobile m = World.FindMobile(pvSrc.ReadInt32());

            if (m != null)
                from.Attack(m);
        }

        public static void HuePickerResponse(NetState state, PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int value = pvSrc.ReadInt16();
            int hue = pvSrc.ReadInt16() & 0x3FFF;

            hue = Utility.ClipDyedHue(hue);

            foreach (HuePicker huePicker in state.HuePickers)
            {
                if (huePicker.Serial == serial)
                {
                    state.RemoveHuePicker(huePicker);

                    huePicker.OnResponse(hue);

                    break;
                }
            }
        }

        public static void TripTime(NetState state, PacketReader pvSrc)
        {
            int unk1 = pvSrc.ReadByte();
            int unk2 = pvSrc.ReadInt32();

            state.Send(new TripTimeResponse(unk1));
        }

        public static void UTripTime(NetState state, PacketReader pvSrc)
        {
            int unk1 = pvSrc.ReadByte();
            int unk2 = pvSrc.ReadInt32();

            state.Send(new UTripTimeResponse(unk1));
        }

        public static void ChangeZ(NetState state, PacketReader pvSrc)
        {
            if (VerifyGC(state))
            {
                int x = pvSrc.ReadInt16();
                int y = pvSrc.ReadInt16();
                int z = pvSrc.ReadSByte();

                Console.WriteLine("God Client: {0}: Change Z ({1}, {2}, {3})", state, x, y, z);
            }
        }

        public static void SystemInfo(NetState state, PacketReader pvSrc)
        {
            int v1 = pvSrc.ReadByte();
            int v2 = pvSrc.ReadUInt16();
            int v3 = pvSrc.ReadByte();
            string s1 = pvSrc.ReadString(32);
            string s2 = pvSrc.ReadString(32);
            string s3 = pvSrc.ReadString(32);
            string s4 = pvSrc.ReadString(32);
            int v4 = pvSrc.ReadUInt16();
            int v5 = pvSrc.ReadUInt16();
            int v6 = pvSrc.ReadInt32();
            int v7 = pvSrc.ReadInt32();
            int v8 = pvSrc.ReadInt32();
        }

        public static void Edit(NetState state, PacketReader pvSrc)
        {
            if (VerifyGC(state))
            {
                int type = pvSrc.ReadByte(); // 10 = static, 7 = npc, 4 = dynamic
                int x = pvSrc.ReadInt16();
                int y = pvSrc.ReadInt16();
                int id = pvSrc.ReadInt16();
                int z = pvSrc.ReadSByte();
                int hue = pvSrc.ReadUInt16();

                Console.WriteLine("God Client: {0}: Edit {6} ({1}, {2}, {3}) 0x{4:X} (0x{5:X})", state, x, y, z, id, hue, type);
            }
        }

        public static void DeleteStatic(NetState state, PacketReader pvSrc)
        {
            if (VerifyGC(state))
            {
                int x = pvSrc.ReadInt16();
                int y = pvSrc.ReadInt16();
                int z = pvSrc.ReadInt16();
                int id = pvSrc.ReadUInt16();

                Console.WriteLine("God Client: {0}: Delete Static ({1}, {2}, {3}) 0x{4:X}", state, x, y, z, id);
            }
        }

        public static void NewAnimData(NetState state, PacketReader pvSrc)
        {
            if (VerifyGC(state))
            {
                Console.WriteLine("God Client: {0}: New tile animation", state);

                pvSrc.Trace(state);
            }
        }

        public static void NewTerrain(NetState state, PacketReader pvSrc)
        {
            if (VerifyGC(state))
            {
                int x = pvSrc.ReadInt16();
                int y = pvSrc.ReadInt16();
                int id = pvSrc.ReadUInt16();
                int width = pvSrc.ReadInt16();
                int height = pvSrc.ReadInt16();

                Console.WriteLine("God Client: {0}: New Terrain ({1}, {2})+({3}, {4}) 0x{5:X4}", state, x, y, width, height, id);
            }
        }

        public static void NewRegion(NetState state, PacketReader pvSrc)
        {
            if (VerifyGC(state))
            {
                string name = pvSrc.ReadString(40);
                int unk = pvSrc.ReadInt32();
                int x = pvSrc.ReadInt16();
                int y = pvSrc.ReadInt16();
                int width = pvSrc.ReadInt16();
                int height = pvSrc.ReadInt16();
                int zStart = pvSrc.ReadInt16();
                int zEnd = pvSrc.ReadInt16();
                string desc = pvSrc.ReadString(40);
                int soundFX = pvSrc.ReadInt16();
                int music = pvSrc.ReadInt16();
                int nightFX = pvSrc.ReadInt16();
                int dungeon = pvSrc.ReadByte();
                int light = pvSrc.ReadInt16();

                Console.WriteLine("God Client: {0}: New Region '{1}' ('{2}')", state, name, desc);
            }
        }

        public static void AccountID(NetState state, PacketReader pvSrc)
        {
        }

        public static bool VerifyGC(NetState state)
        {
            if (state.Mobile == null || state.Mobile.AccessLevel <= AccessLevel.Counselor)
            {
                if (state.Running)
                    Console.WriteLine("Warning: {0}: Player using godclient, disconnecting", state);

                state.Dispose();
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void TextCommand(NetState state, PacketReader pvSrc)
        {
            int type = pvSrc.ReadByte();
            string command = pvSrc.ReadString();

            Mobile m = state.Mobile;

            switch (type)
            {
                case 0x00: // Go
                    {
                        if (VerifyGC(state))
                        {
                            try
                            {
                                string[] split = command.Split(' ');

                                int x = Utility.ToInt32(split[0]);
                                int y = Utility.ToInt32(split[1]);

                                int z;

                                if (split.Length >= 3)
                                    z = Utility.ToInt32(split[2]);
                                else if (m.Map != null)
                                    z = m.Map.GetAverageZ(x, y);
                                else
                                    z = 0;

                                m.Location = new Point3D(x, y, z);
                            }
                            catch
                            {
                            }
                        }

                        break;
                    }
                case 0xC7: // Animate
                    {
                        EventSink.InvokeAnimateRequest(new AnimateRequestEventArgs(m, command));

                        break;
                    }
                case 0x24: // Use skill
                    {
                        int skillIndex;

                        if (!int.TryParse(command.Split(' ')[0], out skillIndex))
                            break;

                        Skills.UseSkill(m, skillIndex);

                        break;
                    }
                case 0x43: // Open spellbook
                    {
                        int booktype;

                        if (!int.TryParse(command, out booktype))
                            booktype = 1;

                        EventSink.InvokeOpenSpellbookRequest(new OpenSpellbookRequestEventArgs(m, booktype));

                        break;
                    }
                case 0x27: // Cast spell from book
                    {
                        string[] split = command.Split(' ');

                        if (split.Length > 0)
                        {
                            int spellID = Utility.ToInt32(split[0]) - 1;
                            int serial = split.Length > 1 ? Utility.ToInt32(split[1]) : -1;

                            EventSink.InvokeCastSpellRequest(new CastSpellRequestEventArgs(m, spellID, World.FindItem(serial)));
                        }

                        break;
                    }
                case 0x58: // Open door
                    {
                        EventSink.InvokeOpenDoorMacroUsed(new OpenDoorMacroEventArgs(m));

                        break;
                    }
                case 0x56: // Cast spell from macro
                    {
                        int spellID = Utility.ToInt32(command) - 1;

                        EventSink.InvokeCastSpellRequest(new CastSpellRequestEventArgs(m, spellID, null));

                        break;
                    }
                case 0xF4: // Invoke virtues from macro
                    {
                        int virtueID = Utility.ToInt32(command) - 1;

                        EventSink.InvokeVirtueMacroRequest(new VirtueMacroRequestEventArgs(m, virtueID));

                        break;
                    }
                default:
                    {
                        Console.WriteLine("Client: {0}: Unknown text-command type 0x{1:X2}: {2}", state, type, command);
                        break;
                    }
            }
        }

        public static void GodModeRequest(NetState state, PacketReader pvSrc)
        {
            if (VerifyGC(state))
            {
                state.Send(new GodModeReply(pvSrc.ReadBoolean()));
            }
        }

        public static void AsciiPromptResponse(NetState state, PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int prompt = pvSrc.ReadInt32();
            int type = pvSrc.ReadInt32();
            string text = pvSrc.ReadStringSafe();

            if (text.Length > 128)
                return;

            Mobile from = state.Mobile;
            Prompt p = from.Prompt;

            if (p != null && p.Serial == serial && p.Serial == prompt)
            {
                from.Prompt = null;

                if (type == 0)
                    p.OnCancel(from);
                else
                    p.OnResponse(from, text);
            }
        }

        public static void UnicodePromptResponse(NetState state, PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int prompt = pvSrc.ReadInt32();
            int type = pvSrc.ReadInt32();
            string lang = pvSrc.ReadString(4);
            string text = pvSrc.ReadUnicodeStringLESafe();

            if (text.Length > 128)
                return;

            Mobile from = state.Mobile;
            Prompt p = from.Prompt;

            if (p != null && p.Serial == serial && p.Serial == prompt)
            {
                from.Prompt = null;

                if (type == 0)
                    p.OnCancel(from);
                else
                    p.OnResponse(from, text);
            }
        }

        public static void MenuResponse(NetState state, PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int menuID = pvSrc.ReadInt16(); // unused in our implementation
            int index = pvSrc.ReadInt16();
            int itemID = pvSrc.ReadInt16();
            int hue = pvSrc.ReadInt16();

            index -= 1; // convert from 1-based to 0-based

            foreach (IMenu menu in state.Menus)
            {
                if (menu.Serial == serial)
                {
                    state.RemoveMenu(menu);

                    if (index >= 0 && index < menu.EntryLength)
                    {
                        menu.OnResponse(state, index);
                    }
                    else
                    {
                        menu.OnCancel(state);
                    }

                    break;
                }
            }
        }

        public static void ProfileReq(NetState state, PacketReader pvSrc)
        {
            int type = pvSrc.ReadByte();
            Serial serial = pvSrc.ReadInt32();

            Mobile beholder = state.Mobile;
            Mobile beheld = World.FindMobile(serial);

            if (beheld == null)
                return;

            switch (type)
            {
                case 0x00: // display request
                    {
                        EventSink.InvokeProfileRequest(new ProfileRequestEventArgs(beholder, beheld));

                        break;
                    }
                case 0x01: // edit request
                    {
                        pvSrc.ReadInt16(); // Skip
                        int length = pvSrc.ReadUInt16();

                        if (length > 511)
                            return;

                        string text = pvSrc.ReadUnicodeString(length);

                        EventSink.InvokeChangeProfileRequest(new ChangeProfileRequestEventArgs(beholder, beheld, text));

                        break;
                    }
            }
        }

        public static void Disconnect(NetState state, PacketReader pvSrc)
        {
            int minusOne = pvSrc.ReadInt32();
        }

        public static void LiftReq(NetState state, PacketReader pvSrc)
        {
            Serial serial = pvSrc.ReadInt32();
            int amount = pvSrc.ReadUInt16();
            Item item = World.FindItem(serial);

            bool rejected;
            LRReason reject;

            state.Mobile.Lift(item, amount, out rejected, out reject);
        }

        public static void EquipReq(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;
            Item item = from.Holding;

            bool valid = (item != null && item.HeldBy == from && item.Map == Map.Internal);

            from.Holding = null;

            if (!valid)
            {
                return;
            }

            pvSrc.Seek(5, SeekOrigin.Current);
            Mobile to = World.FindMobile(pvSrc.ReadInt32());

            if (to == null)
                to = from;

            if (!to.AllowEquipFrom(from) || !to.EquipItem(item))
                item.Bounce(from);

            item.ClearBounce();
        }

        public static void DropReq(NetState state, PacketReader pvSrc)
        {
            pvSrc.ReadInt32(); // serial, ignored
            int x = pvSrc.ReadInt16();
            int y = pvSrc.ReadInt16();
            int z = pvSrc.ReadSByte();
            Serial dest = pvSrc.ReadInt32();

            Point3D loc = new Point3D(x, y, z);

            Mobile from = state.Mobile;

            if (dest.IsMobile)
                from.Drop(World.FindMobile(dest), loc);
            else if (dest.IsItem)
                from.Drop(World.FindItem(dest), loc);
            else
                from.Drop(loc);
        }

        public static void DropReq6017(NetState state, PacketReader pvSrc)
        {
            pvSrc.ReadInt32(); // serial, ignored
            int x = pvSrc.ReadInt16();
            int y = pvSrc.ReadInt16();
            int z = pvSrc.ReadSByte();
            pvSrc.ReadByte(); // Grid Location?
            Serial dest = pvSrc.ReadInt32();

            Point3D loc = new Point3D(x, y, z);

            Mobile from = state.Mobile;

            if (dest.IsMobile)
                from.Drop(World.FindMobile(dest), loc);
            else if (dest.IsItem)
                from.Drop(World.FindItem(dest), loc);
            else
                from.Drop(loc);
        }

        public static void ConfigurationFile(NetState state, PacketReader pvSrc)
        {
        }

        public static void LogoutReq(NetState state, PacketReader pvSrc)
        {
            state.Send(new LogoutAck());
        }

        public static void ChangeSkillLock(NetState state, PacketReader pvSrc)
        {
            Skill s = state.Mobile.Skills[pvSrc.ReadInt16()];

            if (s != null)
                s.SetLockNoRelay((SkillLock)pvSrc.ReadByte());
        }

        public static void HelpRequest(NetState state, PacketReader pvSrc)
        {
            EventSink.InvokeHelpRequest(new HelpRequestEventArgs(state.Mobile));
        }

        public static void TargetResponse(NetState state, PacketReader pvSrc)
        {
            int type = pvSrc.ReadByte();
            int targetID = pvSrc.ReadInt32();
            int flags = pvSrc.ReadByte();
            Serial serial = pvSrc.ReadInt32();
            int x = pvSrc.ReadInt16(), y = pvSrc.ReadInt16(), z = pvSrc.ReadInt16();
            int graphic = pvSrc.ReadUInt16();

            if (targetID == unchecked((int)0xDEADBEEF))
                return;

            Mobile from = state.Mobile;

            Target t = from.Target;

            if (t != null)
            {
                TargetProfile prof = TargetProfile.Acquire(t.GetType());

                if (prof != null)
                {
                    prof.Start();
                }

                try
                {
                    if (x == -1 && y == -1 && !serial.IsValid)
                    {
                        // User pressed escape
                        t.Cancel(from, TargetCancelType.Canceled);
                    }
                    else
                    {
                        object toTarget;

                        if (type == 1)
                        {
                            if (graphic == 0)
                            {
                                toTarget = new LandTarget(new Point3D(x, y, z), from.Map);
                            }
                            else
                            {
                                Map map = from.Map;

                                if (map == null || map == Map.Internal)
                                {
                                    t.Cancel(from, TargetCancelType.Canceled);
                                    return;
                                }
                                else
                                {
                                    StaticTile[] tiles = map.Tiles.GetStaticTiles(x, y, !t.DisallowMultis);

                                    bool valid = false;

                                    if (state.HighSeas)
                                    {
                                        ItemData id = TileData.ItemTable[graphic & TileData.MaxItemValue];
                                        if (id.Surface)
                                        {
                                            z -= id.Height;
                                        }
                                    }

                                    for (int i = 0; !valid && i < tiles.Length; ++i)
                                    {
                                        if (tiles[i].Z == z && tiles[i].ID == graphic)
                                            valid = true;
                                    }

                                    if (!valid)
                                    {
                                        t.Cancel(from, TargetCancelType.Canceled);
                                        return;
                                    }
                                    else
                                    {
                                        toTarget = new StaticTarget(new Point3D(x, y, z), graphic);
                                    }
                                }
                            }
                        }
                        else if (serial.IsMobile)
                        {
                            toTarget = World.FindMobile(serial);
                        }
                        else if (serial.IsItem)
                        {
                            toTarget = World.FindItem(serial);
                        }
                        else
                        {
                            t.Cancel(from, TargetCancelType.Canceled);
                            return;
                        }

                        t.Invoke(from, toTarget);
                    }
                }
                finally
                {
                    if (prof != null)
                    {
                        prof.Finish();
                    }
                }
            }
        }

        public static void DisplayGumpResponse(NetState state, PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int typeID = pvSrc.ReadInt32();
            int buttonID = pvSrc.ReadInt32();

            foreach (Gump gump in state.Gumps)
            {
                if (gump.Serial == serial && gump.TypeID == typeID)
                {
                    var buttonExists = buttonID == 0; // 0 is always 'close'

                    if (!buttonExists)
                    {
                        foreach (var e in gump.Entries)
                        {
                            if (e is GumpButton && ((GumpButton)e).ButtonID == buttonID)
                            {
                                buttonExists = true;
                                break;
                            }

                            if (e is GumpImageTileButton && ((GumpImageTileButton)e).ButtonID == buttonID)
                            {
                                buttonExists = true;
                                break;
                            }
                        }
                    }

                    if (!buttonExists)
                    {
                        state.WriteConsole("Invalid gump response, disconnecting...");
                        state.Dispose();
                        return;
                    }

                    int switchCount = pvSrc.ReadInt32();

                    if (switchCount < 0 || switchCount > gump.m_Switches)
                    {
                        state.WriteConsole("Invalid gump response, disconnecting...");
                        state.Dispose();
                        return;
                    }

                    int[] switches = new int[switchCount];

                    for (int j = 0; j < switches.Length; ++j)
                        switches[j] = pvSrc.ReadInt32();

                    int textCount = pvSrc.ReadInt32();

                    if (textCount < 0 || textCount > gump.m_TextEntries)
                    {
                        state.WriteConsole("Invalid gump response, disconnecting...");
                        state.Dispose();
                        return;
                    }

                    TextRelay[] textEntries = new TextRelay[textCount];

                    for (int j = 0; j < textEntries.Length; ++j)
                    {
                        int entryID = pvSrc.ReadUInt16();
                        int textLength = pvSrc.ReadUInt16();

                        if (textLength > 239)
                        {
                            state.WriteConsole("Invalid gump response, disconnecting...");
                            state.Dispose();
                            return;
                        }

                        string text = pvSrc.ReadUnicodeStringSafe(textLength);
                        textEntries[j] = new TextRelay(entryID, text);
                    }

                    state.RemoveGump(gump);

                    GumpProfile prof = GumpProfile.Acquire(gump.GetType());

                    if (prof != null)
                    {
                        prof.Start();
                    }

                    gump.OnResponse(state, new RelayInfo(buttonID, switches, textEntries));

                    if (prof != null)
                    {
                        prof.Finish();
                    }

                    return;
                }
            }

            if (typeID == 461)
            { // Virtue gump
                int switchCount = pvSrc.ReadInt32();

                if (buttonID == 1 && switchCount > 0)
                {
                    Mobile beheld = World.FindMobile(pvSrc.ReadInt32());

                    if (beheld != null)
                    {
                        EventSink.InvokeVirtueGumpRequest(new VirtueGumpRequestEventArgs(state.Mobile, beheld));
                    }
                }
                else
                {
                    Mobile beheld = World.FindMobile(serial);

                    if (beheld != null)
                    {
                        EventSink.InvokeVirtueItemRequest(new VirtueItemRequestEventArgs(state.Mobile, beheld, buttonID));
                    }
                }
            }
        }

        public static void SetWarMode(NetState state, PacketReader pvSrc)
        {
            state.Mobile.DelayChangeWarmode(pvSrc.ReadBoolean());
        }

        public static void Resynchronize(NetState state, PacketReader pvSrc)
        {
            Mobile m = state.Mobile;

            if (state.StygianAbyss)
            {
                state.Send(new MobileUpdate(m));
                state.Send(new MobileIncoming(m, m));
            }
            else
            {
                state.Send(new MobileUpdateOld(m));
                state.Send(new MobileIncomingOld(m, m));
            }

            m.SendEverything();

            state.Sequence = 0;

            m.ClearFastwalkStack();
        }

        private static int[] m_EmptyInts = new int[0];

        public static void AsciiSpeech(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;

            MessageType type = (MessageType)pvSrc.ReadByte();
            int hue = pvSrc.ReadInt16();
            pvSrc.ReadInt16(); // font
            string text = pvSrc.ReadStringSafe().Trim();

            if (text.Length <= 0 || text.Length > 128)
                return;

            if (!Enum.IsDefined(typeof(MessageType), type))
                type = MessageType.Regular;

            from.DoSpeech(text, m_EmptyInts, type, Utility.ClipDyedHue(hue));
        }

        private static KeywordList m_KeywordList = new KeywordList();

        public static void UnicodeSpeech(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;

            MessageType type = (MessageType)pvSrc.ReadByte();
            int hue = pvSrc.ReadInt16();
            pvSrc.ReadInt16(); // font
            string lang = pvSrc.ReadString(4);
            string text;

            bool isEncoded = (type & MessageType.Encoded) != 0;
            int[] keywords;

            if (isEncoded)
            {
                int value = pvSrc.ReadInt16();
                int count = (value & 0xFFF0) >> 4;
                int hold = value & 0xF;

                if (count < 0 || count > 50)
                    return;

                KeywordList keyList = m_KeywordList;

                for (int i = 0; i < count; ++i)
                {
                    int speechID;

                    if ((i & 1) == 0)
                    {
                        hold <<= 8;
                        hold |= pvSrc.ReadByte();
                        speechID = hold;
                        hold = 0;
                    }
                    else
                    {
                        value = pvSrc.ReadInt16();
                        speechID = (value & 0xFFF0) >> 4;
                        hold = value & 0xF;
                    }

                    if (!keyList.Contains(speechID))
                        keyList.Add(speechID);
                }

                text = pvSrc.ReadUTF8StringSafe();

                keywords = keyList.ToArray();
            }
            else
            {
                text = pvSrc.ReadUnicodeStringSafe();

                keywords = m_EmptyInts;
            }

            text = text.Trim();

            if (text.Length <= 0 || text.Length > 128)
                return;

            type &= ~MessageType.Encoded;

            if (!Enum.IsDefined(typeof(MessageType), type))
                type = MessageType.Regular;

            from.Language = lang;
            from.DoSpeech(text, keywords, type, Utility.ClipDyedHue(hue));
        }

        public static void UseReq(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;

            if (from.AccessLevel >= AccessLevel.Counselor || DateTime.Now >= from.NextActionTime)
            {
                int value = pvSrc.ReadInt32();

                if ((value & ~0x7FFFFFFF) != 0)
                {
                    from.OnPaperdollRequest();
                }
                else
                {
                    Serial s = value;

                    if (s.IsMobile)
                    {
                        Mobile m = World.FindMobile(s);

                        if (m != null && !m.Deleted)
                            from.Use(m);
                    }
                    else if (s.IsItem)
                    {
                        Item item = World.FindItem(s);

                        if (item != null && !item.Deleted)
                            from.Use(item);
                    }
                }

                from.NextActionTime = DateTime.Now + TimeSpan.FromSeconds(0.1);
            }
            else
            {
                from.SendActionMessage();
            }
        }

        private static bool m_SingleClickProps;

        public static bool SingleClickProps
        {
            get { return m_SingleClickProps; }
            set { m_SingleClickProps = value; }
        }

        public static void LookReq(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;

            Serial s = pvSrc.ReadInt32();

            if (s.IsMobile)
            {
                Mobile m = World.FindMobile(s);

                if (m != null && from.CanSee(m) && Utility.InUpdateRange(from, m))
                {
                    if (m_SingleClickProps)
                    {
                        m.OnAosSingleClick(from);
                    }
                    else
                    {
                        if (from.Region.OnSingleClick(from, m))
                            m.OnSingleClick(from);
                    }
                }
            }
            else if (s.IsItem)
            {
                Item item = World.FindItem(s);

                if (item != null && !item.Deleted && from.CanSee(item) && Utility.InUpdateRange(from.Location, item.GetWorldLocation()))
                {
                    if (m_SingleClickProps)
                    {
                        item.OnAosSingleClick(from);
                    }
                    else if (from.Region.OnSingleClick(from, item))
                    {
                        if (item.Parent is Item)
                            ((Item)item.Parent).OnSingleClickContained(from, item);

                        item.OnSingleClick(from);
                    }
                }
            }
        }

        public static void PingReq(NetState state, PacketReader pvSrc)
        {
            state.Send(PingAck.Instantiate(pvSrc.ReadByte()));
        }

        public static void SetUpdateRange(NetState state, PacketReader pvSrc)
        {
            state.Send(ChangeUpdateRange.Instantiate(18));
        }

        private const int BadFood = unchecked((int)0xBAADF00D);
        private const int BadUOTD = unchecked((int)0xFFCEFFCE);

        public static void MovementReq(NetState state, PacketReader pvSrc)
        {
            Direction dir = (Direction)pvSrc.ReadByte();
            int seq = pvSrc.ReadByte();
            int key = pvSrc.ReadInt32();

            Mobile m = state.Mobile;

            if ((state.Sequence == 0 && seq != 0) || !m.Move(dir))
            {
                state.Send(new MovementRej(seq, m));
                state.Sequence = 0;

                m.ClearFastwalkStack();
            }
            else
            {
                ++seq;

                if (seq == 256)
                    seq = 1;

                state.Sequence = seq;
            }
        }

        public static int[] m_ValidAnimations = new int[]
            {
                6, 21, 32, 33,
                100, 101, 102,
                103, 104, 105,
                106, 107, 108,
                109, 110, 111,
                112, 113, 114,
                115, 116, 117,
                118, 119, 120,
                121, 123, 124,
                125, 126, 127,
                128
            };

        public static int[] ValidAnimations { get { return m_ValidAnimations; } set { m_ValidAnimations = value; } }

        public static void Animate(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;
            int action = pvSrc.ReadInt32();

            action = Mobile.AnimateMod(action, from.Body, from.RaceID);

            bool ok = false;

            for (int i = 0; !ok && i < m_ValidAnimations.Length; ++i)
                ok = (action == m_ValidAnimations[i]);

            if (from != null && ok && from.Alive && from.Body.IsHuman && !from.Mounted)
                from.Animate(action, 7, 1, true, false, 0);
        }

        public static void QuestArrow(NetState state, PacketReader pvSrc)
        {
            bool rightClick = pvSrc.ReadBoolean();
            Mobile from = state.Mobile;

            if (from != null && from.QuestArrow != null)
                from.QuestArrow.OnClick(rightClick);
        }

        public static void ExtendedCommand(NetState state, PacketReader pvSrc)
        {
            int packetID = pvSrc.ReadUInt16();

            PacketHandler ph = GetExtendedHandler(packetID);

            if (ph != null)
            {
                if (ph.Ingame && state.Mobile == null)
                {
                    Console.WriteLine("Client: {0}: Sent ingame packet (0xBFx{1:X2}) before having been attached to a mobile", state, packetID);
                    state.Dispose();
                }
                else if (ph.Ingame && state.Mobile.Deleted)
                {
                    state.Dispose();
                }
                else
                {
                    ph.OnReceive(state, pvSrc);
                }
            }
            else
            {
                pvSrc.Trace(state);
            }
        }

        public static void CastSpell(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;

            if (from == null)
                return;

            Item spellbook = null;

            if (pvSrc.ReadInt16() == 1)
                spellbook = World.FindItem(pvSrc.ReadInt32());

            int spellID = pvSrc.ReadInt16() - 1;

            EventSink.InvokeCastSpellRequest(new CastSpellRequestEventArgs(from, spellID, spellbook));
        }

        public static void BatchQueryProperties(NetState state, PacketReader pvSrc)
        {
            if (!ObjectPropertyList.Enabled)
                return;

            Mobile from = state.Mobile;

            int length = pvSrc.Size - 3;

            if (length < 0 || (length % 4) != 0)
                return;

            int count = length / 4;

            for (int i = 0; i < count; ++i)
            {
                Serial s = pvSrc.ReadInt32();

                if (s.IsMobile)
                {
                    Mobile m = World.FindMobile(s);

                    if (m != null && from.CanSee(m) && Utility.InUpdateRange(from, m))
                        m.SendPropertiesTo(from);
                }
                else if (s.IsItem)
                {
                    Item item = World.FindItem(s);

                    if (item != null && !item.Deleted && from.CanSee(item) && Utility.InUpdateRange(from.Location, item.GetWorldLocation()))
                        item.SendPropertiesTo(from);
                }
            }
        }

        public static void QueryProperties(NetState state, PacketReader pvSrc)
        {
            if (!ObjectPropertyList.Enabled)
                return;

            Mobile from = state.Mobile;

            Serial s = pvSrc.ReadInt32();

            if (s.IsMobile)
            {
                Mobile m = World.FindMobile(s);

                if (m != null && from.CanSee(m) && Utility.InUpdateRange(from, m))
                    m.SendPropertiesTo(from);
            }
            else if (s.IsItem)
            {
                Item item = World.FindItem(s);

                if (item != null && !item.Deleted && from.CanSee(item) && Utility.InUpdateRange(from.Location, item.GetWorldLocation()))
                    item.SendPropertiesTo(from);
            }
        }

        public static void PartyMessage(NetState state, PacketReader pvSrc)
        {
            if (state.Mobile == null)
                return;

            switch (pvSrc.ReadByte())
            {
                case 0x01: PartyMessage_AddMember(state, pvSrc); break;
                case 0x02: PartyMessage_RemoveMember(state, pvSrc); break;
                case 0x03: PartyMessage_PrivateMessage(state, pvSrc); break;
                case 0x04: PartyMessage_PublicMessage(state, pvSrc); break;
                case 0x06: PartyMessage_SetCanLoot(state, pvSrc); break;
                case 0x08: PartyMessage_Accept(state, pvSrc); break;
                case 0x09: PartyMessage_Decline(state, pvSrc); break;
                default: pvSrc.Trace(state); break;
            }
        }

        public static void PartyMessage_AddMember(NetState state, PacketReader pvSrc)
        {
            if (PartyCommands.Handler != null)
                PartyCommands.Handler.OnAdd(state.Mobile);
        }

        public static void PartyMessage_RemoveMember(NetState state, PacketReader pvSrc)
        {
            if (PartyCommands.Handler != null)
                PartyCommands.Handler.OnRemove(state.Mobile, World.FindMobile(pvSrc.ReadInt32()));
        }

        public static void PartyMessage_PrivateMessage(NetState state, PacketReader pvSrc)
        {
            if (PartyCommands.Handler != null)
                PartyCommands.Handler.OnPrivateMessage(state.Mobile, World.FindMobile(pvSrc.ReadInt32()), pvSrc.ReadUnicodeStringSafe());
        }

        public static void PartyMessage_PublicMessage(NetState state, PacketReader pvSrc)
        {
            if (PartyCommands.Handler != null)
                PartyCommands.Handler.OnPublicMessage(state.Mobile, pvSrc.ReadUnicodeStringSafe());
        }

        public static void PartyMessage_SetCanLoot(NetState state, PacketReader pvSrc)
        {
            if (PartyCommands.Handler != null)
                PartyCommands.Handler.OnSetCanLoot(state.Mobile, pvSrc.ReadBoolean());
        }

        public static void PartyMessage_Accept(NetState state, PacketReader pvSrc)
        {
            if (PartyCommands.Handler != null)
                PartyCommands.Handler.OnAccept(state.Mobile, World.FindMobile(pvSrc.ReadInt32()));
        }

        public static void PartyMessage_Decline(NetState state, PacketReader pvSrc)
        {
            if (PartyCommands.Handler != null)
                PartyCommands.Handler.OnDecline(state.Mobile, World.FindMobile(pvSrc.ReadInt32()));
        }

        public static void StunRequest(NetState state, PacketReader pvSrc)
        {
            EventSink.InvokeStunRequest(new StunRequestEventArgs(state.Mobile));
        }

        public static void DisarmRequest(NetState state, PacketReader pvSrc)
        {
            EventSink.InvokeDisarmRequest(new DisarmRequestEventArgs(state.Mobile));
        }

        public static void StatLockChange(NetState state, PacketReader pvSrc)
        {
            int stat = pvSrc.ReadByte();
            int lockValue = pvSrc.ReadByte();

            if (lockValue > 2) lockValue = 0;

            Mobile m = state.Mobile;

            if (m != null)
            {
                switch (stat)
                {
                    case 0: m.StrLock = (StatLockType)lockValue; break;
                    case 1: m.DexLock = (StatLockType)lockValue; break;
                    case 2: m.IntLock = (StatLockType)lockValue; break;
                }
            }
        }

        public static void ScreenSize(NetState state, PacketReader pvSrc)
        {
            int width = pvSrc.ReadInt32();
            int unk = pvSrc.ReadInt32();
        }

        public static void ContextMenuResponse(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;

            if (from != null)
            {
                ContextMenu menu = from.ContextMenu;

                from.ContextMenu = null;

                if (menu != null && from != null && from == menu.From)
                {
                    IEntity entity = World.FindEntity(pvSrc.ReadInt32());

                    if (entity != null && entity == menu.Target && from.CanSee(entity))
                    {
                        Point3D p;

                        if (entity is Mobile)
                            p = entity.Location;
                        else if (entity is Item)
                            p = ((Item)entity).GetWorldLocation();
                        else
                            return;

                        int index = pvSrc.ReadUInt16();

                        if (index >= 0 && index < menu.Entries.Length)
                        {
                            ContextMenuEntry e = menu.Entries[index];

                            int range = e.Range;

                            if (range == -1)
                                range = 18;

                            if (e.Enabled && from.InRange(p, range))
                                e.OnClick();
                        }
                    }
                }
            }
        }

        public static void ContextMenuRequest(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;
            IEntity target = World.FindEntity(pvSrc.ReadInt32());

            if (from != null && target != null && from.Map == target.Map && from.CanSee(target))
            {
                if (target is Mobile && !Utility.InUpdateRange(from.Location, target.Location))
                    return;
                else if (target is Item && !Utility.InUpdateRange(from.Location, ((Item)target).GetWorldLocation()))
                    return;

                if (!from.CheckContextMenuDisplay(target))
                    return;

                ContextMenu c = new ContextMenu(from, target);

                if (c.Entries.Length > 0)
                {
                    if (target is Item)
                    {
                        object root = ((Item)target).RootParent;

                        if (root is Mobile && root != from && ((Mobile)root).AccessLevel >= from.AccessLevel)
                        {
                            for (int i = 0; i < c.Entries.Length; ++i)
                            {
                                if (!c.Entries[i].NonLocalUse)
                                    c.Entries[i].Enabled = false;
                            }
                        }
                    }

                    from.ContextMenu = c;
                }
            }
        }

        public static void CloseStatus(NetState state, PacketReader pvSrc)
        {
            Serial serial = pvSrc.ReadInt32();
        }

        public static void Language(NetState state, PacketReader pvSrc)
        {
            string lang = pvSrc.ReadString(4);

            if (state.Mobile != null)
                state.Mobile.Language = lang;
        }

        public static void AssistVersion(NetState state, PacketReader pvSrc)
        {
            int unk = pvSrc.ReadInt32();
            string av = pvSrc.ReadString();
        }

        public static void ClientVersion(NetState state, PacketReader pvSrc)
        {
            CV version = state.Version = new CV(pvSrc.ReadString());

            EventSink.InvokeClientVersionReceived(new ClientVersionReceivedArgs(state, version));
        }

        public static void ClientType(NetState state, PacketReader pvSrc)
        {
            pvSrc.ReadUInt16();

            int type = pvSrc.ReadUInt16();
            CV version = state.Version = new CV(pvSrc.ReadString());

            //EventSink.InvokeClientVersionReceived( new ClientVersionReceivedArgs( state, version ) );//todo
        }

        public static void MobileQuery(NetState state, PacketReader pvSrc)
        {
            Mobile from = state.Mobile;

            pvSrc.ReadInt32(); // 0xEDEDEDED
            int type = pvSrc.ReadByte();
            Mobile m = World.FindMobile(pvSrc.ReadInt32());

            if (m != null)
            {
                switch (type)
                {
                    case 0x00: // Unknown, sent by godclient
                        {
                            if (VerifyGC(state))
                                Console.WriteLine("God Client: {0}: Query 0x{1:X2} on {2} '{3}'", state, type, m.Serial, m.Name);

                            break;
                        }
                    case 0x04: // Stats
                        {
                            m.OnStatsQuery(from);
                            break;
                        }
                    case 0x05:
                        {
                            m.OnSkillsQuery(from);
                            break;
                        }
                    default:
                        {
                            pvSrc.Trace(state);
                            break;
                        }
                }
            }
        }

        private class LoginTimer : Timer
        {
            private NetState m_State;
            private Mobile m_Mobile;

            public LoginTimer(NetState state, Mobile m) : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0))
            {
                m_State = state;
                m_Mobile = m;
            }

            protected override void OnTick()
            {
                if (m_State == null)
                    Stop();
                if (m_State.Version != null)
                {
                    m_State.BlockAllPackets = false;
                    DoLogin(m_State, m_Mobile);
                    Stop();
                }
            }
        }

        public static void PlayCharacter(NetState state, PacketReader pvSrc)
        {
            pvSrc.ReadInt32(); // 0xEDEDEDED

            string name = pvSrc.ReadString(30);

            pvSrc.Seek(2, SeekOrigin.Current);
            int flags = pvSrc.ReadInt32();
            pvSrc.Seek(24, SeekOrigin.Current);

            int charSlot = pvSrc.ReadInt32();
            int clientIP = pvSrc.ReadInt32();

            IAccount a = state.Account;

            if (a == null || charSlot < 0 || charSlot >= a.Length)
            {
                state.Dispose();
            }
            else
            {
                Mobile m = a[charSlot];

                // Check if anyone is using this account
                for (int i = 0; i < a.Length; ++i)
                {
                    Mobile check = a[i];

                    if (check != null && check.Map != Map.Internal && check != m)
                    {
                        Console.WriteLine("Login: {0}: Account in use", state);
                        state.Send(new PopupMessage(PMMessage.CharInWorld));
                        return;
                    }
                }

                if (m == null)
                {
                    state.Dispose();
                }
                else
                {
                    if (m.NetState != null)
                        m.NetState.Dispose();

                    NetState.ProcessDisposedQueue();

                    state.Send(new ClientVersionReq());

                    state.BlockAllPackets = true;

                    state.Flags = (ClientFlags)flags;

                    state.Mobile = m;
                    m.NetState = state;

                    new LoginTimer(state, m).Start();
                }
            }
        }

        public static void DoLogin(NetState state, Mobile m)
        {
            state.Send(new LoginConfirm(m));

            if (m.Map != null)
                state.Send(new MapChange(m));

            state.Send(new MapPatches());

            state.Send(SeasonChange.Instantiate(m.GetSeason(), true));

            state.Send(SupportedFeatures.Instantiate(state));

            state.Sequence = 0;

            if (state.StygianAbyss)
            {
                state.Send(new MobileUpdate(m));
                state.Send(new MobileUpdate(m));

                m.CheckLightLevels(true);

                state.Send(new MobileUpdate(m));

                state.Send(new MobileIncoming(m, m));
                //state.Send( new MobileAttributes( m ) );
                state.Send(new MobileStatus(m, m));
                state.Send(Server.Network.SetWarMode.Instantiate(m.Warmode));

                m.SendEverything();

                state.Send(SupportedFeatures.Instantiate(state));
                state.Send(new MobileUpdate(m));
                //state.Send( new MobileAttributes( m ) );
                state.Send(new MobileStatus(m, m));
                state.Send(Server.Network.SetWarMode.Instantiate(m.Warmode));
                state.Send(new MobileIncoming(m, m));
            }
            else
            {
                state.Send(new MobileUpdateOld(m));
                state.Send(new MobileUpdateOld(m));

                m.CheckLightLevels(true);

                state.Send(new MobileUpdateOld(m));

                state.Send(new MobileIncomingOld(m, m));
                //state.Send( new MobileAttributes( m ) );
                state.Send(new MobileStatus(m, m));
                state.Send(Server.Network.SetWarMode.Instantiate(m.Warmode));

                m.SendEverything();

                state.Send(SupportedFeatures.Instantiate(state));
                state.Send(new MobileUpdateOld(m));
                //state.Send( new MobileAttributes( m ) );
                state.Send(new MobileStatus(m, m));
                state.Send(Server.Network.SetWarMode.Instantiate(m.Warmode));
                state.Send(new MobileIncomingOld(m, m));
            }

            state.Send(LoginComplete.Instance);
            state.Send(new CurrentTime());
            state.Send(SeasonChange.Instantiate(m.GetSeason(), true));
            state.Send(new MapChange(m));

            EventSink.InvokeLogin(new LoginEventArgs(m));

            m.ClearFastwalkStack();
        }

        public static void CreateCharacter(NetState state, PacketReader pvSrc)
        {
            int unk1 = pvSrc.ReadInt32();
            int unk2 = pvSrc.ReadInt32();
            int unk3 = pvSrc.ReadByte();
            string name = pvSrc.ReadString(30);

            pvSrc.Seek(2, SeekOrigin.Current);
            int flags = pvSrc.ReadInt32();
            pvSrc.Seek(8, SeekOrigin.Current);
            int prof = pvSrc.ReadByte();
            pvSrc.Seek(15, SeekOrigin.Current);

            //bool female = pvSrc.ReadBoolean();

            int genderRace = pvSrc.ReadByte();

            int str = pvSrc.ReadByte();
            int dex = pvSrc.ReadByte();
            int intl = pvSrc.ReadByte();
            int is1 = pvSrc.ReadByte();
            int vs1 = pvSrc.ReadByte();
            int is2 = pvSrc.ReadByte();
            int vs2 = pvSrc.ReadByte();
            int is3 = pvSrc.ReadByte();
            int vs3 = pvSrc.ReadByte();
            int hue = pvSrc.ReadUInt16();
            int hairVal = pvSrc.ReadInt16();
            int hairHue = pvSrc.ReadInt16();
            int hairValf = pvSrc.ReadInt16();
            int hairHuef = pvSrc.ReadInt16();
            pvSrc.ReadByte();
            int cityIndex = pvSrc.ReadByte();
            int charSlot = pvSrc.ReadInt32();
            int clientIP = pvSrc.ReadInt32();
            int shirtHue = pvSrc.ReadInt16();
            int pantsHue = pvSrc.ReadInt16();

            /*
			Pre-7.0.0.0:
			0x00, 0x01 -> Human Male, Human Female
			0x02, 0x03 -> Elf Male, Elf Female

			Post-7.0.0.0:
			0x00, 0x01
			0x02, 0x03 -> Human Male, Human Female
			0x04, 0x05 -> Elf Male, Elf Female
			0x05, 0x06 -> Gargoyle Male, Gargoyle Female
			*/

            bool female = ((genderRace % 2) != 0);

            Race race = null;

            if (state.StygianAbyss)
            {
                byte raceID = (byte)(genderRace < 4 ? 0 : ((genderRace / 2) - 1));
                race = Race.Races[raceID];
            }
            else
            {
                race = Race.Races[(byte)(genderRace / 2)];
            }

            if (race == null)
                race = Race.DefaultRace;

            CityInfo[] info = state.CityInfo;
            IAccount a = state.Account;

            if (info == null || a == null || cityIndex < 0 || cityIndex >= info.Length)
            {
                state.Dispose();
            }
            else
            {
                // Check if anyone is using this account
                for (int i = 0; i < a.Length; ++i)
                {
                    Mobile check = a[i];

                    if (check != null && check.Map != Map.Internal)
                    {
                        Console.WriteLine("Login: {0}: Account in use", state);
                        state.Send(new PopupMessage(PMMessage.CharInWorld));
                        return;
                    }
                }

                state.Flags = (ClientFlags)flags;

                CharacterCreatedEventArgs args = new CharacterCreatedEventArgs(
                    state, a,
                    name, female, hue,
                    str, dex, intl,
                    info[cityIndex],
                    new SkillNameValue[3]
                    {
                        new SkillNameValue( (SkillName)is1, vs1 ),
                        new SkillNameValue( (SkillName)is2, vs2 ),
                        new SkillNameValue( (SkillName)is3, vs3 ),
                    },
                    shirtHue, pantsHue,
                    hairVal, hairHue,
                    hairValf, hairHuef,
                    prof,
                    race
                    );

                state.Send(new ClientVersionReq());

                state.BlockAllPackets = true;

                EventSink.InvokeCharacterCreated(args);

                Mobile m = args.Mobile;

                if (m != null)
                {
                    state.Mobile = m;
                    m.NetState = state;
                    new LoginTimer(state, m).Start();
                }
                else
                {
                    state.BlockAllPackets = false;
                    state.Dispose();
                }
            }
        }

        public static void CreateCharacter70160(NetState state, PacketReader pvSrc)
        {
            int unk1 = pvSrc.ReadInt32();
            int unk2 = pvSrc.ReadInt32();
            int unk3 = pvSrc.ReadByte();
            string name = pvSrc.ReadString(30);

            pvSrc.Seek(2, SeekOrigin.Current);
            int flags = pvSrc.ReadInt32();
            pvSrc.Seek(8, SeekOrigin.Current);
            int prof = pvSrc.ReadByte();
            pvSrc.Seek(15, SeekOrigin.Current);

            int genderRace = pvSrc.ReadByte();

            int str = pvSrc.ReadByte();
            int dex = pvSrc.ReadByte();
            int intl = pvSrc.ReadByte();
            int is1 = pvSrc.ReadByte();
            int vs1 = pvSrc.ReadByte();
            int is2 = pvSrc.ReadByte();
            int vs2 = pvSrc.ReadByte();
            int is3 = pvSrc.ReadByte();
            int vs3 = pvSrc.ReadByte();
            int is4 = pvSrc.ReadByte();
            int vs4 = pvSrc.ReadByte();

            int hue = pvSrc.ReadUInt16();
            int hairVal = pvSrc.ReadInt16();
            int hairHue = pvSrc.ReadInt16();
            int hairValf = pvSrc.ReadInt16();
            int hairHuef = pvSrc.ReadInt16();
            pvSrc.ReadByte();
            int cityIndex = pvSrc.ReadByte();
            int charSlot = pvSrc.ReadInt32();
            int clientIP = pvSrc.ReadInt32();
            int shirtHue = pvSrc.ReadInt16();
            int pantsHue = pvSrc.ReadInt16();

            /*
			0x00, 0x01
			0x02, 0x03 -> Human Male, Human Female
			0x04, 0x05 -> Elf Male, Elf Female
			0x05, 0x06 -> Gargoyle Male, Gargoyle Female
			*/

            bool female = ((genderRace % 2) != 0);

            Race race = null;

            byte raceID = (byte)(genderRace < 4 ? 0 : ((genderRace / 2) - 1));
            race = Race.Races[raceID];

            if (race == null)
                race = Race.DefaultRace;

            CityInfo[] info = state.CityInfo;
            IAccount a = state.Account;

            if (info == null || a == null || cityIndex < 0 || cityIndex >= info.Length)
            {
                state.Dispose();
            }
            else
            {
                // Check if anyone is using this account
                for (int i = 0; i < a.Length; ++i)
                {
                    Mobile check = a[i];

                    if (check != null && check.Map != Map.Internal)
                    {
                        Console.WriteLine("Login: {0}: Account in use", state);
                        state.Send(new PopupMessage(PMMessage.CharInWorld));
                        return;
                    }
                }

                state.Flags = (ClientFlags)flags;

                CharacterCreatedEventArgs args = new CharacterCreatedEventArgs(
                    state, a,
                    name, female, hue,
                    str, dex, intl,
                    info[cityIndex],
                    new SkillNameValue[4]
                    {
                        new SkillNameValue( (SkillName)is1, vs1 ),
                        new SkillNameValue( (SkillName)is2, vs2 ),
                        new SkillNameValue( (SkillName)is3, vs3 ),
                        new SkillNameValue( (SkillName)is4, vs4 ),
                    },
                    shirtHue, pantsHue,
                    hairVal, hairHue,
                    hairValf, hairHuef,
                    prof,
                    race
                    );

                state.Send(new ClientVersionReq());

                state.BlockAllPackets = true;

                EventSink.InvokeCharacterCreated(args);

                Mobile m = args.Mobile;

                if (m != null)
                {
                    state.Mobile = m;
                    m.NetState = state;
                    new LoginTimer(state, m).Start();
                }
                else
                {
                    state.BlockAllPackets = false;
                    state.Dispose();
                }
            }
        }


        private static bool m_ClientVerification = true;

        public static bool ClientVerification
        {
            get { return m_ClientVerification; }
            set { m_ClientVerification = value; }
        }

        internal struct AuthIDPersistence
        {
            public DateTime Age;
            public ClientVersion Version;

            public AuthIDPersistence(ClientVersion v)
            {
                Age = DateTime.Now;
                Version = v;
            }
        }

        private const int m_AuthIDWindowSize = 128;
        private static Dictionary<int, AuthIDPersistence> m_AuthIDWindow = new Dictionary<int, AuthIDPersistence>(m_AuthIDWindowSize);

        private static int GenerateAuthID(NetState state)
        {
            if (m_AuthIDWindow.Count == m_AuthIDWindowSize)
            {
                int oldestID = 0;
                DateTime oldest = DateTime.MaxValue;

                foreach (KeyValuePair<int, AuthIDPersistence> kvp in m_AuthIDWindow)
                {
                    if (kvp.Value.Age < oldest)
                    {
                        oldestID = kvp.Key;
                        oldest = kvp.Value.Age;
                    }
                }

                m_AuthIDWindow.Remove(oldestID);
            }

            int authID;

            do
            {
                authID = Utility.Random(1, int.MaxValue - 1);

                if (Utility.RandomBool())
                    authID |= 1 << 31;
            } while (m_AuthIDWindow.ContainsKey(authID));

            m_AuthIDWindow[authID] = new AuthIDPersistence(state.Version);

            return authID;
        }

        public static void GameLogin(NetState state, PacketReader pvSrc)
        {
            if (state.SentFirstPacket)
            {
                state.Dispose();
                return;
            }

            state.SentFirstPacket = true;

            int authID = pvSrc.ReadInt32();

            if (m_AuthIDWindow.ContainsKey(authID))
            {
                AuthIDPersistence ap = m_AuthIDWindow[authID];
                m_AuthIDWindow.Remove(authID);

                state.Version = ap.Version;
            }
            else if (m_ClientVerification)
            {
                Console.WriteLine("Login: {0}: Invalid client detected, disconnecting", state);
                state.Dispose();
                return;
            }

            if (state.m_AuthID != 0 && authID != state.m_AuthID)
            {
                Console.WriteLine("Login: {0}: Invalid client detected, disconnecting", state);
                state.Dispose();
                return;
            }
            else if (state.m_AuthID == 0 && authID != state.m_Seed)
            {
                Console.WriteLine("Login: {0}: Invalid client detected, disconnecting", state);
                state.Dispose();
                return;
            }

            string username = pvSrc.ReadString(30);
            string password = pvSrc.ReadString(30);

            GameLoginEventArgs e = new GameLoginEventArgs(state, username, password);

            EventSink.InvokeGameLogin(e);

            if (e.Accepted)
            {
                state.CityInfo = e.CityInfo;
                state.CompressionEnabled = true;

                state.Send(SupportedFeatures.Instantiate(state));

                if (state.NewCharacterList)
                {
                    state.Send(new CharacterList(state.Account, state.CityInfo));
                }
                else
                {
                    state.Send(new CharacterListOld(state.Account, state.CityInfo));
                }
            }
            else
            {
                state.Dispose();
            }
        }

        public static void PlayServer(NetState state, PacketReader pvSrc)
        {
            int index = pvSrc.ReadInt16();
            ServerInfo[] info = state.ServerInfo;
            IAccount a = state.Account;

            if (info == null || a == null || index < 0 || index >= info.Length)
            {
                state.Dispose();
            }
            else
            {
                ServerInfo si = info[index];

                state.m_AuthID = PlayServerAck.m_AuthID = GenerateAuthID(state);

                state.SentFirstPacket = false;
                state.Send(new PlayServerAck(si));
            }
        }

        public static void LoginServerSeed(NetState state, PacketReader pvSrc)
        {
            state.m_Seed = pvSrc.ReadInt32();
            state.Seeded = true;

            if (state.m_Seed == 0)
            {
                Console.WriteLine("Login: {0}: Invalid client detected, disconnecting", state);
                state.Dispose();
                return;
            }

            int clientMaj = pvSrc.ReadInt32();
            int clientMin = pvSrc.ReadInt32();
            int clientRev = pvSrc.ReadInt32();
            int clientPat = pvSrc.ReadInt32();

            state.Version = new ClientVersion(clientMaj, clientMin, clientRev, clientPat);
        }

        public static void AccountLogin(NetState state, PacketReader pvSrc)
        {
            if (state.SentFirstPacket)
            {
                state.Dispose();
                return;
            }

            state.SentFirstPacket = true;

            string username = pvSrc.ReadString(30);
            string password = pvSrc.ReadString(30);

            AccountLoginEventArgs e = new AccountLoginEventArgs(state, username, password);

            EventSink.InvokeAccountLogin(e);

            if (e.Accepted)
                AccountLogin_ReplyAck(state);
            else
                AccountLogin_ReplyRej(state, e.RejectReason);
        }

        public static void AccountLogin_ReplyAck(NetState state)
        {
            ServerListEventArgs e = new ServerListEventArgs(state, state.Account);

            EventSink.InvokeServerList(e);

            if (e.Rejected)
            {
                state.Account = null;
                state.Send(new AccountLoginRej(ALRReason.BadComm));
                state.Dispose();
            }
            else
            {
                ServerInfo[] info = e.Servers.ToArray();

                state.ServerInfo = info;

                state.Send(new AccountLoginAck(info));
            }
        }

        public static void AccountLogin_ReplyRej(NetState state, ALRReason reason)
        {
            state.Send(new AccountLoginRej(reason));
            state.Dispose();
        }
    }
}