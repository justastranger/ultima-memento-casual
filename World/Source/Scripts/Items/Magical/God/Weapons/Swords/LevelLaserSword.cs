using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
    public class LevelLaserSword : BaseLevelSword
    {
        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ArmorIgnore; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.DoubleStrike; } }
        public override WeaponAbility ThirdAbility { get { return WeaponAbility.DefenseMastery; } }
        public override WeaponAbility FourthAbility { get { return WeaponAbility.ElementalStrike; } }
        public override WeaponAbility FifthAbility { get { return WeaponAbility.DeathBlow; } }

        public override int AosStrengthReq { get { return 35; } }
        public override int AosMinDamage { get { return 20; } }
        public override int AosMaxDamage { get { return 24; } }
        public override int AosSpeed { get { return 30; } }
        public override float MlSpeed { get { return 3.00f; } }

        public override int OldStrengthReq { get { return 40; } }
        public override int OldMinDamage { get { return 6; } }
        public override int OldMaxDamage { get { return 34; } }
        public override int OldSpeed { get { return 30; } }

        public override int DefHitSound { get { return 0x53D; } }
        public override int DefMissSound { get { return 0x53E; } }

        public override int InitMinHits { get { return 50; } }
        public override int InitMaxHits { get { return 110; } }

        public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Slash1H; } }

        [Constructable]
        public LevelLaserSword() : base(0x3FBA)
        {
            Weight = 5.0;
            AosElementDamages.Physical = 50;
            AosElementDamages.Energy = 50;
            Name = "Laser Sword";
            Layer = Layer.OneHanded;
            Attributes.NightSight = 1;
        }

        public override bool OnEquip(Mobile from)
        {
            from.PlaySound(0x53F);

            base.OnEquip(from);

            return true;
        }

        public LevelLaserSword(Serial serial) : base(serial)
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