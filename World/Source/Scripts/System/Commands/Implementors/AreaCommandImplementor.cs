using System;
using System.Collections;
using Server;
using Server.Targeting;

namespace Server.Commands.Generic
{
    public class AreaCommandImplementor : BaseCommandImplementor
    {
        public AreaCommandImplementor()
        {
            Accessors = new string[] { "Area", "Group" };
            SupportRequirement = CommandSupport.Area;
            SupportsConditionals = true;
            AccessLevel = AccessLevel.GameMaster;
            Usage = "Area <command> [condition]";
            Description = "Invokes the command on all appropriate objects in a targeted area. Optional condition arguments can further restrict the set of objects.";
        }

        public override void Process(Mobile from, BaseCommand command, string[] args)
        {
            BoundingBoxPicker.Begin(from, new BoundingBoxCallback(OnTarget), new object[] { command, args });
        }

        public void OnTarget(Mobile from, Map map, Point3D start, Point3D end, object state)
        {
            try
            {
                object[] states = (object[])state;
                BaseCommand command = (BaseCommand)states[0];
                string[] args = (string[])states[1];

                Rectangle2D rect = new Rectangle2D(start.X, start.Y, end.X - start.X + 1, end.Y - start.Y + 1);

                Extensions ext = Extensions.Parse(from, ref args);

                bool items, mobiles;

                if (!CheckObjectTypes(command, ext, out items, out mobiles))
                    return;

                IPooledEnumerable eable;

                if (items && mobiles)
                    eable = map.GetObjectsInBounds(rect);
                else if (items)
                    eable = map.GetItemsInBounds(rect);
                else if (mobiles)
                    eable = map.GetMobilesInBounds(rect);
                else
                    return;

                ArrayList objs = new ArrayList();

                foreach (object obj in eable)
                {
                    if (mobiles && obj is Mobile && !BaseCommand.IsAccessible(from, obj))
                        continue;

                    if (ext.IsValid(obj))
                        objs.Add(obj);
                }

                eable.Free();

                ext.Filter(objs);

                RunCommand(from, objs, command, args);
            }
            catch (Exception ex)
            {
                from.SendMessage(ex.Message);
            }
        }

        public void OnTarget(Mobile from, object targeted, object state)
        {
            try
            {
                object[] states = (object[])state;
                BaseCommand command = (BaseCommand)states[0];
                string[] args = (string[])states[1];

                switch (command.ObjectTypes)
                {
                    case ObjectTypes.Both:
                        {
                            if (!(targeted is Item) && !(targeted is Mobile))
                            {
                                from.SendMessage("This command does not work on that.");
                                return;
                            }

                            break;
                        }
                    case ObjectTypes.Items:
                        {
                            if (!(targeted is Item))
                            {
                                from.SendMessage("This command only works on items.");
                                return;
                            }

                            break;
                        }
                    case ObjectTypes.Mobiles:
                        {
                            if (!(targeted is Mobile))
                            {
                                from.SendMessage("This command only works on mobiles.");
                                return;
                            }

                            break;
                        }
                }

                RunCommand(from, targeted, command, args);

                from.BeginTarget(-1, command.ObjectTypes == ObjectTypes.All, TargetFlags.None, new TargetStateCallback(OnTarget), new object[] { command, args });
            }
            catch (Exception ex)
            {
                from.SendMessage(ex.Message);
            }
        }
    }
}