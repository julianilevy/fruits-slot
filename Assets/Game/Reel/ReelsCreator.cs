using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using WinSystemsSlotTest.Settings;

namespace WinSystemsSlotTest
{
    public class ReelsCreator : MonoBehaviour
    {
        #region External Dependencies

        [Header("External Dependencies")]
        public ReelsManager reelsManager;
        public GameObject slotBackground;
        public Reel reelPrefab;
        public ReelContent reelContentPrefab;
        public OutOfSlotDetectorForReelContent outOfSlotDetectorForReelContentPrefab;

        #endregion

        #region Settings

        [Header("Settings")]
        public Vector2 paddingBetweenContent = Vector2.zero;
        public Vector2 initialReelsPosition = Vector2.zero;
        public int maxContentPerReel = 6;
        public int maxVisibleContentAtTimePerReel = 3;

        #endregion

        private void Start()
        {
            CreateReels();
        }

        private void CreateReels()
        {
            var reels = SettingsData.Get().reels;

            var reelsContainer = new GameObject();
            reelsContainer.name = "ReelsContainer";

            var previousPositionX = 0f;

            Bounds reelContentBounds = new Bounds();

            for (int x = 0; x < reels.Length; x++)
            {
                var previousPositionY = 0f;
                var reelContentIndexY = Random.Range(0, reels[x].Length);

                var newReel = Instantiate(reelPrefab);
                newReel.name = "Reel " + x;
                newReel.SetConfig(reels[x], reelContentIndexY, reelsManager.minTimeSpinning, paddingBetweenContent.y);

                ReelContent newReelContent = null;

                for (int y = 0; y < maxContentPerReel; y++)
                {
                    newReelContent = Instantiate(reelContentPrefab);
                    newReel.SetReelContent(newReelContent, new Vector2(previousPositionX, previousPositionY), reels[x][reelContentIndexY]);
                    newReel.reelContents.Enqueue(newReelContent);

                    reelContentBounds = newReelContent.GetBounds();
                    previousPositionY -= reelContentBounds.size.y + paddingBetweenContent.y;

                    if (y == 0)
                        newReel.SetYDisplacementBetweenContents(previousPositionY);

                    reelContentIndexY++;
                    if (reelContentIndexY >= reels[x].Length)
                        reelContentIndexY = 0;
                }

                var initialReelsPositionY = initialReelsPosition.y + (reelContentBounds.size.y * ((maxContentPerReel - 1) - maxVisibleContentAtTimePerReel)) + (paddingBetweenContent.y * (maxContentPerReel - 1));
                newReel.transform.position = new Vector2(initialReelsPosition.x, initialReelsPositionY);
                newReel.reelContents = new Queue<ReelContent>(newReel.reelContents.Reverse());
                newReel.transform.SetParent(reelsContainer.transform);
                newReel.SpinningStopped += reelsManager.OnOneReelSpinningStopped;

                reelsManager.AddReel(newReel);

                previousPositionX += reelContentBounds.size.x + paddingBetweenContent.x;
            }

            var slotBackgroundBounds = slotBackground.GetBounds();

            var outOfSlotDetectorForReelContent = Instantiate(outOfSlotDetectorForReelContentPrefab);
            outOfSlotDetectorForReelContent.name = "OutOfSlotDetectorForReelContent";
            outOfSlotDetectorForReelContent.GetComponent<BoxCollider2D>().size = slotBackgroundBounds.size;
            outOfSlotDetectorForReelContent.transform.position = new Vector3(0f, -slotBackgroundBounds.size.y - reelContentBounds.extents.y, 0f);

            for (int i = 0; i < maxVisibleContentAtTimePerReel; i++)
                reelsManager.AddRayOriginLocationForDetectingPossibleRewardingReelContents(new Vector2(slotBackgroundBounds.min.x, initialReelsPosition.y - (reelContentBounds.size.y * i)));
        }
    }
}