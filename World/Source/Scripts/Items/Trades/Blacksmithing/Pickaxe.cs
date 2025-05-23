using System;
using Server.Items;
using Server.Network;
using Server.Engines.Harvest;

namespace Server.Items
{
    [FlipableAttribute(0xE86, 0xE85)]
    public class Pickaxe : BaseAxe, IUsesRemaining
    {
        public override string DefaultDescription { get { return "These picks are used by miners, to dig up ore in caves and on mountain stone."; } }

        public override HarvestSystem HarvestSystem { get { return Mining.System; } }

        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DoubleStrike; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }
        public override WeaponAbility ThirdAbility { get { return WeaponAbility.MagicProtection2; } }
        public override WeaponAbility FourthAbility { get { return WeaponAbility.ZapDexStrike; } }
        public override WeaponAbility FifthAbility { get { return WeaponAbility.ShadowStrike; } }

        public override int AosStrengthReq { get { return 10; } }
        public override int AosMinDamage { get { return 9; } }
        public override int AosMaxDamage { get { return 11; } }
        public override int AosSpeed { get { return 35; } }
        public override float MlSpeed { get { return 3.00f; } }

        public override int OldStrengthReq { get { return 10; } }
        public override int OldMinDamage { get { return 1; } }
        public override int OldMaxDamage { get { return 15; } }
        public override int OldSpeed { get { return 35; } }

        public override int InitMinHits { get { return 31; } }
        public override int InitMaxHits { get { return 60; } }

        public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Slash1H; } }

        [Constructable]
        public Pickaxe() : base(0xE86)
        {
            Weight = 11.0;
            UsesRemaining = 50;
            ShowUsesRemaining = true;
        }

        public Pickaxe(Serial serial) : base(serial)
        {
        }

        public static Pickaxe CreateGargoylePickaxe()
        {
            return new Pickaxe
            {
                Name = "gargoyle pickaxe",
                Resource = CraftResource.Dwarven,
            };
        }

        public static bool IsGargoylePickaxe(Item item)
        {
            return item.Name == "gargoyle pickaxe" && item.Resource == CraftResource.Dwarven;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            ShowUsesRemaining = true;
        }
    }
}