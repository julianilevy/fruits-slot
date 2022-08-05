using UnityEngine;
using DG.Tweening;

public class PingPongTransformRotation : PingPongValue<Vector3>
{
    protected override void TweenToTargetValue(ref Sequence tweenSequence)
    {
        tweenSequence.Append(transform.DORotate(targetValue, pingPongDuration));
    }

    protected override void TweenToOriginalValue(ref Sequence tweenSequence)
    {
        tweenSequence.Append(transform.DORotate(originalValue, pingPongDuration));
    }

    protected override void OnStartPingPong()
    {
        originalValue = transform.rotation.eulerAngles;
    }

    protected override void OnStopPingPong()
    {
        transform.rotation = Quaternion.Euler(originalValue);
    }
}