using System;
namespace Server
{
    /// <summary>
    /// You may apply explicit overrides `MySettings` overrides here.
    /// This file will never be touched by Memento development.
    /// </summary>
    public static class SettingOverrides
    {
        public static void Initialize()
        {
            // Example:
            // MySettings.S_SaveOnCharacterLogout = true;
            MySettings.S_ServerSaveMinutes = 15.0;
            MySettings.S_MaxMerchant = 4000;
            MySettings.S_BlackMarket = true;
            //MySettings.S_CustomMerchant = true;
            MySettings.S_HousesPerAccount = -1;
            MySettings.S_HouseStorage = true;
            MySettings.S_DamageToPets = 1.2;
            MySettings.S_Stables = 7;
            MySettings.S_SkillBoost = 10;
            MySettings.S_StatCap = 350;
            Console.WriteLine("Settings Overridden.");
        }
    }
}