using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Engines.MLQuests.Gumps;
using Server.Engines.MLQuests.Objectives;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using System.Collections;
using Server.Commands.Generic;
using Server.Items;
using System.IO;
using System.Reflection;
using Server.Engines.MLQuests.Definitions;
using System.Linq;

namespace Server.Engines.MLQuests
{
    public static class MLQuestSystem
    {
        public const int MaxConcurrentQuests = 10;
        public const int SpeechColor = 0x3B2;

        public static readonly bool AutoGenerateNew = true;
        public static readonly bool Debug = false;
        public static readonly List<MLQuest> EmptyList = new List<MLQuest>();
        public static Dictionary<Type, MLQuest> Quests { get; private set; }
        public static Dictionary<Type, List<MLQuest>> QuestGivers { get; private set; }
        public static Dictionary<PlayerMobile, MLQuestContext> Contexts { get; private set; }

        static MLQuestSystem()
        {
            Quests = new Dictionary<Type, MLQuest>();
            QuestGivers = new Dictionary<Type, List<MLQuest>>();
            Contexts = new Dictionary<PlayerMobile, MLQuestContext>();

            string cfgPath = Path.Combine(Core.BaseDirectory, Path.Combine("Data", "MLQuests.cfg"));

            Type baseQuestType = typeof(MLQuest);
            Type baseQuesterType = typeof(IQuestGiver);

            if (File.Exists(cfgPath))
            {
                using (StreamReader sr = new StreamReader(cfgPath))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Length == 0 || line.StartsWith("#"))
                            continue;

                        string[] split = line.Split('\t');

                        Type type = ScriptCompiler.FindTypeByName(split[0]);

                        if (type == null || !baseQuestType.IsAssignableFrom(type))
                        {
                            if (Debug)
                                Console.WriteLine("Warning: {1} quest type '{0}'", split[0], (type == null) ? "Unknown" : "Invalid");

                            continue;
                        }

                        MLQuest quest = null;

                        try
                        {
                            quest = Activator.CreateInstance(type) as MLQuest;
                        }
                        catch { }

                        if (quest == null)
                            continue;

                        Register(type, quest);

                        for (int i = 1; i < split.Length; ++i)
                        {
                            Type questerType = ScriptCompiler.FindTypeByName(split[i]);

                            if (questerType == null || !baseQuesterType.IsAssignableFrom(questerType))
                            {
                                if (Debug)
                                    Console.WriteLine("Warning: {1} quester type '{0}'", split[i], (questerType == null) ? "Unknown" : "Invalid");

                                continue;
                            }

                            RegisterQuestGiver(quest, questerType);
                        }
                    }
                }
            }
            else
            {
                // Dynamically discover and register
                foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    if (!baseQuestType.IsAssignableFrom(type)) continue;

                    MLQuest quest = null;

                    try
                    {
                        quest = Activator.CreateInstance(type) as MLQuest;
                    }
                    catch { }

                    if (quest == null) continue;

                    Register(type, quest);

                    if (quest.Objectives != null)
                        ValidateDummyObjectiveText(quest.Objectives);

                    foreach (var questGiver in quest.GetQuestGivers())
                    {
                        RegisterQuestGiver(quest, questGiver);
                    }
                }
            }
        }

        private static void ValidateDummyObjectiveText(IEnumerable<BaseObjective> objectives)
        {
            foreach (var objective in objectives.Where(x => x is DummyObjective).Cast<DummyObjective>())
            {
                const int MAX_LENGTH = 60;
                var text = objective.Name.String;
                if (string.IsNullOrWhiteSpace(text) || text.Length <= MAX_LENGTH) continue;

                Console.WriteLine("Warning: Objective text is too long.");
                Console.WriteLine(text.Substring(0, MAX_LENGTH));
                Console.WriteLine(text.Substring(MAX_LENGTH));
                Console.WriteLine();
            }
        }

        private static void Register(Type type, MLQuest quest)
        {
            Quests[type] = quest;
        }

        private static void RegisterQuestGiver(MLQuest quest, Type questerType)
        {
            List<MLQuest> questList;

            if (!QuestGivers.TryGetValue(questerType, out questList))
                QuestGivers[questerType] = questList = new List<MLQuest>();

            questList.Add(quest);
        }

        public static void Register(MLQuest quest, params Type[] questerTypes)
        {
            Register(quest.GetType(), quest);

            foreach (Type questerType in questerTypes)
                RegisterQuestGiver(quest, questerType);
        }

        public static void Initialize()
        {
            if (AutoGenerateNew)
            {
                foreach (MLQuest quest in Quests.Values)
                {
                    if (quest != null && !quest.Deserialized)
                        quest.Generate();
                }
            }

            MLQuestPersistence.EnsureExistence();

            CommandSystem.Register("MLQuestsInfo", AccessLevel.Administrator, new CommandEventHandler(MLQuestsInfo_OnCommand));
            CommandSystem.Register("SaveQuest", AccessLevel.Administrator, new CommandEventHandler(SaveQuest_OnCommand));
            CommandSystem.Register("SaveAllQuests", AccessLevel.Administrator, new CommandEventHandler(SaveAllQuests_OnCommand));
            CommandSystem.Register("InvalidQuestItems", AccessLevel.Administrator, new CommandEventHandler(InvalidQuestItems_OnCommand));
            CommandSystem.Register("MLQuestsGenerate", AccessLevel.Administrator, new CommandEventHandler(MLQuestsGenerate_OnCommand));
            CommandSystem.Register("MLQuestsClearSpawners", AccessLevel.Administrator, new CommandEventHandler(MLQuestsClearSpawners_OnCommand));

            TargetCommands.Register(new ViewQuestsCommand());
            TargetCommands.Register(new ViewContextCommand());
            TargetCommands.Register(new ResetQuestTimersCommand());
        }

        [Usage("MLQuestsInfo")]
        [Description("Displays general information about the ML quest system, or a quest by type name.")]
        public static void MLQuestsInfo_OnCommand(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            if (e.Length == 0)
            {
                m.SendMessage("Quest table length: {0}", Quests.Count);
                return;
            }

            Type index = ScriptCompiler.FindTypeByName(e.GetString(0));
            MLQuest quest;

            if (index == null || !Quests.TryGetValue(index, out quest))
            {
                m.SendMessage("Invalid quest type name.");
                return;
            }

            m.SendMessage("Activated: {0}", quest.Activated);
            m.SendMessage("Number of objectives: {0}", quest.Objectives.Count);
            m.SendMessage("Objective type: {0}", quest.ObjectiveType);
            m.SendMessage("Number of active instances: {0}", quest.Instances.Count);
        }

        public class ViewQuestsCommand : BaseCommand
        {
            public ViewQuestsCommand()
            {
                AccessLevel = AccessLevel.GameMaster;
                Supports = CommandSupport.Simple;
                Commands = new string[] { "ViewQuests" };
                ObjectTypes = ObjectTypes.Mobiles;
                Usage = "ViewQuests";
                Description = "Displays a targeted mobile's quest overview.";
            }

            public override void Execute(CommandEventArgs e, object obj)
            {
                Mobile from = e.Mobile;
                PlayerMobile pm = obj as PlayerMobile;

                if (pm == null)
                {
                    LogFailure("That is not a player.");
                    return;
                }

                CommandLogging.WriteLine(from, "{0} {1} viewing quest overview of {2}", from.AccessLevel, CommandLogging.Format(from), CommandLogging.Format(pm));
                from.SendGump(new QuestLogGump(pm, false));
            }
        }

        private class ViewContextCommand : BaseCommand
        {
            public ViewContextCommand()
            {
                AccessLevel = AccessLevel.GameMaster;
                Supports = CommandSupport.Simple;
                Commands = new string[] { "ViewMLContext" };
                ObjectTypes = ObjectTypes.Mobiles;
                Usage = "ViewMLContext";
                Description = "Opens the ML quest context for a targeted mobile.";
            }

            public override void Execute(CommandEventArgs e, object obj)
            {
                PlayerMobile pm = obj as PlayerMobile;

                if (pm == null)
                    LogFailure("They have no ML quest context.");
                else
                    e.Mobile.SendGump(new PropertiesGump(e.Mobile, GetOrCreateContext(pm)));
            }
        }

        private class ResetQuestTimersCommand : BaseCommand
        {
            public ResetQuestTimersCommand()
            {
                AccessLevel = AccessLevel.GameMaster;
                Supports = CommandSupport.Simple;
                Commands = new string[] { "ResetQuestTimers" };
                ObjectTypes = ObjectTypes.Mobiles;
                Usage = "ResetQuestTimers";
                Description = "Resets quest cooldowns for a targeted mobile.";
            }

            public override void Execute(CommandEventArgs e, object obj)
            {
                PlayerMobile pm = obj as PlayerMobile;
                MLQuestContext context;
                if (pm == null || !Contexts.TryGetValue(pm, out context))
                {
                    LogFailure("They have no ML quest context.");
                    return;
                }

                foreach (var quest in Quests.Values)
                {
                    if (!quest.HasRestartDelay) continue;

                    context.RemoveDoneQuest(quest);
                }
            }
        }

        [Usage("SaveQuest <type> [saveEnabled=true]")]
        [Description("Allows serialization for a specific quest to be turned on or off.")]
        public static void SaveQuest_OnCommand(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            if (e.Length == 0 || e.Length > 2)
            {
                m.SendMessage("Syntax: SaveQuest <id> [saveEnabled=true]");
                return;
            }

            Type index = ScriptCompiler.FindTypeByName(e.GetString(0));
            MLQuest quest;

            if (index == null || !Quests.TryGetValue(index, out quest))
            {
                m.SendMessage("Invalid quest type name.");
                return;
            }

            bool enable = (e.Length == 2) ? e.GetBoolean(1) : true;

            quest.SaveEnabled = enable;
            m.SendMessage("Serialization for quest {0} is now {1}.", quest.GetType().Name, enable ? "enabled" : "disabled");

            if (AutoGenerateNew && !enable)
                m.SendMessage("Please note that automatic generation of new quests is ON. This quest will be regenerated on the next server start.");
        }

        [Usage("SaveAllQuests [saveEnabled=true]")]
        [Description("Allows serialization for all quests to be turned on or off.")]
        public static void SaveAllQuests_OnCommand(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            if (e.Length > 1)
            {
                m.SendMessage("Syntax: SaveAllQuests [saveEnabled=true]");
                return;
            }

            bool enable = (e.Length == 1) ? e.GetBoolean(0) : true;

            foreach (MLQuest quest in Quests.Values)
                quest.SaveEnabled = enable;

            m.SendMessage("Serialization for all quests is now {0}.", enable ? "enabled" : "disabled");

            if (AutoGenerateNew && !enable)
                m.SendMessage("Please note that automatic generation of new quests is ON. All quests will be regenerated on the next server start.");
        }

        [Usage("InvalidQuestItems")]
        [Description("Provides an overview of all quest items not located in the top-level of a player's backpack.")]
        public static void InvalidQuestItems_OnCommand(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            ArrayList found = new ArrayList();

            foreach (Item item in World.Items.Values)
            {
                if (item.QuestItem)
                {
                    Backpack pack = item.Parent as Backpack;

                    if (pack != null)
                    {
                        PlayerMobile player = pack.Parent as PlayerMobile;

                        if (player != null && player.Backpack == pack)
                            continue;
                    }

                    found.Add(item);
                }
            }

            if (found.Count == 0)
                m.SendMessage("No matching objects found.");
            else
                m.SendGump(new InterfaceGump(m, new string[] { "Object" }, found, 0, null));
        }

        [Usage("MLQuestsGenerate")]
        [Description("Forces all MLQuests to execute the Generate logic.")]
        public static void MLQuestsGenerate_OnCommand(CommandEventArgs e)
        {
            foreach (MLQuest quest in Quests.Values)
            {
                if (quest == null) continue;

                quest.Generate();
            }

            e.Mobile.SendMessage("Executed Generate for '{0}' quests.", Quests.Count);
        }

        [Usage("MLQuestsClearSpawners")]
        [Description("Delete all MLQS spawners.")]
        public static void MLQuestsClearSpawners_OnCommand(CommandEventArgs e)
        {
            var spawners = World.Items.Values
                .Where(x => x is Spawner)
                .Cast<Spawner>()
                .Where(x => x.Name != null && x.Name.StartsWith("MLQS-"))
                .ToList();
            spawners.ForEach(x => x.Delete());
            e.Mobile.SendMessage("Deleted '{0}' quest spawners.", spawners.Count);
        }

        private static bool FindQuest(IQuestGiver quester, PlayerMobile pm, MLQuestContext context, out MLQuest quest, out MLQuestInstance entry)
        {
            quest = null;
            entry = null;

            List<MLQuest> quests = quester.MLQuests;
            Type questerType = quester.GetType();

            // 1. Check quests in progress with this NPC (overriding deliveries is intended)
            if (context != null)
            {
                foreach (MLQuest questEntry in quests)
                {
                    MLQuestInstance instance = context.FindInstance(questEntry);

                    if (instance != null && (instance.Quester == quester || instance.QuesterType == questerType))
                    {
                        entry = instance;
                        quest = questEntry;
                        return true;
                    }
                }
            }

            // 2. Check deliveries (overriding chain offers is intended)
            if ((entry = HandleDelivery(pm, quester, questerType)) != null)
            {
                quest = entry.Quest;
                return true;
            }

            // 3. Check chain quest offers
            if (context != null)
            {
                foreach (MLQuest questEntry in quests)
                {
                    if (questEntry.IsChainTriggered && context.ChainOffers.Contains(questEntry))
                    {
                        quest = questEntry;
                        return true;
                    }
                }
            }

            // 4. Random quest
            quest = RandomStarterQuest(quester, pm, context);

            return (quest != null);
        }

        /// <summary>
        /// Returns True if there was an interaction with the ML Quest System.
        /// </summary>
        public static bool OnDoubleClick(IQuestGiver quester, PlayerMobile pm, bool replyFailure)
        {
            if (quester.Deleted || !pm.Alive)
                return false;

            MLQuestContext context = GetContext(pm);

            MLQuest quest;
            MLQuestInstance entry;

            if (!FindQuest(quester, pm, context, out quest, out entry))
            {
                if (replyFailure) Tell(quester, pm, 1080107); // I'm sorry, I have nothing for you at this time.
                return false;
            }

            if (entry != null)
            {
                // Check if Quester was Deleted (ex: [buildworld + restart)
                // This seems like the safest point to set this
                // If we got here, there's a really high chance that it was because the Quester could give the quest.
                if (entry.Quester == null)
                    entry.Quester = quester;

                TurnToFace(quester, pm);
                if (entry.Failed) return true; // Note: OSI sends no gump at all for failed quests, they have to be cancelled in the quest overview

                if (entry.ClaimReward)
                    entry.SendRewardOffer();
                else if (entry.IsCompleted())
                    entry.SendReportBackGump();
                else
                    entry.SendProgressGump();
            }
            else if (quest.CanOffer(quester, pm, context, true))
            {
                TurnToFace(quester, pm);

                quest.SendOffer(quester, pm);
            }

            return true;
        }

        public static bool CanMarkQuestItem(PlayerMobile pm, Item item, Type type)
        {
            MLQuestContext context = GetContext(pm);

            if (context != null)
            {
                foreach (MLQuestInstance quest in context.QuestInstances)
                {
                    if (!quest.ClaimReward && quest.AllowsQuestItem(item, type))
                        return true;
                }
            }

            return false;
        }

        private static void OnMarkQuestItem(PlayerMobile pm, Item item, Type type)
        {
            MLQuestContext context = GetContext(pm);

            if (context == null)
                return;

            List<MLQuestInstance> instances = context.QuestInstances;

            // We don't foreach because CheckComplete() can potentially modify the MLQuests list
            for (int i = instances.Count - 1; i >= 0; --i)
            {
                MLQuestInstance instance = instances[i];

                if (instance.ClaimReward)
                    continue;

                foreach (BaseObjectiveInstance objective in instance.Objectives)
                {
                    if (!objective.Expired && objective.AllowsQuestItem(item, type))
                    {
                        objective.CheckComplete(); // yes, this can happen multiple times (for multiple quests)
                        break;
                    }
                }
            }
        }

        public static bool MarkQuestItem(PlayerMobile pm, Item item)
        {
            Type type = item.GetType();

            if (CanMarkQuestItem(pm, item, type))
            {
                item.QuestItem = true;
                OnMarkQuestItem(pm, item, type);

                return true;
            }

            return false;
        }

        public static void HandleSkillGain(PlayerMobile pm, SkillName skill)
        {
            MLQuestContext context = GetContext(pm);

            if (context == null)
                return;

            List<MLQuestInstance> instances = context.QuestInstances;

            for (int i = instances.Count - 1; i >= 0; --i)
            {
                MLQuestInstance instance = instances[i];

                if (instance.ClaimReward)
                    continue;

                foreach (BaseObjectiveInstance objective in instance.Objectives)
                {
                    if (!objective.Expired && objective is GainSkillObjectiveInstance && ((GainSkillObjectiveInstance)objective).Handles(skill))
                    {
                        objective.CheckComplete();
                        break;
                    }
                }
            }
        }

        public static void HandleKill(PlayerMobile pm, Mobile mob)
        {
            MLQuestContext context = GetContext(pm);

            if (context == null)
                return;

            List<MLQuestInstance> instances = context.QuestInstances;

            Type type = null;

            for (int i = instances.Count - 1; i >= 0; --i)
            {
                MLQuestInstance instance = instances[i];

                if (instance.ClaimReward)
                    continue;

                /* A kill only counts for a single objective within a quest,
				 * but it can count for multiple quests. This is something not
				 * currently observable on OSI, so it is assumed behavior.
				 */
                foreach (BaseObjectiveInstance objective in instance.Objectives)
                {
                    if (!objective.Expired && objective is KillObjectiveInstance)
                    {
                        KillObjectiveInstance kill = (KillObjectiveInstance)objective;

                        if (type == null)
                            type = mob.GetType();

                        if (kill.AddKill(mob, type))
                        {
                            kill.CheckComplete();
                            break;
                        }
                    }
                }
            }
        }

        public static MLQuestInstance HandleDelivery(PlayerMobile pm, IQuestGiver quester, Type questerType)
        {
            MLQuestContext context = GetContext(pm);

            if (context == null)
                return null;

            List<MLQuestInstance> instances = context.QuestInstances;
            MLQuestInstance deliverInstance = null;

            for (int i = instances.Count - 1; i >= 0; --i)
            {
                MLQuestInstance instance = instances[i];

                // Do NOT skip quests on ClaimReward, because the quester still needs the quest ref!
                //if ( instance.ClaimReward )
                //	continue;

                foreach (BaseObjectiveInstance objective in instance.Objectives)
                {
                    // Note: On OSI, expired deliveries can still be completed. Bug?
                    if (!objective.Expired && objective is DeliverObjectiveInstance)
                    {
                        DeliverObjectiveInstance deliver = (DeliverObjectiveInstance)objective;

                        if (deliver.IsDestination(quester, questerType))
                        {
                            if (!deliver.HasCompleted) // objective completes only once
                            {
                                deliver.HasCompleted = true;
                                deliver.CheckComplete();

                                // The quest is continued with this NPC (important for chains)
                                instance.Quester = quester;
                            }

                            if (deliverInstance == null)
                                deliverInstance = instance;

                            break; // don't return, we may have to complete more deliveries
                        }
                    }
                    else if (objective is GetArtifactRumorObjectiveInstance)
                    {
                        var citizenObjective = objective as GetArtifactRumorObjectiveInstance;
                        citizenObjective.TryGetRumor(quester);

                        if (citizenObjective.IsCompleted())
                        {
                            citizenObjective.CheckComplete();
                        }
                    }
                }
            }

            return deliverInstance;
        }

        public static MLQuestContext GetContext(PlayerMobile pm)
        {
            MLQuestContext context;
            Contexts.TryGetValue(pm, out context);

            return context;
        }

        public static MLQuestContext GetOrCreateContext(PlayerMobile pm)
        {
            MLQuestContext context;

            if (!Contexts.TryGetValue(pm, out context))
                Contexts[pm] = context = new MLQuestContext(pm);

            return context;
        }

        public static void HandleDeath(PlayerMobile pm)
        {
            MLQuestContext context = GetContext(pm);

            if (context != null)
                context.HandleDeath();
        }

        public static void HandleDeletion(PlayerMobile pm)
        {
            MLQuestContext context = GetContext(pm);

            if (context != null)
            {
                context.HandleDeletion();
                Contexts.Remove(pm);
            }
        }

        public static void HandleDeletion(IQuestGiver quester)
        {
            foreach (MLQuest quest in quester.MLQuests)
            {
                List<MLQuestInstance> instances = quest.Instances;

                for (int i = instances.Count - 1; i >= 0; --i)
                {
                    MLQuestInstance instance = instances[i];

                    if (instance.Quester == quester)
                        instance.OnQuesterDeleted();
                }
            }
        }

        private static List<MLQuest> m_EligiblePool = new List<MLQuest>();

        public static MLQuest RandomStarterQuest(IQuestGiver quester, PlayerMobile pm, MLQuestContext context)
        {
            List<MLQuest> quests = quester.MLQuests;

            if (quests.Count == 0)
                return null;

            m_EligiblePool.Clear();
            MLQuest fallback = null;

            foreach (MLQuest quest in quests)
            {
                if (quest.IsChainTriggered || (context != null && context.IsDoingQuest(quest)))
                    continue;

                /*
				 * Save first quest that reaches the CanOffer call.
				 * If no quests are valid at all, return this quest for displaying the CanOffer error message.
				 */
                if (fallback == null)
                    fallback = quest;

                if (quest.CanOffer(quester, pm, context, false))
                    m_EligiblePool.Add(quest);
            }

            if (m_EligiblePool.Count == 0)
                return fallback;

            return m_EligiblePool[Utility.Random(m_EligiblePool.Count)];
        }

        public static void TurnToFace(IQuestGiver quester, Mobile mob)
        {
            if (quester is Mobile)
            {
                Mobile m = (Mobile)quester;
                m.Direction = m.GetDirectionTo(mob);
            }
        }

        public static void Tell(IQuestGiver quester, PlayerMobile pm, int cliloc)
        {
            TurnToFace(quester, pm);

            if (quester is Mobile)
                ((Mobile)quester).PrivateOverheadMessage(MessageType.Regular, SpeechColor, cliloc, pm.NetState);
            else if (quester is Item)
                MessageHelper.SendLocalizedMessageTo((Item)quester, pm, cliloc, SpeechColor);
            else
                pm.SendLocalizedMessage(cliloc, "", SpeechColor);
        }

        public static void Tell(IQuestGiver quester, PlayerMobile pm, int cliloc, string args)
        {
            TurnToFace(quester, pm);

            if (quester is Mobile)
                ((Mobile)quester).PrivateOverheadMessage(MessageType.Regular, SpeechColor, cliloc, args, pm.NetState);
            else if (quester is Item)
                MessageHelper.SendLocalizedMessageTo((Item)quester, pm, cliloc, args, SpeechColor);
            else
                pm.SendLocalizedMessage(cliloc, args, SpeechColor);
        }

        public static void Tell(IQuestGiver quester, PlayerMobile pm, string message)
        {
            TurnToFace(quester, pm);

            if (quester is Mobile)
                ((Mobile)quester).PrivateOverheadMessage(MessageType.Regular, SpeechColor, false, message, pm.NetState);
            else if (quester is Item)
                MessageHelper.SendMessageTo((Item)quester, pm, message, SpeechColor);
            else
                pm.SendMessage(SpeechColor, message);
        }

        public static void TellDef(IQuestGiver quester, PlayerMobile pm, TextDefinition def)
        {
            if (def == null)
                return;

            if (def.Number > 0)
                Tell(quester, pm, def.Number);
            else if (def.String != null)
                Tell(quester, pm, def.String);
        }

        public static void WriteQuestRef(GenericWriter writer, MLQuest quest)
        {
            writer.Write((quest != null && quest.SaveEnabled) ? quest.GetType().FullName : null);
        }

        public static MLQuest ReadQuestRef(GenericReader reader)
        {
            string typeName = reader.ReadString();

            if (typeName == null)
                return null; // not serialized

            Type questType = ScriptCompiler.FindTypeByFullName(typeName);

            if (questType == null)
                return null; // no longer a type

            return FindQuest(questType);
        }

        public static MLQuest FindQuest(Type questType)
        {
            MLQuest result;
            Quests.TryGetValue(questType, out result);

            return result;
        }

        public static List<MLQuest> FindQuestList(Type questerType)
        {
            List<MLQuest> result;

            if (QuestGivers.TryGetValue(questerType, out result))
                return result;

            return EmptyList;
        }
    }
}
