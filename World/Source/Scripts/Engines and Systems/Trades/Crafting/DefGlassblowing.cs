using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Craft
{
    public class DefGlassblowing : CraftSystem
    {
        public override SkillName MainSkill
        {
            get { return SkillName.Alchemy; }
        }

        public override int GumpImage
        {
            get { return 9603; }
        }

        public override int GumpTitleNumber
        {
            get { return 1044622; } // <CENTER>Glassblowing MENU</CENTER>
        }

        public override string CraftSystemTxt
        {
            get { return "Crafting: Glassblowing"; }
        }

        private static CraftSystem m_CraftSystem;

        public static CraftSystem CraftSystem
        {
            get
            {
                if (m_CraftSystem == null)
                    m_CraftSystem = new DefGlassblowing();

                return m_CraftSystem;
            }
        }

        public override double GetChanceAtMin(CraftItem item)
        {
            return 0.0; // 0%
        }

        private DefGlassblowing() : base(1, 1, 1.25)// base( 1, 2, 1.7 )
        {
        }

        public override int CanCraft(Mobile from, BaseTool tool, Type itemType)
        {
            if (tool == null || tool.Deleted || tool.UsesRemaining < 0)
                return 1044038; // You have worn out your tool!
            else if (!BaseTool.CheckTool(tool, from))
                return 1048146; // If you have a tool equipped, you must use that tool.
            else if (!(from is PlayerMobile && ((PlayerMobile)from).Glassblowing && from.Skills[SkillName.Alchemy].Base >= 100.0))
                return 1044634; // You havent learned glassblowing.
            else if (!BaseTool.CheckAccessible(tool, from))
                return 1044263; // The tool must be on your person to use.

            bool anvil, forge;

            DefBlacksmithy.CheckAnvilAndForge(from, 2, out anvil, out forge);

            if (forge)
                return 0;

            return 1044628; // You must be near a forge to blow glass.
        }

        public override void PlayCraftEffect(Mobile from)
        {
            CraftSystem.CraftSound(from, 0x2B, m_Tools); // bellows
        }

        public override int PlayEndingEffect(Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, CraftItem item)
        {
            if (toolBroken)
                from.SendLocalizedMessage(1044038); // You have worn out your tool

            if (failed)
            {
                if (lostMaterial)
                    return 1044043; // You failed to create the item, and some of your materials are lost.
                else
                    return 1044157; // You failed to create the item, but no materials were lost.
            }
            else
            {
                from.PlaySound(0x41); // glass breaking

                if (quality == 0)
                    return 502785; // You were barely able to make this item.  It's quality is below average.
                else if (quality == 2)
                    return 1044155; // You create an exceptional quality item.
                else
                    return 1044154; // You create the item.
            }
        }

        public override void InitCraftList()
        {
            int index;
            index = AddCraft(typeof(Bottle), 1044050, 1023854, 52.5, 102.5, typeof(Sand), 1044625, 1, 1044627);
            index = AddCraft(typeof(Bottle), 1044050, "a batch of bottles", 102.5, 102.5, typeof(Sand), 1044625, 1, 1044627);
            SetUseAllRes(index, true);

            index = AddCraft(typeof(Jar), 1044050, "jar", 52.5, 102.5, typeof(Sand), 1044625, 1, 1044627);
            index = AddCraft(typeof(Jar), 1044050, "a batch of jars", 102.5, 102.5, typeof(Sand), 1044625, 1, 1044627);
            SetUseAllRes(index, true);

            AddCraft(typeof(SmallFlask), 1044050, 1044610, 52.5, 102.5, typeof(Sand), 1044625, 2, 1044627);
            AddCraft(typeof(MediumFlask), 1044050, 1044611, 52.5, 102.5, typeof(Sand), 1044625, 3, 1044627);
            AddCraft(typeof(CurvedFlask), 1044050, 1044612, 55.0, 105.0, typeof(Sand), 1044625, 2, 1044627);
            AddCraft(typeof(LongFlask), 1044050, 1044613, 57.5, 107.5, typeof(Sand), 1044625, 4, 1044627);
            AddCraft(typeof(LargeFlask), 1044050, 1044623, 60.0, 110.0, typeof(Sand), 1044625, 5, 1044627);
            AddCraft(typeof(AniSmallBlueFlask), 1044050, 1044614, 60.0, 110.0, typeof(Sand), 1044625, 5, 1044627);
            AddCraft(typeof(AniLargeVioletFlask), 1044050, 1044615, 60.0, 110.0, typeof(Sand), 1044625, 5, 1044627);
            AddCraft(typeof(AniRedRibbedFlask), 1044050, 1044624, 60.0, 110.0, typeof(Sand), 1044625, 7, 1044627);
            AddCraft(typeof(EmptyVialsWRack), 1044050, 1044616, 65.0, 115.0, typeof(Sand), 1044625, 8, 1044627);
            AddCraft(typeof(FullVialsWRack), 1044050, 1044617, 65.0, 115.0, typeof(Sand), 1044625, 9, 1044627);
            AddCraft(typeof(SpinningHourglass), 1044050, 1044618, 75.0, 125.0, typeof(Sand), 1044625, 10, 1044627);
        }
    }
}