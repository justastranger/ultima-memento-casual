using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Spells.Necromancy
{
    public class SummonFamiliarSpell : NecromancerSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Summon Familiar", "Kal Xen Bal",
                203,
                9031,
                Reagent.BatWing,
                Reagent.GraveDust,
                Reagent.DaemonBlood
            );

        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(2.0); } }

        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 17; } }

        public SummonFamiliarSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        private static Hashtable m_Table = new Hashtable();

        public static Hashtable Table { get { return m_Table; } }

        public override bool CheckCast()
        {
            BaseCreature check = (BaseCreature)m_Table[Caster];

            if (check != null && !check.Deleted)
            {
                Caster.SendLocalizedMessage(1061605); // You already have a familiar.
                return false;
            }

            return base.CheckCast();
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.CloseGump(typeof(SummonFamiliarGump));
                Caster.SendGump(new SummonFamiliarGump(Caster, m_Entries, this));
            }

            FinishSequence();
        }

        private static SummonFamiliarEntry[] m_Entries = new SummonFamiliarEntry[]
            {
                new SummonFamiliarEntry( typeof( HordeMinionFamiliar ), 1060146,  30.0,  30.0 ), // Horde Minion
				new SummonFamiliarEntry( typeof( ShadowWispFamiliar ), 1060142,  50.0,  50.0 ), // Shadow Wisp
				new SummonFamiliarEntry( typeof( DarkWolfFamiliar ), 1060143,  60.0,  60.0 ), // Dark Wolf
				new SummonFamiliarEntry( typeof( DeathAdder ), 1060145,  80.0,  80.0 ), // Death Adder
				new SummonFamiliarEntry( typeof( VampireBatFamiliar ), 1060144, 100.0, 100.0 )  // Vampire Bat
			};

        public static SummonFamiliarEntry[] Entries { get { return m_Entries; } }
    }

    public class SummonFamiliarEntry
    {
        private Type m_Type;
        private object m_Name;
        private double m_ReqNecromancy;
        private double m_ReqSpiritualism;

        public Type Type { get { return m_Type; } }
        public object Name { get { return m_Name; } }
        public double ReqNecromancy { get { return m_ReqNecromancy; } }
        public double ReqSpiritualism { get { return m_ReqSpiritualism; } }

        public SummonFamiliarEntry(Type type, object name, double reqNecromancy, double reqSpiritualism)
        {
            m_Type = type;
            m_Name = name;
            m_ReqNecromancy = reqNecromancy;
            m_ReqSpiritualism = reqSpiritualism;
        }
    }

    public class SummonFamiliarGump : Gump
    {
        private Mobile m_From;
        private SummonFamiliarEntry[] m_Entries;

        private SummonFamiliarSpell m_Spell;

        private const int EnabledColor16 = 0x0F20;
        private const int DisabledColor16 = 0x262A;

        private const int EnabledColor32 = 0x18CD00;
        private const int DisabledColor32 = 0x4A8B52;

        public SummonFamiliarGump(Mobile from, SummonFamiliarEntry[] entries, SummonFamiliarSpell spell) : base(200, 100)
        {
            m_From = from;
            m_Entries = entries;
            m_Spell = spell;

            AddPage(0);

            AddBackground(10, 10, 250, 178, 9270);
            AddAlphaRegion(20, 20, 230, 158);

            AddItem(217, 16, 6883);
            AddItem(228, 168, 6881);
            AddItem(8, 15, 6882);
            AddItem(0, 168, 6880);

            AddHtmlLocalized(30, 26, 200, 20, 1060147, EnabledColor16, false, false); // Chose thy familiar...

            double necro = Spell.ItemSkillValue(from, SkillName.Necromancy, false);
            double spirit = Spell.ItemSkillValue(from, SkillName.Spiritualism, false);

            for (int i = 0; i < entries.Length; ++i)
            {
                object name = entries[i].Name;

                bool enabled = (necro >= entries[i].ReqNecromancy && spirit >= entries[i].ReqSpiritualism);

                AddButton(27, 53 + (i * 21), 9702, 9703, i + 1, GumpButtonType.Reply, 0);

                if (name is int)
                    AddHtmlLocalized(50, 51 + (i * 21), 150, 20, (int)name, enabled ? EnabledColor16 : DisabledColor16, false, false);
                else if (name is string)
                    AddHtml(50, 51 + (i * 21), 150, 20, String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", enabled ? EnabledColor32 : DisabledColor32, name), false, false);
            }
        }

        private static Hashtable m_Table = new Hashtable();

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            int index = info.ButtonID - 1;

            if (index >= 0 && index < m_Entries.Length)
            {
                SummonFamiliarEntry entry = m_Entries[index];

                double necro = Spell.ItemSkillValue(m_From, SkillName.Necromancy, false);
                double spirit = Spell.ItemSkillValue(m_From, SkillName.Spiritualism, false);

                BaseCreature check = (BaseCreature)SummonFamiliarSpell.Table[m_From];

                if (check != null && !check.Deleted)
                {
                    m_From.SendLocalizedMessage(1061605); // You already have a familiar.
                }
                else if (necro < entry.ReqNecromancy || spirit < entry.ReqSpiritualism)
                {
                    // That familiar requires ~1_NECROMANCY~ Necromancy and ~2_SPIRIT~ Spiritualism.
                    m_From.SendLocalizedMessage(1061606, String.Format("{0:F1}\t{1:F1}", entry.ReqNecromancy, entry.ReqSpiritualism));

                    m_From.CloseGump(typeof(SummonFamiliarGump));
                    m_From.SendGump(new SummonFamiliarGump(m_From, SummonFamiliarSpell.Entries, m_Spell));
                }
                else if (entry.Type == null)
                {
                    m_From.SendMessage("That familiar has not yet been defined.");

                    m_From.CloseGump(typeof(SummonFamiliarGump));
                    m_From.SendGump(new SummonFamiliarGump(m_From, SummonFamiliarSpell.Entries, m_Spell));
                }
                else
                {
                    try
                    {
                        BaseCreature bc = (BaseCreature)Activator.CreateInstance(entry.Type);

                        bc.Skills.MagicResist = m_From.Skills.MagicResist;

                        if (BaseCreature.Summon(bc, m_From, m_From.Location, -1, TimeSpan.FromDays(1.0)))
                        {
                            m_From.FixedParticles(0x3728, 1, 10, 9910, EffectLayer.Head);
                            bc.PlaySound(bc.GetIdleSound());

                            bc.DamageMin = bc.DamageMin + (int)((necro + spirit) / 25);
                            bc.DamageMax = bc.DamageMax + (int)((necro + spirit) / 25);

                            int health = bc.HitsMax + (int)((necro + spirit) / 2);
                            bc.SetHits(health);

                            SummonFamiliarSpell.Table[m_From] = bc;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                m_From.SendLocalizedMessage(1061825); // You decide not to summon a familiar.
            }
        }
    }
}