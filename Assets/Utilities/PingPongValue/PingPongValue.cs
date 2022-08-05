using UnityEngine;
using DG.Tweening;
using System.Collections;

public abstract class PingPongValue<T> : MonoBehaviour
{
    #region Settings

    [Header("Settings")]
    public T targetValue;
    public int pingPongAmount = 3;
    public float pingPongDuration = 0.4f;
    public bool loop = true;
    public float loopInterval = 1f;

    #endregion

    protected T originalValue;

    private Coroutine _waitLoopInterval;
    private Sequence _tweenSequence;

    protected abstract void TweenToTargetValue(ref Sequence tweenSequence);
    protected abstract void TweenToOriginalValue(ref Sequence tweenSequence);
    protected abstract void OnStartPingPong();
    protected abstract void OnStopPingPong();

    public void StartPingPong()
    {
        OnStartPingPong();

        _tweenSequence = DOTween.Sequence();
        _tweenSequence.onComplete += OnPingPongComplete;

        for (int i = 0; i < pingPongAmount * 2; i++)
        {
            if (i % 2 == 0)
                TweenToTargetValue(ref _tweenSequence);
            else
                TweenToOriginalValue(ref _tweenSequence);
        }
    }

    private void OnPingPongComplete()
    {
        if (loop)
            _waitLoopInterval = StartCoroutine(_WaitLoopInterval());
    }

    private IEnumerator _WaitLoopInterval()
    {
        yield return new WaitForSeconds(loopInterval);

        StartPingPong();
    }

    public void StopPingPong()
    {
        if (_waitLoopInterval != null)
            StopCoroutine(_waitLoopInterval);
        if (_tweenSequence != null)
            _tweenSequence.Kill();

        OnStopPingPong();
    }
}