using System;
using Server;
using Server.Network;
using Server.Gumps;

namespace Server.Items
{
    [Flipable(0x12AB, 0x12AC)]
    public class TarotDeck : Item
    {
        private static string GetFortuneMsg(int MyFortune)
        {
            string phrase = "";

            switch (MyFortune)
            {
                default:
                case 0: phrase = "the Fool! They should watch their step and use their head."; break;
                case 1: phrase = "the Magician! They exhibit increased control of their destiny."; break;
                case 2: phrase = "the High Priestess! Their path will become clear to them."; break;
                case 3: phrase = "the Empress! Life is running smoothly."; break;
                case 4: phrase = "the Emperor! They must fight for what is theirs."; break;
                case 5: phrase = "the Hierophant! They must acknowledge their fallibility."; break;
                case 6: phrase = "the Lovers! They will be faced with an important choice."; break;
                case 7: phrase = "the Chariot! They are in a position to defeat their enemies. Strike now!"; break;
                case 8: phrase = "Justice! They will get what they deserve."; break;
                case 9: phrase = "the Hermit! They will discover a great truth."; break;
                case 10: phrase = "the Wheel of Fortune! Their fate is based on the caprice of the gods."; break;
                case 11: phrase = "Strength! They will face a great test of their endurance."; break;
                case 12: phrase = "the Hanged Man! They must sacrifice to attain their goal."; break;
                case 13: phrase = "Death! Their life will change completely...soon."; break;
                case 14: phrase = "Temperance! They must be patient!"; break;
                case 15: phrase = "the Devil! They shouldn't take the easy way out, it could mean destruction!"; break;
                case 16: phrase = "the Tower! They have overstepped their bounds."; break;
                case 17: phrase = "the Star! What they seek is within their grasp."; break;
                case 18: phrase = "the Moon! They should be wary of forces beyond their control lest they control them!"; break;
                case 19: phrase = "the Sun! They have worked hard. Now they can enjoy the fruits of their labor."; break;
                case 20: phrase = "Judgment! Their success is assured. They should strike while the iron is hot!"; break;
                case 21: phrase = "the World! They have achieved a complete success in their endeavor."; break;
            }
            return phrase;
        }

        private int GetFortuneImg(int MyFortune)
        {
            int value = 0;

            switch (MyFortune)
            {
                default:
                case 0: value = 0x454; break;
                case 1: value = 0x45C; break;
                case 2: value = 0x458; break;
                case 3: value = 0x453; break;
                case 4: value = 0x452; break;
                case 5: value = 0x457; break;
                case 6: value = 0x45B; break;
                case 7: value = 0x44F; break;
                case 8: value = 0x45A; break;
                case 9: value = 0x456; break;
                case 10: value = 0x463; break;
                case 11: value = 0x45F; break;
                case 12: value = 0x455; break;
                case 13: value = 0x450; break;
                case 14: value = 0x461; break;
                case 15: value = 0x451; break;
                case 16: value = 0x462; break;
                case 17: value = 0x45E; break;
                case 18: value = 0x45D; break;
                case 19: value = 0x460; break;
                case 20: value = 0x459; break;
                case 21: value = 0x464; break;
            }
            return value;
        }

        private class TarotGump : Gump
        {
            public TarotGump(int card) : base(0, 0)
            {
                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;
                AddPage(0);
                AddImage(52, 52, card);
            }
        }

        [Constructable]
        public TarotDeck() : base(0x12AB)
        {
            Name = "tarot deck";
        }

        public TarotDeck(Serial serial) : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            from.CloseGump(typeof(TarotGump));
            int MyFortune = Utility.Random(22);

            switch (((Item)this).ItemID)
            {
                case 0x12AB: // Closed north
                    if (Utility.Random(2) == 0)
                        ((Item)this).ItemID = 0x12A5;
                    else
                        ((Item)this).ItemID = 0x12A8;
                    break;
                case 0x12AC: // Closed east
                    if (Utility.Random(2) == 0)
                        ((Item)this).ItemID = 0x12A6;
                    else
                        ((Item)this).ItemID = 0x12A7;
                    break;
                case 0x12A5:
                    from.PublicOverheadMessage(MessageType.Regular, 0, false, string.Format("{0} draws " + GetFortuneMsg(MyFortune) + "", from.Name));
                    from.SendGump(new TarotGump(GetFortuneImg(MyFortune)));
                    break;
                case 0x12A6:
                    from.PublicOverheadMessage(MessageType.Regular, 0, false, string.Format("{0} draws " + GetFortuneMsg(MyFortune) + "", from.Name));
                    from.SendGump(new TarotGump(GetFortuneImg(MyFortune)));
                    break;
                case 0x12A8:
                    from.PublicOverheadMessage(MessageType.Regular, 0, false, string.Format("{0} draws " + GetFortuneMsg(MyFortune) + "", from.Name));
                    from.SendGump(new TarotGump(GetFortuneImg(MyFortune)));
                    break;
                case 0x12A7:
                    from.PublicOverheadMessage(MessageType.Regular, 0, false, string.Format("{0} draws " + GetFortuneMsg(MyFortune) + "", from.Name));
                    from.SendGump(new TarotGump(GetFortuneImg(MyFortune)));
                    break;
            }
        }

        public override void OnAdded(object target)
        {
            switch (((Item)this).ItemID)
            {
                case 0x12A5: ((Item)this).ItemID = 0x12AB; break; // Open north
                case 0x12A6: ((Item)this).ItemID = 0x12AC; break; // Open east
                case 0x12A8: ((Item)this).ItemID = 0x12AB; break; // Open north
                case 0x12A7: ((Item)this).ItemID = 0x12AC; break; // Open east
            }
        }
    }

    [Flipable(0x12AB, 0x12AC)]
    public class DecoTarotDeck : Item
    {
        [Constructable]
        public DecoTarotDeck() : base(0x12AB)
        {
            Name = "tarot deck";
        }

        public DecoTarotDeck(Serial serial) : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            switch (((Item)this).ItemID)
            {
                case 0x12AB: // Closed north
                    if (Utility.Random(2) == 0)
                        ((Item)this).ItemID = 0x12A5;
                    else
                        ((Item)this).ItemID = 0x12A8;
                    break;
                case 0x12AC: // Closed east
                    if (Utility.Random(2) == 0)
                        ((Item)this).ItemID = 0x12A6;
                    else
                        ((Item)this).ItemID = 0x12A7;
                    break;
                case 0x12A5: ((Item)this).ItemID = 0x12AB; break; // Open north
                case 0x12A6: ((Item)this).ItemID = 0x12AC; break; // Open east
                case 0x12A8: ((Item)this).ItemID = 0x12AB; break; // Open north
                case 0x12A7: ((Item)this).ItemID = 0x12AC; break; // Open east
            }
        }
    }
}
