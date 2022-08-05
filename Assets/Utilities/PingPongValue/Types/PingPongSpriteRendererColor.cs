using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class PingPongSpriteRendererColor : PingPongValue<Color>
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void TweenToTargetValue(ref Sequence tweenSequence)
    {
        tweenSequence.Append(_spriteRenderer.DOColor(targetValue, pingPongDuration));
    }

    protected override void TweenToOriginalValue(ref Sequence tweenSequence)
    {
        tweenSequence.Append(_spriteRenderer.DOColor(originalValue, pingPongDuration));
    }

    protected override void OnStartPingPong()
    {
        originalValue = _spriteRenderer.color;
    }

    protected override void OnStopPingPong()
    {
        _spriteRenderer.color = originalValue;
    }
}