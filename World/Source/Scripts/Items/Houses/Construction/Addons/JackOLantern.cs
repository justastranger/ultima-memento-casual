using System;
using System.Text;
using System.Net;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class JackOLantern : BaseAddon
    {
        public override bool ShareHue
        {
            get { return false; }
        }

        private AddonComponent GetComponent(int itemID, int hue)
        {
            AddonComponent ac = new AddonComponent(itemID);

            ac.Hue = hue;
            ac.Name = "jack-o-latern";

            return ac;
        }

        [Constructable]
        public JackOLantern()
            : this(1 > Utility.Random(2))
        {
        }

        [Constructable]
        public JackOLantern(bool south)
        {
            AddComponent(new AddonComponent(5703), 0, 0, +0);

            int hue = 1161;
            //( 1 > Utility.Random( 5 ) ? 2118 : 1161 );

            if (!south)
            {
                AddComponent(GetComponent(3178, 0000), 0, 0, -1);
                AddComponent(GetComponent(3883, hue), 0, 0, +1);
                AddComponent(GetComponent(3862, hue), 0, 0, +0);
            }
            else
            {
                AddComponent(GetComponent(3179, 0000), 0, 0, +0);
                AddComponent(GetComponent(3885, hue), 0, 0, -1);
                AddComponent(GetComponent(3871, hue), 0, 0, +0);
            }
        }

        public JackOLantern(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((byte)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadByte();

            if (version == 0)
            {
                Timer.DelayCall(TimeSpan.Zero, delegate ()
                {
                    for (int i = 0; i < Components.Count; ++i)
                    {
                        AddonComponent ac = Components[i] as AddonComponent;

                        if (ac != null && ac.Hue == 2118)
                            ac.Hue = 1161;
                    }
                });
            }
        }
    }
}