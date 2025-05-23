using System;

namespace Server.Items
{
    public class SpecialDyeTub : DyeTub
    {
        public override CustomHuePicker CustomHuePicker { get { return CustomHuePicker.SpecialDyeTub; } }
        public override int LabelNumber { get { return 1041285; } } // Special Dye Tub

        private bool m_IsRewardItem;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsRewardItem
        {
            get { return m_IsRewardItem; }
            set { m_IsRewardItem = value; }
        }

        [Constructable]
        public SpecialDyeTub()
        {
        }

        public SpecialDyeTub(Serial serial) : base(serial)
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