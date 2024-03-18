using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CircleLoadingView : ViewBase
{
    [Header("ローディングアニメーションする丸を持つオブジェクト")]
    [SerializeField]
    private Transform _circuleParent;

    private Sequence _sequence;

    protected override void Init()
    {
        Hide(CanvasGroup);
        _sequence = DOTween.Sequence();
        SetLoadingAnimation(Ct);
        _sequence
            .Pause()
            .ToUniTask(cancellationToken: Ct)
            .Forget();
    }

    protected override void Destroy()
    {
        _sequence.Kill();
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        Show(CanvasGroup);
        await _sequence
            .Play()
            .ToUniTask(cancellationToken: ct);
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        await _sequence
            .Pause()
            .ToUniTask(cancellationToken: ct);
        Hide(CanvasGroup);
    }

    private void SetLoadingAnimation(CancellationToken ct)
    {
        Image[] _circules = _circuleParent.GetComponentsInChildren<Image>();
        float circuleAnimTime = AnimationTime / _circules.Length;

        for (int i = 0; i < _circules.Length; i++)
        {
            _sequence
                .Append(_circules[i]
                    .DOFade(0, circuleAnimTime)
                    .SetEase(Ease.Linear));
        }

        for (int i = 0; i < _circules.Length; i++)
        {
            _sequence
                .Append(_circules[i]
                    .DOFade(1, circuleAnimTime)
                    .SetEase(Ease.Linear));
        }

        _sequence
            .SetLoops(-1, LoopType.Incremental)
            .ToUniTask(cancellationToken: ct);
    }
}
