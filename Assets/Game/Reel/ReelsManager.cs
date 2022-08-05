using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using WinSystemsSlotTest.Settings;

namespace WinSystemsSlotTest
{
    public class ReelsManager : MonoBehaviour
    {
        #region Settings

        [Header("Settings")]
        public float delayBetweenReelsSpins = 0.1f;
        public float minTimeSpinning = 2f;
        public float maxTimeSpinning = 4f;

        #endregion

        private Dictionary<int, RewardsSettings> _rewardsByReelContentId = new Dictionary<int, RewardsSettings>();
        private List<Reel> _reels = new List<Reel>();
        private List<Vector3> _rayOriginLocationsForDetectingPossibleRewardingReelContents = new List<Vector3>();
        private List<List<ReelContent>> _rewardingReelContentLines = new List<List<ReelContent>>();
        private int _spinningReelsStopped = 0;

        public event Action<List<ReelContent>, int> RewardsShowed;
        public event Action ReadyToSpin;

        private void Awake()
        {
            _rewardsByReelContentId = SettingsData.Get().rewardsByReelContentId;
        }

        public void Spin()
        {
            if (_rewardingReelContentLines.Count > 0)
            {
                foreach (var rewardingReelContentLine in _rewardingReelContentLines)
                {
                    foreach (var rewardingReelContent in rewardingReelContentLine)
                        rewardingReelContent.HideReward();
                }

                _rewardingReelContentLines.Clear();
            }

            StartCoroutine(_ManageReelsSpinning());
        }

        private IEnumerator _ManageReelsSpinning()
        {
            var timeSpinning = UnityEngine.Random.Range(minTimeSpinning, maxTimeSpinning);

            foreach (var reel in _reels)
            {
                reel.Spin();

                yield return new WaitForSeconds(delayBetweenReelsSpins);
            }

            yield return new WaitForSeconds(timeSpinning);

            foreach (var reel in _reels)
            {
                reel.StopSpinning();

                yield return new WaitForSeconds(delayBetweenReelsSpins);
            }
        }

        public void OnOneReelSpinningStopped()
        {
            if (_spinningReelsStopped >= _reels.Count - 1)
            {
                OnAllReelSpinningsStopped();

                _spinningReelsStopped = 0;
            }
            else
                _spinningReelsStopped++;
        }

        private void OnAllReelSpinningsStopped()
        {
            FindRewardingReelContents();

            if (_rewardingReelContentLines.Count > 0)
            {
                foreach (var rewardingReelContentLine in _rewardingReelContentLines)
                {
                    RewardsShowed(rewardingReelContentLine, GetReward(rewardingReelContentLine));

                    foreach (var rewardingReelContent in rewardingReelContentLine)
                        rewardingReelContent.ShowReward();
                }

                StartCoroutine(_WaitingTimeAfterShowingRewards());
            }
            else
                ReadyToSpin?.Invoke();
        }

        private void FindRewardingReelContents()
        {
            var allHitsByLine = new List<RaycastHit2D[]>();
            foreach (var rayOrigin in _rayOriginLocationsForDetectingPossibleRewardingReelContents)
                allHitsByLine.Add(Physics2D.RaycastAll(rayOrigin, Vector2.right, Mathf.Infinity, 1 << Layer.ReelContent));

            var possibleRewardingReelContents = new ReelContent[_reels.Count, allHitsByLine.Count];
            for (int y = 0; y < allHitsByLine.Count; y++)
            {
                for (int x = 0; x < _reels.Count; x++)
                    possibleRewardingReelContents[x, y] = allHitsByLine[y][x].collider.GetComponent<ReelContent>();
            }

            for (int y = 0; y < possibleRewardingReelContents.GetLength(1); y++)
            {
                var possibleRewardingReelContentLine = new List<ReelContent>();

                for (int x = 0; x < possibleRewardingReelContents.GetLength(0); x++)
                {
                    var currentPossibleRewardingReelContent = possibleRewardingReelContents[x, y];

                    if (possibleRewardingReelContentLine.Count >= 1)
                    {
                        var previousPossibleRewardingReelContent = possibleRewardingReelContentLine[possibleRewardingReelContentLine.Count - 1];

                        if (currentPossibleRewardingReelContent.id == previousPossibleRewardingReelContent.id)
                        {
                            possibleRewardingReelContentLine.Add(currentPossibleRewardingReelContent);

                            if (x >= possibleRewardingReelContents.GetLength(0) - 1)
                            {
                                if (possibleRewardingReelContentLine.Count >= 2)
                                {
                                    if (_rewardsByReelContentId[previousPossibleRewardingReelContent.id].rewardsByCombinationAmount.ContainsKey(possibleRewardingReelContentLine.Count))
                                        _rewardingReelContentLines.Add(new List<ReelContent>(possibleRewardingReelContentLine));
                                }
                            }
                        }
                        else
                        {
                            if (possibleRewardingReelContentLine.Count >= 2)
                            {
                                if (_rewardsByReelContentId[previousPossibleRewardingReelContent.id].rewardsByCombinationAmount.ContainsKey(possibleRewardingReelContentLine.Count))
                                    _rewardingReelContentLines.Add(new List<ReelContent>(possibleRewardingReelContentLine));
                            }

                            possibleRewardingReelContentLine.Clear();
                            possibleRewardingReelContentLine.Add(currentPossibleRewardingReelContent);
                        }
                    }
                    else
                        possibleRewardingReelContentLine.Add(currentPossibleRewardingReelContent);
                }
            }
        }

        private int GetReward(List<ReelContent> rewardingReelContentLine)
        {
            var reward = _rewardsByReelContentId[rewardingReelContentLine[0].id].rewardsByCombinationAmount[rewardingReelContentLine.Count];

            return reward;
        }

        private IEnumerator _WaitingTimeAfterShowingRewards()
        {
            yield return new WaitForSeconds(1f);

            ReadyToSpin?.Invoke();
        }

        public void AddReel(Reel reel)
        {
            _reels.Add(reel);
        }

        public void AddRayOriginLocationForDetectingPossibleRewardingReelContents(Vector3 location)
        {
            _rayOriginLocationsForDetectingPossibleRewardingReelContents.Add(location);
        }
    }
}