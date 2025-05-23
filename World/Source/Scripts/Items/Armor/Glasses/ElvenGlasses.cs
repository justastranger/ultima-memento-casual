using System;
using Server;

namespace Server.Items
{
    public class ElvenGlasses : BaseArmor
    {
        public override int LabelNumber { get { return 1032216; } } //elven glasses

        public override int BasePhysicalResistance { get { return 2; } }
        public override int BaseFireResistance { get { return 4; } }
        public override int BaseColdResistance { get { return 4; } }
        public override int BasePoisonResistance { get { return 3; } }
        public override int BaseEnergyResistance { get { return 2; } }

        public override int InitMinHits { get { return 36; } }
        public override int InitMaxHits { get { return 48; } }

        public override int AosStrReq { get { return 45; } }
        public override int OldStrReq { get { return 40; } }

        public override int ArmorBase { get { return 30; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Leather; } }
        public override CraftResource DefaultResource { get { return CraftResource.RegularLeather; } }
        public override ArmorMeditationAllowance DefMedAllowance { get { return ArmorMeditationAllowance.All; } }

        private AosWeaponAttributes m_AosWeaponAttributes;

        [CommandProperty(AccessLevel.GameMaster)]
        public AosWeaponAttributes WeaponAttributes
        {
            get { return m_AosWeaponAttributes; }
        }

        [Constructable]
        public ElvenGlasses() : base(0x2FB8)
        {
            Weight = 2;
            m_AosWeaponAttributes = new AosWeaponAttributes(this);
        }

        public ElvenGlasses(Serial serial) : base(serial)
        {
        }

        public override void AppendChildNameProperties(ObjectPropertyList list)
        {
            base.AppendChildNameProperties(list);

            int prop;

            if ((prop = m_AosWeaponAttributes.HitColdArea) != 0)
                list.Add(1060416, prop.ToString()); // hit cold area ~1_val~%

            if ((prop = m_AosWeaponAttributes.HitDispel) != 0)
                list.Add(1060417, prop.ToString()); // hit dispel ~1_val~%

            if ((prop = m_AosWeaponAttributes.HitEnergyArea) != 0)
                list.Add(1060418, prop.ToString()); // hit energy area ~1_val~%

            if ((prop = m_AosWeaponAttributes.HitFireArea) != 0)
                list.Add(1060419, prop.ToString()); // hit fire area ~1_val~%

            if ((prop = m_AosWeaponAttributes.HitFireball) != 0)
                list.Add(1060420, prop.ToString()); // hit fireball ~1_val~%

            if ((prop = m_AosWeaponAttributes.HitHarm) != 0)
                list.Add(1060421, prop.ToString()); // hit harm ~1_val~%

            if ((prop = m_AosWeaponAttributes.HitLeechHits) != 0)
                list.Add(1060422, prop.ToString()); // hit life leech ~1_val~%

            if ((prop = m_AosWeaponAttributes.HitLightning) != 0)
                list.Add(1060423, prop.ToString()); // hit lightning ~1_val~%

            if ((prop = m_AosWeaponAttributes.HitLowerAttack) != 0)
                list.Add(1060424, prop.ToString()); // hit lower attack ~1_val~%

            if ((prop = m_AosWeaponAttributes.HitLowerDefend) != 0)
                list.Add(1060425, prop.ToString()); // hit lower defense ~1_val~%

            if ((prop = m_AosWeaponAttributes.HitMagicArrow) != 0)
                list.Add(1060426, prop.ToString()); // hit magic arrow ~1_val~%

            if ((prop = m_AosWeaponAttributes.HitLeechMana) != 0)
                list.Add(1060427, prop.ToString()); // hit mana leech ~1_val~%

            if ((prop = m_AosWeaponAttributes.HitPhysicalArea) != 0)
                list.Add(1060428, prop.ToString()); // hit physical area ~1_val~%

            if ((prop = m_AosWeaponAttributes.HitPoisonArea) != 0)
                list.Add(1060429, prop.ToString()); // hit poison area ~1_val~%

            if ((prop = m_AosWeaponAttributes.HitLeechStam) != 0)
                list.Add(1060430, prop.ToString()); // hit stamina leech ~1_val~%
        }

        private static void SetSaveFlag(ref SaveFlag flags, SaveFlag toSet, bool setIf)
        {
            if (setIf)
                flags |= toSet;
        }

        private static bool GetSaveFlag(SaveFlag flags, SaveFlag toGet)
        {
            return ((flags & toGet) != 0);
        }

        private enum SaveFlag
        {
            None = 0x00000000,
            WeaponAttributes = 0x00000001,
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version			

            SaveFlag flags = SaveFlag.None;

            SetSaveFlag(ref flags, SaveFlag.WeaponAttributes, !m_AosWeaponAttributes.IsEmpty);

            writer.Write((int)flags);

            if (GetSaveFlag(flags, SaveFlag.WeaponAttributes))
                m_AosWeaponAttributes.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            SaveFlag flags = (SaveFlag)reader.ReadInt();

            if (GetSaveFlag(flags, SaveFlag.WeaponAttributes))
                m_AosWeaponAttributes = new AosWeaponAttributes(this, reader);
            else
                m_AosWeaponAttributes = new AosWeaponAttributes(this);
        }
    }
}
