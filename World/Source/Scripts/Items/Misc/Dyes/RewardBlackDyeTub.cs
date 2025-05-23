using System;

namespace Server.Items
{
    public class RewardBlackDyeTub : DyeTub
    {
        public override int LabelNumber { get { return 1006008; } } // Black Dye Tub

        private bool m_IsRewardItem;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsRewardItem
        {
            get { return m_IsRewardItem; }
            set { m_IsRewardItem = value; }
        }

        [Constructable]
        public RewardBlackDyeTub()
        {
            Hue = DyedHue = 0x0001;
            Redyable = false;
        }

        public RewardBlackDyeTub(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((bool)m_IsRewardItem);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_IsRewardItem = reader.ReadBool();
                        break;
                    }
            }
        }
    }
}