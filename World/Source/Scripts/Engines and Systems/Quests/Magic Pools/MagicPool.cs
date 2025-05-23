using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Mobiles;
using System.Collections.Generic;
using Server.Misc;
using System.Collections;
using Server.ContextMenus;

namespace Server.Items
{
    public class MagicPool : BaseAddon
    {
        private int m_Pool;
        private int m_Uses;
        private int m_Bonus;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Pool
        {
            get { return m_Pool; }
            set { m_Pool = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Uses
        {
            get { return m_Uses; }
            set { m_Uses = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Bonus
        {
            get { return m_Bonus; }
            set { m_Bonus = value; InvalidateProperties(); }
        }

        private DateTime m_DecayTime;
        private Timer m_DecayTimer;

        public virtual TimeSpan DecayDelay { get { return TimeSpan.FromMinutes(30.0); } } // HOW LONG UNTIL THE POOL DECAYS IN MINUTES

        [Constructable]
        public MagicPool()
        {
            int pool_type = Utility.Random(8);

            if (pool_type == 1)
            {
                AddComplexComponent((BaseAddon)this, 767, -1, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 768, -1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 8669, 0, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 8669, 1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 8669, 0, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 767, -1, 0, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 767, 1, 0, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 765, 1, 1, 2, 0, -1, "magic pool", 1);
            }
            else if (pool_type == 2)
            {
                AddComplexComponent((BaseAddon)this, 273, -1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 271, 0, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 271, 0, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 272, -1, 0, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 272, -1, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 271, 1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 272, 1, 0, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 270, 1, 1, 2, 0, -1, "magic pool", 1);
            }
            else if (pool_type == 3)
            {
                AddComplexComponent((BaseAddon)this, 48, -1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 47, 0, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 47, 0, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 46, -1, 0, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 46, -1, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 47, 1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 46, 1, 0, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 45, 1, 1, 2, 0, -1, "magic pool", 1);
            }
            else if (pool_type == 4)
            {
                AddComplexComponent((BaseAddon)this, 700, -1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 699, -1, 0, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 699, -1, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 698, 0, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 698, 0, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 698, 1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 699, 1, 0, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 697, 1, 1, 2, 0, -1, "magic pool", 1);
            }
            else if (pool_type == 5)
            {
                AddComplexComponent((BaseAddon)this, 223, -1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 222, 0, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 222, 0, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 221, -1, 0, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 221, -1, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 222, 1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 221, 1, 0, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 220, 1, 1, 2, 0, -1, "magic pool", 1);
            }
            else if (pool_type == 6)
            {
                AddComplexComponent((BaseAddon)this, 108, -1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 105, 0, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 105, 0, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 106, -1, 0, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 106, -1, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 107, 1, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 105, 1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 106, 1, 0, 2, 0, -1, "magic pool", 1);
            }
            else if (pool_type == 7)
            {
                AddComplexComponent((BaseAddon)this, 490, -1, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 490, -1, 0, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 491, -1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 488, 1, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 489, 0, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 489, 0, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 489, 1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 490, 1, 0, 2, 0, -1, "magic pool", 1);
            }
            else
            {
                AddComplexComponent((BaseAddon)this, 10585, -1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 10579, 0, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 10579, 1, -1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 10576, -1, 0, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 10576, -1, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 10576, 1, 0, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 10579, 0, 1, 2, 0, -1, "magic pool", 1);
                AddComplexComponent((BaseAddon)this, 10582, 1, 1, 2, 0, -1, "magic pool", 1);
            }
            AddComplexComponent((BaseAddon)this, 14186, 1, 1, 7, 0, -1, "magic pool", 1);
            AddComplexComponent((BaseAddon)this, 6039, 0, 0, 2, 0, -1, "magic pool", 1);
            AddComplexComponent((BaseAddon)this, 6039, 1, 0, 2, 0, -1, "magic pool", 1);
            AddComplexComponent((BaseAddon)this, 6039, 0, 1, 2, 0, -1, "magic pool", 1);
            AddComplexComponent((BaseAddon)this, 6039, 1, 1, 2, 0, -1, "magic pool", 1);

            m_Pool = Utility.Random(10);
            if (Utility.Random(100) > 90) { m_Pool = 100; } // TREASURE CHEST
            m_Uses = Utility.RandomMinMax(1, 10);
            m_Bonus = Utility.RandomMinMax(3, 10);

            RefreshDecay(true);
            Timer.DelayCall(TimeSpan.Zero, new TimerCallback(CheckAddComponents));
        }

        public MagicPool(Serial serial) : base(serial)
        {
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource)
        {
            AddComplexComponent(addon, item, xoffset, yoffset, zoffset, hue, lightsource, null, 1);
        }

        private static void AddComplexComponent(BaseAddon addon, int item, int xoffset, int yoffset, int zoffset, int hue, int lightsource, string name, int amount)
        {
            AddonComponent ac;
            ac = new AddonComponent(item);
            if (name != null && name.Length > 0)
                ac.Name = name;
            if (hue != 0)
                ac.Hue = hue;
            if (amount > 1)
            {
                ac.Stackable = true;
                ac.Amount = amount;
            }
            if (lightsource != -1)
                ac.Light = (LightType)lightsource;
            addon.AddComponent(ac, xoffset, yoffset, zoffset);
        }

        public virtual bool Apply(Mobile from, StatType Type, int Bonus)
        {
            bool applied = Spells.SpellHelper.AddStatOffset(from, Type, Bonus, TimeSpan.FromMinutes(2.0));

            if (!applied)
                from.SendLocalizedMessage(502173); // You are already under a similar effect.

            return applied;
        }

        public static int AvailPoints(Mobile from, int val)
        {
            int points = from.StatCap - (from.RawStr + from.RawInt + from.RawDex);

            if (val > points) { val = points; }

            return val;
        }

        public override void OnComponentUsed(AddonComponent ac, Mobile from)
        {
            if (from.Blessed)
            {
                from.SendMessage("You cannot drink from the pool while in this state.");
            }
            else if (!from.InRange(GetWorldLocation(), 3))
            {
                from.SendMessage("You will have to get closer to drink from the magical pool!");
            }
            else if (m_Uses > 0)
            {
                if (m_Pool == 1) // GAIN STATS
                {
                    if (from.StatCap > (from.RawStatTotal))
                    {
                        from.PlaySound(Utility.RandomList(0x30, 0x2D6));
                        int water = Utility.RandomMinMax(1, 3);
                        int up = 1;

                        int chance = Utility.RandomMinMax(1, 100);

                        if (chance >= 98) { up = AvailPoints(from, 5); }
                        else if (chance >= 87) { up = AvailPoints(from, 4); }
                        else if (chance >= 75) { up = AvailPoints(from, 3); }
                        else if (chance >= 50) { up = AvailPoints(from, 2); }

                        if (water == 1) { from.RawInt = from.RawInt + up; from.SendMessage("You drink from the pool and you feel much smarter!"); }
                        else if (water == 2) { from.RawStr = from.RawStr + up; from.SendMessage("You drink from the pool and you feel much stronger!"); }
                        else { from.RawDex = from.RawDex + up; from.SendMessage("You drink from the pool and you feel much quicker!"); }

                        this.m_Uses = 0;
                    }
                    else
                    {
                        from.PlaySound(Utility.RandomList(0x30, 0x2D6));
                        from.SendMessage("You drink from the pool and nothing happens!");
                        this.m_Uses = this.m_Uses - 1;
                    }
                }
                else if (m_Pool == 2) // CURE
                {
                    from.PlaySound(Utility.RandomList(0x30, 0x2D6));
                    if (from.Poisoned)
                    {
                        from.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                        from.CurePoison(from);
                        from.SendMessage("You feel much better after drinking from the pool!");
                        this.m_Uses = this.m_Uses - 1;
                    }
                    else
                    {
                        from.PlaySound(Utility.RandomList(0x30, 0x2D6));
                        from.SendMessage("You drink from the pool and nothing happens!");
                        this.m_Uses = this.m_Uses - 1;
                    }
                }
                else if (m_Pool == 3) // HEAL
                {
                    from.PlaySound(Utility.RandomList(0x30, 0x2D6));
                    if (from.Hits < from.HitsMax)
                    {
                        if (from.Poisoned || MortalStrike.IsWounded(from))
                        {
                            from.SendMessage("You drink from the pool and nothing happens!");
                            this.m_Uses = this.m_Uses - 1;
                        }
                        else
                        {
                            from.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                            int min = 50;
                            int max = 75;
                            if (m_Bonus > 8) { min = 125; max = 175; }
                            else if (m_Bonus > 5) { min = 75; max = 125; }
                            from.Heal(Utility.RandomMinMax(min, max));
                            from.SendMessage("You drink from the pool and your wounds magically heal!");
                            this.m_Uses = this.m_Uses - 1;
                        }
                    }
                    else
                    {
                        from.PlaySound(Utility.RandomList(0x30, 0x2D6));
                        from.SendMessage("You drink from the pool and nothing happens!");
                        this.m_Uses = this.m_Uses - 1;
                    }
                }
                else if (m_Pool == 4) // WATER ELEMENTAL
                {
                    from.PlaySound(Utility.RandomList(0x30, 0x2D6));
                    try
                    {
                        Map map = this.Map;
                        BaseCreature bc = (BaseCreature)Activator.CreateInstance(typeof(WaterElemental));

                        Point3D spawnLoc = this.Location;

                        for (int i = 0; i < 10; i++)
                        {
                            int x = Location.X + Utility.Random(4);
                            int y = Location.Y + Utility.Random(4);
                            int z = Map.GetAverageZ(x, y);

                            if (Map.CanSpawnMobile(new Point2D(x, y), this.Z))
                                spawnLoc = new Point3D(x, y, this.Z);
                            else if (Map.CanSpawnMobile(new Point2D(x, y), z))
                                spawnLoc = new Point3D(x, y, z);
                        }

                        Timer.DelayCall(TimeSpan.FromSeconds(1), delegate ()
                        {
                            bc.Home = Location;
                            bc.RangeHome = 5;
                            bc.FightMode = FightMode.Closest;
                            bc.MoveToWorld(spawnLoc, map);
                            bc.ForceReacquire();
                        });
                    }
                    catch
                    {
                    }
                    from.SendMessage("A water elemental emerges from the pool!");
                    this.m_Uses = this.m_Uses - 1;
                }
                else if (m_Pool == 5) // GOLD TO LEAD
                {
                    from.PlaySound(Utility.RandomList(0x30, 0x2D6));
                    Container cont = from.Backpack;
                    int nDull = 0;

                    int m_gAmount = from.Backpack.GetAmount(typeof(Gold));
                    int m_cAmount = from.Backpack.GetAmount(typeof(DDCopper));
                    int m_sAmount = from.Backpack.GetAmount(typeof(DDSilver));
                    int m_xAmount = from.Backpack.GetAmount(typeof(DDXormite));

                    if (cont.ConsumeTotal(typeof(Gold), m_gAmount))
                    {
                        from.AddToBackpack(new LeadCoin(m_gAmount));
                        nDull = 1;
                    }
                    if (cont.ConsumeTotal(typeof(DDCopper), m_cAmount))
                    {
                        from.AddToBackpack(new LeadCoin(m_cAmount));
                        nDull = 1;
                    }
                    if (cont.ConsumeTotal(typeof(DDSilver), m_sAmount))
                    {
                        from.AddToBackpack(new LeadCoin(m_sAmount));
                        nDull = 1;
                    }
                    if (cont.ConsumeTotal(typeof(DDXormite), m_xAmount))
                    {
                        from.AddToBackpack(new LeadCoin(m_xAmount));
                        nDull = 1;
                    }
                    if (nDull > 0)
                    {
                        from.SendMessage("After drinking from the pool, you notice all of your coins has turned to lead!");
                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

                        LoggingFunctions.LogGenericQuest(from, "had all of their coins turn to lead after drinking from a strange pool");
                    }
                    else
                    {
                        from.PlaySound(Utility.RandomList(0x30, 0x2D6));
                        from.SendMessage("You drink from the pool and nothing happens!");
                    }
                    this.m_Uses = this.m_Uses - 1;
                }
                else if (m_Pool == 6) // EQUIPPED ITEM DISAPPEARS
                {
                    from.PlaySound(Utility.RandomList(0x30, 0x2D6));
                    this.m_Uses = this.m_Uses - 1;
                    int mReturn = 0;
                    Item ILost = HiddenTrap.GetMyItem(from);
                    if (ILost != null) { ILost.Delete(); mReturn = 1; }
                    if (mReturn != 1)
                    {
                        from.SendMessage("After drinking from the pool, you notice one of your equipped items disappears!");
                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

                        LoggingFunctions.LogGenericQuest(from, "had an item vanish after drinking from a strange pool");
                    }
                }
                else if (m_Pool == 7) // LOSE A STAT POINT
                {
                    from.PlaySound(Utility.RandomList(0x30, 0x2D6));
                    this.m_Uses = this.m_Uses - 1;
                    int mCurse = 1;

                    if (m_Bonus > 8)
                    {
                        if (from.RawStr > 10) { from.RawStr = from.RawStr - 1; from.SendMessage("You feel weaker after drinking from the pool!"); }
                        else { from.SendMessage("You drink from the pool and nothing happens!"); mCurse = 0; }
                    }
                    else if (m_Bonus > 5)
                    {
                        if (from.RawDex > 10) { from.RawDex = from.RawDex - 1; from.SendMessage("You feel sluggish after drinking from the pool!"); }
                        else { from.SendMessage("You drink from the pool and nothing happens!"); mCurse = 0; }
                    }
                    else
                    {
                        if (from.RawInt > 10) { from.RawInt = from.RawInt - 1; from.SendMessage("Your mind is foggy after drinking from the pool!"); }
                        else { from.SendMessage("You drink from the pool and nothing happens!"); mCurse = 0; }
                    }

                    if (mCurse == 1)
                    {
                        from.FixedParticles(0x3779, 1, 15, 9905, 32, 2, EffectLayer.Head);
                        from.FixedParticles(0x37B9, 1, 14, 9502, 32, 5, (EffectLayer)255);
                    }
                }
                else if (m_Pool == 8) // TREASURE CHEST
                {
                    from.PlaySound(0x364);
                    from.SendMessage("You pull a mystical chest out from the pool!");
                    this.m_Uses = 0;
                    LootChest MyChest = new LootChest(6);
                    MyChest.ItemID = Utility.RandomList(0x2823, 0x2824, 0x4FE6, 0x4FE7, 0x281F, 0x2820);
                    MyChest.Hue = Utility.RandomList(0x961, 0x962, 0x963, 0x964, 0x965, 0x966, 0x967, 0x968, 0x969, 0x96A, 0x96B, 0x96C, 0x96D, 0x96E, 0x96F, 0x970, 0x971, 0x972, 0x973, 0x974, 0x975, 0x976, 0x977, 0x978, 0x979, 0x97A, 0x97B, 0x97C, 0x97D, 0x97E, 0x4AA);
                    Region reg = Region.Find(from.Location, from.Map);
                    MyChest.Name = "mystical chest from " + Server.Misc.Worlds.GetRegionName(from.Map, from.Location);
                    int xTraCash = Utility.RandomMinMax(5000, 8000);
                    LootPackChange.AddGoldToContainer(xTraCash, MyChest, from, 6);
                    int artychance = GetPlayerInfo.LuckyPlayerArtifacts(from.Luck) + 10;
                    if (Utility.RandomMinMax(0, 100) < artychance)
                    {
                        Item arty = Loot.RandomArty();
                        MyChest.DropItem(arty);
                    }
                    from.AddToBackpack(MyChest);

                    LoggingFunctions.LogGenericQuest(from, "found a chest full of treasure in some strange pool");
                }
                else if (m_Pool == 9) // COPPER SILVER TO GOLD
                {
                    from.PlaySound(Utility.RandomList(0x30, 0x2D6));
                    Container cont = from.Backpack;
                    int nShine = 0;

                    int m_cAmount = from.Backpack.GetAmount(typeof(DDCopper));
                    int m_sAmount = from.Backpack.GetAmount(typeof(DDSilver));
                    int m_dAmount = from.Backpack.GetAmount(typeof(LeadCoin));

                    if (cont.ConsumeTotal(typeof(DDCopper), m_cAmount))
                    {
                        from.AddToBackpack(new Gold(m_cAmount));
                        nShine = 1;
                    }
                    if (cont.ConsumeTotal(typeof(DDSilver), m_sAmount))
                    {
                        from.AddToBackpack(new Gold(m_sAmount));
                        nShine = 1;
                    }
                    if (cont.ConsumeTotal(typeof(LeadCoin), m_dAmount))
                    {
                        from.AddToBackpack(new Gold(m_dAmount));
                        nShine = 1;
                    }
                    if (nShine > 0)
                    {
                        from.SendMessage("After drinking from the pool, you notice your meager coins turn to gold!");
                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

                        LoggingFunctions.LogGenericQuest(from, "had all of their meager coins turn to gold after drinking from a strange pool");
                    }
                    else
                    {
                        from.SendMessage("You drink from the pool and nothing happens!");
                    }
                    this.m_Uses = 0;
                }
                else // POISON
                {
                    if (from.Poisoned)
                    {
                        from.SendMessage("You are too sick to drink from this pool!");
                    }
                    else
                    {
                        Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x36B0, 1, 14, 63, 7, 9915, 0);
                        from.PlaySound(Utility.RandomList(0x30, 0x2D6));
                        if (m_Bonus > 9) { from.ApplyPoison(from, Poison.Deadly); }
                        else if (m_Bonus > 7) { from.ApplyPoison(from, Poison.Greater); }
                        else if (m_Bonus > 4) { from.ApplyPoison(from, Poison.Regular); }
                        else { from.ApplyPoison(from, Poison.Lesser); }
                        from.SendMessage("You feel more sick after drinking from the pool!");
                        this.m_Uses = this.m_Uses - 1;
                    }
                }
            }
            else
            {
                from.SendMessage("The magic from the pool seems to be drained!");
            }
        }

        public void CheckAddComponents()
        {
            if (Deleted)
                return;
            AddComponents();
        }

        public virtual void AddComponents()
        {
        }

        public virtual void RefreshDecay(bool setDecayTime)
        {
            if (Deleted)
                return;
            if (m_DecayTimer != null)
                m_DecayTimer.Stop();
            if (setDecayTime)
                m_DecayTime = DateTime.Now + DecayDelay;

            TimeSpan ts = m_DecayTime - DateTime.Now;

            if (ts < TimeSpan.FromMinutes(2.0))
                ts = TimeSpan.FromMinutes(2.0);

            m_DecayTimer = Timer.DelayCall(ts, new TimerCallback(Delete));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(m_DecayTime);
            writer.WriteEncodedInt((int)m_Pool);
            writer.WriteEncodedInt((int)m_Uses);
            writer.WriteEncodedInt((int)m_Bonus);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            switch (version)
            {
                case 0:
                    {
                        m_DecayTime = reader.ReadDateTime();
                        RefreshDecay(false);
                        break;
                    }
            }
            m_Pool = reader.ReadEncodedInt();
            m_Uses = reader.ReadEncodedInt();
            m_Bonus = reader.ReadEncodedInt();
        }
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Server.Items
{
    public class LeadCoin : Item
    {
        public override double DefaultWeight
        {
            get { return 0.02; }
        }

        [Constructable]
        public LeadCoin() : this(1)
        {
        }

        [Constructable]
        public LeadCoin(int amountFrom, int amountTo) : this(Utility.RandomMinMax(amountFrom, amountTo))
        {
        }

        [Constructable]
        public LeadCoin(int amount) : base(0xEF0)
        {
            Stackable = true;
            Name = "lead coins";
            Amount = amount;
            Hue = 0x967;
            Light = LightType.Circle150;
        }

        public LeadCoin(Serial serial) : base(serial)
        {
        }

        public override int GetDropSound()
        {
            if (Amount <= 1)
                return 0x2E4;
            else if (Amount <= 5)
                return 0x2E5;
            else
                return 0x2E6;
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