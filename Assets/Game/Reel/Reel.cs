using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using DG.Tweening;
using WinSystemsSlotTest.Data;

namespace WinSystemsSlotTest
{
    public class Reel : MonoBehaviour
    {
        #region External Dependencies

        [Header("External Dependencies")]
        public ReelData reelData;
        public Queue<ReelContent> reelContents = new Queue<ReelContent>();

        #endregion

        #region Settings

        [Header("Settings")]
        public float spinningSpeed = 50f;

        #endregion

        private int[] _reel;
        private int _currentReelIndex;
        private float _minTimeSpinning;
        private float _yPaddingBetweenContent;
        private float _yDisplacementBetweenContents;

        public event Action SpinningStopped;

        public void SetConfig(int[] reel, int currentReelIndex, float minTimeSpinning, float yPaddingBetweenContent)
        {
            _reel = reel;
            _currentReelIndex = currentReelIndex;
            _minTimeSpinning = minTimeSpinning;
            _yPaddingBetweenContent = yPaddingBetweenContent;
        }

        public void SetYDisplacementBetweenContents(float yDisplacementBetweenContents)
        {
            _yDisplacementBetweenContents = yDisplacementBetweenContents;
        }

        public void SetReelContent(ReelContent reelContent, Vector2 position, int id)
        {
            reelContent.name = reelData.reelContentData[id].name;
            reelContent.transform.position = position;
            reelContent.SetGraphics(reelData.reelContentData[id].sprite, reelData.reelContentData[id].frameSprite, reelData.reelContentData[id].frameColor, reelData.reelContentData[id].backgroundSprite, reelData.reelContentData[id].backgroundColor);
            reelContent.transform.SetParent(transform);
            reelContent.reel = this;
            reelContent.id = id;
        }

        public void Spin()
        {
            var tweener = transform.DOMoveY(transform.position.y + 1f, 0.15f);
            tweener.onComplete += OnPreSpinTweenComplete;
        }

        private void OnPreSpinTweenComplete()
        {
            foreach (var reelContent in reelContents)
                reelContent.Spin(spinningSpeed, -1);

            transform.DOMoveY(transform.position.y - 1f, _minTimeSpinning);
        }

        public void StopSpinning()
        {
            var yDisplacementBetweenContentsAux = 0f;
            var reelContentsReversed = reelContents.Reverse();

            foreach (var reelContent in reelContentsReversed)
            {
                reelContent.StopSpinning();
                reelContent.transform.localPosition = new Vector2(reelContent.transform.localPosition.x, yDisplacementBetweenContentsAux);

                yDisplacementBetweenContentsAux += _yDisplacementBetweenContents;
            }

            var tweener = transform.DOPunchPosition(Vector3.up, 0.5f, 10, 0.3f, false);
            tweener.onComplete += OnStopSpinningComplete;
        }

        private void OnStopSpinningComplete()
        {
            SpinningStopped?.Invoke();
        }

        public void UpdateReel()
        {
            var firstReelContent = reelContents.Dequeue();
            var position = new Vector2(firstReelContent.transform.position.x, Utilities.GetBounds(reelContents).max.y + firstReelContent.GetBounds().extents.y + _yPaddingBetweenContent);

            _currentReelIndex--;
            if (_currentReelIndex < 0)
                _currentReelIndex = _reel.Length - 1;

            SetReelContent(firstReelContent, position, _reel[_currentReelIndex]);

            reelContents.Enqueue(firstReelContent);
        }
    }
}