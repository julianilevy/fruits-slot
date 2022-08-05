using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class FadingGameObject<T> : MonoBehaviour where T : MaskableGraphic
{
    public float fadingTime = 1f;

    private T _gameObject;
    private Color _originalColor;
    private Tweener _tweener;

    private void Awake()
    {
        _gameObject = GetComponent<T>();
        _originalColor = _gameObject.color;
    }

    public void FadeIn()
    {
        _tweener = _gameObject.DOColor(new Color(_originalColor.r, _originalColor.g, _originalColor.b, 1f), fadingTime);
    }

    public void FadeOut()
    {
        _tweener = _gameObject.DOColor(new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0f), fadingTime);
    }

    private void OnDestroy()
    {
        if (_tweener != null)
            _tweener.Kill();
    }
}