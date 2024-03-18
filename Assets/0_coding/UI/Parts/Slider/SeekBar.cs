using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class SeekBar : SliderBase
{
    public System.Action OnPointerDownEvent;
    public System.Action OnPointerUpEvent;

    private RectTransform _handleRect;
    private Vector2 _iniSize = Vector2.zero;
    private Vector2 _targetSize = Vector2.zero;

    protected override void Init()
    {
        _handleRect = Slider.handleRect;
        _targetSize = RectTransform.sizeDelta;
        _iniSize = _targetSize - new Vector2(0, _targetSize.y * 0.5f);

        RectTransform.sizeDelta = _iniSize;
        _handleRect.localScale = Vector2.zero;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUpEvent.Invoke();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownEvent.Invoke();
    }

    public override async void OnPointerEnter(PointerEventData eventData)
    {
        await Show(Ct);
    }

    public override async void OnPointerExit(PointerEventData eventData)
    {
        await Hide(Ct);
    }

    public async UniTask Show(CancellationToken ct)
    {
        RectTransform.DOComplete();

        _handleRect.localScale = Vector2.one;
        await RectTransform
            .DOSizeDelta(_targetSize, AnimationTime)
            .SetEase(Ease.Linear)
            .ToUniTask(cancellationToken : ct);
    }

    public async UniTask Hide(CancellationToken ct)
    {
        RectTransform.DOComplete();

        _handleRect.localScale = Vector2.zero;
        await RectTransform
            .DOSizeDelta(_iniSize, AnimationTime)
            .SetEase(Ease.Linear)
            .ToUniTask(cancellationToken: ct);
    }
}
