using System;
using Server.Items;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
    public class Actor : BaseCreature
    {
        [Constructable]
        public Actor() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
        {
            InitStats(31, 41, 51);

            SpeechHue = Utility.RandomTalkHue();

            Hue = Utility.RandomSkinColor();

            if (this.Female = Utility.RandomBool())
            {
                this.Body = 0x191;
                this.Name = NameList.RandomName("female");
                AddItem(new FancyDress(Utility.RandomDyedHue()));
                Title = "the actress";
            }
            else
            {
                this.Body = 0x190;
                this.Name = NameList.RandomName("male");
                AddItem(new LongPants(Utility.RandomNeutralHue()));
                AddItem(new FancyShirt(Utility.RandomDyedHue()));
                Title = "the actor";
            }

            AddItem(new Boots(Utility.RandomNeutralHue()));

            Utility.AssignRandomHair(this);

            Container pack = new Backpack();

            pack.DropItem(new Gold(250, 300));

            pack.Movable = false;

            AddItem(pack);
        }

        public override bool ClickTitle { get { return false; } }

        public Actor(Serial serial) : base(serial)
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
