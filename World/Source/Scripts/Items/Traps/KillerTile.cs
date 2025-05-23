using System;
using Server;
using Server.Mobiles;
using Server.Misc;
using Server.Network;

namespace Server.Items
{
    public class KillerTile : Item
    {
        [Constructable]
        public KillerTile() : base(0x4228)
        {
            Movable = false;
            Visible = false;
            Name = "killer";
        }

        public KillerTile(Serial serial) : base(serial)
        {
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m is PlayerMobile && Spells.Research.ResearchAirWalk.UnderEffect(m))
            {
                Point3D air = new Point3D((m.X + 1), (m.Y + 1), (m.Z + 5));
                Effects.SendLocationParticles(EffectItem.Create(air, m.Map, EffectItem.DefaultDuration), 0x2007, 9, 32, Server.Misc.PlayerSettings.GetMySpellHue(true, m, 0), 0, 5022, 0);
                m.PlaySound(0x014);
            }
            else if (m is PlayerMobile && m.Blessed == false && m.Alive && m.AccessLevel <= AccessLevel.Counselor && Server.Misc.SeeIfGemInBag.GemInPocket(m) == false && Server.Misc.SeeIfJewelInBag.JewelInPocket(m) == false)
            {
                double chance = 90 <= m.Skills[SkillName.RemoveTrap].Value
                    ? 0.25 + ((m.Skills[SkillName.RemoveTrap].Value - 90) / 100) // Flat 25% + 1% per skill pint
                    : 0;
                if (0 < chance && m.CheckSkill(SkillName.RemoveTrap, chance))
                {
                    m.PlaySound(m.Female ? 778 : 1049); m.Say("*ah!*");
                    m.LocalOverheadMessage(MessageType.Emote, 0x916, true, "Watch out!");
                    m.Hits = 1;
                }
                else
                {
                    m.LocalOverheadMessage(MessageType.Emote, 0x916, true, "You made a fatal mistake!");
                    m.Damage(10000, m);
                    LoggingFunctions.LogKillTile(m, this.Name);
                }
            }
            return true;
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