/***************************************************************************
 *                                Effects.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id$
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using System.Collections;
using Server.Network;

namespace Server
{
    public enum EffectLayer
    {
        Head = 0,
        RightHand = 1,
        LeftHand = 2,
        Waist = 3,
        LeftFoot = 4,
        RightFoot = 5,
        CenterFeet = 7
    }

    public enum ParticleSupportType
    {
        Full,
        Detect,
        None
    }

    public static class Effects
    {
        private static ParticleSupportType m_ParticleSupportType = ParticleSupportType.Detect;

        public static ParticleSupportType ParticleSupportType
        {
            get { return m_ParticleSupportType; }
            set { m_ParticleSupportType = value; }
        }

        public static bool SendParticlesTo(NetState state)
        {
            return (m_ParticleSupportType == ParticleSupportType.Full || (m_ParticleSupportType == ParticleSupportType.Detect && state.IsUOTDClient));
        }

        public static void PlaySound(IPoint3D p, Map map, int soundID)
        {
            if (soundID <= -1)
                return;

            if (map != null)
            {
                Packet playSound = null;

                IPooledEnumerable eable = map.GetClientsInRange(new Point3D(p));

                foreach (NetState state in eable)
                {
                    state.Mobile.ProcessDelta();

                    if (playSound == null)
                        playSound = Packet.Acquire(new PlaySound(soundID, p));

                    state.Send(playSound);
                }

                Packet.Release(playSound);

                eable.Free();
            }
        }

        public static void SendBoltEffect(IEntity e)
        {
            SendBoltEffect(e, true, 0);
        }

        public static void SendBoltEffect(IEntity e, bool sound)
        {
            SendBoltEffect(e, sound, 0);
        }

        public static void SendBoltEffect(IEntity e, bool sound, int hue)
        {
            Map map = e.Map;

            if (map == null)
                return;

            e.ProcessDelta();

            Packet preEffect = null, boltEffect = null, playSound = null;

            IPooledEnumerable eable = map.GetClientsInRange(e.Location);

            foreach (NetState state in eable)
            {
                if (state.Mobile.CanSee(e))
                {
                    if (SendParticlesTo(state))
                    {
                        if (preEffect == null)
                            preEffect = Packet.Acquire(new TargetParticleEffect(e, 0, 10, 5, 0, 0, 5031, 3, 0));

                        state.Send(preEffect);
                    }

                    if (boltEffect == null)
                        boltEffect = Packet.Acquire(new BoltEffect(e, hue));

                    state.Send(boltEffect);

                    if (sound)
                    {
                        if (playSound == null)
                            playSound = Packet.Acquire(new PlaySound(0x29, e));

                        state.Send(playSound);
                    }
                }
            }

            Packet.Release(preEffect);
            Packet.Release(boltEffect);
            Packet.Release(playSound);

            eable.Free();
        }

        public static void SendLocationEffect(IPoint3D p, Map map, int itemID, int duration)
        {
            SendLocationEffect(p, map, itemID, duration, 10, 0, 0);
        }

        public static void SendLocationEffect(IPoint3D p, Map map, int itemID, int duration, int speed)
        {
            SendLocationEffect(p, map, itemID, duration, speed, 0, 0);
        }

        public static void SendLocationEffect(IPoint3D p, Map map, int itemID, int duration, int hue, int renderMode)
        {
            SendLocationEffect(p, map, itemID, duration, 10, hue, renderMode);
        }

        public static void SendLocationEffect(IPoint3D p, Map map, int itemID, int duration, int speed, int hue, int renderMode)
        {
            SendPacket(p, map, new LocationEffect(p, itemID, speed, duration, hue, renderMode));
        }

        public static void SendLocationParticles(IEntity e, int itemID, int speed, int duration, int effect)
        {
            SendLocationParticles(e, itemID, speed, duration, 0, 0, effect, 0);
        }

        public static void SendLocationParticles(IEntity e, int itemID, int speed, int duration, int effect, int unknown)
        {
            SendLocationParticles(e, itemID, speed, duration, 0, 0, effect, unknown);
        }

        public static void SendLocationParticles(IEntity e, int itemID, int speed, int duration, int hue, int renderMode, int effect, int unknown)
        {
            Map map = e.Map;

            if (map != null)
            {
                Packet particles = null, regular = null;

                IPooledEnumerable eable = map.GetClientsInRange(e.Location);

                foreach (NetState state in eable)
                {
                    state.Mobile.ProcessDelta();

                    if (SendParticlesTo(state))
                    {
                        if (particles == null)
                            particles = Packet.Acquire(new LocationParticleEffect(e, itemID, speed, duration, hue, renderMode, effect, unknown));

                        state.Send(particles);
                    }
                    else if (itemID != 0)
                    {
                        if (regular == null)
                            regular = Packet.Acquire(new LocationEffect(e, itemID, speed, duration, hue, renderMode));

                        state.Send(regular);
                    }
                }

                Packet.Release(particles);
                Packet.Release(regular);

                eable.Free();
            }
            //SendPacket( e.Location, e.Map, new LocationParticleEffect( e, itemID, speed, duration, hue, renderMode, effect, unknown ) );
        }

        public static void SendTargetEffect(IEntity target, int itemID, int duration)
        {
            SendTargetEffect(target, itemID, duration, 0, 0);
        }

        public static void SendTargetEffect(IEntity target, int itemID, int speed, int duration)
        {
            SendTargetEffect(target, itemID, speed, duration, 0, 0);
        }

        public static void SendTargetEffect(IEntity target, int itemID, int duration, int hue, int renderMode)
        {
            SendTargetEffect(target, itemID, 10, duration, hue, renderMode);
        }

        public static void SendTargetEffect(IEntity target, int itemID, int speed, int duration, int hue, int renderMode)
        {
            if (target is Mobile)
                ((Mobile)target).ProcessDelta();

            SendPacket(target.Location, target.Map, new TargetEffect(target, itemID, speed, duration, hue, renderMode));
        }

        public static void SendTargetParticles(IEntity target, int itemID, int speed, int duration, int effect, EffectLayer layer)
        {
            SendTargetParticles(target, itemID, speed, duration, 0, 0, effect, layer, 0);
        }

        public static void SendTargetParticles(IEntity target, int itemID, int speed, int duration, int effect, EffectLayer layer, int unknown)
        {
            SendTargetParticles(target, itemID, speed, duration, 0, 0, effect, layer, unknown);
        }

        public static void SendTargetParticles(IEntity target, int itemID, int speed, int duration, int hue, int renderMode, int effect, EffectLayer layer, int unknown)
        {
            if (target is Mobile)
                ((Mobile)target).ProcessDelta();

            Map map = target.Map;

            if (map != null)
            {
                Packet particles = null, regular = null;

                IPooledEnumerable eable = map.GetClientsInRange(target.Location);

                foreach (NetState state in eable)
                {
                    state.Mobile.ProcessDelta();

                    if (SendParticlesTo(state))
                    {
                        if (particles == null)
                            particles = Packet.Acquire(new TargetParticleEffect(target, itemID, speed, duration, hue, renderMode, effect, (int)layer, unknown));

                        state.Send(particles);
                    }
                    else if (itemID != 0)
                    {
                        if (regular == null)
                            regular = Packet.Acquire(new TargetEffect(target, itemID, speed, duration, hue, renderMode));

                        state.Send(regular);
                    }
                }

                Packet.Release(particles);
                Packet.Release(regular);

                eable.Free();
            }

            //SendPacket( target.Location, target.Map, new TargetParticleEffect( target, itemID, speed, duration, hue, renderMode, effect, (int)layer, unknown ) );
        }

        public static void SendMovingEffect(IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes)
        {
            SendMovingEffect(from, to, itemID, speed, duration, fixedDirection, explodes, 0, 0);
        }

        public static void SendMovingEffect(IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode)
        {
            if (from is Mobile)
                ((Mobile)from).ProcessDelta();

            if (to is Mobile)
                ((Mobile)to).ProcessDelta();

            // ADDED TO GIVE MISSILE FIRE A 10 PIXEL ELEVATION

            int up = 0;
            if (from.X > to.X)
                up = 10;

            int dn = up;
            if (explodes)
                dn = 0;

            IEntity mfrom = new Entity(Serial.Zero, new Point3D(from.X, from.Y, from.Z + up), from.Map);
            IEntity mto = new Entity(Serial.Zero, new Point3D(to.X, to.Y, to.Z + dn), to.Map);
            Point3D loc = new Point3D(from.X, from.Y, from.Z + up);
            if (speed > 7) { speed = 7; duration = 0; }

            SendPacket(loc, from.Map, new MovingEffect(mfrom, mto, itemID, speed, duration, fixedDirection, explodes, hue, renderMode));
        }

        public static void SendMovingParticles(IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int effect, int explodeEffect, int explodeSound)
        {
            SendMovingParticles(from, to, itemID, speed, duration, fixedDirection, explodes, 0, 0, effect, explodeEffect, explodeSound, 0);
        }

        public static void SendMovingParticles(IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int effect, int explodeEffect, int explodeSound, int unknown)
        {
            SendMovingParticles(from, to, itemID, speed, duration, fixedDirection, explodes, 0, 0, effect, explodeEffect, explodeSound, unknown);
        }

        public static void SendMovingParticles(IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode, int effect, int explodeEffect, int explodeSound, int unknown)
        {
            SendMovingParticles(from, to, itemID, speed, duration, fixedDirection, explodes, hue, renderMode, effect, explodeEffect, explodeSound, (EffectLayer)255, unknown);
        }

        public static void SendMovingParticles(IEntity from, IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode, int effect, int explodeEffect, int explodeSound, EffectLayer layer, int unknown)
        {
            if (from is Mobile)
                ((Mobile)from).ProcessDelta();

            if (to is Mobile)
                ((Mobile)to).ProcessDelta();

            Map map = from.Map;

            // ADDED TO GIVE MISSILE FIRE A 10 PIXEL ELEVATION

            int up = 0;
            if (from.X > to.X)
                up = 10;

            int dn = up;
            if (explodes)
                dn = 0;

            IEntity mfrom = new Entity(Serial.Zero, new Point3D(from.X, from.Y, from.Z + up), from.Map);
            IEntity mto = new Entity(Serial.Zero, new Point3D(to.X, to.Y, to.Z + dn), to.Map);
            if (speed > 7) { speed = 7; duration = 0; }

            if (map != null)
            {
                Packet particles = null, regular = null;

                IPooledEnumerable eable = map.GetClientsInRange(from.Location);

                foreach (NetState state in eable)
                {
                    state.Mobile.ProcessDelta();

                    if (SendParticlesTo(state))
                    {
                        if (particles == null)
                            particles = Packet.Acquire(new MovingParticleEffect(mfrom, mto, itemID, speed, duration, fixedDirection, explodes, hue, renderMode, effect, explodeEffect, explodeSound, layer, unknown));

                        state.Send(particles);
                    }
                    else if (itemID > 1)
                    {
                        if (regular == null)
                            regular = Packet.Acquire(new MovingEffect(mfrom, mto, itemID, speed, duration, fixedDirection, explodes, hue, renderMode));

                        state.Send(regular);
                    }
                }

                Packet.Release(particles);
                Packet.Release(regular);

                eable.Free();
            }

            //SendPacket( from.Location, from.Map, new MovingParticleEffect( from, to, itemID, speed, duration, fixedDirection, explodes, hue, renderMode, effect, explodeEffect, explodeSound, unknown ) );
        }

        public static void SendPacket(Point3D origin, Map map, Packet p)
        {
            if (map != null)
            {
                IPooledEnumerable eable = map.GetClientsInRange(origin);

                p.Acquire();

                foreach (NetState state in eable)
                {
                    state.Mobile.ProcessDelta();

                    state.Send(p);
                }

                p.Release();

                eable.Free();
            }
        }

        public static void SendPacket(IPoint3D origin, Map map, Packet p)
        {
            if (map != null)
            {
                IPooledEnumerable eable = map.GetClientsInRange(new Point3D(origin));

                p.Acquire();

                foreach (NetState state in eable)
                {
                    state.Mobile.ProcessDelta();

                    state.Send(p);
                }

                p.Release();

                eable.Free();
            }
        }
    }
}