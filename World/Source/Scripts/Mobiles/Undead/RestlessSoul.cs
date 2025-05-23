using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.ContextMenus;

namespace Server.Mobiles
{
    [CorpseName("a ghostly corpse")]
    public class RestlessSoul : BaseCreature
    {
        [Constructable]
        public RestlessSoul() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8)
        {
            Name = "restless soul";
            Body = 0x3CA;
            Hue = 0x453;

            SetStr(26, 40);
            SetDex(26, 40);
            SetInt(26, 40);

            SetHits(16, 24);

            SetDamage(1, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 25, 40);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 20.1, 30.0);
            SetSkill(SkillName.Swords, 20.1, 30.0);
            SetSkill(SkillName.Tactics, 20.1, 30.0);
            SetSkill(SkillName.FistFighting, 20.1, 30.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 6;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
        }

        public override bool AlwaysAttackable { get { return true; } }
        public override bool BleedImmune { get { return true; } }
        public override int Cloths { get { return 5; } }
        public override ClothType ClothType { get { return ClothType.Haunted; } }

        public override void DisplayPaperdollTo(Mobile to)
        {
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i] is ContextMenus.PaperdollEntry)
                    list.RemoveAt(i--);
            }
        }

        public override int GetIdleSound()
        {
            return 0x107;
        }

        public override int GetAngerSound()
        {
            return 0x1BF;
        }

        public override int GetDeathSound()
        {
            return 0xFD;
        }

        public RestlessSoul(Serial serial) : base(serial)
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