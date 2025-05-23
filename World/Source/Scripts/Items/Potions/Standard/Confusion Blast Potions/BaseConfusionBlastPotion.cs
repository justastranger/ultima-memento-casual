using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Mobiles;
using Server.Misc;

namespace Server.Items
{
    public abstract class BaseConfusionBlastPotion : BasePotion
    {
        public override string DefaultDescription { get { return "These potions, when thrown, will cause those nearby to be confused for about 5 seconds."; } }

        public abstract int Radius { get; }

        public override bool RequireFreeHand { get { return false; } }

        public BaseConfusionBlastPotion(PotionEffect effect) : base(0x180F, effect)
        {
        }

        public BaseConfusionBlastPotion(Serial serial) : base(serial)
        {
        }

        public override void Drink(Mobile from)
        {
            if (Core.AOS && (from.Paralyzed || from.Blessed || from.Frozen || (from.Spell != null && from.Spell.IsCasting)))
            {
                from.SendMessage("You cannot do that yet.");
                return;
            }
            else if (!from.Region.AllowHarmful(from, from))
            {
                from.SendMessage("That doesn't feel like a good idea.");
                return;
            }

            int delay = GetDelay(from);

            if (delay > 0)
            {
                from.SendLocalizedMessage(1072529, String.Format("{0}\t{1}", delay, delay > 1 ? "seconds." : "second.")); // You cannot use that for another ~1_NUM~ ~2_TIMEUNITS~
                return;
            }

            ThrowTarget targ = from.Target as ThrowTarget;

            if (targ != null && targ.Potion == this)
                return;

            from.RevealingAction();

            if (!m_Users.Contains(from))
                m_Users.Add(from);

            from.Target = new ThrowTarget(this);
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

        private List<Mobile> m_Users = new List<Mobile>();

        public void Explode_Callback(object state)
        {
            object[] states = (object[])state;

            Explode((Mobile)states[0], (Point3D)states[1], (Map)states[2]);
        }

        public virtual void Explode(Mobile from, Point3D loc, Map map)
        {
            if (Deleted || map == null)
                return;

            Consume();

            // Check if any other players are using this potion
            for (int i = 0; i < m_Users.Count; i++)
            {
                ThrowTarget targ = m_Users[i].Target as ThrowTarget;

                if (targ != null && targ.Potion == this)
                    Target.Cancel(from);
            }

            // Effects
            Effects.PlaySound(loc, map, 0x207);

            Geometry.Circle2D(loc, map, Radius, new DoEffect_Callback(BlastEffect), 270, 90);

            Timer.DelayCall(TimeSpan.FromSeconds(0.3), new TimerStateCallback(CircleEffect2), new object[] { loc, map });

            foreach (Mobile mobile in map.GetMobilesInRange(loc, Radius))
            {
                if (mobile is BaseCreature && !(mobile is RuneGuardian))
                {
                    BaseCreature mon = (BaseCreature)mobile;

                    mon.Pacify(from, DateTime.Now + TimeSpan.FromSeconds(5.0)); // TODO check
                }
                else if (mobile.Alive && from != mobile && mobile.Blessed == false && from.CanBeHarmful(mobile, true))
                {
                    mobile.Paralyze(TimeSpan.FromSeconds(5.0));
                }
            }
        }

        #region Effects
        public virtual void BlastEffect(Point3D p, Map map)
        {
            if (map.CanFit(p, 12, true, false))
                Effects.SendLocationEffect(p, map, 0x376A, 4, 9);
        }

        public void CircleEffect2(object state)
        {
            object[] states = (object[])state;

            Geometry.Circle2D((Point3D)states[0], (Map)states[1], Radius, new DoEffect_Callback(BlastEffect), 90, 270);
        }
        #endregion

        #region Delay
        private static Hashtable m_Delay = new Hashtable();

        public static void AddDelay(Mobile m)
        {
            Timer timer = m_Delay[m] as Timer;

            if (timer != null)
                timer.Stop();

            m_Delay[m] = Timer.DelayCall(TimeSpan.FromSeconds(20), new TimerStateCallback(EndDelay_Callback), m);
        }

        public static int GetDelay(Mobile m)
        {
            Timer timer = m_Delay[m] as Timer;

            if (timer != null && timer.Next > DateTime.Now)
                return (int)(timer.Next - DateTime.Now).TotalSeconds;

            return 0;
        }

        private static void EndDelay_Callback(object obj)
        {
            if (obj is Mobile)
                EndDelay((Mobile)obj);
        }

        public static void EndDelay(Mobile m)
        {
            Timer timer = m_Delay[m] as Timer;

            if (timer != null)
            {
                timer.Stop();
                m_Delay.Remove(m);
            }
        }
        #endregion

        private class ThrowTarget : Target
        {
            private BaseConfusionBlastPotion m_Potion;

            public BaseConfusionBlastPotion Potion
            {
                get { return m_Potion; }
            }

            public ThrowTarget(BaseConfusionBlastPotion potion) : base(12, true, TargetFlags.None)
            {
                m_Potion = potion;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Potion.Deleted || m_Potion.Map == Map.Internal)
                    return;

                IPoint3D p = targeted as IPoint3D;

                if (p == null || from.Map == null)
                    return;

                // Add delay
                BaseConfusionBlastPotion.AddDelay(from);

                SpellHelper.GetSurfaceTop(ref p);

                from.RevealingAction();

                IEntity to;

                if (p is Mobile)
                    to = (Mobile)p;
                else
                    to = new Entity(Serial.Zero, new Point3D(p), from.Map);

                Effects.SendMovingEffect(from, to, 0xF0D, 7, 0, false, false, m_Potion.Hue, 0);
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(m_Potion.Explode_Callback), new object[] { from, new Point3D(p), from.Map });
            }
        }
    }
}
