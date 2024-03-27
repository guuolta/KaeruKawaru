using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

public class SliderPanelView : PanelViewBase
{
    [Header("スライドする前の位置")]
    [SerializeField]
    private float _slideBeforePos;
    [Header("スライドする後の位置")]
    [SerializeField]
    private float _slideAfterPos;

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        RectTransform.DOComplete();

        await RectTransform
            .DOAnchorPosX(_slideBeforePos, AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: ct);
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        RectTransform.DOComplete();

        await RectTransform
            .DOAnchorPosX(_slideAfterPos, AnimationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: ct);
    }
}
