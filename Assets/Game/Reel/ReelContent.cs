using UnityEngine;

namespace WinSystemsSlotTest
{
    public class ReelContent : MonoBehaviour
    {
        #region Internal Dependencies

        [Header("Internal Dependencies")]
        public PingPongTransformScale content;
        public PingPongSpriteRendererColor frame;
        public PingPongTransformRotation background;

        #endregion

        [HideInInspector]
        public Reel reel;
        [HideInInspector]
        public int id;

        private SpriteRenderer _contentSpriteRenderer;
        private SpriteRenderer _frameSpriteRenderer;
        private SpriteRenderer _backgroundSpriteRenderer;
        private bool _spinning;
        private float _spinningSpeed;
        private int _spinningDirectionY;

        private void Awake()
        {
            _contentSpriteRenderer = content.GetComponent<SpriteRenderer>();
            _frameSpriteRenderer = frame.GetComponent<SpriteRenderer>();
            _backgroundSpriteRenderer = background.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (_spinning)
                transform.position += new Vector3(0f, _spinningSpeed * _spinningDirectionY * Time.deltaTime, 0f);
        }

        public void SetGraphics(Sprite sprite, Sprite frameSprite, Color frameColor, Sprite backgroundSprite, Color backgroundColor)
        {
            _contentSpriteRenderer.sprite = sprite;
            _frameSpriteRenderer.sprite = frameSprite;
            _frameSpriteRenderer.color = frameColor;
            _backgroundSpriteRenderer.sprite = backgroundSprite;
            _backgroundSpriteRenderer.color = backgroundColor;
        }

        public void Spin(float spinningSpeed, int spinningDirectionY)
        {
            _spinning = true;
            _spinningSpeed = spinningSpeed;
            _spinningDirectionY = spinningDirectionY;
        }

        public void StopSpinning()
        {
            _spinning = false;
        }

        public void ShowReward()
        {
            content.StartPingPong();
            frame.StartPingPong();
            _frameSpriteRenderer.enabled = true;
            background.StartPingPong();
            _backgroundSpriteRenderer.enabled = true;
        }

        public void HideReward()
        {
            content.StopPingPong();
            frame.StopPingPong();
            _frameSpriteRenderer.enabled = false;
            background.StopPingPong();
            _backgroundSpriteRenderer.enabled = false;
        }
    }
}