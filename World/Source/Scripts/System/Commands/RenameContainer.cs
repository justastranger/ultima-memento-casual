using Server.Gumps;
using Server.Items;
using Server.Targeting;
using System;

namespace Server.Commands
{
    public class RenameContainer
    {
        public static void Initialize()
        {
            CommandSystem.Register("Rename", AccessLevel.Player, new CommandEventHandler(RenameItem_OnCommand));
        }

        [Usage("Rename")]
        [Description("Lets you target a container and open a gump to rename it.")]
        private static void RenameItem_OnCommand(CommandEventArgs e)
        {
            if (e.Length == 1)
            {
                Item item = World.FindItem(e.GetInt32(0));

                if (item == null)
                    e.Mobile.SendMessage("No item with that serial was found.");
                else if (!e.Mobile.Backpack.Items.Contains(item) || !e.Mobile.BankBox.Items.Contains(item))
                    e.Mobile.SendMessage("That item is not in your backpack or bankbox.");
                else if (item is Container)
                    e.Mobile.SendGump(new InternalGump((Container)item));
                else
                    e.Mobile.SendMessage("Only containers can be renamed using this command");
            }
            else if (e.Length == 0)
            {
                e.Mobile.Target = new InternalTarget();
            }
        }

        private class InternalTarget : Target
        {
            public InternalTarget() : base(-1, true, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!(targeted is Container))
                {
                    from.SendMessage("The targeted item cannot be renamed using this command");
                    return;
                }

                Container cont = (Container)targeted;
                if (!from.Backpack.Items.Contains(cont) && !from.BankBox.Items.Contains(cont))
                {
                    from.SendMessage("The targeted container needs to be in either your backpack or bankbox.");
                    return;
                }
                if (targeted is LockableContainer)
                {
                    LockableContainer loco = (LockableContainer)cont;
                    if (loco.Locked)
                    {
                        from.SendMessage("A locked container can't be renamed.");
                        return;
                    }
                }

                from.CloseGump(typeof(InternalGump));
                from.SendGump(new InternalGump(cont));

            }
        }

        private class InternalGump : Gump
        {
            private Container m_Target;

            private enum Buttons
            {
                Cancel,
                Okay,
                Text
            }

            public InternalGump(Container target) : base(0, 0)
            {
                m_Target = target;

                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;

                AddBackground(50, 50, 400, 300, 0xA28);

                AddPage(0);

                AddHtmlLocalized(50, 70, 400, 20, 1072359, 0x0, false, false); // <CENTER>Renaming Tool</CENTER>
                AddHtml(75, 95, 350, 145, "Please enter the text to add to the selected object. Leave the text area blank to remove any existing text.", true, true); // Please enter the text to add to the selected object. Leave the text area blank to remove any existing text.  Removing text does not use a charge.
                AddButton(125, 300, 0x81A, 0x81B, (int)Buttons.Okay, GumpButtonType.Reply, 0);
                AddButton(320, 300, 0x819, 0x818, (int)Buttons.Cancel, GumpButtonType.Reply, 0);
                AddImageTiled(75, 245, 350, 40, 0xDB0);
                AddImageTiled(76, 245, 350, 2, 0x23C5);
                AddImageTiled(75, 245, 2, 40, 0x23C3);
                AddImageTiled(75, 285, 350, 2, 0x23C5);
                AddImageTiled(425, 245, 2, 42, 0x23C3);

                AddTextEntry(75, 245, 350, 40, 0x0, (int)Buttons.Text, "");
            }

            public override void OnResponse(Server.Network.NetState state, RelayInfo info)
            {
                if (info.ButtonID == (int)Buttons.Okay)
                {
                    TextRelay relay = info.GetTextEntry((int)Buttons.Text);

                    if (relay != null)
                    {
                        if (String.IsNullOrEmpty(relay.Text))
                        {
                            m_Target.Name = null;
                            state.Mobile.SendMessage("You clear your mind of this item's name."); // You remove the engraving from the object.
                        }
                        else
                        {
                            if (relay.Text.Length > 64)
                                m_Target.Name = relay.Text.Substring(0, 64);
                            else
                                m_Target.Name = relay.Text;

                            state.Mobile.SendMessage("You memorize a new name for this item."); // You engraved the object.
                        }
                    }
                }
                else
                    state.Mobile.SendMessage("You changed your mind."); // The object was not engraved.
            }
        }
    }
}
