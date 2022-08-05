using System.Collections.Generic;
using UnityEngine;

namespace WinSystemsSlotTest.Data
{
    [CreateAssetMenu(fileName = "ReelData", menuName = "WinSystemsSlotTest/ReelData")]
    public class ReelData : ScriptableObject
    {
        public List<ReelContentData> reelContentData;
    }
}