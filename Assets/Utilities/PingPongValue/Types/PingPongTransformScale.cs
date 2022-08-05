using UnityEngine;
using DG.Tweening;

public class PingPongTransformScale : PingPongValue<Vector3>
{
    protected override void TweenToTargetValue(ref Sequence tweenSequence)
    {
        tweenSequence.Append(transform.DOScale(targetValue, pingPongDuration));
    }

    protected override void TweenToOriginalValue(ref Sequence tweenSequence)
    {
        tweenSequence.Append(transform.DOScale(originalValue, pingPongDuration));
    }

    protected override void OnStartPingPong()
    {
        originalValue = transform.localScale;
    }

    protected override void OnStopPingPong()
    {
        transform.localScale = originalValue;
    }
}