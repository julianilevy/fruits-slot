using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace WinSystemsSlotTest.Settings
{
    [Serializable]
    public class SettingsData
    {
        public int[][] reels;
        public Dictionary<int, RewardsSettings> rewardsByReelContentId;

        public static SettingsData Get()
        {
            var settingsDataFilePath = Path.Combine(Application.streamingAssetsPath, "Settings.json");
            var settingsDataAsJson = File.ReadAllText(settingsDataFilePath);
            var settingsData = JsonConvert.DeserializeObject<SettingsData>(settingsDataAsJson);

            return settingsData;
        }
    }
}