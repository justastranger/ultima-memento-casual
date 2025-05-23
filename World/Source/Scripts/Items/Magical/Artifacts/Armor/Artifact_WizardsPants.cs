using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class Artifact_WizardsPants : GiftLeatherLegs
    {
        public override int InitMinHits { get { return 80; } }
        public override int InitMaxHits { get { return 160; } }

        public override int BaseColdResistance { get { return 14; } }
        public override int BaseEnergyResistance { get { return 8; } }
        public override int BasePhysicalResistance { get { return 6; } }
        public override int BasePoisonResistance { get { return 6; } }
        public override int BaseFireResistance { get { return 7; } }

        [Constructable]
        public Artifact_WizardsPants()
        {
            Name = "Wizard's Pants";
            Hue = 1265;
            ItemID = 0x13cb;
            ArmorAttributes.MageArmor = 1;
            Attributes.LowerManaCost = 5;
            Attributes.LowerRegCost = 15;
            Attributes.Luck = 150;
            Attributes.RegenMana = 5;
            Attributes.SpellDamage = 10;
            Attributes.BonusMana = 15;
            ArtifactLevel = 2;
            Server.Misc.Arty.ArtySetup(this, 12, "");
        }

        public Artifact_WizardsPants(Serial serial) : base(serial)
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
