﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Items
{
    public class Artifact_JesterHatofChuckles : GiftJesterHat
    {
        [Constructable]
        public Artifact_JesterHatofChuckles()
        {
            Name = "Jester Hat of Chuckles";
            ItemID = 5916;
            Hue = Utility.RandomList(0x13e, 0x03, 0x172, 0x3f);
            Attributes.Luck = 300;
            Resistances.Physical = 12;
            Resistances.Cold = 12;
            Resistances.Energy = 12;
            Resistances.Fire = 12;
            Resistances.Poison = 12;
            ArtifactLevel = 2;
            Server.Misc.Arty.ArtySetup(this, 8, "");
        }

        public Artifact_JesterHatofChuckles(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            ArtifactLevel = 2;
            int version = reader.ReadInt();
        }
    }
}
