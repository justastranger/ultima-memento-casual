using System;
using Server;
using Server.Spells;

namespace Server.Items
{
    public class Artifact_FesteringWound : GiftKryss
    {
        public override int InitMinHits { get { return 80; } }
        public override int InitMaxHits { get { return 160; } }

        [Constructable]
        public Artifact_FesteringWound()
        {
            Hue = 1272;
            Name = "Festering Wound";
            Attributes.AttackChance = 30;
            Attributes.SpellChanneling = 1;
            Attributes.WeaponSpeed = 30;
            Attributes.WeaponDamage = 25;
            WeaponAttributes.UseBestSkill = 1;
            WeaponAttributes.HitMagicArrow = 20;
            ArtifactLevel = 2;
            Server.Misc.Arty.ArtySetup(this, 8, "");
        }

        public override void GetDamageTypes(Mobile weilder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            phys = 20;
            nrgy = 10;
            cold = 10;
            pois = 50;
            fire = 10;
            chaos = 0;
            direct = 0;
        }

        public Artifact_FesteringWound(Serial serial)
            : base(serial)
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
            ArtifactLevel = 2;

            int version = reader.ReadInt();
        }
    }
}