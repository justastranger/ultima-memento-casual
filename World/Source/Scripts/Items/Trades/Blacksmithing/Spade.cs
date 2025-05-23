using System;
using Server.Network;
using Server.Items;
using Server.Engines.Harvest;

namespace Server.Items
{
    public class Spade : BaseAxe, IUsesRemaining
    {
        public override string DefaultDescription { get { return "These shovels are used by miners, to dig up ore in caves and on mountain stone. There are also times when you may learn of buried treasure out in the land, that you wish to dig up. These shovels are needed for that as well."; } }

        public override HarvestSystem HarvestSystem { get { return Mining.System; } }

        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DoubleStrike; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ConcussionBlow; } }
        public override WeaponAbility ThirdAbility { get { return WeaponAbility.MeleeProtection; } }
        public override WeaponAbility FourthAbility { get { return WeaponAbility.MagicProtection; } }
        public override WeaponAbility FifthAbility { get { return WeaponAbility.DeathBlow; } }

        public override int AosStrengthReq { get { return 10; } }
        public override int AosMinDamage { get { return 9; } }
        public override int AosMaxDamage { get { return 11; } }
        public override int AosSpeed { get { return 48; } }
        public override float MlSpeed { get { return 2.25f; } }

        public override int OldStrengthReq { get { return 10; } }
        public override int OldMinDamage { get { return 8; } }
        public override int OldMaxDamage { get { return 28; } }
        public override int OldSpeed { get { return 48; } }

        public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Bash2H; } }

        public override int InitMinHits { get { return 31; } }
        public override int InitMaxHits { get { return 60; } }

        [Constructable]
        public Spade() : base(0xF39)
        {
            Name = "shovel";
            Weight = 10.0;
            ShowUsesRemaining = true;
            NeedsBothHands = true;
        }

        public Spade(Serial serial) : base(serial)
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
            int version = reader.ReadInt();
        }
    }
}