using Server.Engines.MLQuests.Gumps;
using Server.Gumps;

namespace Server.Engines.MLQuests.Objectives
{
    public class DummyObjective : BaseObjective
    {
        public static readonly DummyObjective AllOfTheFollowing = new DummyObjective("All of the following");
        public static readonly DummyObjective AnyOfTheFollowing = new DummyObjective("Only one of the following");
        public static readonly DummyObjective CraftAndMarkQuestItems = new DummyObjective("Craft and toggle the following as Quest Items:");

        public readonly TextDefinition Name;

        public DummyObjective(TextDefinition message)
        {
            Name = message;
        }

        public override void WriteToGump(Gump g, ref int y)
        {
            if (Name.Number > 0)
                g.AddHtmlLocalized(98, y, 312, 32, Name.Number, BaseQuestGump.COLOR_LOCALIZED, false, false);
            else if (Name.String != null)
                g.AddLabel(98, y, BaseQuestGump.COLOR_LABEL, Name.String);

            y += 16;
        }

        public override BaseObjectiveInstance CreateInstance(MLQuestInstance instance)
        {
            return new DummyObjectiveInstance(instance, this);
        }
    }

    public class DummyObjectiveInstance : BaseObjectiveInstance
    {
        public BaseObjective Objective { get; private set; }

        public DummyObjectiveInstance(MLQuestInstance instance, BaseObjective obj) : base(instance, obj)
        {
            Objective = obj;
        }

        public override void WriteToGump(Gump g, ref int y)
        {
            Objective.WriteToGump(g, ref y);
        }

        public override bool IsCompleted()
        {
            return true;
        }
    }
}
