using System;
using System.Collections;
using Server;

namespace Knives.Chat3
{
    public class Avatar
    {
        private static Hashtable s_Avatars = new Hashtable();

        public static Hashtable Avatars { get { return s_Avatars; } }
        public static ArrayList AvaKeys { get { return new ArrayList(s_Avatars.Keys); } }

        public static void Initialize()
        {
            new Avatar(1, 18, 18);
            new Avatar(2, 18, 18);
            new Avatar(3, 18, 18);
            new Avatar(4, 18, 18);
            new Avatar(5, 18, 18);
            new Avatar(6, 18, 18);
            new Avatar(7, 18, 18);
            new Avatar(8, 18, 18);
            new Avatar(10, 18, 18);
            new Avatar(11, 18, 18);
            new Avatar(16, 18, 18);
            new Avatar(17, 18, 18);
            new Avatar(18, 18, 18);
            new Avatar(19, 18, 18);
            new Avatar(20, 18, 18);
            new Avatar(21, 18, 18);
            new Avatar(22, 18, 18);
            new Avatar(23, 18, 18);
            new Avatar(24, 18, 18);
            new Avatar(25, 18, 18);
            new Avatar(26, 18, 18);
            new Avatar(27, 18, 18);
            new Avatar(28, 18, 18);
            new Avatar(43, 18, 18);
            new Avatar(44, 18, 18);
            new Avatar(45, 18, 18);
            new Avatar(46, 18, 18);
            new Avatar(48, 18, 18);
            new Avatar(1000, 18, 18);
            new Avatar(1001, 18, 18);
            new Avatar(1002, 18, 18);
            new Avatar(1003, 18, 18);
            new Avatar(1004, 18, 18);
            new Avatar(1005, 18, 18);
            new Avatar(1006, 18, 18);
            new Avatar(1007, 18, 18);
            new Avatar(1008, 18, 18);
            new Avatar(1009, 18, 18);
            new Avatar(1010, 18, 18);
            new Avatar(1011, 18, 18);
            new Avatar(1012, 18, 18);
            new Avatar(1013, 18, 18);
            new Avatar(1014, 18, 18);
            new Avatar(1015, 18, 18);
            new Avatar(1016, 18, 18);
            new Avatar(1017, 18, 18);
            new Avatar(1018, 18, 18);
            new Avatar(1019, 18, 18);
            new Avatar(1020, 18, 18);
            new Avatar(1021, 18, 18);
            new Avatar(1022, 18, 18);
            new Avatar(1023, 18, 18);
            new Avatar(1024, 18, 18);
            new Avatar(1025, 18, 18);
            new Avatar(1026, 18, 18);
            new Avatar(1027, 18, 18);
            new Avatar(1028, 18, 18);
            new Avatar(1029, 18, 18);
            new Avatar(1030, 18, 18);
            new Avatar(1031, 18, 18);
            new Avatar(1032, 18, 18);
            new Avatar(1033, 18, 18);
            new Avatar(1034, 18, 18);
            new Avatar(1035, 18, 18);
            new Avatar(1036, 18, 18);
            new Avatar(1037, 18, 18);
            new Avatar(1038, 18, 18);
            new Avatar(1039, 18, 18);
            new Avatar(1040, 18, 18);
            new Avatar(1041, 18, 18);
            new Avatar(1042, 18, 18);
            new Avatar(1043, 18, 18);
            new Avatar(1044, 18, 18);
            new Avatar(1202, 18, 18);
            new Avatar(2240, 18, 18);
            new Avatar(2241, 18, 18);
            new Avatar(2242, 18, 18);
            new Avatar(2243, 18, 18);
            new Avatar(2244, 18, 18);
            new Avatar(2245, 18, 18);
            new Avatar(2246, 18, 18);
            new Avatar(2247, 18, 18);
            new Avatar(2248, 18, 18);
            new Avatar(2249, 18, 18);
            new Avatar(2250, 18, 18);
            new Avatar(2251, 18, 18);
            new Avatar(2252, 18, 18);
            new Avatar(2253, 18, 18);
            new Avatar(2254, 18, 18);
            new Avatar(2255, 18, 18);
            new Avatar(2256, 18, 18);
            new Avatar(2257, 18, 18);
            new Avatar(2258, 18, 18);
            new Avatar(2259, 18, 18);
            new Avatar(2260, 18, 18);
            new Avatar(2261, 18, 18);
            new Avatar(2262, 18, 18);
            new Avatar(2263, 18, 18);
            new Avatar(2264, 18, 18);
            new Avatar(2265, 18, 18);
            new Avatar(2266, 18, 18);
            new Avatar(2267, 18, 18);
            new Avatar(2268, 18, 18);
            new Avatar(2269, 18, 18);
            new Avatar(2270, 18, 18);
            new Avatar(2271, 18, 18);
            new Avatar(2272, 18, 18);
            new Avatar(2273, 18, 18);
            new Avatar(2274, 18, 18);
            new Avatar(2275, 18, 18);
            new Avatar(2276, 18, 18);
            new Avatar(2277, 18, 18);
            new Avatar(2278, 18, 18);
            new Avatar(2279, 18, 18);
            new Avatar(2280, 18, 18);
            new Avatar(2281, 18, 18);
            new Avatar(2282, 18, 18);
            new Avatar(2283, 18, 18);
            new Avatar(2284, 18, 18);
            new Avatar(2285, 18, 18);
            new Avatar(2286, 18, 18);
            new Avatar(2287, 18, 18);
            new Avatar(2288, 18, 18);
            new Avatar(2289, 18, 18);
            new Avatar(2290, 18, 18);
            new Avatar(2291, 18, 18);
            new Avatar(2292, 18, 18);
            new Avatar(2293, 18, 18);
            new Avatar(2294, 18, 18);
            new Avatar(2295, 18, 18);
            new Avatar(2296, 18, 18);
            new Avatar(2297, 18, 18);
            new Avatar(2298, 18, 18);
            new Avatar(2299, 18, 18);
            new Avatar(2300, 18, 18);
            new Avatar(2301, 18, 18);
            new Avatar(2302, 18, 18);
            new Avatar(2303, 18, 18);
            new Avatar(2405, 18, 18);
            new Avatar(2406, 18, 18);
            new Avatar(2407, 18, 18);
            new Avatar(2408, 18, 18);
            new Avatar(2409, 18, 18);
            new Avatar(2410, 18, 18);
            new Avatar(2411, 18, 18);
            new Avatar(2412, 18, 18);
            new Avatar(2413, 18, 18);
            new Avatar(2414, 18, 18);
            new Avatar(2415, 18, 18);
            new Avatar(2416, 18, 18);
            new Avatar(2417, 18, 18);
            new Avatar(2418, 18, 18);
            new Avatar(11195, 18, 18);
            new Avatar(11196, 18, 18);
            new Avatar(11197, 18, 18);
            new Avatar(11198, 18, 18);
            new Avatar(11199, 18, 18);
            new Avatar(11200, 18, 18);
            new Avatar(11201, 18, 18);
            new Avatar(11202, 18, 18);
            new Avatar(11203, 18, 18);
            new Avatar(11204, 18, 18);
            new Avatar(11205, 18, 18);
            new Avatar(11206, 18, 18);
            new Avatar(11207, 18, 18);
            new Avatar(11208, 18, 18);
            new Avatar(11209, 18, 18);
            new Avatar(11210, 18, 18);
            new Avatar(11211, 18, 18);
            new Avatar(11212, 18, 18);
            new Avatar(11213, 18, 18);
            new Avatar(11214, 18, 18);
            new Avatar(11215, 18, 18);
            new Avatar(11216, 18, 18);
            new Avatar(11217, 18, 18);
            new Avatar(11218, 18, 18);
            new Avatar(11219, 18, 18);
            new Avatar(11220, 18, 18);
            new Avatar(11221, 18, 18);
            new Avatar(11222, 18, 18);
            new Avatar(11223, 18, 18);
            new Avatar(11224, 18, 18);
            new Avatar(11225, 18, 18);
            new Avatar(11226, 18, 18);
            new Avatar(11227, 18, 18);
            new Avatar(11228, 18, 18);
            new Avatar(11229, 18, 18);
            new Avatar(11230, 18, 18);
            new Avatar(11231, 18, 18);
            new Avatar(11232, 18, 18);
            new Avatar(11233, 18, 18);
            new Avatar(11234, 18, 18);
            new Avatar(11235, 18, 18);
            new Avatar(11236, 18, 18);
            new Avatar(11237, 18, 18);
            new Avatar(11238, 18, 18);
            new Avatar(11239, 18, 18);
            new Avatar(11240, 18, 18);
            new Avatar(11241, 18, 18);
            new Avatar(11242, 18, 18);
            new Avatar(11243, 18, 18);
            new Avatar(11244, 18, 18);
            new Avatar(11245, 18, 18);
            new Avatar(11246, 18, 18);
            new Avatar(11247, 18, 18);
            new Avatar(11248, 18, 18);
            new Avatar(11249, 18, 18);
            new Avatar(11250, 18, 18);
            new Avatar(11251, 18, 18);
            new Avatar(11252, 18, 18);
            new Avatar(11253, 18, 18);
            new Avatar(11254, 18, 18);
            new Avatar(11255, 18, 18);
            new Avatar(11256, 18, 18);
            new Avatar(11257, 18, 18);
            new Avatar(11258, 18, 18);
            new Avatar(20480, 18, 18);
            new Avatar(20481, 18, 18);
            new Avatar(20482, 18, 18);
            new Avatar(20483, 18, 18);
            new Avatar(20484, 18, 18);
            new Avatar(20485, 18, 18);
            new Avatar(20486, 18, 18);
            new Avatar(20487, 18, 18);
            new Avatar(20488, 18, 18);
            new Avatar(20489, 18, 18);
            new Avatar(20490, 18, 18);
            new Avatar(20491, 18, 18);
            new Avatar(20492, 18, 18);
            new Avatar(20493, 18, 18);
            new Avatar(20494, 18, 18);
            new Avatar(20495, 18, 18);
            new Avatar(20496, 18, 18);
            new Avatar(20736, 18, 18);
            new Avatar(20737, 18, 18);
            new Avatar(20738, 18, 18);
            new Avatar(20739, 18, 18);
            new Avatar(20740, 18, 18);
            new Avatar(20741, 18, 18);
            new Avatar(20742, 18, 18);
            new Avatar(20743, 18, 18);
            new Avatar(20744, 18, 18);
            new Avatar(20745, 18, 18);
            new Avatar(20992, 18, 18);
            new Avatar(20993, 18, 18);
            new Avatar(20994, 18, 18);
            new Avatar(20995, 18, 18);
            new Avatar(20996, 18, 18);
            new Avatar(20997, 18, 18);
            new Avatar(20998, 18, 18);
            new Avatar(20999, 18, 18);
            new Avatar(21000, 18, 18);
            new Avatar(21001, 18, 18);
            new Avatar(21002, 18, 18);
            new Avatar(21003, 18, 18);
            new Avatar(21004, 18, 18);
            new Avatar(21005, 18, 18);
            new Avatar(21006, 18, 18);
            new Avatar(21007, 18, 18);
            new Avatar(21008, 18, 18);
            new Avatar(21009, 18, 18);
            new Avatar(21010, 18, 18);
            new Avatar(21011, 18, 18);
            new Avatar(21012, 18, 18);
            new Avatar(21013, 18, 18);
            new Avatar(21014, 18, 18);
            new Avatar(21015, 18, 18);
            new Avatar(21016, 18, 18);
            new Avatar(21017, 18, 18);
            new Avatar(21018, 18, 18);
            new Avatar(21019, 18, 18);
            new Avatar(21020, 18, 18);
            new Avatar(21021, 18, 18);
            new Avatar(21022, 18, 18);
            new Avatar(21536, 18, 18);
            new Avatar(21537, 18, 18);
            new Avatar(21538, 18, 18);
            new Avatar(21539, 18, 18);
            new Avatar(21540, 18, 18);
            new Avatar(21541, 18, 18);
            new Avatar(21542, 18, 18);

            General.LoadAvatarFile();
        }

        public static Avatar GetAvatar(Mobile m)
        {
            if (s_Avatars[Data.GetData(m).Avatar] == null)
                Data.GetData(m).Avatar = (int)AvaKeys[0];

            return (Avatar)s_Avatars[Data.GetData(m).Avatar];
        }

        private int c_Id, c_X, c_Y;

        public int Id { get { return c_Id; } }
        public int X { get { return c_X; } }
        public int Y { get { return c_Y; } }

        public Avatar(int id, int x, int y)
        {
            c_Id = id;
            c_X = x;
            c_Y = y;

            s_Avatars[id] = this;
        }
    }
}