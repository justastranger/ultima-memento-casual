using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;

namespace Server.Mobiles
{
    [CorpseName("a horde minion corpse")]
    public class HordeMinionFamiliar : BaseFamiliar
    {
        public override bool DisplayWeight { get { return true; } }

        public HordeMinionFamiliar()
        {
            Name = "a horde minion";
            Body = 776;
            BaseSoundID = 0x39D;

            SetStr(100);
            SetDex(110);
            SetInt(100);

            SetHits(70);
            SetStam(110);
            SetMana(0);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 50, 55);
            SetResistance(ResistanceType.Poison, 25, 30);
            SetResistance(ResistanceType.Energy, 25, 30);

            SetSkill(SkillName.FistFighting, 70.1, 75.0);
            SetSkill(SkillName.Tactics, 50.0);

            ControlSlots = 1;

            Container pack = Backpack;

            if (pack != null)
                pack.Delete();

            pack = new Backpack();
            pack.Movable = false;
            pack.Weight = 13.0;

            AddItem(pack);
        }

        private DateTime m_NextPickup;

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now < m_NextPickup)
                return;

            m_NextPickup = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));

            Container pack = this.Backpack;

            if (pack == null)
                return;

            ArrayList list = new ArrayList();

            foreach (Item item in this.GetItemsInRange(2))
            {
                if (item.Movable && item.Stackable)
                    list.Add(item);
            }

            int pickedUp = 0;

            for (int i = 0; i < list.Count; ++i)
            {
                Item item = (Item)list[i];

                if (!pack.CheckHold(this, item, false, true))
                    return;

                bool rejected;
                LRReason reject;

                NextActionTime = DateTime.Now;

                Lift(item, item.Amount, out rejected, out reject);

                if (rejected)
                    continue;

                Drop(this, Point3D.Zero);

                if (++pickedUp == 3)
                    break;
            }
        }

        private void ConfirmRelease_Callback(Mobile from, bool okay, object state)
        {
            if (okay)
                EndRelease(from);
        }

        public override void BeginRelease(Mobile from)
        {
            Container pack = this.Backpack;

            if (pack != null && pack.Items.Count > 0)
                from.SendGump(new WarningGump(1060635, 30720, 1061672, 32512, 420, 280, new WarningGumpCallback(ConfirmRelease_Callback), null));
            else
                EndRelease(from);
        }

        #region Pack Animal Methods
        public override bool OnBeforeDeath()
        {
            if (!base.OnBeforeDeath())
                return false;

            PackAnimal.CombineBackpacks(this);

            return true;
        }

        public override DeathMoveResult GetInventoryMoveResultFor(Item item)
        {
            return DeathMoveResult.MoveToCorpse;
        }

        public override bool IsSnoop(Mobile from)
        {
            if (PackAnimal.CheckAccess(this, from))
                return false;

            return base.IsSnoop(from);
        }

        public override bool OnDragDrop(Mobile from, Item item)
        {
            if (CheckFeed(from, item))
                return true;

            if (PackAnimal.CheckAccess(this, from))
            {
                AddToBackpack(item);
                return true;
            }

            return base.OnDragDrop(from, item);
        }

        public override bool CheckNonlocalDrop(Mobile from, Item item, Item target)
        {
            return PackAnimal.CheckAccess(this, from);
        }

        public override bool CheckNonlocalLift(Mobile from, Item item)
        {
            return PackAnimal.CheckAccess(this, from);
        }

        public override void OnDoubleClick(Mobile from)
        {
            PackAnimal.TryPackOpen(this, from);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            PackAnimal.GetContextMenuEntries(this, from, list);
        }
        #endregion

        public HordeMinionFamiliar(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}