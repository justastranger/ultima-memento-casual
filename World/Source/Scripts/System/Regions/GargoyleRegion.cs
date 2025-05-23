using System;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fourth;
using Server.Spells.Sixth;
using Server.Spells.Chivalry;
using Server.Spells.Undead;
using Server.Spells.Herbalist;
using System.Text;
using System.IO;
using Server.Misc;
using Server.Network;

namespace Server.Regions
{
    public class GargoyleRegion : BaseRegion
    {
        public GargoyleRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
        {
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            return false;
        }

        public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            if (this.Name == "the Burning Mines")
            {
                global = LightCycle.CaveLevel;
            }
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);
            if (m is PlayerMobile)
            {
                LoggingFunctions.LogRegions(m, this.Name, "enter");
            }

            Server.Misc.RegionMusic.MusicRegion(m, this);
        }

        public override void OnExit(Mobile m)
        {
            base.OnExit(m);
            if (m is PlayerMobile)
            {
                LoggingFunctions.LogRegions(m, this.Name, "exit");
            }
        }
    }
}