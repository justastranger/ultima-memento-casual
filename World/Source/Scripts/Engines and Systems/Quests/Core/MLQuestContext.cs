﻿using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Engines.MLQuests
{
    [Flags]
    public enum MLQuestFlag
    {
        None = 0x00,
    }

    [PropertyObject]
    public class MLQuestContext
    {
        private class MLDoneQuestInfo
        {
            public readonly MLQuest Quest;
            public DateTime NextAvailable;

            public MLDoneQuestInfo(MLQuest quest, DateTime nextAvailable)
            {
                Quest = quest;
                NextAvailable = nextAvailable;
            }

            public void Serialize(GenericWriter writer)
            {
                MLQuestSystem.WriteQuestRef(writer, Quest);
                writer.Write(NextAvailable);
            }

            public static MLDoneQuestInfo Deserialize(GenericReader reader, int version)
            {
                MLQuest quest = MLQuestSystem.ReadQuestRef(reader);
                DateTime nextAvailable = reader.ReadDateTime();

                if (quest == null || !quest.RecordCompletion)
                    return null; // forget about this record

                return new MLDoneQuestInfo(quest, nextAvailable);
            }
        }

        private List<MLDoneQuestInfo> m_DoneQuests;
        private MLQuestFlag m_Flags;

        public PlayerMobile Owner { get; private set; }

        public List<MLQuestInstance> QuestInstances { get; private set; }

        public List<MLQuest> ChainOffers { get; private set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsFull
        {
            get { return QuestInstances.Count >= MLQuestSystem.MaxConcurrentQuests; }
        }

        public MLQuestContext(PlayerMobile owner)
        {
            Owner = owner;
            QuestInstances = new List<MLQuestInstance>();
            ChainOffers = new List<MLQuest>();
            m_DoneQuests = new List<MLDoneQuestInfo>();
            m_Flags = MLQuestFlag.None;
        }

        public bool HasDoneQuest(Type questType)
        {
            MLQuest quest = MLQuestSystem.FindQuest(questType);

            return (quest != null && HasDoneQuest(quest));
        }

        public bool HasDoneQuest(MLQuest quest)
        {
            foreach (MLDoneQuestInfo info in m_DoneQuests)
            {
                if (info.Quest == quest)
                    return true;
            }

            return false;
        }

        public bool HasDoneQuest(MLQuest quest, out DateTime nextAvailable)
        {
            nextAvailable = DateTime.MinValue;

            foreach (MLDoneQuestInfo info in m_DoneQuests)
            {
                if (info.Quest == quest)
                {
                    nextAvailable = info.NextAvailable;
                    return true;
                }
            }

            return false;
        }

        public void SetDoneQuest(MLQuest quest)
        {
            SetDoneQuest(quest, DateTime.MinValue);
        }

        public void SetDoneQuest(MLQuest quest, DateTime nextAvailable)
        {
            foreach (MLDoneQuestInfo info in m_DoneQuests)
            {
                if (info.Quest == quest)
                {
                    info.NextAvailable = nextAvailable;
                    return;
                }
            }

            m_DoneQuests.Add(new MLDoneQuestInfo(quest, nextAvailable));
        }

        public void RemoveDoneQuest(MLQuest quest)
        {
            for (int i = m_DoneQuests.Count - 1; i >= 0; --i)
            {
                MLDoneQuestInfo info = m_DoneQuests[i];

                if (info.Quest == quest)
                    m_DoneQuests.RemoveAt(i);
            }
        }

        public void HandleDeath()
        {
            for (int i = QuestInstances.Count - 1; i >= 0; --i)
                QuestInstances[i].OnPlayerDeath();
        }

        public void HandleDeletion()
        {
            for (int i = QuestInstances.Count - 1; i >= 0; --i)
                QuestInstances[i].Remove();
        }

        public MLQuestInstance FindInstance(Type questType)
        {
            MLQuest quest = MLQuestSystem.FindQuest(questType);

            if (quest == null)
                return null;

            return FindInstance(quest);
        }

        public MLQuestInstance FindInstance(MLQuest quest)
        {
            foreach (MLQuestInstance instance in QuestInstances)
            {
                if (instance.Quest == quest)
                    return instance;
            }

            return null;
        }

        public bool IsDoingQuest(Type questType)
        {
            MLQuest quest = MLQuestSystem.FindQuest(questType);

            return (quest != null && IsDoingQuest(quest));
        }

        public bool IsDoingQuest(MLQuest quest)
        {
            return (FindInstance(quest) != null);
        }

        public void Serialize(GenericWriter writer)
        {
            // Version info is written in MLQuestPersistence.Serialize

            writer.WriteMobile<PlayerMobile>(Owner);
            writer.Write(QuestInstances.Count);

            foreach (MLQuestInstance instance in QuestInstances)
                instance.Serialize(writer);

            writer.Write(m_DoneQuests.Count);

            foreach (MLDoneQuestInfo info in m_DoneQuests)
                info.Serialize(writer);

            writer.Write(ChainOffers.Count);

            foreach (MLQuest quest in ChainOffers)
                MLQuestSystem.WriteQuestRef(writer, quest);

            writer.WriteEncodedInt((int)m_Flags);
        }

        public MLQuestContext(GenericReader reader, int version)
        {
            Owner = reader.ReadMobile<PlayerMobile>();
            QuestInstances = new List<MLQuestInstance>();
            ChainOffers = new List<MLQuest>();
            m_DoneQuests = new List<MLDoneQuestInfo>();

            int instances = reader.ReadInt();

            for (int i = 0; i < instances; ++i)
            {
                MLQuestInstance instance = MLQuestInstance.Deserialize(reader, version, Owner);

                if (instance != null)
                    QuestInstances.Add(instance);
            }

            int doneQuests = reader.ReadInt();

            for (int i = 0; i < doneQuests; ++i)
            {
                MLDoneQuestInfo info = MLDoneQuestInfo.Deserialize(reader, version);

                if (info != null)
                    m_DoneQuests.Add(info);
            }

            int chainOffers = reader.ReadInt();

            for (int i = 0; i < chainOffers; ++i)
            {
                MLQuest quest = MLQuestSystem.ReadQuestRef(reader);

                if (quest != null && quest.IsChainTriggered)
                    ChainOffers.Add(quest);
            }

            m_Flags = (MLQuestFlag)reader.ReadEncodedInt();
        }

        public bool GetFlag(MLQuestFlag flag)
        {
            return ((m_Flags & flag) != 0);
        }

        public void SetFlag(MLQuestFlag flag, bool value)
        {
            if (value)
                m_Flags |= flag;
            else
                m_Flags &= ~flag;
        }
    }
}
