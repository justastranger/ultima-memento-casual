using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.ContextMenus;
using Server.Network;
using Server.Regions;
using Server.Spells;
using Server.Spells.Ninjitsu;

namespace Server.Items
{
    public class BallOfSummoning : Item, ITranslocationItem
    {
        private int m_Charges;
        private int m_Recharges;
        private BaseCreature m_Pet;
        private string m_PetName;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Charges
        {
            get { return m_Charges; }
            set
            {
                if (value > this.MaxCharges)
                    m_Charges = this.MaxCharges;
                else if (value < 0)
                    m_Charges = 0;
                else
                    m_Charges = value;

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Recharges
        {
            get { return m_Recharges; }
            set
            {
                if (value > this.MaxRecharges)
                    m_Recharges = this.MaxRecharges;
                else if (value < 0)
                    m_Recharges = 0;
                else
                    m_Recharges = value;

                InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxCharges { get { return 20; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxRecharges { get { return 255; } }

        public string TranslocationItemName { get { return "crystal ball of pet summoning"; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseCreature Pet
        {
            get
            {
                if (m_Pet != null && m_Pet.Deleted)
                {
                    m_Pet = null;
                    InternalUpdatePetName();
                }

                return m_Pet;
            }
            set
            {
                m_Pet = value;
                InternalUpdatePetName();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string PetName { get { return m_PetName; } }

        [Constructable]
        public BallOfSummoning() : base(0xE2E)
        {
            Weight = 10.0;
            Light = LightType.Circle150;

            m_Charges = Utility.RandomMinMax(3, 9);
            m_PetName = "";
            Name = "Orb of Pet Summoning";
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1070722, "Charges: " + m_Charges.ToString());
            if (m_PetName != "" && m_PetName != null) { list.Add(1049644, "Pet: " + m_PetName); }
        }

        private delegate void BallCallback(Mobile from);

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.Alive && this.RootParent == from)
            {
                if (Pet == null)
                {
                    list.Add(new BallEntry(new BallCallback(LinkPet), 6180));
                }
                else
                {
                    list.Add(new BallEntry(new BallCallback(CastSummonPet), 6181));
                    list.Add(new BallEntry(new BallCallback(UpdatePetName), 6183));
                    list.Add(new BallEntry(new BallCallback(UnlinkPet), 6182));
                }
            }
        }

        private class BallEntry : ContextMenuEntry
        {
            private BallCallback m_Callback;

            public BallEntry(BallCallback callback, int number) : base(number, 2)
            {
                m_Callback = callback;
            }

            public override void OnClick()
            {
                Mobile from = Owner.From;

                if (from.CheckAlive())
                    m_Callback(from);
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (this.RootParent != from) // TODO: Previous implementation allowed use on ground, without house protection checks. What is the correct behavior?
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1042001); // That must be in your pack for you to use it.
                return;
            }

            AnimalFormContext animalContext = AnimalForm.GetContext(from);

            if (Core.ML && animalContext != null)
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1080073); // You cannot use a Crystal Ball of Pet Summoning while in animal form.
                return;
            }

            if (Pet == null)
            {
                LinkPet(from);
            }
            else
            {
                CastSummonPet(from);
            }
        }

        public void LinkPet(Mobile from)
        {
            BaseCreature pet = this.Pet;

            if (Deleted || pet != null || this.RootParent != from)
                return;

            from.SendLocalizedMessage(1054114); // Target your pet that you wish to link to this Crystal Ball of Pet Summoning.
            from.Target = new PetLinkTarget(this);
        }

        private class PetLinkTarget : Target
        {
            private BallOfSummoning m_Ball;

            public PetLinkTarget(BallOfSummoning ball) : base(-1, false, TargetFlags.None)
            {
                m_Ball = ball;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Ball.Deleted || m_Ball.Pet != null)
                    return;

                if (m_Ball.RootParent != from)
                {
                    from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1042001); // That must be in your pack for you to use it.
                }
                else if (targeted is BaseCreature)
                {
                    BaseCreature creature = (BaseCreature)targeted;

                    if (!creature.Controlled || creature.ControlMaster != from)
                    {
                        MessageHelper.SendLocalizedMessageTo(m_Ball, from, 1054117, 0x59); // You may only link your own pets to a Crystal Ball of Pet Summoning.
                    }
                    else if (!creature.IsBonded)
                    {
                        MessageHelper.SendLocalizedMessageTo(m_Ball, from, 1054118, 0x59); // You must bond with your pet before it can be linked to a Crystal Ball of Pet Summoning.
                    }
                    else
                    {
                        MessageHelper.SendLocalizedMessageTo(m_Ball, from, 1054119, 0x59); // Your pet is now linked to this Crystal Ball of Pet Summoning.

                        m_Ball.Pet = creature;
                    }
                }
                else if (targeted == m_Ball)
                {
                    MessageHelper.SendLocalizedMessageTo(m_Ball, from, 1054115, 0x59); // The Crystal Ball of Pet Summoning cannot summon itself.
                }
                else
                {
                    MessageHelper.SendLocalizedMessageTo(m_Ball, from, 1054116, 0x59); // Only pets can be linked to this Crystal Ball of Pet Summoning.
                }
            }
        }

        public void CastSummonPet(Mobile from)
        {
            BaseCreature pet = this.Pet;

            if (Deleted || pet == null || this.RootParent != from)
                return;

            if (Charges == 0)
            {
                SendLocalizedMessageTo(from, 1054122); // The Crystal Ball darkens. It must be charged before it can be used again.
            }
            else if (pet is BaseMount && ((BaseMount)pet).Rider == from)
            {
                MessageHelper.SendLocalizedMessageTo(this, from, 1054124, 0x36); // The Crystal Ball fills with a yellow mist. Why would you summon your pet while riding it?
            }
            else if (pet.Map == Map.Internal && (!pet.IsStabled || (from.Followers + pet.ControlSlots) > from.FollowersMax))
            {
                MessageHelper.SendLocalizedMessageTo(this, from, 1054125, 0x5); // The Crystal Ball fills with a blue mist. Your pet is not responding to the summons.
            }
            else if ((!pet.Controlled || pet.ControlMaster != from) && !from.Stabled.Contains(pet))
            {
                MessageHelper.SendLocalizedMessageTo(this, from, 1054126, 0x8FD); // The Crystal Ball fills with a grey mist. You are not the owner of the pet you are attempting to summon.
            }
            else if (!pet.IsBonded)
            {
                MessageHelper.SendLocalizedMessageTo(this, from, 1054127, 0x22); // The Crystal Ball fills with a red mist. You appear to have let your bond to your pet deteriorate.
            }
            else if (from.Map == Map.Underworld || from.Region.IsPartOf(typeof(DungeonRegion)) || from.Region.IsPartOf(typeof(Jail)))
            {
                from.Send(new AsciiMessage(this.Serial, this.ItemID, MessageType.Regular, 0x22, 3, "", "You cannot summon your pet to this location."));
            }
            else if (Core.ML && from is PlayerMobile && DateTime.Now < ((PlayerMobile)from).LastPetBallTime.AddSeconds(15.0))
            {
                MessageHelper.SendLocalizedMessageTo(this, from, 1080072, 0x22); // You must wait a few seconds before you can summon your pet.
            }
            else
            {
                if (Core.ML)
                    new PetSummoningSpell(this, from).Cast();
                else
                    SummonPet(from);
            }

        }


        public void SummonPet(Mobile from)
        {
            BaseCreature pet = this.Pet;

            Charges--;

            if (pet.IsStabled)
            {
                pet.SetControlMaster(from);

                if (pet.Summoned)
                    pet.SummonMaster = from;

                pet.ControlTarget = from;
                pet.ControlOrder = OrderType.Follow;

                pet.IsStabled = false;
                from.Stabled.Remove(pet);
            }

            pet.MoveToWorld(from.Location, from.Map);

            MessageHelper.SendLocalizedMessageTo(this, from, 1054128, 0x43); // The Crystal Ball fills with a green mist. Your pet has been summoned.

            if (from is PlayerMobile)
            {
                ((PlayerMobile)from).LastPetBallTime = DateTime.Now;
            }
        }

        public void UnlinkPet(Mobile from)
        {
            if (!Deleted && Pet != null && this.RootParent == from)
            {
                Pet = null;

                SendLocalizedMessageTo(from, 1054120); // This crystal ball is no longer linked to a pet.
            }
        }

        public void UpdatePetName(Mobile from)
        {
            InternalUpdatePetName();
        }

        private void InternalUpdatePetName()
        {
            BaseCreature pet = this.Pet;

            if (pet == null)
                m_PetName = "";
            else
                m_PetName = pet.Name;

            InvalidateProperties();
        }

        public BallOfSummoning(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt((int)1); // version

            writer.WriteEncodedInt((int)m_Recharges);

            writer.WriteEncodedInt((int)m_Charges);
            writer.Write((Mobile)this.Pet);
            writer.Write((string)m_PetName);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();

            switch (version)
            {
                case 1:
                    {
                        m_Recharges = reader.ReadEncodedInt();
                        goto case 0;
                    }
                case 0:
                    {
                        m_Charges = Math.Min(reader.ReadEncodedInt(), MaxCharges);
                        this.Pet = (BaseCreature)reader.ReadMobile();
                        m_PetName = reader.ReadString();
                        break;
                    }
            }
        }

        private class PetSummoningSpell : Spell
        {
            private static SpellInfo m_Info = new SpellInfo("Ball Of Summoning", "", 230);

            private BallOfSummoning m_Ball;
            private Mobile m_Caster;

            public PetSummoningSpell(BallOfSummoning ball, Mobile caster)
                : base(caster, null, m_Info)
            {
                m_Caster = caster;
                m_Ball = ball;
            }

            public override bool ClearHandsOnCast { get { return false; } }
            public override bool RevealOnCast { get { return true; } }

            public override TimeSpan GetCastRecovery()
            {
                return TimeSpan.Zero;
            }

            public override double CastDelayFastScalar { get { return 0; } }

            public override TimeSpan CastDelayBase
            {
                get
                {
                    return TimeSpan.FromSeconds(2.0);
                }
            }

            public override int GetMana()
            {
                return 0;
            }

            public override bool ConsumeReagents()
            {
                return true;
            }

            public override bool CheckFizzle()
            {
                return true;
            }

            private bool m_Stop;

            public void Stop()
            {
                m_Stop = true;
                Disturb(DisturbType.Hurt, false, false);
            }

            public override bool CheckDisturb(DisturbType type, bool checkFirst, bool resistable)
            {
                if (type == DisturbType.EquipRequest || type == DisturbType.UseRequest/* || type == DisturbType.Hurt*/ )
                    return false;

                return true;
            }

            public override void DoHurtFizzle()
            {
                if (!m_Stop)
                    base.DoHurtFizzle();
            }

            public override void DoFizzle()
            {
                if (!m_Stop)
                    base.DoFizzle();
            }

            public override void OnDisturb(DisturbType type, bool message)
            {
                if (message && !m_Stop)
                    Caster.SendLocalizedMessage(1080074); // You have been disrupted while attempting to summon your pet!
            }

            public override void OnCast()
            {
                m_Ball.SummonPet(m_Caster);

                FinishSequence();
            }
        }
    }
}
