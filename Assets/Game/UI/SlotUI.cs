using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace WinSystemsSlotTest.UI
{
    public class SlotUI : MonoBehaviour
    {
        #region External Dependencies

        [Header("External Dependencies")]
        public ReelsManager reelsManager;
        public TextMeshProUGUI rewardTextPrefab;

        #endregion

        #region Internal Dependencies

        [Header("Internal Dependencies")]
        public Button spinButton;

        #endregion

        private List<FadingText> _rewardTexts = new List<FadingText>();

        private void Awake()
        {
            reelsManager.RewardsShowed += OnRewardsShowed;
            reelsManager.ReadyToSpin += OnReadyToSpin;
        }

        private void OnRewardsShowed(List<ReelContent> rewardingReelContentLine, int rewardValue)
        {
            var newRewardText = Instantiate(rewardTextPrefab);
            newRewardText.transform.SetParent(transform);
            newRewardText.fontSize += (20f * (rewardingReelContentLine.Count - 2));
            newRewardText.text = "+" + rewardValue;
            newRewardText.transform.position = Utilities.GetBounds(rewardingReelContentLine).center;
            newRewardText.transform.localScale = Vector3.one;

            _rewardTexts.Add(newRewardText.GetComponent<FadingText>());
        }

        private void OnReadyToSpin()
        {
            spinButton.interactable = true;
        }

        #region UI Buttons

        // Called from button.
        public void OnSpinButtonPressed()
        {
            reelsManager.Spin();
            spinButton.interactable = false;

            if (_rewardTexts.Count > 0)
            {
                foreach (var rewardText in _rewardTexts)
                {
                    if (rewardText != null)
                        rewardText.FadeOut();
                }

                _rewardTexts.Clear();
            }
        }

        #endregion
    }
}