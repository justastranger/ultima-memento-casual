using System;
using Server.Items;

namespace Server.Items
{
    public class GiftPlateBattleKabuto : BaseGiftArmor
    {
        public override int BasePhysicalResistance { get { return 6; } }
        public override int BaseFireResistance { get { return 2; } }
        public override int BaseColdResistance { get { return 2; } }
        public override int BasePoisonResistance { get { return 2; } }
        public override int BaseEnergyResistance { get { return 3; } }

        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 65; } }

        public override int AosStrReq { get { return 70; } }
        public override int OldStrReq { get { return 70; } }

        public override int ArmorBase { get { return 3; } }

        public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

        [Constructable]
        public GiftPlateBattleKabuto() : base(0x2785)
        {
            Weight = 6.0;
        }

        public GiftPlateBattleKabuto(Serial serial) : base(serial)
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