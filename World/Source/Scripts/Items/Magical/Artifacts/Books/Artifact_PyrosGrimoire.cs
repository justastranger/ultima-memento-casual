﻿using System;

namespace Server.Items
{
    public class Artifact_PyrosGrimoire : ElementalSpellbook
    {
        [Constructable]
        public Artifact_PyrosGrimoire() : base()
        {
            ItemID = 0x6719;
            Name = "Grimoire of the Daemon King";
            Attributes.RegenMana = Utility.RandomMinMax(1, 5);
            Attributes.CastSpeed = Utility.RandomMinMax(1, 5);
            Attributes.SpellDamage = 10;
            Slayer = SlayerName.SummerWind;
            ArtifactLevel = 1;

            switch (Utility.RandomMinMax(0, 6))
            {
                case 0: this.Content = 0xFFFFFFFF; break;
                case 1: this.Content = 0xFFFFFFF; break;
                case 2: this.Content = 0xFFFFFF; break;
                case 3: this.Content = 0xFFFFFF; break;
                case 4: this.Content = 0xFFFF; break;
                case 5: this.Content = 0xFFFF; break;
                case 6: this.Content = 0xFFFF; break;
            }

            int attributeCount = Utility.RandomMinMax(8, 15);
            int min = Utility.RandomMinMax(15, 25);
            int max = min + 40;
            BaseRunicTool.ApplyAttributesTo((Spellbook)this, attributeCount, min, max);

            SkillBonuses.SetValues(0, SkillName.Elementalism, (10.0 + (Utility.RandomMinMax(0, 2) * 5)));
            SkillBonuses.SetValues(1, SkillName.MagicResist, (10.0 + (Utility.RandomMinMax(0, 2) * 5)));
            SkillBonuses.SetValues(2, SkillName.Focus, (10.0 + (Utility.RandomMinMax(0, 2) * 5)));
            SkillBonuses.SetValues(3, SkillName.Meditation, (10.0 + (Utility.RandomMinMax(0, 2) * 5)));
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add(1049644, "Pyros' Book of Spells");
        }

        public Artifact_PyrosGrimoire(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); //version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            ArtifactLevel = 1;
        }
    }
}
